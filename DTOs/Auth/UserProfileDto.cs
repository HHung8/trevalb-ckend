namespace TrevalApp.DTOs.Auth;

public record UserProfileDto(
    Guid Id,
    string FullName,
    string Email,
    string? Phone,
    string? AvatarUrl,
    string Role
    );