namespace TrevalApp.Models;

public class UpdateProfileResult
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string Role { get; set; } = null!;
    public string? AvatarUrl { get; set; }
}

public class NotificationPreferencesResult
{
    public bool PushEnabled { get; set; }
    public bool EmailEnabled { get; set; }
}