namespace TrevalApp.Models;

/// <summary>Dùng cho: search_destinations</summary>
public class DestinationSearchResult
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ThumbnailUrl { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool IsFeatured { get; set; }
    public long TourCount { get; set; }
    public long HotelCount { get; set; }
    public long TotalCount { get; set; }
}
 
/// <summary>Dùng cho: get_destination_by_id</summary>
public class DestinationDetailResult
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ThumbnailUrl { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? Climate { get; set; }
    public string? BestTimeToVisit { get; set; }
    public bool IsFeatured { get; set; }
}
 
/// <summary>Dùng cho: get_destination_images</summary>
public class DestinationImageResult
{
    public string ImageUrl { get; set; } = string.Empty;
}
 
/// <summary>Dùng cho: get_featured_destinations</summary>
public class DestinationFeaturedResult
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ThumbnailUrl { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool IsFeatured { get; set; }
    public long TourCount { get; set; }
    public long HotelCount { get; set; }
}
 
/// <summary>Dùng cho: create_destination, update_destination</summary>
public class DestinationBasicResult
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ThumbnailUrl { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool IsFeatured { get; set; }
}