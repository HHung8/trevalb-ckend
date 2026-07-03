namespace TrevalApp.DTOs.Destination;

public record DestinationDetailDto(
    Guid Id,
    string Name,
    string Country,
    string City,
    string? Description,
    string? ThumbnailUrl,
    double Latitude,
    double Longitude,
    string? Climate,
    string? BestTimeToVisit,
    bool IsFeatured,
    IEnumerable<string> Images
    );