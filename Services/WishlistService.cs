using Microsoft.EntityFrameworkCore;
using Npgsql;
using TravelApp.Infrastructure.Data.DTOs.Wishlish;
using TravelApp.Infrastructure.Data.Exceptions;
using TrevalApp.DTOs.Common;
using TrevalApp.Interfaces.Services;
using TrevalApp.Models;

namespace TravelApp.Infrastructure.Data.Services;

public class WishlistService : IWishlistService
{
    private readonly AppDbContext _context;
    public WishlistService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ToggleWishlistResultDto> ToggleAsync(Guid userId, ToggleWishlistDto dto)
    {
        const string sql = @"SELECT ""IsWishlisted"" AS ""Value"" FROM toggle_wishlist({0},{1},{2})";
        bool result;
        try
        {
            result = await _context.Database
                .SqlQueryRaw<bool>(sql, userId, dto.ItemType, dto.ItemId)
                .FirstAsync();
        }
        catch (PostgresException ex) when (ex.MessageText == "INVALID_ITEM_TYPE")
        { throw new BadRequestException("Loại mục yêu thích không hợp lệ (tour/hotel/destination)."); }
        catch (PostgresException ex) when (ex.MessageText == "TOUR_NOT_FOUND")
        { throw new NotFoundException("Tour", dto.ItemId); }
        catch (PostgresException ex) when (ex.MessageText == "HOTEL_NOT_FOUND")
        { throw new NotFoundException("Hotel", dto.ItemId); }
        catch (PostgresException ex) when (ex.MessageText == "DESTINATION_NOT_FOUND")
        { throw new NotFoundException("Destination", dto.ItemId); }
         return new ToggleWishlistResultDto(result);
    }

    public async Task<bool> IsWishlistedAsync(Guid userId, string itemType, Guid itemId)
    {
        const string sql = @"SELECT is_wishlisted({0},{1},{2}) AS ""Value""";
        return await _context.Database.SqlQueryRaw<bool>(sql, userId, itemType, itemId).FirstAsync();
    }

    public async Task<PagedResultDto<WishlistTourDto>> GetToursAsync(Guid userId, WishlistQueryDto query)
    {
        const string sql = @"SELECT * FROM get_user_wishlist_tours({0},{1},{2})";
        var rows = await _context.Database.SqlQueryRaw<WishlistTourResult>(sql, userId, query.Page, query.PageSize)
            .ToListAsync();
        var total = rows.FirstOrDefault()?.TotalCount ?? 0;
        var dtos = rows.Select(r => new WishlistTourDto(
            r.WishlistId, r.Id, r.DestinationId, r.DestinationName, r.Title,
            r.Price, r.DiscountPrice, r.DurationDays, r.ThumbnailUrl, 
            r.AverageRating, r.ReviewCount, r.SavedAt
        ));
        return new PagedResultDto<WishlistTourDto>(dtos, (int)total, query.Page, query.PageSize,
            (int)Math.Ceiling(total / (double)query.PageSize));
    }

    public async Task<PagedResultDto<WishlistHotelDto>> GetHotelsAsync(Guid userId, WishlistQueryDto query)
    {
        const string sql = @"SELECT * FROM get_user_wishlist_hotels({0}, {1}, {2})";
        var rows = await _context.Database.SqlQueryRaw<WishlistHotelResult>(sql, userId, query.Page, query.PageSize)
            .ToListAsync();
        var total = rows.FirstOrDefault()?.TotalCount ?? 0;
        var dtos = rows.Select(r => new WishlistHotelDto(
            r.WishlistId, r.Id, r.DestinationId, r.DestinationName, r.Name,
            r.StarRating, r.ThumbnailUrl, r.AverageRating, r.ReviewCount,
            r.MinRoomPrice, r.SavedAt
        ));
        return new PagedResultDto<WishlistHotelDto>(dtos, (int)total, query.Page, query.PageSize,
            (int)Math.Ceiling(total / (double)query.PageSize));
    }

    public async Task RemoveAsync(Guid id, Guid userId)
    {
        const string sql = @"SELECT remove_from_wishlist({0},{1})";
        try
        {
            await _context.Database.ExecuteSqlRawAsync(sql, id, userId);
        }
        catch (PostgresException ex) when(ex.MessageText == "WISHLIST_ITEM_NOT_FOUND")
        {
            throw new NotFoundException("WishlistItem", id);
        }
    }
}