using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.Infrastructure.Data.DTOs.Profile;
using TravelApp.Infrastructure.Data.Helpers;
using TrevalApp.DTOs.APIRESPONSE;
using TrevalApp.Interfaces.Services;

namespace TravelApp.Infrastructure.Data.Controllers;

[ApiController]
[Route("api/account")]
[Authorize]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    public AccountController(IAccountService accountService) => _accountService = accountService;
    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
    {
        var result = await _accountService.UpdateProfileAsync(User.GetUserId(), dto);
        return Ok(ApiResponse<ProfileDto>.Ok(result, "Cập nhật thông tin thành công."));
    }
    
    [HttpPut("password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        await _accountService.ChangePasswordAsync(User.GetUserId(), dto);
        return Ok(ApiResponse<object>.Ok(new { }, "Đổi mật khẩu thành công."));
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAccount()
    {
        await _accountService.DeleteAccountAsync(User.GetUserId());
        return Ok(ApiResponse<object>.Ok(new { }, "Tài khoản đã được xoá"));
    }

    [HttpGet("notification-preferences")]
    public async Task<IActionResult> GetNotificationPreferences()
    {
        var result = await _accountService.GetNotificationPreferencesAsync(User.GetUserId());
        return Ok(ApiResponse<NotificationPreferencesDto>.Ok(result));
    }

    [HttpPut("notification-preferences")]
    public async Task<IActionResult> SetNotificationPreferences([FromBody] NotificationPreferencesDto dto)
    {
        await _accountService.SetNotificationPreferencesAsync(User.GetUserId(), dto);
        return Ok(ApiResponse<object>.Ok(new { }, "Đã cập nhật cài đặt thông "));
    }
}