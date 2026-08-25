using Microsoft.EntityFrameworkCore;
using Npgsql;
using TravelApp.Infrastructure.Data.Exceptions;
using TrevalApp.DTOs.Booking;
using TrevalApp.Interfaces.Services;
using TrevalApp.Models;

namespace TravelApp.Infrastructure.Data.Services;

public class BookingService : IBookingService
{
    private readonly AppDbContext _context;
    public BookingService(AppDbContext context)
    {
        _context = context;
    }
 
    public async Task<TourBookingDto> CreateTourBookingAsync(Guid userId, CreateTourBookingDto dto)
    {
        TourBookingResult result;
        // Nếu có ScheduleId → đặt theo lịch cụ thể (trừ slot, dùng OverridePrice nếu có)
        // Nếu không có ScheduleId → đặt theo yêu cầu (dùng giá gốc, không trừ slot)
        if (dto.ScheduleId.HasValue)
        {
            const string sql = @"SELECT * FROM create_tour_booking_with_schedule({0},{1},{2},{3},{4})";
            try
            {
                result = await _context.Database
                    .SqlQueryRaw<TourBookingResult>(sql,
                        userId, dto.TourId, dto.ScheduleId.Value,
                        dto.NumGuests,
                        (object?)dto.SpecialRequest ?? DBNull.Value)
                    .FirstAsync();
            }
            catch (PostgresException ex) when (ex.MessageText == "TOUR_NOT_FOUND")
                { throw new NotFoundException("Tour", dto.TourId); }
            catch (PostgresException ex) when (ex.MessageText == "SCHEDULE_NOT_FOUND")
                { throw new NotFoundException("TourSchedule", dto.ScheduleId.Value); }
            catch (PostgresException ex) when (ex.MessageText == "NOT_ENOUGH_SLOTS")
                { throw new BadRequestException("Không đủ chỗ cho lịch khởi hành này."); }
        }
        else
        {
            // Đặt không theo lịch — khách tự chọn ngày
            const string sql = @"SELECT * FROM create_tour_booking({0},{1},{2},{3},{4},{5})";
            try
            {
                result = await _context.Database
                    .SqlQueryRaw<TourBookingResult>(sql,
                        userId, dto.TourId, DBNull.Value,
                        dto.NumGuests, dto.TravelDate,
                        (object?)dto.SpecialRequest ?? DBNull.Value)
                    .FirstAsync();
            }
            catch (PostgresException ex) when (ex.MessageText == "TOUR_NOT_FOUND")
                { throw new NotFoundException("Tour", dto.TourId); }
            catch (PostgresException ex) when (ex.MessageText == "EXCEEDS_CAPACITY")
                { throw new BadRequestException("Số lượng khách vượt quá sức chứa của tour."); }
        }
 
        return MapToTourDto(result);
    }
 
    public async Task<HotelBookingDto> CreateHotelBookingAsync(Guid userId, CreateHotelBookingDto dto)
    {
        const string sql = @"SELECT * FROM create_hotel_booking({0},{1},{2},{3},{4},{5})";
        HotelBookingResult result;
        try
        {
            result = await _context.Database
                .SqlQueryRaw<HotelBookingResult>(sql,
                    userId, dto.RoomId, dto.CheckIn, dto.CheckOut,
                    dto.NumGuests,
                    (object?)dto.SpecialRequest ?? DBNull.Value)
                .FirstAsync();
        }
        catch (PostgresException ex) when (ex.MessageText == "ROOM_NOT_FOUND")
            { throw new NotFoundException("Room", dto.RoomId); }
        catch (PostgresException ex) when (ex.MessageText == "EXCEEDS_CAPACITY")
            { throw new BadRequestException("Số lượng khách vượt quá sức chứa của phòng."); }
        catch (PostgresException ex) when (ex.MessageText == "ROOM_NOT_AVAILABLE")
            { throw new ConflictException("Phòng đã được đặt trong khoảng thời gian này."); }
 
        return MapToHotelDto(result);
    }
 
    public async Task<IEnumerable<TourBookingDto>> GetUserTourBookingsAsync(Guid userId)
    {
        const string sql = @"SELECT * FROM get_user_tour_bookings({0})";
        var rows = await _context.Database
            .SqlQueryRaw<TourBookingResult>(sql, userId).ToListAsync();
        return rows.Select(MapToTourDto);
    }
 
    public async Task<IEnumerable<HotelBookingDto>> GetUserHotelBookingsAsync(Guid userId)
    {
        const string sql = @"SELECT * FROM get_user_hotel_bookings({0})";
        var rows = await _context.Database
            .SqlQueryRaw<HotelBookingResult>(sql, userId).ToListAsync();
        return rows.Select(MapToHotelDto);
    }
 
    public async Task<TourBookingDto> GetTourBookingByIdAsync(Guid id, Guid userId)
    {
        const string sql = @"SELECT * FROM get_tour_booking_by_id({0},{1})";
        TourBookingResult result;
        try
        {
            result = await _context.Database
                .SqlQueryRaw<TourBookingResult>(sql, id, userId).FirstAsync();
        }
        catch (PostgresException ex) when (ex.MessageText == "BOOKING_NOT_FOUND")
            { throw new NotFoundException("TourBooking", id); }
        return MapToTourDto(result);
    }
 
    public async Task<HotelBookingDto> GetHotelBookingByIdAsync(Guid id, Guid userId)
    {
        const string sql = @"SELECT * FROM get_hotel_booking_by_id({0},{1})";
        HotelBookingResult result;
        try
        {
            result = await _context.Database
                .SqlQueryRaw<HotelBookingResult>(sql, id, userId).FirstAsync();
        }
        catch (PostgresException ex) when (ex.MessageText == "BOOKING_NOT_FOUND")
            { throw new NotFoundException("HotelBooking", id); }
        return MapToHotelDto(result);
    }
 
    public async Task CancelTourBookingAsync(Guid id, Guid userId)
    {
        const string sql = @"SELECT cancel_tour_booking({0},{1})";
        try { await _context.Database.ExecuteSqlRawAsync(sql, id, userId); }
        catch (PostgresException ex) when (ex.MessageText == "BOOKING_NOT_FOUND_OR_CANNOT_CANCEL")
            { throw new BadRequestException("Booking không tồn tại hoặc không thể hủy."); }
    }
 
    public async Task CancelHotelBookingAsync(Guid id, Guid userId)
    {
        const string sql = @"SELECT cancel_hotel_booking({0},{1})";
        try { await _context.Database.ExecuteSqlRawAsync(sql, id, userId); }
        catch (PostgresException ex) when (ex.MessageText == "BOOKING_NOT_FOUND_OR_CANNOT_CANCEL")
            { throw new BadRequestException("Booking không tồn tại hoặc không thể hủy."); }
    }

    public async Task<BookingPublicInfoResult?> GetPublicInfoAsync(Guid bookingId)
    {
        var rows = await _context.Database.SqlQueryRaw<BookingPublicInfoResult>(
            @"SELECT * FROM get_booking_public_info({0})", bookingId).ToListAsync();
        return rows.FirstOrDefault();
    }

    private static TourBookingDto MapToTourDto(TourBookingResult r) =>
        new(r.Id, r.BookingCode, r.TourId, r.TourTitle, r.ThumbnailUrl,
            r.NumGuests, r.TotalPrice, r.Status, r.TravelDate, r.CreatedAt);
 
    private static HotelBookingDto MapToHotelDto(HotelBookingResult r) =>
        new(r.Id, r.BookingCode, r.RoomId, r.RoomType, r.HotelName, r.ThumbnailUrl,
            r.CheckIn, r.CheckOut, r.NumGuests, r.TotalPrice, r.Status, r.CreatedAt);
}