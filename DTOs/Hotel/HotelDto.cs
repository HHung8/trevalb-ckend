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