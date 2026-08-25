using TrevalApp.DTOs.Common;
using TrevalApp.DTOs.Post;
namespace TrevalApp.Interfaces.Services;

public interface IPostService
{
    Task<PagedResultDto<PostDto>> GetFeedAsync(PostQueryDto query);
    Task<PagedResultDto<PostDto>> GetByUserAsync(Guid userId, PostQueryDto query);
    Task<PostDetailDto> GetByIdAsync(Guid id);
    Task<PostDto> CreateAsync(Guid userId, CreatePostDto dto);
    Task<PostDto> UpdateAsync(Guid id, Guid userId, UpdatePostDto dto);
    Task DeleteAsync(Guid id, Guid userId);
    Task<ToggleLikeResultDto> ToggleLikeAsync(Guid postId, Guid userId);
    Task<PostMediaDto> AddMediaAsync(Guid postId, AddPostMediaDto dto);
    Task DeleteMediaAsync(Guid mediaId);
    Task<IEnumerable<PostCommentDto>> GetCommentsAsync(Guid postId);
    Task<PostCommentDto> AddCommentAsync(Guid postId, Guid userId, CreatePostCommentDto dto);
    Task DeleteCommentAsync(Guid commentId, Guid userId);
}