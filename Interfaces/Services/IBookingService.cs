using TrevalApp.DTOs.Booking;
namespace TrevalApp.Interfaces.Services;

public interface IBookingService
{
    Task<TourBookingDto> CreateTourBookingAsync(Guid userId, CreateTourBookingDto dto);
    Task<HotelBookingDto> CreateHotelBookingAsync(Guid userId, CreateHotelBookingDto dto);
    Task<IEnumerable<TourBookingDto>> GetUserTourBookingsAsync(Guid userId);
    Task<IEnumerable<HotelBookingDto>> GetUserHotelBookingsAsync(Guid userId);
    Task<TourBookingDto> GetTourBookingByIdAsync(Guid id, Guid userId);
    Task<HotelBookingDto> GetHotelBookingByIdAsync(Guid id, Guid userId);
    Task CancelTourBookingAsync(Guid id, Guid userId);
    Task CancelHotelBookingAsync(Guid id, Guid userId);
}