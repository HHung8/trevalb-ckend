namespace TrevalApp.DTOs.Review;

public record CreateReviewDto(
    string TargetType, 
    Guid TargetId, 
    int Rating,
    string? Comment);