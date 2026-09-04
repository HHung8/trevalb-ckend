namespace TrevalApp.DTOs.Hotel;

public record HotelDto(
    Guid Id, 
    string Name, 
    string Address, 
    int StarRating,
    string? Description, 
    string? ThumbnailUrl, 
    double Latitude, 
    double Longitude,
    double? AverageRating, 
    int ReviewCount, 
    Guid DestinationId, 
    string DestinationName,
    decimal? MinRoomPrice
    );
    
    public record RoomImageDto(
        Guid Id,
        Guid RoomId,
        string ImageUrl,
        int DisplayOrder,
        DateTime CreatedAt,
        DateTime UpdatedAt
        );

    public class CreateRoomImageDto
    {
        public string ImageUrl { get; set; } = default!;
        public int DisplayOrder { get; set; } = 0;
    }

    public class UpdateRoomImageOrderDto
    {
        public int DisplayOrder { get; set; } 
    }