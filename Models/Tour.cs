namespace TrevalApp.Models;

public class Tour : BaseEntity
{
    public Guid DestinationId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Highlights { get; set; }
    public string? Includes { get; set; }
    public string? Excludes { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public int DurationDays { get; set; }
    public int MaxCapacity { get; set; }
    public string Difficulty { get; set; } = "easy"; // easy | medium | hard
    public string? ThumbnailUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public double? AverageRating { get; set; }
    public int ReviewCount { get; set; } = 0;
 
    // Navigation
    public Destination Destination { get; set; } = null!;
    public ICollection<TourBooking> Bookings { get; set; } = new List<TourBooking>();
    public ICollection<TourImage> Images { get; set; } = new List<TourImage>();
    public ICollection<TourSchedule> Schedules { get; set; } = new List<TourSchedule>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}