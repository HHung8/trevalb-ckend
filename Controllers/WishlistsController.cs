using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.Infrastructure.Data.DTOs.Wishlish;
using TravelApp.Infrastructure.Data.Helpers;
using TrevalApp.DTOs.APIRESPONSE;
using TrevalApp.Interfaces.Services;

namespace TravelApp.Infrastructure.Data.Controllers;

[ApiController]
[Route("api/wishlists")]
[Authorize]
public class WishlistsController : ControllerBase
{
    private readonly IWishlistService _wishlistService;
    public WishlistsController (IWishlistService wishlistService) => _wishlistService = wishlistService;

    [HttpPost("toggle")]
    public async Task<IActionResult> Toggle([FromBody] ToggleWishlistDto dto)
    {
        var result = await _wishlistService.ToggleAsync(User.GetUserId(), dto);
        var message = result.IsWishlisted ? "Đã thêm vào yêu thích" : "Đã bỏ khỏi yêu thích.";
        return Ok(ApiResponse<ToggleWishlistResultDto>.Ok(result, message));
    }

    [HttpGet("check")]
    public async Task<IActionResult> IsWishlisted([FromQuery] string itemType, [FromQuery] Guid itemId)
    {
        var result = await _wishlistService.IsWishlistedAsync(User.GetUserId(), itemType, itemId);
        return Ok(ApiResponse<object>.Ok(new {isWishlisted = result}));
    }

    [HttpGet("tours")]
    public async Task<IActionResult> GetTours([FromQuery] WishlistQueryDto query)
    {
        var result = await _wishlistService.GetToursAsync(User.GetUserId(), query);
        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpGet("hotels")]
    public async Task<IActionResult> GetHotels([FromQuery] WishlistQueryDto query)
    {
        var result = await _wishlistService.GetHotelsAsync(User.GetUserId(), query);
        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Remove(Guid id)
    {
        await _wishlistService.RemoveAsync(id, User.GetUserId());
        return Ok(ApiResponse<object>.Ok(new { }, "Đã xoá khỏi danh sách yêu thích."));
    }
}