using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.Infrastructure.Data.Helpers;
using TrevalApp.DTOs.APIRESPONSE;
using TrevalApp.DTOs.Review;
using TrevalApp.Interfaces.Services;

namespace TravelApp.Infrastructure.Data.Controllers;

[ApiController]
[Route("api/reviews")]
public class ReviewsController : ControllerBase
{
    private readonly IReviewService _reviewService;
    public ReviewsController(IReviewService reviewService) => _reviewService = reviewService;
    
    [HttpGet("{targetType}/{targetId:guid}")]
    public async Task<IActionResult> GetByTarget(string targetType, Guid targetId, [FromQuery] ReviewQueryDto query)
    {
        var result = await _reviewService.GetByTargetAsync(targetType, targetId, query);
        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpGet("detail/{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _reviewService.GetByIdAsync(id);
        return Ok(ApiResponse<ReviewDetailDto>.Ok(result));
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReviewDto dto)
    {
        var result = await _reviewService.CreateAsync(User.GetUserId(), dto);
        return Ok(ApiResponse<ReviewDto>.Ok(result, "Đánh giá thành công."));
    }

    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateReviewDto dto)
    {
        var result = await _reviewService.UpdateAsync(id, User.GetUserId(), dto);
        return Ok(ApiResponse<ReviewDto>.Ok(result, "Cập nhật đánh giá thành công."));
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _reviewService.DeleteAsync(id, User.GetUserId());
        return Ok(ApiResponse<object>.Ok(new { }, "Xóa đánh giá thành công."));
    }

    [Authorize]
    [HttpPost("{id:guid}/images")]
    public async Task<IActionResult> AddImage(Guid id, [FromBody] AddReviewImageDto dto)
    {
        var result = await _reviewService.AddImageAsync(id, dto);
        return Ok(ApiResponse<ReviewImageDto>.Ok(result, "Thêm ảnh đánh giá thành công."));
    }

    [Authorize]
    [HttpDelete("images/{imageId:guid}")]
    public async Task<IActionResult> DeleteImage(Guid imageId)
    {
        await _reviewService.DeleteImageAsync(imageId);
        return Ok(ApiResponse<object>.Ok(new { }, "Xóa ảnh đánh giá thành công."));
    }
}