using Microsoft.EntityFrameworkCore;
using TravelApp.Infrastructure.Data;
using TrevalApp.Interfaces.Repositories;
using TrevalApp.Models;

namespace TravelApp.Infrastructure.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context) { }

    public async Task<User?> GetByEmailAsync(string email) =>
        await _dbSet.FirstOrDefaultAsync(u => u.Email == email && u.IsActive);

    public async Task<User?> GetByRefreshTokenAsync(string refreshToken) =>
        await _dbSet.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken
            && u.RefreshTokenExpiry > DateTime.UtcNow);
}

public class DestinationRepository : GenericRepository<Destination>, IDestinationRepository
{
    public DestinationRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Destination>> SearchAsync(string? keyword, string? country, int page, int pageSize)
    {
        var query = _dbSet.AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
            query = query.Where(d => d.Name.Contains(keyword) || d.City.Contains(keyword));

        if (!string.IsNullOrWhiteSpace(country))
            query = query.Where(d => d.Country == country);

        return await query
            .OrderBy(d => d.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public Task<Destination?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Destination?> GetWithDetailsAsync(Guid id) =>
        await _dbSet
            .Include(d => d.Images)
            .Include(d => d.Tours.Where(t => t.IsActive))
            .Include(d => d.Hotels.Where(h => h.IsActive))
            .Include(d => d.Attractions)
            .FirstOrDefaultAsync(d => d.Id == id);

    public async Task<IEnumerable<Destination>> GetFeaturedAsync(int count = 6) =>
        await _dbSet
            .Where(d => d.IsFeatured)
            .OrderBy(d => d.Name)
            .Take(count)
            .ToListAsync();
}

public class TourRepository : GenericRepository<Tour>, ITourRepository
{
    public TourRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Tour>> SearchAsync(
        Guid? destinationId, string? keyword, decimal? minPrice, decimal? maxPrice,
        string? difficulty, int page, int pageSize)
    {
        var query = _dbSet.Include(t => t.Destination).Where(t => t.IsActive);

        if (destinationId.HasValue)
            query = query.Where(t => t.DestinationId == destinationId.Value);

        if (!string.IsNullOrWhiteSpace(keyword))
            query = query.Where(t => t.Title.Contains(keyword) || t.Description!.Contains(keyword));

        if (minPrice.HasValue) query = query.Where(t => t.Price >= minPrice.Value);
        if (maxPrice.HasValue) query = query.Where(t => t.Price <= maxPrice.Value);
        if (!string.IsNullOrWhiteSpace(difficulty)) query = query.Where(t => t.Difficulty == difficulty);

        return await query
            .OrderByDescending(t => t.AverageRating)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Tour?> GetWithDetailsAsync(Guid id) =>
        await _dbSet
            .Include(t => t.Destination)
            .Include(t => t.Images)
            .Include(t => t.Schedules.Where(s => s.IsActive && s.StartDate > DateTime.UtcNow))
            .FirstOrDefaultAsync(t => t.Id == id);

    public async Task<IEnumerable<Tour>> GetByDestinationAsync(Guid destinationId) =>
        await _dbSet
            .Where(t => t.DestinationId == destinationId && t.IsActive)
            .OrderByDescending(t => t.AverageRating)
            .ToListAsync();
}

public class HotelRepository : GenericRepository<Hotel>, IHotelRepository
{
    public HotelRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Hotel>> SearchAsync(
        Guid? destinationId, string? keyword, int? minStars,
        decimal? minPrice, decimal? maxPrice, int page, int pageSize)
    {
        var query = _dbSet
            .Include(h => h.Rooms)
            .Include(h => h.Destination)
            .Where(h => h.IsActive);

        if (destinationId.HasValue) query = query.Where(h => h.DestinationId == destinationId.Value);
        if (!string.IsNullOrWhiteSpace(keyword)) query = query.Where(h => h.Name.Contains(keyword));
        if (minStars.HasValue) query = query.Where(h => h.StarRating >= minStars.Value);

        if (minPrice.HasValue || maxPrice.HasValue)
        {
            query = query.Where(h => h.Rooms.Any(r =>
                (!minPrice.HasValue || r.PricePerNight >= minPrice.Value) &&
                (!maxPrice.HasValue || r.PricePerNight <= maxPrice.Value)));
        }

        return await query
            .OrderByDescending(h => h.AverageRating)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Hotel?> GetWithDetailsAsync(Guid id) =>
        await _dbSet
            .Include(h => h.Destination)
            .Include(h => h.Images)
            .Include(h => h.Rooms.Where(r => r.IsAvailable))
                .ThenInclude(r => r.Images)
            .FirstOrDefaultAsync(h => h.Id == id);
}

public class TourBookingRepository : GenericRepository<TourBooking>, ITourBookingRepository
{
    public TourBookingRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<TourBooking>> GetByUserAsync(Guid userId) =>
        await _dbSet
            .Include(b => b.Tour).ThenInclude(t => t.Destination)
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();

    public async Task<TourBooking?> GetByCodeAsync(string bookingCode) =>
        await _dbSet.FirstOrDefaultAsync(b => b.BookingCode == bookingCode);

    public async Task<TourBooking?> GetWithDetailsAsync(Guid id) =>
        await _dbSet
            .Include(b => b.Tour).ThenInclude(t => t.Destination)
            .Include(b => b.User)
            .Include(b => b.Payment)
            .FirstOrDefaultAsync(b => b.Id == id);
}

public class HotelBookingRepository : GenericRepository<HotelBooking>, IHotelBookingRepository
{
    public HotelBookingRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<HotelBooking>> GetByUserAsync(Guid userId) =>
        await _dbSet
            .Include(b => b.Room).ThenInclude(r => r.Hotel)
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();

    public async Task<HotelBooking?> GetByCodeAsync(string bookingCode) =>
        await _dbSet.FirstOrDefaultAsync(b => b.BookingCode == bookingCode);

    public async Task<HotelBooking?> GetWithDetailsAsync(Guid id) =>
        await _dbSet
            .Include(b => b.Room).ThenInclude(r => r.Hotel).ThenInclude(h => h.Destination)
            .Include(b => b.User)
            .Include(b => b.Payment)
            .FirstOrDefaultAsync(b => b.Id == id);

    public async Task<bool> CheckRoomAvailabilityAsync(Guid roomId, DateTime checkIn, DateTime checkOut) =>
        !await _dbSet.AnyAsync(b =>
            b.RoomId == roomId &&
            b.Status != "cancelled" &&
            b.CheckIn < checkOut &&
            b.CheckOut > checkIn);
}

public class ReviewRepository : GenericRepository<Review>, IReviewRepository
{
    public ReviewRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Review>> GetByTargetAsync(
        string targetType, Guid targetId, int page, int pageSize) =>
        await _dbSet
            .Include(r => r.User)
            .Include(r => r.Images)
            .Where(r => r.TargetType == targetType && r.TargetId == targetId)
            .OrderByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

    public async Task<double> GetAverageRatingAsync(string targetType, Guid targetId)
    {
        var ratings = await _dbSet
            .Where(r => r.TargetType == targetType && r.TargetId == targetId)
            .Select(r => (double)r.Rating)
            .ToListAsync();
        return ratings.Any() ? ratings.Average() : 0;
    }
}

public class PostRepository : GenericRepository<Post>, IPostRepository
{
    public PostRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Post>> GetFeedAsync(int page, int pageSize) =>
        await _dbSet
            .Include(p => p.User)
            .Include(p => p.Destination)
            .Include(p => p.Media.OrderBy(m => m.DisplayOrder).Take(1))
            .Where(p => p.IsPublished)
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

    public async Task<IEnumerable<Post>> GetByUserAsync(Guid userId, int page, int pageSize) =>
        await _dbSet
            .Include(p => p.Destination)
            .Include(p => p.Media.OrderBy(m => m.DisplayOrder).Take(1))
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

    public async Task<Post?> GetWithDetailsAsync(Guid id) =>
        await _dbSet
            .Include(p => p.User)
            .Include(p => p.Destination)
            .Include(p => p.Media.OrderBy(m => m.DisplayOrder))
            .Include(p => p.Comments.OrderByDescending(c => c.CreatedAt).Take(20))
                .ThenInclude(c => c.User)
            .FirstOrDefaultAsync(p => p.Id == id);
}