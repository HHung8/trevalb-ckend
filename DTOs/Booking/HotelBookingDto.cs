namespace TrevalApp.DTOs.Booking;

public record HotelBookingDto(
    Guid Id, 
    string BookingCode, 
    Guid RoomId, 
    string RoomType,
    string HotelName, 
    string? ThumbnailUrl,
    DateTime CheckIn, 
    DateTime CheckOut, 
    int NumGuests, 
    decimal TotalPrice, 
    string Status, 
    DateTime CreatedAt);