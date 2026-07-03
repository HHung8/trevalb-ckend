namespace TrevalApp.DTOs.Tour;

public record TourDto(
    Guid Id, 
    string Title, 
    string? Description, 
    decimal Price, 
    decimal? DiscountPrice,
    int DurationDays, 
    int MaxCapacity, 
    string Difficulty, 
    string? ThumbnailUrl,
    double? AverageRating, 
    int ReviewCount, 
    Guid DestinationId, 
    string DestinationName);