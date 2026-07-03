using TrevalApp.DTOs.Common;
using TrevalApp.DTOs.Post;
namespace TrevalApp.Interfaces.Services;

public interface IPostService
{
    Task<PagedResultDto<PostDto>> GetFeedAsync(int page, int pageSize);
    Task<PagedResultDto<PostDto>> GetByUserAsync(Guid userId, int page, int pageSize);
    Task<PostDetailDto> GetByIdAsync(Guid id);
    Task<PostDto> CreateAsync(Guid userId, CreatePostDto dto);
    Task<PostDto> UpdateAsync(Guid id, Guid userId, UpdatePostDto dto);
    Task DeleteAsync(Guid id, Guid userId);
    Task<bool> ToggleLikeAsync(Guid postId, Guid userId);
}