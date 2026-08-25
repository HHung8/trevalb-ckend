namespace TrevalApp.Models;

public class AttractionSearchResult
{
    public Guid Id { get; set; }
    public Guid DestinationId { get; set; }
    public string DestinationName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Category { get; set; } = null!;
    public string? Description { get; set; }
    public string? ThumbnailUrl { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? OpeningHours { get; set; }
    public decimal? EntryFee { get; set; }
    public string? Website { get; set; }
    public long TotalCount { get; set; }
}

public class AttractionDetailResult
{
    public Guid Id { get; set; }
    public Guid DestinationId { get; set; }
    public string DestinationName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Category { get; set; } = null!;
    public string? Description { get; set; }
    public string? ThumbnailUrl { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? OpeningHours { get; set; }
    public decimal? EntryFee { get; set; }
    public string? Website { get; set; }
}

public class AttractionByDestinationResult
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Category { get; set; } = null!;
    public string? Description { get; set; }
    public string? ThumbnailUrl { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? OpeningHours { get; set; }
    public decimal? EntryFee { get; set; }
    public string? Website { get; set; }
}

public class AttractionBasicResult
{
    public Guid Id { get; set; }
    public Guid DestinationId { get; set; }
    public string Name { get; set; } = null!;
    public string Category { get; set; } = null!;
    public string? Description { get; set; }
    public string? ThumbnailUrl { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? OpeningHours { get; set; }
    public decimal? EntryFee { get; set; }   
    public string? Website { get; set; }
}