namespace TrevalApp.DTOs.Booking;

public record CreateTourBookingDto(
    Guid TourId, 
    Guid? ScheduleId, 
    int NumGuests, 
    DateTime TravelDate, 
    string? SpecialRequest
);