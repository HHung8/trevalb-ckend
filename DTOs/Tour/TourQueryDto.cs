namespace TrevalApp.DTOs.Tour;

public record TourQueryDto(
    Guid? DestinationId = null, 
    string? Keyword = null,
    decimal? MinPrice = null, 
    decimal? MaxPrice = null,
    string? Difficulty = null, 
    int Page = 1, 
    int PageSize = 10
);