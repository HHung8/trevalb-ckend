using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.Infrastructure.Data.Helpers;
using TrevalApp.DTOs.APIRESPONSE;
using TrevalApp.DTOs.Post;
using TrevalApp.Interfaces.Services;

namespace TravelApp.Infrastructure.Data.Controllers;

[ApiController]
[Route("api/posts")]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;
    public PostsController(IPostService postService) => _postService = postService;
    
    [HttpGet]
    public async Task<IActionResult> GetFeed([FromQuery] PostQueryDto query)
    {
        var result = await _postService.GetFeedAsync(query);
        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpGet("users/{userId:guid}")]
    public async Task<IActionResult> GetByUser(Guid userId, [FromQuery] PostQueryDto query)
    {
        var result = await _postService.GetByUserAsync(userId, query);
        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _postService.GetByIdAsync(id);
        return Ok(ApiResponse<PostDetailDto>.Ok(result));
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePostDto dto)
    {
        var result = await _postService.CreateAsync(User.GetUserId(), dto);
        return Ok(ApiResponse<PostDto>.Ok(result, "Tạo bài viết thành công."));
    }
    
    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePostDto dto)
    {
        var result = await _postService.UpdateAsync(id, User.GetUserId(), dto);
        return Ok(ApiResponse<PostDto>.Ok(result, "Cập nhật bài viết thành công."));
    }
    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _postService.DeleteAsync(id, User.GetUserId());
        return Ok(ApiResponse<object>.Ok(new { }, "Xóa bài viết thành công."));
    }

    [Authorize]
    [HttpPost("{id:guid}/like")]
    public async Task<IActionResult> ToggleLike(Guid id)
    {
        var result = await _postService.ToggleLikeAsync(id, User.GetUserId());
        var message = result.IsLiked ? "Đã thích bài viết." : "Đã bỏ thích bài viết.";
        return Ok(ApiResponse<ToggleLikeResultDto>.Ok(result, message));
    }

    [Authorize]
    [HttpPost("{id:guid}/media")]
    public async Task<IActionResult> AddMedia(Guid id, [FromBody] AddPostMediaDto dto)
    {
        var result = await _postService.AddMediaAsync(id, dto);
        return Ok(ApiResponse<PostMediaDto>.Ok(result, "Thêm ảnh thành công."));
    }
    
    [Authorize]
    [HttpDelete("media/{mediaId:guid}")]
    public async Task<IActionResult> DeleteMedia(Guid mediaId)
    {
        await _postService.DeleteMediaAsync(mediaId);
        return Ok(ApiResponse<object>.Ok(new { }, "Xóa ảnh thành công."));
    }

    [HttpGet("{id:guid}/comments")]
    public async Task<IActionResult> GetComments(Guid id)
    {
        var result = await _postService.GetCommentsAsync(id);
        return Ok(ApiResponse<IEnumerable<PostCommentDto>>.Ok(result));
    }

    [Authorize]
    [HttpPost("{id:guid}/comments")]
    public async Task<IActionResult> AddComment(Guid id, [FromBody] CreatePostCommentDto dto)
    {
        var result = await _postService.AddCommentAsync(id, User.GetUserId(), dto);
        return Ok(ApiResponse<PostCommentDto>.Ok(result, "Bình luận thành công."));
    }
    
    [Authorize]
    [HttpDelete("comments/{commentId:guid}")]
    public async Task<IActionResult> DeleteComment(Guid commentId)
    {
        await _postService.DeleteCommentAsync(commentId, User.GetUserId());
        return Ok(ApiResponse<object>.Ok(new { }, "Xóa bình luận thành công."));
    }
    
}