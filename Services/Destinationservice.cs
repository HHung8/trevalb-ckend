using TrevalApp.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using TravelApp.Infrastructure.Data.Exceptions;
using TrevalApp.DTOs.Common;
using TrevalApp.DTOs.Destination;
using TrevalApp.Models;

namespace TravelApp.Infrastructure.Data.Services;

public class Destinationservice : IDestinationService
{
    private readonly AppDbContext _context;
    public Destinationservice(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<PagedResultDto<DestinationDto>> GetAllAsync(DestinationQueryDto query)
    {
        const string sql = @"SELECT * FROM search_destinations({0},{1},{2},{3})";
        var rows = await _context.Database.SqlQueryRaw<DestinationSearchResult>(sql,
                (object?)query.Keyword ?? DBNull.Value,
                (object?)query.Country ?? DBNull.Value,
                query.Page,
                query.PageSize)
            .ToListAsync();
        var total = rows.FirstOrDefault()?.TotalCount ?? 0;
        var dtos = rows.Select(r => new DestinationDto(
            r.Id, r.Name, r.Country, r.City, r.Description, r.ThumbnailUrl,
            r.Latitude, r.Longitude, r.IsFeatured, (int)r.TourCount, (int)r.HotelCount));   
        return new PagedResultDto<DestinationDto>(dtos, (int)total, query.Page, query.PageSize,   (int)Math.Ceiling(total / (double)query.PageSize));
    }

    public async Task<DestinationDetailDto> GetByIdAsync(Guid id)
    {
        const string detailSql = @"SELECT * FROM get_destination_by_id({0})";
        DestinationDetailResult detail;
        try
        {
            detail = await _context.Database.SqlQueryRaw<DestinationDetailResult>(detailSql, id).FirstAsync();
        }
        catch (Exception e)
        {
            throw new NotFoundException("Destination", id);
        }

        const string imagesSql = @"SELECT * FROM get_destination_images({0})";
        var images = await _context.Database.SqlQueryRaw<DestinationImageResult>(imagesSql, id).ToListAsync();
        return new DestinationDetailDto(
            detail.Id,
            detail.Name,
            detail.Country,
            detail.City,
            detail.Description,
            detail.ThumbnailUrl,
            detail.Latitude,
            detail.Longitude,
            detail.Climate,
            detail.BestTimeToVisit,
            detail.IsFeatured,
            images.Select(i => i.ImageUrl));
    }

    public async Task<IEnumerable<DestinationDto>> GetFeaturedAsync(int count = 6)
    {
        const string sql = @"SELECT * FROM get_featured_destinations({0})";
        var rows = await _context.Database.SqlQueryRaw<DestinationFeaturedResult>(sql,count).ToListAsync();
        return rows.Select(r => new DestinationDto(
            r.Id, r.Name, r.Country, r.City, r.Description, r.ThumbnailUrl,
            r.Latitude, r.Longitude, r.IsFeatured, (int)r.TourCount, (int)r.HotelCount));
    }

    public async Task<DestinationDto> CreateAsync(CreateDestinationDto dto)
    {
        const string sql = @"SELECT * FROM create_destination({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8})";
 
        var result = await _context.Database
            .SqlQueryRaw<DestinationBasicResult>(sql,
                dto.Name, dto.Country, dto.City,
                (object?)dto.Description ?? DBNull.Value,
                dto.Latitude, dto.Longitude,
                (object?)dto.Climate ?? DBNull.Value,
                (object?)dto.BestTimeToVisit ?? DBNull.Value,
                dto.IsFeatured)
            .FirstAsync();
        return MapBasicToDto(result, 0, 0);
    }

    public async Task<DestinationDto> UpdateAsync(Guid id, UpdateDestinationDto dto)
    {
        const string sql = @"SELECT * FROM update_destination({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9})";
        DestinationBasicResult result;
        try
        {
            result = await _context.Database
                .SqlQueryRaw<DestinationBasicResult>(
                    sql,
                    id,
                    (object?)dto.Name ?? DBNull.Value,
                    (object?)dto.Country ?? DBNull.Value,
                    (object?)dto.City ?? DBNull.Value,
                    (object?)dto.Description ?? DBNull.Value,
                    (object?)dto.Latitude ?? DBNull.Value,
                    (object?)dto.Longitude ?? DBNull.Value,
                    (object?)dto.Climate ?? DBNull.Value,
                    (object?)dto.BestTimeToVisit ?? DBNull.Value,
                    (object?)dto.IsFeatured ?? DBNull.Value)
                .FirstAsync();
        }
        catch (PostgresException ex) when(ex.MessageText == "DESTINATION_NOT_FOUND")
        {
            throw new NotFoundException("Destination", id);
        }
        const string tourCountSql = @"SELECT COUNT(1) AS ""Value"" FROM ""Tours"" WHERE ""DestinationId"" = {0} AND ""IsActive"" = true AND ""IsDeleted"" = false";
        var tourCount = await _context.Database.SqlQueryRaw<long>(tourCountSql, id).FirstAsync();
        
        const string hotelCountSql = @"SELECT COUNT(1) AS ""Value"" FROM ""Hotels"" WHERE ""DestinationId"" = {0} AND ""IsActive"" = true AND ""IsDeleted"" = false";
        var hotelCont = await _context.Database.SqlQueryRaw<long>(hotelCountSql, id).FirstAsync();
        
        return MapBasicToDto(result, (int)tourCount, (int)hotelCont);
    }

    public async Task DeleteAsync(Guid id)
    {
        const string sql = @"SELECT delete_destination({0})";
        try
        {
            await _context.Database.ExecuteSqlRawAsync(sql, id);
        }
        catch (PostgresException ex) when (ex.MessageText == "DESTINATION_NOT_FOUND")
        {
            throw new NotFoundException("Destination", id);
        }
    }
    
    private static DestinationDto MapBasicToDto(DestinationBasicResult r, int tourCount, int hotelCount) => 
    new (r.Id, r.Name, r.Country, r.City, r.Description, r.ThumbnailUrl, r.Latitude, r.Longitude, r.IsFeatured, tourCount, hotelCount);
}
