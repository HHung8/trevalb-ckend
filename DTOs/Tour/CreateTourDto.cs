namespace TrevalApp.DTOs.Tour;

public record CreateTourDto(
    Guid DestinationId, 
    string Title, 
    string? Description, 
    string? Highlights,
    string? Includes, 
    string? Excludes, 
    decimal Price, 
    decimal? DiscountPrice,
    int DurationDays, 
    int MaxCapacity, 
    string Difficulty = "EASY"
);