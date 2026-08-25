using Microsoft.EntityFrameworkCore;
using Npgsql;
using TravelApp.Infrastructure.Data.Exceptions;
using TrevalApp.DTOs.Hotel;
using TrevalApp.Interfaces.Services;
using TrevalApp.Models;
using CreateRoomDto = TrevalApp.DTOs.Hotel.CreateRoomDto;


namespace TravelApp.Infrastructure.Data.Services;

public class RoomService : IRoomService
{
    private readonly AppDbContext _context;
    public RoomService(AppDbContext context) { _context = context;}
    public async Task<RoomDto> CreateRoomAsync(Guid hotelId, CreateRoomDto dto)
    {
        const string sql = @"SELECT * FROM create_room({0},{1},{2},{3},{4},{5},{6})";
        HotelRoomResult result;
        try
        {
            result = await _context.Database.SqlQueryRaw<HotelRoomResult>(sql, 
                    hotelId,
                    dto.RoomType,
                    (object?) dto.Description ?? DBNull.Value,
                    dto.PricePerNight,
                    dto.Capacity,
                    dto.TotalRooms,
                    (object?)dto.Amenities ?? DBNull.Value
                ).FirstAsync();
        }
        catch (PostgresException ex) when (ex.MessageText == "HOTEL_NOT_FOUND")
        {
            throw new NotFoundException("Hotel", hotelId);
        }
        return MapToDto(result);
    }

    public async Task<RoomDto> UpdateRoomAsync(Guid id, UpdateRoomDto dto)
    {
        const string sql = @"SELECT * FROM update_room({0},{1},{2},{3},{4},{5},{6},{7})";
        HotelRoomResult result;
        try
        {
            result = await _context.Database
                .SqlQueryRaw<HotelRoomResult>(sql,
                    id,
                    (object?)dto.RoomType ?? DBNull.Value,
                    (object?)dto.Description ?? DBNull.Value,
                    (object?)dto.PricePerNight ?? DBNull.Value,
                    (object?)dto.Capacity ?? DBNull.Value,
                    (object?)dto.TotalRooms ?? DBNull.Value,
                    (object?)dto.Amenities ?? DBNull.Value,
                    (object?)dto.IsAvailable ?? DBNull.Value)
                .FirstAsync();
        }
        catch (PostgresException ex) when (ex.MessageText == "ROOM_NOT_FOUND")
        {
            throw new NotFoundException("Room", id);
        }
        return MapToDto(result);
    }

    public async Task DeleteRoomAsync(Guid id)
    {
        const string sql = @"SELECT delete_room({0})";
        try
        {
            await _context.Database.ExecuteSqlRawAsync(sql, id);
        }
        catch (PostgresException ex) when (ex.MessageText == "ROOM_NOT_FOUND")
        {
            throw new NotFoundException("Room", id);
        }
    }
    
    private static RoomDto MapToDto(HotelRoomResult r) => 
        new(r.Id, r.RoomType, r.Description, r.PricePerNight,
        r.Capacity, r.Amenities, r.ThumbnailUrl, r.IsAvailable);
    
}