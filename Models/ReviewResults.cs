namespace TrevalApp.Models;

public class ReviewListResult
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = null!;
    public string? AvatarUrl { get; set; }
    public string TargetType { get; set; } = null!;
    public Guid TargetId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public bool IsVerified { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public long TotalCount { get; set; }
}

public class ReviewDetailResult
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = null!;
    public string? AvatarUrl { get; set; }
    public string TargetType { get; set; } = null!;
    public Guid TargetId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public bool IsVerified { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public class ReviewBasicResult
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string TargetType { get; set; } = null!;
    public Guid TargetId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public bool IsVerified { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public class MyReviewResult
{
    public Guid Id { get; set; }
    public string TargetType { get; set; } = null!;
    public Guid TargetId { get; set; }
    public string TargetTitle { get; set; } = null!;
    public string? ThumbnailUrl { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public bool IsVerified { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public long TotalCount { get; set; }
    
}

public class ReviewImageResult
{
    public Guid Id { get; set; }
    public string ImageUrl { get; set; } = null!;
    public int DisplayOrder { get; set; }
}

