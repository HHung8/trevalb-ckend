namespace TrevalApp.Models;

public class HotelSearchResult
{
    public Guid Id { get; set; }
    public Guid DestinationId { get; set; }
    public string DestinationName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public int StarRating { get; set; }
    public string? Description { get; set; }
    public string? ThumbnailUrl { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double? AverageRating { get; set; }
    public int ReviewCount { get; set; }
    public decimal? MinRoomPrice { get; set; }
    public long TotalCount { get; set; }
}

public class HotelDetailResult
{
    public Guid Id { get; set; }
    public Guid DestinationId { get; set; }
    public string DestinationName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public int StarRating { get; set; }
    public string? Description { get; set; }
    public string? ThumbnailUrl { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public string? Amenities { get; set; }
    public double? AverageRating { get; set; }
    public int ReviewCount { get; set; }
}

public class HotelRoomResult
{
    public Guid Id { get; set; }
    public string RoomType { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal PricePerNight { get; set; }
    public int Capacity { get; set; }
    public string? Amenities { get; set; }
    public string? ThumbnailUrl { get; set; }
    public int TotalRooms { get; set; }
    public bool IsAvailable { get; set; }
}

public class HotelImageResult
{
    public string ImageUrl { get; set; } = string.Empty;
}

public class HotelBasicResult
{
    public Guid Id { get; set; }
    public Guid DestinationId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public int StarRating { get; set; }
    public string? Description { get; set; }
    public string? ThumbnailUrl { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double? AverageRating { get; set; }
    public int ReviewCount { get; set; }
}























