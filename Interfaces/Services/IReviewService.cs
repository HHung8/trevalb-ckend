using TrevalApp.DTOs.Common;
using TrevalApp.DTOs.Review;
namespace TrevalApp.Interfaces.Services;

public interface IReviewService
{
    Task<PagedResultDto<ReviewDto>> GetByTargetAsync(string targetType, Guid targetId, ReviewQueryDto query);
    Task<ReviewDetailDto> GetByIdAsync(Guid id);
    Task<ReviewDto> CreateAsync(Guid userId, CreateReviewDto dto);
    Task<ReviewDto> UpdateAsync(Guid id, Guid userId, UpdateReviewDto dto);
    Task DeleteAsync(Guid id, Guid userId);
    Task<ReviewImageDto> AddImageAsync(Guid reviewId, AddReviewImageDto dto);
    Task DeleteImageAsync(Guid imageId);
}