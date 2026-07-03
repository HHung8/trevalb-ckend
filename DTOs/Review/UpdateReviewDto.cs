namespace TrevalApp.DTOs.Review;

public record UpdateReviewDto(
    int? Rating,
    string? Comment
);