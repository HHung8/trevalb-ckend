namespace TrevalApp.DTOs.Post;

public record PostDetailDto(
    Guid Id, 
    Guid UserId, 
    string UserName, 
    string? UserAvatar, 
    string Title, 
    string Content, 
    string? ThumbnailUrl, 
    Guid? DestinationId, 
    string? DestinationName, 
    string? Tags, 
    int LikesCount, 
    int CommentsCount, 
    DateTime CreatedAt, 
    IEnumerable<PostMediaDto> Media);