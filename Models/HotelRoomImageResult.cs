namespace TrevalApp.Models;

public class HotelRoomImageResult
{
    public Guid Id { get; set; }
    public Guid RoomId { get; set; }
    public string ImageUrl { get; set; } = default;
    public int DisplayOrder { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}