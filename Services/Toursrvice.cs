using Microsoft.EntityFrameworkCore;
using Npgsql;
using TravelApp.Infrastructure.Data.Exceptions;
using TrevalApp.DTOs.Common;
using TrevalApp.DTOs.Tour;
using TrevalApp.Interfaces.Services;
using TrevalApp.Models;

namespace TravelApp.Infrastructure.Data.Services;

public class Toursrvice : ITourService
{
    private readonly AppDbContext _context;

    public Toursrvice(AppDbContext context)
    {
        _context = context;
    }


    public async Task<PagedResultDto<TourDto>> GetAllAsync(TourQueryDto query)
    {
        const string sql = @"SELECT * FROM search_tours({0}, {1}, {2}, {3}, {4}, {5}, {6})";
        var rows = await _context.Database.SqlQueryRaw<TourSearchResult>(sql,
            (object?)query.DestinationId ?? DBNull.Value,
            (object?)query.Keyword ?? DBNull.Value,
            (object?)query.MinPrice ?? DBNull.Value,
            (object?)query.MaxPrice ?? DBNull.Value,
            (object?)query.Difficulty ?? DBNull.Value,
            query.Page,
            query.PageSize)
            .ToListAsync();
        var total = rows.FirstOrDefault()?.TotalCount ?? 0;
        var dtos = rows.Select(r => new TourDto(
            r.Id, r.Title, r.Description, r.Price, r.DiscountPrice,
            r.DurationDays, r.MaxCapacity, r.Difficulty, r.ThumbnailUrl,
            r.AverageRating, r.ReviewCount, r.DestinationId, r.DestinationName
        ));
        return new PagedResultDto<TourDto>(dtos, (int)total, query.Page, query.PageSize,
            (int)Math.Ceiling(total / (double)query.PageSize));
    }

    public async Task<TourDetailDto> GetByIdAsync(Guid id)
    {
        const string detailSql = @"SELECT * FROM get_tour_by_id({0})";
        TourDetailResult detail;
        try
        {
            detail = await _context.Database.SqlQueryRaw<TourDetailResult>(detailSql, id).FirstAsync();
        }
        catch (PostgresException ex) when (ex.MessageText == "TOUR_NOT_FOUND")
        {
            throw new NotFoundException("Tour", id);
        }
        
        const string imagesSql = @"SELECT * FROM get_tour_images({0})";
        var images = await _context.Database.SqlQueryRaw<TourImageResult>(imagesSql, id).ToListAsync();

        return new TourDetailDto(
            detail.Id, detail.Title, detail.Description, detail.Highlights,
            detail.Includes, detail.Excludes, detail.Price, detail.DiscountPrice,
            detail.DurationDays, detail.MaxCapacity, detail.Difficulty, detail.ThumbnailUrl,
            detail.AverageRating, detail.ReviewCount, detail.DestinationId, detail.DestinationName,
            images.Select(i => i.ImageUrl),
            Enumerable.Empty<TourScheduleDto>()
        );
    }

    public async Task<IEnumerable<TourDto>> GetByDestinationAsync(Guid destinationId)
    {
        const string sql =  @"SELECT * FROM get_tours_by_destination({0})";
        var rows = await _context.Database.SqlQueryRaw<TourByDestinationResult>(sql, destinationId).ToListAsync();
        return rows.Select(r => new TourDto(
            r.Id, r.Title, r.Description, r.Price, r.DiscountPrice,
            r.DurationDays, r.MaxCapacity, r.Difficulty, r.ThumbnailUrl,
            r.AverageRating, r.ReviewCount, r.DestinationId, r.DestinationName
        ));
    }

    public async Task<TourDto> CreateAsync(CreateTourDto dto)
    {
        const string sql = @"SELECT * FROM create_tour({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10})";
        TourBasicResult result;
        try
        {
            result = await _context.Database.SqlQueryRaw<TourBasicResult>(sql,
                dto.DestinationId,
                dto.Title,
                (object?) dto.Description ?? DBNull.Value,
                (object?) dto.Highlights ?? DBNull.Value,
                (object?) dto.Includes ?? DBNull.Value,
                (object?) dto.Excludes ?? DBNull.Value,
                dto.Price,
                (object?) dto.DiscountPrice ?? DBNull.Value,
                dto.DurationDays,
                dto.MaxCapacity,
                dto.Difficulty ?? "easy"
            ).FirstAsync();
        }   
        catch (PostgresException ex) when (ex.MessageText == "DESTINATION_NOT_FOUND")
        {
            throw new NotFoundException("Destination", dto.DestinationId);
        }

        return MapToDto(result, string.Empty);
    }

    public async Task<TourDto> UpdateAsync(Guid id, UpdateTourDto dto)
    {
        const string sql = @"SELECT * FROM update_tour({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11})";
        TourBasicResult result;
        try
        {
            result = await _context.Database
                .SqlQueryRaw<TourBasicResult>(sql,
                    id,
                    (object?)dto.Title ?? DBNull.Value,
                    (object?)dto.Description ?? DBNull.Value,
                    (object?)dto.Highlights ?? DBNull.Value,
                    (object?)dto.Includes ?? DBNull.Value,
                    (object?)dto.Excludes ?? DBNull.Value,
                    (object?)dto.Price ?? DBNull.Value,
                    (object?)dto.DiscountPrice ?? DBNull.Value,
                    (object?)dto.DurationDays ?? DBNull.Value,
                    (object?)dto.MaxCapacity ?? DBNull.Value,
                    (object?)dto.Difficulty ?? DBNull.Value,
                    (object?)dto.IsActive ?? DBNull.Value)
                .FirstAsync();
        }
        catch (PostgresException ex) when (ex.MessageText == "TOUR_NOT_FOUND")
        {
            throw new NotFoundException("Tour", id);
        }
        return MapToDto(result, string.Empty);
    }

    public async Task DeleteAsync(Guid id)
    {
        const string sql = @"SELECT delete_tour({0})";
        try
        {
            await _context.Database.ExecuteSqlRawAsync(sql, id);
            
        }
        catch (PostgresException ex) when (ex.MessageText == "TOUR_NOT_FOUND")
        {
            throw new NotFoundException("Tour", id);
        }
    }
    
    private static TourDto MapToDto(TourBasicResult r, string destinationName) =>
        new(r.Id, r.Title, r.Description, r.Price, r.DiscountPrice,
            r.DurationDays, r.MaxCapacity, r.Difficulty, r.ThumbnailUrl,
            r.AverageRating, r.ReviewCount, r.DestinationId, destinationName);
}