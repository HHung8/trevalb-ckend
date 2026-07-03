namespace TrevalApp.DTOs.Hotel;

public record UpdateHotelDto(
    string? Name, 
    string? Address, 
    int? StarRating, 
    string? Description,
    double? Latitude, 
    double? Longitude, 
    string? Phone, 
    string? Email,
    string? Website, 
    string? Amenities, 
    bool? IsActive);