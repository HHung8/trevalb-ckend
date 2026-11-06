namespace TravelApp.Infrastructure.Data.DTOs.Wishlish;

public record ToggleWishlistDto (
    string ItemType, 
    Guid ItemId);

public record ToggleWishlistResultDto(
        bool IsWishlisted);

public record WishlistTourDto(
    Guid WishlistId,
    Guid Id,
    Guid DestinationId,
    string DestinationName,
    string Title,
    decimal Price,
    decimal? DiscountPrice,
    int DurationDays,
    string? ThumbnailUrl,
    double? AverageRating,
    int ReviewCount,
    DateTimeOffset SavedAt
);

public record WishlistHotelDto(
    Guid WishlistId,
    Guid Id,
    Guid DestinationId,
    string DestinationName,
    string Name,
    int StarRating,
    string? ThumbnailUrl,
    double? AverageRating,
    int ReviewCount,
    decimal? MinRoomPrice,
    DateTimeOffset SavedAt
);

public record WishlistQueryDto(
    int Page = 1,
    int PageSize = 10
);

public record WishlistAttractionDto(
    Guid WishlistId,
    Guid Id,
    Guid DestinationId,
    string DestinationName,
    string Name,
    string? Category,
    string? ThumbnailUrl,
    decimal? EntryFee,
    DateTime SavedAt);
    
public record WishlistDestinationDto(
    Guid WishlistId, Guid Id, string Name, string? Country, string? City,
    string? ThumbnailUrl, DateTime SavedAt);