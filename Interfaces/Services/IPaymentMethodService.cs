namespace TrevalApp.Interfaces.Services;

public interface IPaymentMethodService
{
    Task<IEnumerable<PaymentMethodDto>> GetMineAsync(Guid userId);
    Task<PaymentMethodDto> AddAsync(Guid userId, AddPaymentMethodDto dto);
    Task SetDefaultAsync (Guid id, Guid userId);
    Task DeleteAsync(Guid id, Guid userId);
}
