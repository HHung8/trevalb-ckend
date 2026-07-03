namespace TrevalApp.DTOs.Booking;

public record TourBookingDto(
    Guid Id, 
    string BookingCode, 
    Guid TourId, 
    string TourTitle, 
    string? ThumbnailUrl,
    int NumGuests, 
    decimal TotalPrice, 
    string Status, 
    DateTime TravelDate, 
    DateTime CreatedAt);