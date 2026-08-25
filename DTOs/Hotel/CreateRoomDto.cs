namespace TrevalApp.DTOs.Hotel;

public record CreateRoomDto(
    string RoomType,
    string? Description,
    decimal PricePerNight,
    int Capacity,
    int TotalRooms,
    string? Amenities
);

public record UpdateRoomDto(
    string? RoomType,
    string? Description,
    decimal? PricePerNight,
    int? Capacity,
    int? TotalRooms,
    string? Amenities,
    bool? IsAvailable
);