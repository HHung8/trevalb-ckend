namespace TrevalApp.DTOs.Hotel;

public record HotelQueryDto(
    Guid? DestinationId = null, 
    string? Keyword = null, 
    int? MinStars = null,
    decimal? MinPrice = null, 
    decimal? MaxPrice = null, 
    int Page = 1, 
    int PageSize = 10
);