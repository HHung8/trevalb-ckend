namespace TrevalApp.Models;

public class BookingPublicInfoResult
{
    public string BookingCode { get; set; } = null!;
    public string TourTitle { get; set; } = null!;
    public string? ThumbnailUrl { get; set; }
    public DateTime TravelDate { get; set; }
    public int NumGuests { get; set; }
    public string Status { get; set; } = null!;
}