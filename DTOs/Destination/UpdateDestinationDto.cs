namespace TrevalApp.DTOs.Destination;

public record UpdateDestinationDto(
    string? Name,
    string? Country,
    string? City,
    string? Description,
    double? Latitude,
    double? Longitude,
    string? Climate,
    string? BestTimeToVisit,
    bool? IsFeatured
);