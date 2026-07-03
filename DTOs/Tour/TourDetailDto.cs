namespace TrevalApp.DTOs.Tour;

public record TourDetailDto(
    Guid Id, 
    string Title, 
    string? Description, 
    string? Highlights,
    string? Includes, 
    string? Excludes, 
    decimal Price, 
    decimal? DiscountPrice,
    int DurationDays, 
    int MaxCapacity, 
    string Difficulty, 
    string? ThumbnailUrl,
    double? AverageRating, 
    int ReviewCount, 
    Guid DestinationId, 
    string DestinationName,
    IEnumerable<string> Images, IEnumerable<TourScheduleDto> Schedules);