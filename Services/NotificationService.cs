using Microsoft.EntityFrameworkCore;
using Npgsql;
using TravelApp.Infrastructure.Data.DTOs.Notification;
using TravelApp.Infrastructure.Data.Exceptions;
using TrevalApp.Interfaces.Services;
using TrevalApp.Models;

namespace TravelApp.Infrastructure.Data.Services;

public class NotificationService : INotificationService
{
    private readonly AppDbContext _context;
    public NotificationService(AppDbContext context) => _context = context;
    public async Task<NotificationListDto> GetMineAsync(Guid userId, NotificationQueryDto query)
    {
        const string sql = @"SELECT * FROM get_user_notifications({0}, {1}, {2})";
        var rows = await _context.Database.SqlQueryRaw<NotificationListResult>(sql, userId, query.Page, query.PageSize)
            .ToListAsync();
        var total = rows.FirstOrDefault()?.TotalCount ?? 0;
        var unread = rows.FirstOrDefault()?.UnreadCount ?? 0;
        var dtos = rows.Select(r => new NotificationDto(
            r.Id, r.Type, r.Title, r.Message, r.IsRead, r.ActionUrl, r.Metadata, r.CreatedAt
        ));
        return new NotificationListDto(dtos, (int)total, unread, query.Page, query.PageSize, (int)Math.Ceiling(total/(double)query.PageSize));
    }

    public async Task MarkAsReadAsync(Guid id, Guid userId)
    {
        const string sql = @"SELECT mark_notification_read({0},{1})";
        try
        {
            await _context.Database.ExecuteSqlRawAsync(sql, id, userId);
        }
        catch (PostgresException ex) when (ex.MessageText == "NOTIFICATION_NOT_FOUND")
        {
            throw new NotFoundException("Notification", id);
        }
    }

    public async Task MarkAllAsReadAsync(Guid userId)
    {
        const string sql = @"SELECT mark_all_notifications_read({0})";
        await _context.Database.ExecuteSqlRawAsync(sql, userId);
    }


    public async Task DeleteAsync(Guid id, Guid userId)
    {
        const string sql = @"SELECT delete_notification({0},{1})";
        try
        {
            await _context.Database.ExecuteSqlRawAsync(sql, id, userId);
        }
        catch (PostgresException ex) when (ex.MessageText == "NOTIFICATION_NOT_FOUND")
        {
            throw new NotFoundException("Notification", id);
        }
    }

    public async Task CreateAsync(Guid userId, string type, string title, string message, string? actionUrl = null)
    {
        const string sql = @"SELECT * FROM create_notification({0},{1},{2},{3},{4},{5})";
        await _context.Database
            .SqlQueryRaw<CreateNotificationResult>(sql,
                userId, type, title, message,
                (object?)actionUrl ?? DBNull.Value,
                DBNull.Value) // metadata — chưa dùng, để NULL
            .FirstAsync();
    }
}

