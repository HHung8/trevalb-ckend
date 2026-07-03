namespace TrevalApp.DTOs.Destination;

public record DestinationDto(
    Guid Id,
    string Name,
    string Country,
    string City,
    string? Description,
    string? ThumbnailUrl,
    double Latitude,
    double Longitude,
    bool IsFeatured,
    int TourCount,
    int HotelCount
);