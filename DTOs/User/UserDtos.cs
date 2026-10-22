using System.ComponentModel.DataAnnotations;

namespace TravelApp.Infrastructure.Data.DTOs.User;

public class UserDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string? PhoneNumber { get; set; }
    public string Role { get; set; } = "";
    public string? AvatarUrl { get; set; }
    public DateTime? CreatedAt { get; set; }
}


public class UpdateUserDto
{
    [StringLength(100, MinimumLength = 2)]
    public string FullName { get; set; }
    [Phone, StringLength(20)]
    public string PhoneNumber { get; set; }
}

public class NotificationPreferenceDto
{
    public bool PushEnabled { get; set; }
    public bool EmailEnabled { get; set; }
}

public class UserQueryDto
{
    public string? Keyword { get; set; }
    public string? Role { get; set; }
    public bool? IsActive { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
 
public class UserListItemDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string? PhoneNumber { get; set; }
    public string Role { get; set; } = "";
    public string? AvatarUrl { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
 
// Dùng nội bộ để đọc kết quả search_users (có thêm TotalCount)
public class UserListResult : UserListItemDto
{
    public long TotalCount { get; set; }
}
 
public class PagedUsersDto
{
    public List<UserListItemDto> Items { get; set; } = new();
    public long TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}

public class ChangePasswordDto
{
    [Required] public string CurrentPassword { get; set; } = "";
 
    [Required, MinLength(6)]
    public string NewPassword { get; set; } = "";
}
 
public class NotificationPreferencesDto
{
    public bool PushEnabled { get; set; }
    public bool EmailEnabled { get; set; }
}
 
// Dùng nội bộ để đọc kết quả get_user_password_hash
public class PasswordRow
{
    public Guid Id { get; set; }
    public string? PasswordHash { get; set; }
}