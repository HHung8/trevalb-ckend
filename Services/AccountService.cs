using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using TravelApp.Infrastructure.Data.DTOs.Profile;
using TravelApp.Infrastructure.Data.Exceptions;
using TrevalApp.Interfaces.Services;
using TrevalApp.Models;

namespace TravelApp.Infrastructure.Data.Services;

public class AccountService : IAccountService
{
    private readonly AppDbContext _context;
    private readonly PasswordHasher<object> _passwordHasher = new();

    public AccountService(AppDbContext context) => _context = context;

    public async Task<ProfileDto> UpdateProfileAsync(Guid userId, UpdateProfileDto dto)
    {
        const string sql = @"SELECT * FROM update_user_profile({0},{1},{2},{3})";
        UpdateProfileResult result;
        try
        {
            result = await _context.Database
                .SqlQueryRaw<UpdateProfileResult>(sql,
                    userId,
                    (object?)dto.FullName ?? DBNull.Value,
                    (object?)dto.Phone ?? DBNull.Value,
                    (object?)dto.AvatarUrl ?? DBNull.Value)
                .FirstAsync();
        }
        catch (PostgresException ex) when (ex.MessageText == "USER_NOT_FOUND")
        {
            throw new NotFoundException("User", userId);
        }

        return new ProfileDto(result.Id, result.FullName, result.Email, result.PhoneNumber, result.Role, result.AvatarUrl);
    }

    public async Task ChangePasswordAsync(Guid userId, ChangePasswordDto dto)
    {
        const string getHashSql = @"SELECT * FROM get_user_password_hash({0})";
        var current = await _context.Database
            .SqlQueryRaw<PasswordHashResult>(getHashSql, userId)
            .FirstOrDefaultAsync();

        if (current is null)
        {
            throw new NotFoundException("User", userId);
        }

        var verifyResult = _passwordHasher.VerifyHashedPassword(new object(), current.PasswordHash, dto.CurrentPassword);
        if (verifyResult == PasswordVerificationResult.Failed)
        {
            throw new BadRequestException("Mật khẩu hiện tại không đúng.");
        }

        var newHash = _passwordHasher.HashPassword(new object(), dto.NewPassword);

        const string updateSql = @"SELECT update_user_password({0},{1})";
        await _context.Database.ExecuteSqlRawAsync(updateSql, userId, newHash);
    }

    public async Task DeleteAccountAsync(Guid userId)
    {
        const string sql = @"SELECT soft_delete_user({0})";
        try
        {
            await _context.Database.ExecuteSqlRawAsync(sql, userId);
        }
        catch (PostgresException ex) when (ex.MessageText == "USER_NOT_FOUND")
        {
            throw new NotFoundException("User", userId);
        }
    }

    public async Task<NotificationPreferencesDto> GetNotificationPreferencesAsync(Guid userId)
    {
        const string sql = @"SELECT * FROM get_notification_preferences({0})";
        var result = await _context.Database
            .SqlQueryRaw<NotificationPreferencesResult>(sql, userId)
            .FirstAsync();
        return new NotificationPreferencesDto(result.PushEnabled, result.EmailEnabled);
    }

    public async Task SetNotificationPreferencesAsync(Guid userId, NotificationPreferencesDto dto)
    {
        const string sql = @"SELECT set_notification_preferences({0},{1},{2})";
        await _context.Database.ExecuteSqlRawAsync(sql, userId, dto.PushEnabled, dto.EmailEnabled);
    }
}

// class riêng cho get_user_password_hash — đã tồn tại từ Auth, dùng lại nếu đã có sẵn
public class PasswordHashResult
{
    public Guid Id { get; set; }
    public string PasswordHash { get; set; } = null!;
}