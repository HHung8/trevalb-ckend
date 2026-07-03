using TrevalApp.DTOs.Common;
using TrevalApp.DTOs.Review;
namespace TrevalApp.Interfaces.Services;

public interface IReviewService
{
    Task<PagedResultDto<ReviewDto>> GetByTargetAsync(string targetType, Guid targetId, int page, int pageSize);
    Task<ReviewDto> CreateAsync(Guid userId, CreateReviewDto dto);
    Task<ReviewDto> UpdateAsync(Guid id, Guid userId, UpdateReviewDto dto);
    Task DeleteAsync(Guid id, Guid userId);
    
}