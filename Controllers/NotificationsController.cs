using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.Infrastructure.Data.DTOs.Notification;
using TravelApp.Infrastructure.Data.Helpers;
using TrevalApp.DTOs.APIRESPONSE;
using TrevalApp.Interfaces.Services;

namespace TravelApp.Infrastructure.Data.Controllers;

[ApiController]
[Route("api/notifications")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;
    public NotificationsController(INotificationService notificationService) => _notificationService = notificationService;

    [HttpGet]
    public async Task<IActionResult> GetMine([FromQuery] NotificationQueryDto query)
    {
        var result = await _notificationService.GetMineAsync(User.GetUserId(), query);
        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpPut("{id:guid}/read")]
    public async Task<IActionResult> MarkAsRead(Guid id)
    {
        await _notificationService.MarkAsReadAsync(id, User.GetUserId());
        return Ok(ApiResponse<object>.Ok(new { }, "Đã đánh dấu đã đọc."));
    }

    [HttpPut("read-all")]
    public async Task<IActionResult> MarkAllAsRead()
    {
        await _notificationService.MarkAllAsReadAsync(User.GetUserId());
        return Ok(ApiResponse<object>.Ok(new { }, "Đã đánh dấu tất cả đã đọc."));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _notificationService.DeleteAsync(id, User.GetUserId());
        return Ok(ApiResponse<object>.Ok(new { }, "Đã xoá thông báo."));
    }
}