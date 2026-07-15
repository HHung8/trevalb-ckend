namespace TrevalApp.Models;

public class TourBookingResult
{
    public Guid Id { get; set; }
    public string BookingCode { get; set; } = string.Empty;
    public Guid TourId { get; set; }
    public string TourTitle { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public int NumGuests { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime TravelDate { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class HotelBookingResult
{
    public Guid Id { get; set; }
    public string BookingCode { get; set; } = string.Empty;
    public Guid RoomId { get; set; }
    public string RoomType { get; set; } = string.Empty;
    public string HotelName { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public int NumGuests { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}