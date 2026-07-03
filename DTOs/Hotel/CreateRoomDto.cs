namespace TrevalApp.DTOs.Hotel;

public record CreateRoomDto(
    Guid HotelId, 
    string RoomType, 
    string? Description,
    decimal PricePerNight, 
    int Capacity, 
    string? Amenities, 
    int TotalRooms = 1);