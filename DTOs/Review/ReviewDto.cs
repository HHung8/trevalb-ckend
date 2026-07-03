namespace TrevalApp.DTOs.Review;

public record ReviewDto(
    Guid Id, 
    Guid UserId, 
    string UserName, 
    string? UserAvatar, 
    string TargetType, 
    Guid TargetId, 
    int Rating, 
    string? Comment, 
    bool IsVerified, 
    DateTime CreatedAt, 
    IEnumerable<string> Images);