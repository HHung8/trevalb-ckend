namespace TrevalApp.Models;

public class Hotel : BaseEntity
{
    public Guid DestinationId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int StarRating { get; set; }
    public string? Description { get; set; }
    public string? ThumbnailUrl { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public string? Amenities { get; set; } // JSON array
    public double? AverageRating { get; set; }
    public int ReviewCount { get; set; } = 0;
    public bool IsActive { get; set; } = true;
 
    // Navigation
    public Destination Destination { get; set; } = null!;
    public ICollection<Room> Rooms { get; set; } = new List<Room>();
    public ICollection<HotelImage> Images { get; set; } = new List<HotelImage>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
 
public class Room : BaseEntity
{
    public Guid HotelId { get; set; }
    public string RoomType { get; set; } = string.Empty; // single | double | suite | deluxe
    public string? Description { get; set; }
    public decimal PricePerNight { get; set; }
    public int Capacity { get; set; }
    public string? Amenities { get; set; } // JSON array
    public string? ThumbnailUrl { get; set; }
    public int TotalRooms { get; set; } = 1;
    public bool IsAvailable { get; set; } = true;
 
    // Navigation
    public Hotel Hotel { get; set; } = null!;
    public ICollection<HotelBooking> Bookings { get; set; } = new List<HotelBooking>();
    public ICollection<RoomImage> Images { get; set; } = new List<RoomImage>();
}