namespace TrevalApp.DTOs.Booking;

public record AttractionBookingDto(
    Guid Id,
    string BookingCode,
    Guid AttractionId,
    string AttractionName,
    string? ThumbnailUrl,
    int NumGuests,
    DateTime VisitDate,
    decimal TotalPrice,
    string Status,
    DateTime CreatedAt
);

public record CreateAttractionBookingDto(
    Guid AttractionId,
    Guid ScheduleId,
    int NumGuests,
    string? SpecialRequest
);