using TrevalApp.DTOs.Auth;
namespace TrevalApp.Interfaces.Services;

public interface IAuthService
{
     Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
     Task<AuthResponseDto> LoginAsync(LoginDto dto);
     Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);
     Task LogoutAsync(Guid userId);
     Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordDto dto);
}