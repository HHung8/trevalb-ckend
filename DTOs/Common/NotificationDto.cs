namespace TrevalApp.DTOs.Common;

public record NotificationDto(
    Guid Id, 
    string Type, 
    string Title, 
    string Message, 
    bool IsRead, 
    string? ActionUrl,
    DateTime CreatedAt
    );