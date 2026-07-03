using TrevalApp.DTOs.Payment;
namespace TrevalApp.Interfaces.Services;

public interface IPaymentService
{
    Task<PaymentResponseDto> CreatePaymentAsync(Guid userId, CreatePaymentDto dto);
    Task<PaymentDto> GetByIdAsync(Guid id, Guid userId);
    Task HandlePaymentCallbackAsync(PaymentCallbackDto dto);
}