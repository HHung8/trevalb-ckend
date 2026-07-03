using TrevalApp.DTOs.Common;
namespace TrevalApp.Interfaces.Services;

public interface INotificationService
{
    Task<IEnumerable<NotificationDto>> GetByUserAsync(Guid userId);
    Task MarkAsReadAsync(Guid notificationId, Guid userId);
    Task MarkAllAsReadAsync(Guid userId);
    Task SendAsync(Guid userId, string type, string title, string message, string? actionUrl = null);
}