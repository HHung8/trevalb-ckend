namespace TrevalApp.Models;

public class TourBooking : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid TourId { get; set; }
    public Guid? ScheduleId { get; set; }
    public int NumGuests { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = "pending"; // pending | confirmed | cancelled | completed
    public string? SpecialRequest { get; set; }
    public string BookingCode { get; set; } = string.Empty;
    public DateTime TravelDate { get; set; }
 
    // Navigation
    public User User { get; set; } = null!;
    public Tour Tour { get; set; } = null!;
    public TourSchedule? Schedule { get; set; }
    public Payment? Payment { get; set; }
}
 
public class HotelBooking : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid RoomId { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public int NumGuests { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = "pending"; // pending | confirmed | cancelled | completed
    public string? SpecialRequest { get; set; }
    public string BookingCode { get; set; } = string.Empty;
 
    // Navigation
    public User User { get; set; } = null!;
    public Room Room { get; set; } = null!;
    public Payment? Payment { get; set; }
}
 
public class TourSchedule : BaseEntity
{
    public Guid TourId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int AvailableSlots { get; set; }
    public decimal? OverridePrice { get; set; }
    public bool IsActive { get; set; } = true;
 
    // Navigation
    public Tour Tour { get; set; } = null!;
}