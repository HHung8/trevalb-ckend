using TrevalApp.DTOs.Common;
using TrevalApp.DTOs.Payment;
namespace TrevalApp.Interfaces.Services;

public interface IPaymentService
{
    Task<PaymentDto> CreateAsync(Guid userId, CreatePaymentDto dto);
    Task<PaymentDetailDto> GetByIdAsync(Guid id, Guid userId);
    Task<PaymentDto?> GetByBookingAsync(string bookingType, Guid bookingId);
    Task<PagedResultDto<PaymentDto>> GetMyPaymentsAsync(Guid userId, PaymentQueryDto query);
    Task<PaymentDto> ConfirmAsync(Guid id, ConfirmPaymentDto dto);
    Task FailAsync(Guid id, FailPaymentDto dto);
    Task RefundAsync(Guid id);
}