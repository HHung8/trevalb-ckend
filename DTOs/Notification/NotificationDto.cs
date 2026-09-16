namespace TravelApp.Infrastructure.Data.DTOs.Notification;

public record NotificationDto(
    Guid Id,
    string Type,
    string Title,
    string Message,
    bool IsRead,
    string? ActionUrl,
    string? Metadata,
    DateTimeOffset CreatedAt
    );

public record NotificationListDto(
    IEnumerable<NotificationDto> Items,
    int TotalCount,
    long UnreadCount,
    int Page,
    int PageSize,
    int TotalPages
);

public record NotificationQueryDto(
    int Page = 1,
    int PageSize = 20
);
    