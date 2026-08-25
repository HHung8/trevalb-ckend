namespace TrevalApp.Models;

public class WishlistTourResult
{
    public Guid WishlistId { get; set; }
    public Guid Id { get; set; }
    public Guid DestinationId { get; set; }
    public string DestinationName { get; set; } = null!;
    public string Title { get; set; } = null!;
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public int DurationDays { get; set; }
    public string? ThumbnailUrl { get; set; }
    public double? AverageRating { get; set; }
    public int ReviewCount { get; set; }
    public DateTimeOffset SavedAt { get; set; }
    public long TotalCount { get; set; }
}

public class WishlistHotelResult
{
    public Guid WishlistId { get; set; }
    public Guid Id { get; set; }
    public Guid DestinationId { get; set; }
    public string DestinationName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int StarRating { get; set; }
    public string? ThumbnailUrl { get; set; }
    public double? AverageRating { get; set; }
    public int ReviewCount { get; set; }
    public decimal? MinRoomPrice { get; set; }
    public DateTimeOffset SavedAt { get; set; }
    public long TotalCount { get; set; }
}