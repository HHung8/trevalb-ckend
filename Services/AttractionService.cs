using Microsoft.EntityFrameworkCore;
using Npgsql;
using TravelApp.Infrastructure.Data.DTOs.Attraction;
using TravelApp.Infrastructure.Data.Exceptions;
using TrevalApp.DTOs.Common;
using TrevalApp.Interfaces.Services;
using TrevalApp.Models;

namespace TravelApp.Infrastructure.Data.Services;

public class AttractionService : IAttractionService
{
    private readonly AppDbContext _context;
    public AttractionService(AppDbContext context) => _context = context;


    public async Task<PagedResultDto<AttractionDto>> SearchAsync(AttractionQueryDto query)
    {
        const string sql = @"SELECT * FROM search_attractions({0},{1},{2},{3},{4})";
        var rows = await _context.Database.SqlQueryRaw<AttractionSearchResult>(sql,
            (object?)query.Keyword ?? DBNull.Value,
            (object?)query.DestinationId ?? DBNull.Value,
            (object?)query.Category ?? DBNull.Value,
            query.Page, query.PageSize
        ).ToListAsync();
        var total = rows.FirstOrDefault()?.TotalCount ?? 0;
        var dtos = rows.Select(r => new AttractionDto(
            r.Id, r.DestinationId, r.DestinationName, r.Name, r.Category, r.Description,
            r.ThumbnailUrl, r.Latitude, r.Longitude, r.OpeningHours, r.EntryFee, r.Website));

        return new PagedResultDto<AttractionDto>(dtos, (int)total, query.Page, query.PageSize,
            (int)Math.Ceiling(total / (double)query.PageSize));
    }
    
    public async Task<AttractionDto> GetByIdAsync(Guid id)
    {
        const string sql = @"SELECT * FROM get_attraction_by_id({0})";
        AttractionDetailResult result;
        try
        {
            result = await _context.Database.SqlQueryRaw<AttractionDetailResult>(sql, id).FirstAsync();
        }
        catch (PostgresException ex) when(ex.MessageText == "ATTRACTION_NOT_FOUND")
        {
            throw new NotFoundException("Attraction", id);
        }
        return new AttractionDto(
            result.Id, result.DestinationId, result.DestinationName, result.Name, result.Category,
            result.Description, result.ThumbnailUrl, result.Latitude, result.Longitude,
            result.OpeningHours, result.EntryFee, result.Website);
    }

    public async Task<IEnumerable<AttractionSimpleDto>> GetByDestinationAsync(Guid destinationId)
    {
        const string sql = @"SELECT * FROM get_attractions_by_destination({0})";
        var rows = await _context.Database
            .SqlQueryRaw<AttractionByDestinationResult>(sql, destinationId)
            .ToListAsync();

        return rows.Select(r => new AttractionSimpleDto(
            r.Id, r.Name, r.Category, r.Description, r.ThumbnailUrl,
            r.Latitude, r.Longitude, r.OpeningHours, r.EntryFee, r.Website));
    }

    public async Task<AttractionDto> CreateAsync(CreateAttractionDto dto)
    {
        const string sql = @"SELECT * FROM create_attraction({0},{1},{2},{3},{4},{5},{6},{7},{8},{9})";
        AttractionBasicResult result;
        try
        {
            result = await _context.Database
                .SqlQueryRaw<AttractionBasicResult>(sql,
                    dto.DestinationId, dto.Name, dto.Category,
                    (object?)dto.Description ?? DBNull.Value,
                    (object?)dto.ThumbnailUrl ?? DBNull.Value,
                    (object?)dto.Latitude ?? DBNull.Value,
                    (object?)dto.Longitude ?? DBNull.Value,
                    (object?)dto.OpeningHours ?? DBNull.Value,
                    (object?)dto.EntryFee ?? DBNull.Value,
                    (object?)dto.Website ?? DBNull.Value)
                .FirstAsync();
        }
        catch (PostgresException ex) when (ex.MessageText == "DESTINATION_NOT_FOUND")
        {
            throw new NotFoundException("Destination", dto.DestinationId);
        }

        return MapToDto(result, string.Empty);
    }

    public async Task<AttractionDto> UpdateAsync(Guid id, UpdateAttractionDto dto)
    {
        const string sql = @"SELECT * FROM update_attraction({0},{1},{2},{3},{4},{5},{6},{7},{8},{9})";
        AttractionBasicResult result;
        try
        {
            result = await _context.Database
                .SqlQueryRaw<AttractionBasicResult>(sql,
                    id,
                    (object?)dto.Name ?? DBNull.Value,
                    (object?)dto.Category ?? DBNull.Value,
                    (object?)dto.Description ?? DBNull.Value,
                    (object?)dto.ThumbnailUrl ?? DBNull.Value,
                    (object?)dto.Latitude ?? DBNull.Value,
                    (object?)dto.Longitude ?? DBNull.Value,
                    (object?)dto.OpeningHours ?? DBNull.Value,
                    (object?)dto.EntryFee ?? DBNull.Value,
                    (object?)dto.Website ?? DBNull.Value)
                .FirstAsync();
        }
        catch (PostgresException ex) when (ex.MessageText == "ATTRACTION_NOT_FOUND")
        {
            throw new NotFoundException("Attraction", id);
        }

        return MapToDto(result, string.Empty);
    }

    public async Task<IEnumerable<AttractionScheduleDto>> GetAttractionSchedulesAsync(Guid attractionId)
    {
        const string sql = @"SELECT * FROM get_attraction_schedules({0})";
        var rows = await _context.Database
            .SqlQueryRaw<AttractionScheduleResult>(sql, attractionId).ToListAsync();

        return rows.Select(r => new AttractionScheduleDto(
            r.Id, r.AttractionId, r.VisitDate, r.AvailableSlots, r.OverridePrice));
    }

    public async Task<AttractionScheduleDto> CreateAttractionScheduleAsync(CreateAttractionScheduleDto dto)
    {
        const string sql = @"SELECT * FROM create_attraction_schedule({0}, {1}, {2}, {3})";
        AttractionScheduleResult result;
        try
        {
            result = await _context.Database.SqlQueryRaw<AttractionScheduleResult>(sql, 
                dto.AttractionId,
                dto.VisitDate,
                dto.AvailableSlots, 
                (object?)dto.OverridePrice ?? DBNull.Value)
                .FirstAsync();
        }
        catch (PostgresException ex) when (ex.MessageText == "ATTRACTION_NOT_FOUND")
        { throw new NotFoundException("Attraction", dto.AttractionId); }
        catch (PostgresException ex) when (ex.MessageText == "INVALID_SLOTS")
        { throw new BadRequestException("Số lượng slot không hợp lệ."); }
        return new AttractionScheduleDto(result.Id, result.AttractionId, result.VisitDate, result.AvailableSlots,
            result.OverridePrice);
    }

    public async Task DeleteAttractionScheduleAsync(Guid id)
    {
        const string sql = @"SELECT delete_attraction_schedule({0}";
        try
        {
            await _context.Database.ExecuteSqlRawAsync(sql, id);
        }
        catch (PostgresException ex) when (ex.MessageText == "SCHEDULE_NOT_FOUND")
        { throw new NotFoundException("AttractionSchedule", id); }
    }

    public async Task DeleteAsync(Guid id)
    {
        const string sql = @"SELECT delete_attraction({0})";
        try
        {
            await _context.Database.ExecuteSqlRawAsync(sql, id);
        }
        catch (PostgresException ex) when (ex.MessageText == "ATTRACTION_NOT_FOUND")
        {
            throw new NotFoundException("Attraction", id);
        }

    }
    
    private static AttractionDto MapToDto(AttractionBasicResult r, string destinationName) => new(
        r.Id, r.DestinationId, destinationName, r.Name, r.Category, r.Description,
        r.ThumbnailUrl, r.Latitude, r.Longitude, r.OpeningHours, r.EntryFee, r.Website
    );

}