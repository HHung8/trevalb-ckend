namespace TrevalApp.Models;

public class TourScheduleResult
{
    public Guid Id { get; set; }
    public Guid TourId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int AvailableSlots { get; set; }
    public decimal? OverridePrice { get; set; }
}