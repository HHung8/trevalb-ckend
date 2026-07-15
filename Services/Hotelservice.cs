using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using TravelApp.Infrastructure.Data.Exceptions;
using TrevalApp.DTOs.Common;
using TrevalApp.DTOs.Hotel;
using TrevalApp.Interfaces.Services;
using TrevalApp.Models;

namespace TravelApp.Infrastructure.Data.Services;

public class Hotelservice : IHotelService   
{
    private readonly AppDbContext _context;

    public Hotelservice(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<PagedResultDto<HotelDto>> GetAllAsync(HotelQueryDto query)
    {
        const string sql = @"SELECT * FROM search_hotels({0},{1},{2},{3},{4},{5},{6})";
        var rows = await _context.Database.SqlQueryRaw<HotelSearchResult>(sql,
            (object?)query.DestinationId ?? DBNull.Value,
            (object?)query.Keyword ?? DBNull.Value,
            (object?)query.MinStars ?? DBNull.Value,
            (object?)query.MinPrice ?? DBNull.Value,
            (object?)query.MaxPrice ?? DBNull.Value,
            query.Page,
            query.PageSize).ToListAsync();
        var total = rows.FirstOrDefault()?.TotalCount ?? 0;
        var dtos = rows.Select(r => new HotelDto(
            r.Id, r.Name, r.Address ?? string.Empty, r.StarRating, r.Description,
            r.ThumbnailUrl, r.Latitude, r.Longitude, r.AverageRating, r.ReviewCount,
            r.DestinationId, r.DestinationName, r.MinRoomPrice
        ));
        return new PagedResultDto<HotelDto>(dtos, (int)total, query.Page, query.PageSize,
            ((int)Math.Ceiling(total / (double)query.PageSize)));    
    }   
    public async Task<HotelDetailDto> GetByIdAsync(Guid id)
    {
        const string detailSql = @"SELECT * FROM get_hotel_by_id({0})";
        HotelDetailResult detail;
        try
        {
            detail = await _context.Database.SqlQueryRaw<HotelDetailResult>(detailSql, id).FirstAsync();
        }
        catch (PostgresException ex) when (ex.MessageText == "Hotel not found")
        {
            throw new NotFoundException("Hotel", id);
        }

        const string roomsSql = @"SELECT * FROM get_hotel_rooms({0})";
        var rooms = await _context.Database.SqlQueryRaw<HotelRoomResult>(roomsSql, id).ToListAsync();
        const string imagesSql = @"SELECT * FROM get_hotel_images({0})";
        var images = await _context.Database.SqlQueryRaw<HotelImageResult>(imagesSql, id).ToListAsync();
        return new HotelDetailDto(
            detail.Id, detail.Name, detail.Address ?? string.Empty, detail.StarRating,
            detail.Description, detail.ThumbnailUrl, detail.Latitude, detail.Longitude,
            detail.Phone, detail.Email, detail.Website, detail.Amenities,
            detail.AverageRating, detail.ReviewCount, detail.DestinationId, detail.DestinationName,
            images.Select(i => i.ImageUrl),
            rooms.Select(r => new RoomDto(
                r.Id, r.RoomType, r.Description, r.PricePerNight,
                r.Capacity, r.Amenities, r.ThumbnailUrl, r.IsAvailable))
        );
    }
    public async Task<HotelDto> CreateAsync(CreateHotelDto dto)
    {
        const string sql = @"SELECT * FROM create_hotel({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10})";
        HotelBasicResult result;
        try
        {
            result = await _context.Database.SqlQueryRaw<HotelBasicResult>(sql,
                    dto.DestinationId,
                    dto.Name,
                    dto.Address,
                    dto.StarRating,
                    (object?)dto.Description ?? DBNull.Value,
                    dto.Latitude,
                    dto.Longitude,
                    (object?)dto.Phone ?? DBNull.Value,
                    (object?)dto.Email ?? DBNull.Value,
                    (object?)dto.Website ?? DBNull.Value,
                    (object?)dto.Amenities ?? DBNull.Value)
                .FirstAsync();
        }
        catch (PostgresException ex) when (ex.MessageText == "Destination Not Found")
        {
            throw new NotFoundException("Hotel", dto.DestinationId);
        }
        return MapToDto(result, string.Empty, null);
    }
    public async Task<HotelDto> UpdateAsync(Guid id, UpdateHotelDto dto)
    {
        const string sql = @"SELECT * FROM update_hotel({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11})";
        HotelBasicResult result;
        try
        {
            result = await _context.Database.SqlQueryRaw<HotelBasicResult>(sql, 
                id,
                (object?)dto.Name ?? DBNull.Value,
                (object?)dto.Address ?? DBNull.Value,
                (object?)dto.StarRating ?? DBNull.Value,
                (object?)dto.Description ?? DBNull.Value,
                (object?)dto.Latitude ?? DBNull.Value,
                (object?)dto.Longitude ?? DBNull.Value,
                (object?)dto.Phone ?? DBNull.Value,
                (object?)dto.Email ?? DBNull.Value,
                (object?)dto.Website ?? DBNull.Value,
                (object?)dto.Amenities ?? DBNull.Value,
                (object?)dto.IsActive ?? DBNull.Value
                ).FirstAsync();
        }
        catch (PostgresException ex) when (ex.MessageText == "HOTEL_NOT_FOUND")
        {
            throw new NotFoundException("Hotel", id);
        }
        return MapToDto(result, string.Empty, null);
    }
    public async Task DeleteAsync(Guid id)
    {
        const string sql = @"SELECT delete_hotel({0})";
        try
        {  
            await _context.Database.ExecuteSqlRawAsync(sql, id);
        }
        catch (PostgresException ex ) when(ex.MessageText == "HOTEL_NOT_FOUND")
        {
            throw new NotFoundException("Hotel", id);
        }
    }
    private static HotelDto MapToDto(HotelBasicResult r, string destinationName, decimal? minRoomPrice) =>
        new(r.Id, r.Name, r.Address ?? string.Empty, r.StarRating, r.Description,
            r.ThumbnailUrl, r.Latitude, r.Longitude, r.AverageRating, r.ReviewCount,
            r.DestinationId, destinationName, minRoomPrice
        );
}