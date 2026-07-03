namespace TrevalApp.DTOs.Destination;

public record DestinationQueryDto(
    string? Keyword = null,
    string? Country = null,
    bool? IsFeatured = null,
    int Page = 1,
    int PageSize = 10
);