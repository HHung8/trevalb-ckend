using TravelApp.Infrastructure.Data.DTOs.User;
using TrevalApp.DTOs.Auth;
using ChangePasswordDto = TravelApp.Infrastructure.Data.DTOs.User.ChangePasswordDto;

namespace TrevalApp.Interfaces.Services;

public interface IUserService
{
    Task<UserProfileDto> GetProfileAsync(Guid userId);
    Task<UserProfileDto> UpdateProfileAsync(Guid userId, UpdateUserDto dto);
    Task<string> UpdateAvatarAsync(Guid userId, IFormFile file);
    Task DeleteAccountAsync(Guid userId);
    Task<PagedUsersDto> GetAllAsync(UserQueryDto query);
    Task<NotificationPreferencesDto> GetNotificationPreferencesAsync(Guid userId);
    Task SetNotificationPreferencesAsync(Guid userId, NotificationPreferencesDto dto);
}