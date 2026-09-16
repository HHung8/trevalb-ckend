namespace TrevalApp.Models;

public class AttractionScheduleResult
{
    public Guid Id { get; set; }
    public Guid AttractionId { get; set; }
    public DateTime VisitDate { get; set; }
    public int AvailableSlots { get; set; }
    public decimal? OverridePrice { get; set; }
}

public class AttractionBookingResult
{
    public Guid Id { get; set; }
    public string BookingCode { get; set; } = default!;
    public Guid AttractionId { get; set; }
    public string AttractionName { get; set; } = default!;
    public string? ThumbnailUrl { get; set; }
    public int NumGuests { get; set; }
    public DateTime VisitDate { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
}