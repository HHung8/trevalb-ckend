namespace TrevalApp.DTOs.Hotel;

public record CreateHotelDto(
    Guid DestinationId, 
    string Name, 
    string Address, 
    int StarRating,
    string? Description, 
    double Latitude, 
    double Longitude,
    string? Phone, 
    string? Email, 
    string? Website, 
    string? Amenities
);