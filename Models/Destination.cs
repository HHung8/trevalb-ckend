namespace TrevalApp.Models;

public class Destination : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ThumbnailUrl { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? Climate { get; set; }
    public string? BestTimeToVisit { get; set; }
    public bool IsFeatured { get; set; } = false;
 
    // Navigation
    public ICollection<Tour> Tours { get; set; } = new List<Tour>();
    public ICollection<Hotel> Hotels { get; set; } = new List<Hotel>();
    public ICollection<Attraction> Attractions { get; set; } = new List<Attraction>();
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<DestinationImage> Images { get; set; } = new List<DestinationImage>();
}
