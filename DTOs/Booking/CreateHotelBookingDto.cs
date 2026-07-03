namespace TrevalApp.DTOs.Booking;

public record CreateHotelBookingDto(
    Guid RoomId, 
    DateTime CheckIn, 
    DateTime CheckOut, 
    int NumGuests, 
    string? SpecialRequest
);