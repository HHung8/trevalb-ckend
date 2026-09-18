using System.Xml;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using TravelApp.Infrastructure.Data.Exceptions;
using TrevalApp.DTOs.Common;
using TrevalApp.DTOs.Review;
using TrevalApp.Interfaces.Services;
using TrevalApp.Models;

namespace TravelApp.Infrastructure.Data.Services;

public class ReviewService : IReviewService
{
    private readonly AppDbContext _context;
    public ReviewService(AppDbContext context) => _context = context;
    public async Task<PagedResultDto<ReviewDto>> GetByTargetAsync(string targetType, Guid targetId, ReviewQueryDto query)
    {
        const string sql = @"SELECT * FROM get_reviews_by_target({0},{1},{2},{3})";
        var rows = await _context.Database
            .SqlQueryRaw<ReviewListResult>(sql, targetType, targetId, query.Page, query.PageSize)
            .ToListAsync();

        var total = rows.FirstOrDefault()?.TotalCount ?? 0;
        var dtos = rows.Select(r => new ReviewDto(
            r.Id, r.UserId, r.UserName, r.AvatarUrl, r.TargetType, r.TargetId,
            r.Rating, r.Comment, r.IsVerified, r.CreatedAt));

        return new PagedResultDto<ReviewDto>(dtos, (int)total, query.Page, query.PageSize,
            (int)Math.Ceiling(total / (double)query.PageSize));
    }

    public async Task<ReviewDetailDto> GetByIdAsync(Guid id)
    {
        const string sql = @"SELECT * FROM get_review_by_id({0})";
        ReviewDetailResult detail;
        try
        {
            detail = await _context.Database.SqlQueryRaw<ReviewDetailResult>(sql, id).FirstAsync();
        }
        catch (PostgresException ex) when(ex.MessageText == "REVIEW_NOT_FOUND")
        {
            throw new NotFoundException("Review", id);
        }
        const string imagesSql = @"SELECT * FROM get_review_images({0})";
        var images = await _context.Database.SqlQueryRaw<ReviewImageResult>(imagesSql, id).ToListAsync();
        return new ReviewDetailDto(
            detail.Id, detail.UserId, detail.UserName, detail.AvatarUrl,
            detail.TargetType, detail.TargetId, detail.Rating, detail.Comment,
            detail.IsVerified, detail.CreatedAt,
            images.Select(i => new ReviewImageDto(i.Id, i.ImageUrl, i.DisplayOrder))
        );
    }

    public async Task<ReviewDto> CreateAsync(Guid userId, CreateReviewDto dto)
    {
        const string sql = @"SELECT * FROM create_review({0},{1},{2},{3},{4})";
        ReviewBasicResult result;
        try
        {
            result = await _context.Database
                .SqlQueryRaw<ReviewBasicResult>(sql,
                    userId, dto.TargetType, dto.TargetId, dto.Rating,
                    (object?)dto.Comment ?? DBNull.Value)
                .FirstAsync();        }   
        catch (PostgresException ex) when (ex.MessageText == "INVALID_TARGET_TYPE")
        { throw new BadRequestException("Loại đối tượng review không hợp lệ (tour/hotel)."); }
        catch (PostgresException ex) when (ex.MessageText == "INVALID_RATING")
        { throw new BadRequestException("Số sao đánh giá phải từ 1 đến 5."); }
        catch (PostgresException ex) when (ex.MessageText == "TOUR_NOT_FOUND")
        { throw new NotFoundException("Tour", dto.TargetId); }
        catch (PostgresException ex) when (ex.MessageText == "HOTEL_NOT_FOUND")
        { throw new NotFoundException("Hotel", dto.TargetId); }
        catch (PostgresException ex) when( ex.MessageText == "ATTRACTION_NOT_FOUND")
        { throw new NotFoundException("Attraction", dto.TargetId); }
        catch (PostgresException ex) when (ex.MessageText == "ALREADY_REVIEWED")
        { throw new ConflictException("Bạn đã đánh giá mục này rồi."); }
        return MapToDto(result, string.Empty, null);
    }

    public async Task<ReviewDto> UpdateAsync(Guid id, Guid userId, UpdateReviewDto dto)
    {
        const string sql = @"SELECT * FROM update_review({0},{1},{2},{3})";
        ReviewBasicResult result;
        try
        {
            result = await _context.Database.SqlQueryRaw<ReviewBasicResult>(sql,
                 id, userId,
                 (object?)dto.Rating ?? DBNull.Value,
                 (object?)dto.Comment ?? DBNull.Value
                ).FirstAsync();
        }
        catch (PostgresException ex) when (ex.MessageText == "INVALID_RATING")
        { throw new BadRequestException("Số sao đánh giá phải từ 1 đến 5."); }
        catch (PostgresException ex) when (ex.MessageText == "REVIEW_NOT_FOUND_OR_UNAUTHORIZED")
        { throw new BadRequestException("Review không tồn tại hoặc bạn không có quyền sửa."); }
        return MapToDto(result, string.Empty, null);
    }

    public async Task DeleteAsync(Guid id, Guid userId)
    {
        const string sql = @"SELECT delete_review({0},{1})";
        try
        {
            await _context.Database.ExecuteSqlRawAsync(sql, id, userId);
        }
        catch (PostgresException ex) when (ex.MessageText == "REVIEW_NOT_FOUND_OR_UNAUTHORIZED")
        {
            throw new BadRequestException("Review không tồn tại hoặc bạn không có quyền xóa.");
        }
    }

    public async Task<ReviewImageDto> AddImageAsync(Guid reviewId, AddReviewImageDto dto)
    {
        const string sql = @"SELECT * FROM add_review_image({0},{1},{2})";
        ReviewImageResult result;
        try
        {
            result = await _context.Database
                .SqlQueryRaw<ReviewImageResult>(sql, reviewId, dto.ImageUrl, dto.DisplayOrder)
                .FirstAsync();
        }
        catch (PostgresException ex) when (ex.MessageText == "REVIEW_NOT_FOUND")
        {
            throw new NotFoundException("Review", reviewId);
        }

        return new ReviewImageDto(result.Id, result.ImageUrl, result.DisplayOrder);

    }

    public async Task DeleteImageAsync(Guid imageId)
    {
        const string sql = @"SELECT delete_review_image({0})";
        try
        {
            await _context.Database.ExecuteSqlRawAsync(sql, imageId);
        }
        catch (PostgresException ex) when (ex.MessageText == "IMAGE_NOT_FOUND")
        {
            throw new NotFoundException("ReviewImage", imageId);
        }
    }

    public async Task<PagedResultDto<MyReviewDto>> GetMineAsync(Guid userId, ReviewQueryDto query)
    {
        const string sql = @"SELECT * FROM get_user_reviews({0}, {1}, {2})";
        var rows = await _context.Database
            .SqlQueryRaw<MyReviewResult>(sql, userId, query.Page, query.PageSize)
            .ToListAsync();
        var total = rows.FirstOrDefault()?.TotalCount ?? 0;
        var dtos = rows.Select(r => new MyReviewDto(
            r.Id, r.TargetType, r.TargetId, r.TargetTitle, r.ThumbnailUrl,
            r.Rating, r.Comment, r.IsVerified, r.CreatedAt));
        return new PagedResultDto<MyReviewDto>(dtos, (int)total, query.Page, query.PageSize,
            (int)Math.Ceiling(total / (double)query.PageSize));
    }

    private static ReviewDto MapToDto(ReviewBasicResult r, string userName, string? avatarUrl) =>
        new(r.Id, r.UserId, userName, avatarUrl, r.TargetType, r.TargetId,
            r.Rating, r.Comment, r.IsVerified, r.CreatedAt);
}