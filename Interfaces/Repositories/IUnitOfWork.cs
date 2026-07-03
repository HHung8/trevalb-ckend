using TrevalApp.Models;

namespace TrevalApp.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IDestinationRepository Destinations { get; }
    ITourRepository Tours { get; }
    IHotelRepository Hotels { get; }
    IRepository<Room> Rooms { get; }
    ITourBookingRepository TourBookings { get; }
    IHotelBookingRepository HotelBookings { get; }
    IRepository<Payment> Payments { get; }
    IReviewRepository Reviews { get; }
    IPostRepository Posts { get; }
    IRepository<Attraction> Attractions { get; }
    IRepository<Wishlist> Wishlists { get; }
    IRepository<Notification> Notifications { get; }
    Task<int> SaveChangesAsync();
}