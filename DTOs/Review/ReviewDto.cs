namespace TrevalApp.DTOs.Review;

public record ReviewDto(
    Guid Id,
    Guid UserId,
    string UserName,
    string? AvatarUrl,
    string TargetType,
    Guid TargetId,
    int Rating,
    string? Comment,
    bool IsVerified,
    DateTimeOffset CreatedAt
    );
    
public record ReviewDetailDto(
    Guid Id,
    Guid UserId,
    string UserName,
    string? AvatarUrl,
    string TargetType,
    Guid TargetId,
    int Rating,
    string? Comment,
    bool IsVerified,
    DateTimeOffset CreatedAt,
    IEnumerable<ReviewImageDto> Images
);

public record MyReviewDto(
    Guid Id,
    string TargetType,
    Guid TargetId,
    string TargetTitle,
    string? ThumbnailUrl,
    int Rating,
    string? Comment,
    bool IsVerified,
    DateTimeOffset CreatedAt
);


public record ReviewImageDto(
    Guid Id,
    string ImageUrl,
    int DisplayOrder
);

public record CreateReviewDto(
    string TargetType,   // "tour" | "hotel"
    Guid TargetId,
    int Rating,
    string? Comment
);

public record UpdateReviewDto(
    int? Rating,
    string? Comment
);

public record AddReviewImageDto(
    string ImageUrl,
    int DisplayOrder
);

public record ReviewQueryDto(
    int Page = 1,
    int PageSize = 10
);