namespace TrevalApp.Models;

/// <summary>Dùng cho: search_tours</summary>
public class TourSearchResult
{
    public Guid Id { get; set; }
    public Guid DestinationId { get; set; }
    public string DestinationName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public int DurationDays { get; set; }
    public int MaxCapacity { get; set; }
    public string? Difficulty { get; set; }
    public string? ThumbnailUrl { get; set; }
    public double? AverageRating { get; set; }
    public int ReviewCount { get; set; }
    public long TotalCount { get; set; }
}
 
/// <summary>Dùng cho: get_tour_by_id</summary>
public class TourDetailResult
{
    public Guid Id { get; set; }
    public Guid DestinationId { get; set; }
    public string DestinationName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Highlights { get; set; }
    public string? Includes { get; set; }
    public string? Excludes { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public int DurationDays { get; set; }
    public int MaxCapacity { get; set; }
    public string? Difficulty { get; set; }
    public string? ThumbnailUrl { get; set; }
    public double? AverageRating { get; set; }
    public int ReviewCount { get; set; }
}
 
/// <summary>Dùng cho: get_tours_by_destination</summary>
public class TourByDestinationResult
{
    public Guid Id { get; set; }
    public Guid DestinationId { get; set; }
    public string DestinationName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public int DurationDays { get; set; }
    public int MaxCapacity { get; set; }
    public string? Difficulty { get; set; }
    public string? ThumbnailUrl { get; set; }
    public double? AverageRating { get; set; }
    public int ReviewCount { get; set; }
}
 
/// <summary>Dùng cho: get_tour_images</summary>
public class TourImageResult
{
    public string ImageUrl { get; set; } = string.Empty;
}
 
/// <summary>Dùng cho: create_tour, update_tour</summary>
public class TourBasicResult
{
    public Guid Id { get; set; }
    public Guid DestinationId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public int DurationDays { get; set; }
    public int MaxCapacity { get; set; }
    public string? Difficulty { get; set; }
    public string? ThumbnailUrl { get; set; }
    public double? AverageRating { get; set; }
    public int ReviewCount { get; set; }
}