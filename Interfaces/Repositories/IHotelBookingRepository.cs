using TrevalApp.Models;

namespace TrevalApp.Interfaces.Repositories;

public interface IHotelBookingRepository : IRepository<HotelBooking>
{
    Task<IEnumerable<HotelBooking>> GetByUserAsync(Guid userId);
    Task<HotelBooking?> GetByCodeAsync(string bookingCode);
    Task<HotelBooking?> GetWithDetailsAsync(Guid id);
    Task<bool> CheckRoomAvailabilityAsync(Guid roomId, DateTime checkIn, DateTime checkOut);
}