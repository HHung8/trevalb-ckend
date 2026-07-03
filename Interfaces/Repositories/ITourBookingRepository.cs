using TrevalApp.Models;

namespace TrevalApp.Interfaces.Repositories;

public interface ITourBookingRepository : IRepository<TourBooking>
{
    Task<IEnumerable<TourBooking>> GetByUserAsync(Guid userId);
    Task<TourBooking?> GetByCodeAsync(string bookingCode);
    Task<TourBooking?> GetWithDetailsAsync(Guid id);
}