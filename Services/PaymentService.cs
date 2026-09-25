using Microsoft.EntityFrameworkCore;
using Npgsql;
using TravelApp.Infrastructure.Data.Exceptions;
using TrevalApp.DTOs.Common;
using TrevalApp.Interfaces.Services;
using TrevalApp.Models;

namespace TravelApp.Infrastructure.Data.Services;

public class PaymentService : IPaymentService
{
    private readonly AppDbContext _context;
    private readonly INotificationService _notificationService;
    
    public PaymentService(AppDbContext context, INotificationService notificationService)
    {
        _context = context;
        _notificationService = notificationService;
    }

    public async Task<PaymentDto> CreateAsync(Guid userId, CreatePaymentDto dto)
    {
        const string sql = @"SELECT * FROM create_payment({0},{1},{2},{3},{4})";
        PaymentBasicResult result;
        try
        {
            result = await _context.Database
                .SqlQueryRaw<PaymentBasicResult>(sql,
                    userId, dto.BookingType, dto.BookingId, dto.Amount, dto.Method)
                .FirstAsync();
        }       
        catch (PostgresException ex) when (ex.MessageText == "INVALID_BOOKING_TYPE")
        { throw new BadRequestException("Loại booking không hợp lệ (tour/hotel)."); }
        catch (PostgresException ex) when (ex.MessageText == "BOOKING_NOT_FOUND")
        { throw new NotFoundException("Booking", dto.BookingId); }
        catch (PostgresException ex) when (ex.MessageText == "PAYMENT_ALREADY_EXISTS")
        { throw new ConflictException("Booking này đã có giao dịch thanh toán."); }

        return MapToDto(result);
    }

    public async Task<PaymentDetailDto> GetByIdAsync(Guid id, Guid userId)
    {
        const string sql = @"SELECT * FROM get_payment_by_id({0},{1})";
        PaymentDetailResult result;
        try
        {
            result = await _context.Database
                .SqlQueryRaw<PaymentDetailResult>(sql, id, userId).FirstAsync();
        }
        catch (PostgresException ex) when (ex.MessageText == "PAYMENT_NOT_FOUND")
        {
            throw new NotFoundException("Payment", id);
        }

        return new PaymentDetailDto(
            result.Id, result.UserId, result.BookingType, result.BookingId,
            result.Amount, result.Method, result.Status, result.TransactionRef,
            result.GatewayResponse, result.PaidAt, result.CreatedAt
        );
        
    }

    public async Task<PaymentDto?> GetByBookingAsync(string bookingType, Guid bookingId)
    {
        const string sql = @"SELECT * FROM get_payment_by_booking({0},{1})";
        var result = await _context.Database
            .SqlQueryRaw<PaymentBasicResult>(sql, bookingType, bookingId)
            .FirstOrDefaultAsync();

        return result is null ? null : MapToDto(result);
    }

    public async Task<PagedResultDto<PaymentDto>> GetMyPaymentsAsync(Guid userId, PaymentQueryDto query)
    {
        const string sql = @"SELECT * FROM get_user_payments({0},{1},{2})";
        var rows = await _context.Database.SqlQueryRaw<PaymentListResult>(sql, userId, query.Page, query.PageSize)
            .ToListAsync();
        var total = rows.FirstOrDefault()?.TotalCount ?? 0;
        var dtos = rows.Select(r => MapToDto(r));
        return new PagedResultDto<PaymentDto>(dtos, (int)total, query.Page, query.PageSize,
            (int)Math.Ceiling(total / (double)query.PageSize));
    }

    public async Task<PaymentDto> ConfirmAsync(Guid id, ConfirmPaymentDto dto)
    {
        const string sql = @"SELECT * FROM confirm_payment({0},{1},{2})";
        PaymentBasicResult result;
        try
        {
            result = await _context.Database
                .SqlQueryRaw<PaymentBasicResult>(sql,
                    id, dto.TransactionRef,
                    (object?)dto.GatewayResponse ?? DBNull.Value)
                .FirstAsync();
        }   
        catch (PostgresException ex) when (ex.MessageText == "PAYMENT_NOT_FOUND")
        {
            throw new NotFoundException("Payment", id);
        }
        catch (PostgresException ex) when (ex.MessageText == "INVALID_PAYMENT_STATUS")
        {
            throw new ConflictException("Giao dịch đã ở trạng thái khác, không thể xác nhận lại.");
        }
        if (result.BookingType == "tour" || result.BookingType == "hotel" || result.BookingType == "attraction")
        {
            var typeLabel = result.BookingType switch
            {
                "tour" => "tour",
                "hotel" => "hotel",
                "attraction" => "vé tham quan",
                _ => result.BookingType
            };
            await _notificationService.CreateAsync(
                result.UserId,
                "payment_success",
                "Thanh toán thành công",
                $"Thanh toán ${result.Amount} cho đơn {typeLabel} đã thành công.",
                $"booking-detail?type={result.BookingType}&bookingId={result.BookingId}"
            );
        }
        return MapToDto(result);
    }
    
    public async Task FailAsync(Guid id, FailPaymentDto dto)
    {
        const string sql = @"SELECT fail_payment({0},{1})";
        try
        {
            await _context.Database.ExecuteSqlRawAsync(sql,
                id, (object?)dto.GatewayResponse ?? DBNull.Value);
        }
        catch (PostgresException ex) when (ex.MessageText == "PAYMENT_NOT_FOUND")
        {
            throw new NotFoundException("Payment", id);
        }
    }   

    public async Task RefundAsync(Guid id)
    {
        const string sql = @"SELECT refund_payment({0})";
        try
        {
            await _context.Database.ExecuteSqlRawAsync(sql, id);
        }
        catch (PostgresException ex) when (ex.MessageText == "PAYMENT_NOT_FOUND_OR_NOT_REFUNDABLE")
        {
            throw new BadRequestException("Giao dịch không tồn tại hoặc chưa thể hoàn tiền.");
        }
    }
    private static PaymentDto MapToDto(PaymentBasicResult r) =>
        new(r.Id, r.UserId, r.BookingType, r.BookingId, r.Amount, r.Method,
            r.Status, r.TransactionRef, r.PaidAt, r.CreatedAt);
    
}