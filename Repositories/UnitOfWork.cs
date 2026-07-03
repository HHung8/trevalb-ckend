using TravelApp.Infrastructure.Data;
using TrevalApp.Interfaces.Repositories;
using TrevalApp.Models;

namespace TravelApp.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
 
    private IUserRepository? _users;
    private IDestinationRepository? _destinations;
    private ITourRepository? _tours;
    private IHotelRepository? _hotels;
    private IRepository<Room>? _rooms;
    private ITourBookingRepository? _tourBookings;
    private IHotelBookingRepository? _hotelBookings;
    private IRepository<Payment>? _payments;
    private IReviewRepository? _reviews;
    private IPostRepository? _posts;
    private IRepository<Attraction>? _attractions;
    private IRepository<Wishlist>? _wishlists;
    private IRepository<Notification>? _notifications;
 
    public UnitOfWork(AppDbContext context) => _context = context;
    
    public IUserRepository Users => _users ??= new UserRepository(_context);
    public IDestinationRepository Destinations => _destinations ??= new DestinationRepository(_context);
    public ITourRepository Tours => _tours ??= new TourRepository(_context);
    public IHotelRepository Hotels => _hotels ??= new HotelRepository(_context);
    public IRepository<Room> Rooms => _rooms ??= new GenericRepository<Room>(_context);
    public ITourBookingRepository TourBookings => _tourBookings ??= new TourBookingRepository(_context);
    public IHotelBookingRepository HotelBookings => _hotelBookings ??= new HotelBookingRepository(_context);
    public IRepository<Payment> Payments => _payments ??= new GenericRepository<Payment>(_context);
    public IReviewRepository Reviews => _reviews ??= new ReviewRepository(_context);
    public IPostRepository Posts => _posts ??= new PostRepository(_context);
    public IRepository<Attraction> Attractions => _attractions ??= new GenericRepository<Attraction>(_context);
    public IRepository<Wishlist> Wishlists => _wishlists ??= new GenericRepository<Wishlist>(_context);
    public IRepository<Notification> Notifications => _notifications ??= new GenericRepository<Notification>(_context);
    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
    public void Dispose() => _context.Dispose();


}