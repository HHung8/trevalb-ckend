namespace TrevalApp.Models;

public class PostFeedResult
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = null!;
    public string? AvatarUrl { get; set; }
    public Guid? DestinationId { get; set; }
    public string? DestinationName { get; set; }
    public string Title { get; set; } = null!;
    public string? ThumbnailUrl { get; set; }
    public int LikesCount { get; set; }
    public int CommentsCount { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public long TotalCount { get; set; }
}

public class PostDetailResult
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = null!;
    public string? AvatarUrl { get; set; }
    public Guid? DestinationId { get; set; }
    public string? DestinationName { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string? ThumbnailUrl { get; set; }
    public string? Tags { get; set; } // JSON text
    public int LikesCount { get; set; }
    public int CommentsCount { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public class PostMediaResult
{
    public Guid Id { get; set; }
    public string MediaUrl { get; set; } = null!;
    public string MediaType { get; set; } = null!;
    public int DisplayOrder { get; set; }
}

public class PostBasicResult
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = null!;
    public string? ThumbnailUrl { get; set; }
    public Guid? DestinationId { get; set; }
    public int LikesCount { get; set; }
    public int CommentsCount { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public class ToggleLikeResult
{
    public bool IsLiked { get; set; }
    public int LikesCount { get; set; }
}

public class PostCommentResult
{
    public Guid Id { get; set; }
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }
    public string? UserName { get; set; }
    public string? AvatarUrl { get; set; }
    public Guid? ParentId { get; set; }
    public string Content { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
}