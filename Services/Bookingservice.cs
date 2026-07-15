using Microsoft.EntityFrameworkCore;
using Npgsql;
using TravelApp.Infrastructure.Data.Exceptions;
using TrevalApp.DTOs.Booking;
using TrevalApp.Interfaces.Services;
using TrevalApp.Models;

namespace TravelApp.Infrastructure.Data.Services;

public class Bookingservice : IBookingService
{
    private readonly AppDbContext _context;

    public Bookingservice(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<TourBookingDto> CreateTourBookingAsync(Guid userId, CreateTourBookingDto dto)
    {
        const string sql = @"SELECT * FROM create_tour_booking({0},{1},{2},{3},{4},{5})";
        TourBookingResult result;
        try
        {
            result = await _context.Database.SqlQueryRaw<TourBookingResult>(sql,
                userId,
                dto.TourId,
                (object?)dto.ScheduleId ?? DBNull.Value,
                dto.NumGuests,
                dto.TravelDate,
                (object?)dto.SpecialRequest ?? DBNull.Value
            ).FirstAsync();
        }
        catch (PostgresException ex) when (ex.MessageText == "TOUR_NOT_FOUND")
        {
            throw new NotFoundException("Tour", dto.TourId);
        }
        catch (PostgresException ex) when (ex.MessageText == "EXCEEDS_CAPACITY")
        {
            throw new BadRequestException("Số lượng khách vượt quá sức chứa của tour");
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
                    userId,
                    dto.RoomId,
                    dto.CheckIn,
                    dto.CheckOut,
                    dto.NumGuests,
                    (object?)dto.SpecialRequest ?? DBNull.Value)
                .FirstAsync();
        }
        catch (PostgresException ex) when (ex.MessageText == "ROOM_NOT_FOUND")
        {
            throw new NotFoundException("Room", dto.RoomId);
        }
        catch (PostgresException ex) when (ex.MessageText == "EXCEEDS_CAPACITY")
        {
            throw new BadRequestException("Số lượng khách vượt quá sức chứa của phòng.");
        }
        catch (PostgresException ex) when (ex.MessageText == "ROOM_NOT_AVAILABLE")
        {
            throw new ConflictException("Phòng đã được đặt trong khoảng thời gian này.");
        }
 
        return MapToHotelDto(result);
    }

    public async Task<IEnumerable<TourBookingDto>> GetUserTourBookingsAsync(Guid userId)
    {
        const string sql = @"SELECT * FROM get_user_tour_booking({0})";
        var rows = await _context.Database.SqlQueryRaw<TourBookingResult>(sql,userId).ToListAsync();
        return rows.Select(MapToTourDto);
    }

    public async Task<IEnumerable<HotelBookingDto>> GetUserHotelBookingsAsync(Guid userId)
    {
        const string sql = @"SELECT * FROM get_user_hotel_bookings({0})";
        var rows = await _context.Database.SqlQueryRaw<HotelBookingResult>(sql, userId).ToListAsync();
        return rows.Select(MapToHotelDto);
    }

    public async Task<TourBookingDto> GetTourBookingByIdAsync(Guid id, Guid userId)
    {
        const string sql =  @"SELECT * FROM get_tour_booking_by_id({0},{1})";
        TourBookingResult result;
        try
        {
            result = await _context.Database.SqlQueryRaw<TourBookingResult>(sql, id, userId).FirstAsync();
        }
        catch (PostgresException ex) when(ex.MessageText == "BOOKING_NOT_FOUND")
        {
            throw new NotFoundException("TourBooking", id);
        }
        return MapToTourDto(result);
    }

    public async Task<HotelBookingDto> GetHotelBookingByIdAsync(Guid id, Guid userId)
    {
        const string sql = @"SELECT * FROM get_hotel_booking_by_id({0},{1})";
        HotelBookingResult result;
        try
        {
            result = await _context.Database.SqlQueryRaw<HotelBookingResult>(sql, id, userId).FirstAsync();
        }
        catch (PostgresException ex) when(ex.MessageText == "BOOKING_NOT_FOUND")
        {
            throw new NotFoundException("HotelBooking", id);
        }

        return MapToHotelDto(result);
    }

    public async Task CancelTourBookingAsync(Guid id, Guid userId)
    {
        const string sql = @"SELECT * FROM cancel_tour_booking_by_id({0},{1})";
        try
        {
            await _context.Database.ExecuteSqlRawAsync(sql, id, userId);
        }
        catch (PostgresException ex) when(ex.MessageText == "BOOKING_NOT_FOUND_OR_CANNOT_CANCEL")
        {
          throw new BadRequestException("Booking không tồn tại hoặc không thể huỷ");
        }
    }

    public async Task CancelHotelBookingAsync(Guid id, Guid userId)
    {
        const string sql = @"SELECT cancel_hotel_booking({0},{1})";
        try
        {
            await _context.Database.ExecuteSqlRawAsync(sql, id, userId);
        }   
        catch (PostgresException ex) when(ex.MessageText == "BOOKING_NOT_FOUND_OR_CANNOT_CANCEL")
        {
            throw new BadRequestException("Booking không tồn tại hoặc không thể huỷ");
        }
    }

    private static TourBookingDto MapToTourDto(TourBookingResult result) =>
        new(result.Id, result.BookingCode, result.TourId, result.TourTitle, result.ThumbnailUrl,
            result.NumGuests, result.TotalPrice, result.Status, result.TravelDate, result.CreatedAt
        );

    private static HotelBookingDto MapToHotelDto(HotelBookingResult result) =>
        new(result.Id, result.BookingCode, result.RoomId, result.RoomType, result.HotelName, result.ThumbnailUrl,
            result.CheckIn, result.CheckOut, result.NumGuests, result.TotalPrice, result.Status, result.CreatedAt
        );
}