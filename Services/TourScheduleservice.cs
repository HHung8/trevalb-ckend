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
        var rows = await _context.Database
            .SqlQueryRaw<TourScheduleResult>(
                @"SELECT * FROM get_tour_schedules({0})", tourId)
            .ToListAsync();
 
        return rows.Select(MapToDto);
    }

    public async Task<TourScheduleDto> CreateAsync(Guid tourId, CreateTourScheduleDto dto)
    {
        // Dùng NpgsqlParameter thay vì positional {0},{1}...
        // để tránh lỗi khi EF Core parse chuỗi SQL có ký tự đặc biệt
        var conn = (Npgsql.NpgsqlConnection)_context.Database.GetDbConnection();
        await _context.Database.OpenConnectionAsync();
 
        try
        {
            await using var cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM create_tour_schedule(
                @tourId, @startDate, @endDate, @availableSlots, @overridePrice)";
 
            cmd.Parameters.AddWithValue("tourId", tourId);
            cmd.Parameters.AddWithValue("startDate", dto.StartDate);
            cmd.Parameters.AddWithValue("endDate", dto.EndDate);
            cmd.Parameters.AddWithValue("availableSlots", dto.AvailableSlots);
            cmd.Parameters.AddWithValue("overridePrice",
                dto.OverridePrice.HasValue ? (object)dto.OverridePrice.Value : DBNull.Value);
 
            await using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
                throw new BadRequestException("Không thể tạo lịch khởi hành.");
 
            var result = new TourScheduleResult
            {
                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                TourId = reader.GetGuid(reader.GetOrdinal("TourId")),
                StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")),
                AvailableSlots = reader.GetInt32(reader.GetOrdinal("AvailableSlots")),
                OverridePrice = reader.IsDBNull(reader.GetOrdinal("OverridePrice"))
                    ? null
                    : reader.GetDecimal(reader.GetOrdinal("OverridePrice"))
            };
 
            return MapToDto(result);
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
        finally
        {
            await _context.Database.CloseConnectionAsync();
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        try
        {
            await _context.Database.ExecuteSqlRawAsync(
                @"SELECT delete_tour_schedule({0})", id);
        }
        catch (PostgresException ex) when (ex.MessageText == "SCHEDULE_NOT_FOUND")
        {
            throw new NotFoundException("TourSchedule", id);
        }
    }
    
    private static TourScheduleDto MapToDto(TourScheduleResult r) =>
        new(r.Id, r.TourId, r.StartDate, r.EndDate, r.AvailableSlots, r.OverridePrice);
}