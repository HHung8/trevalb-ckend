namespace TravelApp.Infrastructure.Data.DTOs.Attraction;

public record AttractionDto (
    Guid Id,
    Guid DestinationId,
    string DestinationName,
    string Name,
    string Category,
    string? Description,
    string? ThumbnailUrl,
    double? Latitude,
    double? Longitude,
    string? OpeningHours,
    decimal? EntryFee,
    string? Website
 );

public record AttractionSimpleDto(
    Guid Id,
    string Name,
    string Category,
    string? Description,
    string? ThumbnailUrl,
    double? Latitude,
    double? Longitude,
    string? OpeningHours,
    decimal? EntryFee,
    string? Website
);

// TrevalApp.DTOs/Attraction/AttractionScheduleDto.cs — thêm vào cuối file
public record CreateAttractionScheduleDto(
    Guid AttractionId,
    DateTime VisitDate,
    int AvailableSlots,
    decimal? OverridePrice
);

public record AttractionScheduleDto(
    Guid Id,
    Guid AttractionId,
    DateTime VisitDate,
    int AvailableSlots,
    decimal? OverridePrice
);


public record CreateAttractionDto(
    Guid DestinationId,
    string Name,
    string Category,
    string? Description,
    string? ThumbnailUrl,
    double? Latitude,
    double? Longitude,
    string? OpeningHours,
    decimal? EntryFee,
    string? Website
);

public record UpdateAttractionDto(
    string? Name,
    string? Category,
    string? Description,
    string? ThumbnailUrl,
    double? Latitude,
    double? Longitude,
    string? OpeningHours,
    decimal? EntryFee,
    string? Website
);

public record AttractionQueryDto(
    string? Keyword = null,
    Guid? DestinationId = null,
    string? Category = null,
    int Page = 1,
    int PageSize = 10
);
        
        
        
        
        
        
        
        
        
        