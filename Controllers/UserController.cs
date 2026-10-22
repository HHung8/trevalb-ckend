using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelApp.Infrastructure.Data.DTOs.User;
using TravelApp.Infrastructure.Data.Helpers;
using TrevalApp.DTOs.APIRESPONSE;
using TrevalApp.DTOs.Auth;
using TrevalApp.Interfaces.Services;

namespace TravelApp.Infrastructure.Data.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService) => _userService = userService;

    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var result = await _userService.GetProfileAsync(User.GetUserId());
        return Ok(ApiResponse<UserProfileDto>.Ok(result, "Success"));
    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateMe([FromBody] UpdateUserDto dto)
    {
        var result = await _userService.UpdateProfileAsync(User.GetUserId(), dto);
        return Ok(ApiResponse<UserProfileDto>.Ok(result, "Update Successfully"));
    }

    [HttpPost("me/avatar")]
    public async Task<IActionResult> UploadAvatar(IFormFile file)
    {
        var url = await _userService.UpdateAvatarAsync(User.GetUserId(), file);
        return Ok(ApiResponse<object>.Ok(new { avatarUrl = url }, "Upload Successfully"));
    }
    [HttpGet("me/notification-preferences")]
    public async Task<IActionResult> GetNotificationPreferences()
    {
        var result = await _userService.GetNotificationPreferencesAsync(User.GetUserId());
        return Ok(ApiResponse<NotificationPreferencesDto>.Ok(result, "Success"));
    }
 
    [HttpPut("me/notification-preferences")]
    public async Task<IActionResult> SetNotificationPreferences([FromBody] NotificationPreferencesDto dto)
    {
        await _userService.SetNotificationPreferencesAsync(User.GetUserId(), dto);
        return Ok(ApiResponse<NotificationPreferencesDto>.Ok(dto, "Update Successfully"));
    }

    [Authorize(Roles = "admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] UserQueryDto query)
    {
        var result = await _userService.GetAllAsync(query);
        return Ok(ApiResponse<PagedUsersDto>.Ok(result, "Success"));
    }
    
 
    [HttpDelete("me")]
    public async Task<IActionResult> DeleteMe()
    {
        await _userService.DeleteAccountAsync(User.GetUserId());
        return Ok(ApiResponse<object>.Ok(new { }, "Account deactivated"));
    }
}