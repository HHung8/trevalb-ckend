using TravelApp.Infrastructure.Data.DTOs.Notification;
using TrevalApp.DTOs.Common;
namespace TrevalApp.Interfaces.Services;

public interface INotificationService
{
    Task<NotificationListDto> GetMineAsync(Guid userId, NotificationQueryDto query);
    Task MarkAsReadAsync(Guid id, Guid userId);
    Task MarkAllAsReadAsync(Guid userId);
    Task DeleteAsync(Guid id, Guid userId);
    Task CreateAsync(Guid userId, string type, string title, string message, string? actionUrl = null);
}