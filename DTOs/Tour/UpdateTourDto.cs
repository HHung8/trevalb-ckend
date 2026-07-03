namespace TrevalApp.DTOs.Tour;

public record UpdateTourDto(
    string? Title, 
    string? Description, 
    string? Highlights, 
    string? Includes,
    string? Excludes, 
    decimal? Price, 
    decimal? DiscountPrice, 
    int? DurationDays,
    int? MaxCapacity, 
    string? Difficulty, 
    bool? IsActive
 );