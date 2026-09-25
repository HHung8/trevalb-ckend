namespace TravelApp.Infrastructure.Data.DTOs.Profile;

public record UpdateProfileDto(
    string? FullName,
    string? Phone,
    string? AvatarUrl
);

public record ProfileDto(
    Guid Id,
    string FullName,
    string Email,
    string? PhoneNumber,
    string Role,
    string? AvatarUrl
);

public record NotificationPreferencesDto(
    bool PushEnabled,
    bool EmailEnabled
);

public record ChangePasswordDto(
    string CurrentPassword,
    string NewPassword
);