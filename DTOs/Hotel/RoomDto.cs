namespace TrevalApp.DTOs.Hotel;

public record RoomDto(
    Guid Id, 
    string RoomType, 
    string? Description, 
    decimal PricePerNight,
    int Capacity, 
    string? Amenities, 
    string? ThumbnailUrl, 
    bool IsAvailable);