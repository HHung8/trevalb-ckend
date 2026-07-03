namespace TrevalApp.DTOs.Hotel;

public record HotelDetailDto(
    Guid Id, 
    string Name, 
    string Address, 
    int StarRating, 
    string? Description,
    string? ThumbnailUrl, 
    double Latitude, 
    double Longitude, 
    string? Phone, 
    string? Email,
    string? Website, 
    string? Amenities, 
    double? AverageRating, 
    int ReviewCount,
    Guid DestinationId, 
    string DestinationName, 
    IEnumerable<string> Images,
    IEnumerable<RoomDto> Rooms);