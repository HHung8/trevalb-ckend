using Microsoft.EntityFrameworkCore;
using Npgsql;
using TravelApp.Infrastructure.Data.Exceptions;
using TrevalApp.DTOs.Hotel;
using TrevalApp.Interfaces.Services;
using TrevalApp.Models;

namespace TravelApp.Infrastructure.Data.Services;

public class RoomImageService : IRoomImageService
{
    private readonly AppDbContext _context;
    public RoomImageService(AppDbContext context) {_context = context;}

    public async Task<RoomImageDto> CreateRoomImageAsync(Guid roomId, CreateRoomImageDto dto)
    {
        const string sql = @"SELECT * FROM create_room_image({0}, {1}, {2})";
        HotelRoomImageResult result;
        try
        {
            result = await _context.Database
                .SqlQueryRaw<HotelRoomImageResult>(sql, roomId, dto.ImageUrl, dto.DisplayOrder).FirstAsync();
        }
        catch (PostgresException ex) when(ex.MessageText == "ROOM_NOT_FOUND")
        {
            throw new NotFoundException("Room", roomId);
        }
        return MapToDto(result);
    }

    public async Task<List<RoomImageDto>> GetRoomImagesAsync(Guid roomId)
    {
        const string sql = @"SELECT * FROM get_room_images({0})";
        List<HotelRoomImageResult> results;
        try
        {
            results = await _context.Database.SqlQueryRaw<HotelRoomImageResult>(sql, roomId).ToListAsync();
        }
        catch (PostgresException ex) when(ex.MessageText == "ROOM_NOT_FOUND")
        {
            throw new NotFoundException("Room", roomId);
        }
        return results.Select(MapToDto).ToList();
    }

    public async Task DeleteRoomImageAsync(Guid imageId)
    {
        const string sql = @"SELECT delete_room_image({0})";
        try
        {
            await _context.Database.ExecuteSqlRawAsync(sql, imageId);
        }
        catch (PostgresException ex) when(ex.MessageText == "ROOM_IMAGE_NOT_FOUND")
        {
            throw new NotFoundException("RoomImage", imageId);
        }
    }

    public async Task<RoomImageDto> UpdateRoomImageOrderAsync(Guid imageId, int displayOrder)
    {
        const string sql = @"SELECT * FROM update_room_image({0}, {1})";
        HotelRoomImageResult result;
        try
        {
            result = await _context.Database.SqlQueryRaw<HotelRoomImageResult>(sql, imageId, displayOrder).FirstAsync();
        }   
        catch (PostgresException ex) when(ex.MessageText == "ROOM_IMAGE_NOT_FOUND")
        {
            throw new NotFoundException("RoomImage", imageId);
        }
        return MapToDto(result);
    }


    private static RoomImageDto MapToDto(HotelRoomImageResult r) =>
        new(r.Id, r.RoomId, r.ImageUrl, r.DisplayOrder, r.CreatedAt, r.UpdatedAt);
}