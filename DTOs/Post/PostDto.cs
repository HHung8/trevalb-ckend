namespace TrevalApp.DTOs.Post;

public record PostDto(
    Guid Id, 
    Guid UserId, 
    string UserName, 
    string? UserAvatar, 
    string Title, 
    string? ThumbnailUrl, 
    Guid? DestinationId, 
    string? DestinationName, 
    int LikesCount, 
    int CommentsCount, 
    DateTime CreatedAt);