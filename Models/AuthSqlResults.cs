namespace TrevalApp.Models;

/// <summary>Dùng cho: register_user, get_user_by_refresh_token</summary>
public class UserResult
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string Role { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
}

/// <summary>Dùng cho: get_user_for_login (cần thêm PasswordHash + IsActive để verify)</summary>
public class UserLoginResult
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string Role { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

/// <summary>Dùng cho: get_user_password_hash (bước 1 của đổi mật khẩu)</summary>
public class PasswordHashResult
{
    public Guid Id { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
}