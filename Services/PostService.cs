using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using TravelApp.Infrastructure.Data.Exceptions;
using TrevalApp.DTOs.Common;
using TrevalApp.DTOs.Post;
using TrevalApp.Interfaces.Services;
using TrevalApp.Models;

namespace TravelApp.Infrastructure.Data.Services;

public class PostService : IPostService
{
    private readonly AppDbContext _context;
    public PostService(AppDbContext context) => _context = context;
    
    public async Task<PagedResultDto<PostDto>> GetFeedAsync(PostQueryDto query)
    {
        const string sql = @"SELECT * FROM get_post_feed({0},{1})";
        var rows = await _context.Database.SqlQueryRaw<PostFeedResult>(sql, query.Page, query.PageSize).ToListAsync();
        var total = rows.FirstOrDefault()?.TotalCount ?? 0;
        var dtos = rows.Select(MapFeedToDto);
        return new PagedResultDto<PostDto>(dtos, (int)total, query.Page, query.PageSize,
            (int)Math.Ceiling(total / (double)query.PageSize));
    }

    public async Task<PagedResultDto<PostDto>> GetByUserAsync(Guid userId, PostQueryDto query)
    {
        const string sql = @"SELECT * FROM get_posts_by_user({0},{1},{2})";
        var rows = await _context.Database
            .SqlQueryRaw<PostFeedResult>(sql, userId, query.Page, query.PageSize)
            .ToListAsync();

        var total = rows.FirstOrDefault()?.TotalCount ?? 0;
        var dtos = rows.Select(MapFeedToDto);
        return new PagedResultDto<PostDto>(dtos, (int)total, query.Page, query.PageSize,
            (int)Math.Ceiling(total / (double)query.PageSize));
    }

    public async Task<PostDetailDto> GetByIdAsync(Guid id)
    {
        const string sql = @"SELECT * FROM get_post_by_id({0})";
        PostDetailResult detail;
        try
        {
            detail = await _context.Database.SqlQueryRaw<PostDetailResult>(sql, id).FirstAsync();
        }
        catch (PostgresException ex) when (ex.MessageText == "POST_NOT_FOUND")
        {
            throw new NotFoundException("Post", id);
        }
        
        const string mediaSql =  @"SELECT * FROM get_post_media({0})";
        var medias = await _context.Database.SqlQueryRaw<PostMediaResult>(mediaSql, id).ToListAsync();
        
        return new PostDetailDto(
            detail.Id, detail.UserId, detail.UserName, detail.AvatarUrl,
            detail.DestinationId, detail.DestinationName, detail.Title, detail.Content,
            detail.ThumbnailUrl, DesDeserializeTags(detail.Tags),
            detail.LikesCount, detail.CommentsCount, detail.CreatedAt,
            medias.Select(m => new PostMediaDto(m.Id, m.MediaUrl, m.MediaType, m.DisplayOrder))
        );
        
    }

    public async Task<PostDto> CreateAsync(Guid userId, CreatePostDto dto)
    {
        const string sql = @"SELECT * FROM create_post({0},{1},{2},{3},{4})";
        var result = await _context.Database
            .SqlQueryRaw<PostBasicResult>(sql,
                userId, dto.Title, dto.Content,
                (object?)dto.DestinationId ?? DBNull.Value,
                (object?)SerializeTags(dto.Tags) ?? DBNull.Value)
            .FirstAsync();

        return MapBasicToDto(result, string.Empty, null);
    }

    public async Task<PostDto> UpdateAsync(Guid id, Guid userId, UpdatePostDto dto)
    {
        const string sql = @"SELECT * FROM update_post({0},{1},{2},{3},{4},{5},{6})";
        PostBasicResult result;
        try
        {
            result = await _context.Database.SqlQueryRaw<PostBasicResult>(sql, id, userId,
                (object?)dto.Title ?? DBNull.Value,
                (object?)dto.Content ?? DBNull.Value,
                (object?)dto.DestinationId ?? DBNull.Value,
                (object?)SerializeTags(dto.Tags) ?? DBNull.Value,
                (object?)dto.IsPublished ?? DBNull.Value
            ).FirstAsync();
        }
        catch (PostgresException ex) when(ex.MessageText == "POST_NOT_FOUND_OR_UNAUTHORIZED")
        {
            throw new BadRequestException("Bài viết không tồn tại hoặc bạn không có quyền sửa");
        }
        return MapBasicToDto(result, string.Empty, null);
    }

    public async Task DeleteAsync(Guid id, Guid userId)
    {
        const string sql = @"SELECT delete_post({0},{1})";
        try
        {
            await _context.Database.ExecuteSqlRawAsync(sql, id, userId);
        }
        catch (PostgresException ex) when (ex.MessageText == "POST_NOT_FOUND_OR_UNAUTHORIZED")
        {
            throw new BadRequestException("Bài viết không tồn tại hoặc bạn không có quyền xóa.");
        }
    }

    public async Task<ToggleLikeResultDto> ToggleLikeAsync(Guid postId, Guid userId)
    {
        const string sql = @"SELECT * FROM toggle_like_post({0},{1})";
        var result = await _context.Database.SqlQueryRaw<ToggleLikeResultDto>(sql,postId, userId).FirstAsync();
        return new ToggleLikeResultDto(result.IsLiked, result.LikesCount);
    }

    public async Task<PostMediaDto> AddMediaAsync(Guid postId, AddPostMediaDto dto)
    {
        const string sql = @"SELECT * FROM add_post_media({0},{1},{2},{3})";
        PostMediaResult result;
        try
        {
            result = await _context.Database
                .SqlQueryRaw<PostMediaResult>(sql, postId, dto.MediaUrl, dto.MediaType, dto.DisplayOrder).FirstAsync();
        }
        catch (PostgresException ex) when(ex.MessageText == "POST_NOT_FOUND")
        {
            throw new NotFoundException("Post", postId);
        }
        return new PostMediaDto(result.Id, result.MediaUrl, result.MediaType, result.DisplayOrder);
    }

    public async Task DeleteMediaAsync(Guid mediaId)
    {
        const string sql = @"SELECT delete_post_media({0})";
        try
        {
            await _context.Database.ExecuteSqlRawAsync(sql, mediaId);
        }
        catch (PostgresException ex) when (ex.MessageText == "MEDIA_NOT_FOUND")
        {
            throw new NotFoundException("PostMedia", mediaId);
        }
    }
    
    public async Task<IEnumerable<PostCommentDto>> GetCommentsAsync(Guid postId)
    {
        const string sql = @"SELECT * FROM get_post_comments({0})";
        var rows = await _context.Database.SqlQueryRaw<PostCommentResult>(sql, postId).ToListAsync();
        return rows.Select(c => new PostCommentDto(
            c.Id, c.PostId, c.UserId, c.UserName ?? string.Empty, c.AvatarUrl,
            c.ParentId, c.Content, c.CreatedAt));
    }

    public async Task<PostCommentDto> AddCommentAsync(Guid postId, Guid userId, CreatePostCommentDto dto)
    {
        const string sql = @"SELECT * FROM create_post_comment({0},{1},{2},{3})";
        PostCommentResult result;
        try
        {
            result = await _context.Database
                .SqlQueryRaw<PostCommentResult>(sql,
                    postId, userId, dto.Content,
                    (object?)dto.ParentId ?? DBNull.Value)
                .FirstAsync();
        }
        catch (PostgresException ex) when (ex.MessageText == "POST_NOT_FOUND")
        {
            throw new NotFoundException("Post", postId);
        }

        return new PostCommentDto(
            result.Id, result.PostId, result.UserId,
            result.UserName ?? string.Empty, result.AvatarUrl,
            result.ParentId, result.Content, result.CreatedAt);
    }

    public async Task DeleteCommentAsync(Guid commentId, Guid userId)
    {
        const string sql = @"SELECT delete_post_comment({0},{1})";
        try
        {
            await _context.Database.ExecuteSqlRawAsync(sql, commentId, userId);
        }
        catch (PostgresException ex) when(ex.MessageText == "COMMENT_NOT_FOUND_OR_UNAUTHORIZED")
        {
            throw new BadRequestException("Bình luận không tồn tại hoặc bạn không có quyền xóa.");
        }
    }

    private static PostDto MapFeedToDto(PostFeedResult r) =>
        new(r.Id, r.UserId, r.UserName, r.AvatarUrl, r.DestinationId, r.DestinationName,
            r.Title, r.ThumbnailUrl, r.LikesCount, r.CommentsCount, r.CreatedAt
        );
    
    private static PostDto MapBasicToDto(PostBasicResult r, string userName, string ? destinationName) => 
        new(r.Id, r.UserId, userName, null, r.DestinationId, destinationName,
            r.Title, r.ThumbnailUrl, r.LikesCount, r.CommentsCount, r.CreatedAt);
    
    private static string? SerializeTags(List<string>? tags) => 
        tags is null || tags.Count == 0 ? null : JsonSerializer.Serialize(tags);

    private static List<string> DesDeserializeTags(string? tagsJson)
    {
        if (string.IsNullOrWhiteSpace(tagsJson)) return new List<string>();
        try { return JsonSerializer.Deserialize<List<string>>(tagsJson) ?? new List<string>(); }
        catch { return new List<string>(); }
    }
}