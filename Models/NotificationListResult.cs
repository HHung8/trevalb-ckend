namespace TrevalApp.Models;

public class NotificationListResult
{
    public Guid Id { get; set; }
    public string Type { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Message { get; set; } = null!;
    public bool IsRead { get; set; }
    public string? ActionUrl { get; set; }
    public string? Metadata { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public long TotalCount { get; set; }
    public long UnreadCount { get; set; }
}

public class CreateNotificationResult
{
    public Guid Id { get; set; }
}