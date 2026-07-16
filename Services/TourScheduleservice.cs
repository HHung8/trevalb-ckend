using Microsoft.EntityFrameworkCore;
using Npgsql;
using TravelApp.Infrastructure.Data.Exceptions;
using TrevalApp.DTOs.Tour;
using TrevalApp.Interfaces.Services;
using TrevalApp.Models;

namespace TravelApp.Infrastructure.Data.Services;

public class TourScheduleService : ITourScheduleService
{
    private readonly AppDbContext _context;
    public TourScheduleService(AppDbContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<TourScheduleDto>> GetByTourAsync(Guid tourId)
    {
        const string sql = @"SELECT * FROM get_tour_schedules({0})";
        var rows = await _context.Database.SqlQueryRaw<TourScheduleResult>(sql, tourId).ToListAsync();
        return rows.Select(MapToDto);
    }

    public async Task<TourScheduleDto> CreateAsync(Guid tourId, CreateTourScheduleDto dto)
    {
        const string sql = @"SELECT * FROM create_tour_schedule({0},{1},{2},{3},{4}})";
        TourScheduleResult result;
        try
        {
            result = await _context.Database.SqlQueryRaw<TourScheduleResult>(sql,
                tourId,
                dto.StartDate,
                dto.EndDate,
                dto.AvailableSlots,
                (object?)dto.OverridePrice ?? DBNull.Value).FirstAsync();
        }
        catch (PostgresException ex) when (ex.MessageText == "TOUR_NOT_FOUND")
        {
            throw new NotFoundException("Tour", tourId);
        }
        catch (PostgresException ex) when (ex.MessageText == "INVALID_DATE_RANGE")
        {
            throw new BadRequestException("Ngày kết thúc phải sau ngày bắt đầu.");
        }
        catch (PostgresException ex) when (ex.MessageText == "INVALID_SLOTS")
        {
            throw new BadRequestException("Số chỗ phải lớn hơn 0.");
        }
        return MapToDto(result);
    }

    public async Task DeleteAsync(Guid id)
    {
        const string sql = @"SELECT delete_tour_schedule({0})";
        try
        {
            await _context.Database.ExecuteSqlRawAsync(sql, id);
        }
        catch (PostgresException ex) when  (ex.MessageText == "SCHEDULE_NOT_FOUND")
        {
            throw new NotFoundException("TourSchedule", id);
        }
    }
    
    private static TourScheduleDto MapToDto(TourScheduleResult r) =>
        new(r.Id, r.TourId, r.StartDate, r.EndDate, r.AvailableSlots, r.OverridePrice);

}