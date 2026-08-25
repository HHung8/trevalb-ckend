namespace TrevalApp.DTOs.Post;

public record PostDto(
    Guid Id,
    Guid UserId,
    string UserName,
    string? AvatarUrl,
    Guid? DestinationId,
    string? DestinationName,
    string Title,
    string? ThumbnailUrl,
    int LikesCount,
    int CommentsCount,
    DateTimeOffset CreatedAt
);

public record PostDetailDto(
    Guid Id,
    Guid UserId,
    string UserName,
    string? AvatarUrl,
    Guid? DestinationId,
    string? DestinationName,
    string Title,
    string Content,
    string? ThumbnailUrl,
    List<string> Tags,
    int LikesCount,
    int CommentsCount,
    DateTimeOffset CreatedAt,
    IEnumerable<PostMediaDto> Medias
);

public record PostMediaDto(
    Guid Id,
    string MediaUrl,
    string MediaType,
    int DisplayOrder
);

public record CreatePostDto(
    string Title,
    string Content,
    Guid? DestinationId,
    List<string> ?Tags,
    bool? IsPublished
);

public record UpdatePostDto(
    string? Title,
    string? Content,
    Guid? DestinationId,
    List<string>? Tags,
    bool? IsPublished
);

public record AddPostMediaDto(
    string MediaUrl,
    string MediaType,
    int DisplayOrder
);

public record ToggleLikeResultDto(
    bool IsLiked,
    int LikesCount
);

public record PostCommentDto(
    Guid Id,
    Guid PostId,
    Guid UserId,
    string UserName,
    string? AvatarUrl,
    Guid? ParentId,
    string Content,
    DateTimeOffset? CreatedAt
);

public record CreatePostCommentDto(
    string Content,
    Guid? ParentId
);

public record PostQueryDto(
    int Page = 1,
    int PageSize = 10
);


