using TravelApp.Infrastructure.Data.DTOs.Profile;
namespace TrevalApp.Interfaces.Services;

public interface IAccountService
{
    Task<ProfileDto> UpdateProfileAsync(Guid userId, UpdateProfileDto dto);
    Task ChangePasswordAsync(Guid userId, ChangePasswordDto dto);
    Task DeleteAccountAsync(Guid userId);
    Task<NotificationPreferencesDto> GetNotificationPreferencesAsync(Guid userId);
    Task SetNotificationPreferencesAsync(Guid userId, NotificationPreferencesDto dto);
}

