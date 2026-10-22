using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using TravelApp.Infrastructure.Data.DTOs.User;
using TravelApp.Infrastructure.Data.Exceptions;
using TrevalApp.DTOs.Auth;
using TrevalApp.Interfaces.Services;
using TrevalApp.Models;
using ChangePasswordDto = TravelApp.Infrastructure.Data.DTOs.User.ChangePasswordDto;

namespace TravelApp.Infrastructure.Data.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;

    public UserService(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    public async Task<UserProfileDto> GetProfileAsync(Guid userId)
    {
        const string sql = @"SELECT * FROM get_user_profile({0})";
        try
        {
            var result = await _context.Database.SqlQueryRaw<UserResult>(sql, userId).FirstAsync();
            return MapToProfile(result);
        }
        catch (PostgresException ex) when (ex.MessageText == "USER_NOT_FOUND")
        {
            throw new NotFoundException("User", userId);
        }
    }

    public async Task<UserProfileDto> UpdateProfileAsync(Guid userId, UpdateUserDto dto)
    {
        const string sql = @"SELECT * FROM update_user_profile({0}, {1}, {2}, {3})";
        try
        {
            var result = await _context.Database
                .SqlQueryRaw<UserResult>(sql, userId,
                    (object?)dto.FullName ?? DBNull.Value,
                    (object?)dto.PhoneNumber ?? DBNull.Value,
                    DBNull.Value)
                .FirstAsync();
            return MapToProfile(result);
        }
        catch (PostgresException ex) when (ex.MessageText == "USER_NOT_FOUND")
        {
            throw new NotFoundException("User", userId);
        }
    }

    public async Task<string> UpdateAvatarAsync(Guid userId, IFormFile file)
    {
        var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (file.Length == 0 || file.Length > 2 * 1024 * 1024)
            throw new BadRequestException("Ảnh phải nhỏ hơn 2mb");
        if(!allowed.Contains(ext))
            throw new BadRequestException("Chỉ hỗ trợ ảnh .jpg, .jpeg, .png, .webp.");
        
        var root = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
        var dir = Path.Combine(root, "uploads", "avatars");
        Directory.CreateDirectory(dir);

        var fileName = $"{userId:N}_{Guid.NewGuid():N}{ext}";
        await using (var fs = File.Create(Path.Combine(dir, fileName)))
            await file.CopyToAsync(fs);
        var url = $"/uploads/avatars/{fileName}";
        const string sql = @"SELECT * FROM update_user_profile({0}, {1}, {2}, {3})";
        try
        {
            await _context.Database.SqlQueryRaw<UserResult>(sql, userId, DBNull.Value, DBNull.Value, url).FirstAsync();
        }
        catch (PostgresException ex) when (ex.MessageText == "USER_NOT_FOUND")
        {
            throw new NotFoundException("User", userId);
        }

        return url;
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
        return await _context.Database.SqlQueryRaw<NotificationPreferencesDto>(sql, userId).FirstAsync();
    }

    public async Task SetNotificationPreferencesAsync(Guid userId, NotificationPreferencesDto dto)
    {
        const string sql = @"SELECT set_notification_preferences({0},{1}, {2})";
        await _context.Database.ExecuteSqlRawAsync(sql, userId, dto.PushEnabled, dto.EmailEnabled);
    }

    public async Task<PagedUsersDto> GetAllAsync(UserQueryDto query)
    {
        var page = Math.Max(query.Page, 1);
        var pageSize = Math.Clamp(query.PageSize, 1, 100);
        const string sql = @"SELECT * FROM search_users({0}::varchar, {1}::varchar, {2}::boolean, {3}, {4})";
        var rows = await _context.Database
            .SqlQueryRaw<UserListResult>(sql,
                (object?)query.Keyword ?? DBNull.Value,
                (object?)query.Role ?? DBNull.Value,
                (object?)query.IsActive ?? DBNull.Value,
                page, pageSize)
            .ToListAsync();
 
        return new PagedUsersDto
        {
            Items = rows.Cast<UserListItemDto>().ToList(),
            TotalCount = rows.FirstOrDefault()?.TotalCount ?? 0,
            Page = page,
            PageSize = pageSize
        };
    }

    private static UserProfileDto MapToProfile(UserResult r) =>
        new(r.Id, r.FullName, r.Email, r.PhoneNumber, r.AvatarUrl, r.Role);
}

