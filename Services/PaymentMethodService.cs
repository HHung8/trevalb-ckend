using Microsoft.EntityFrameworkCore;
using Npgsql;
using TravelApp.Infrastructure.Data.Exceptions;
using TrevalApp.Interfaces.Services;
using TrevalApp.Models;

namespace TravelApp.Infrastructure.Data.Services;

public class PaymentMethodService : IPaymentMethodService
{
    private readonly AppDbContext _context;
    public PaymentMethodService(AppDbContext context) => _context = context;

    public async Task<IEnumerable<PaymentMethodDto>> GetMineAsync(Guid userId)
    {
        const string sql = @"SELECT * FROM get_user_payment_methods({0})";
        var rows = await _context.Database.SqlQueryRaw<PaymentMethodResult>(sql, userId).ToListAsync();
        return rows.Select(MapToDto);
    }

    public async Task<PaymentMethodDto> AddAsync(Guid userId, AddPaymentMethodDto dto)
    {
        const string sql = @"SELECT * FROM add_payment_method({0}, {1}, {2}, {3}, {4})";
        var result = await _context.Database
            .SqlQueryRaw<PaymentMethodResult>(sql, userId, dto.Brand, dto.Last4, dto.ExpiryMonth, dto.ExpiryYear)
            .FirstAsync();
        return MapToDto(result);
    }

    public async Task SetDefaultAsync(Guid id, Guid userId)
    {
        const string sql = @"SELECT set_default_payment_method({0}, {1})";
        try
        {
            await _context.Database.ExecuteSqlRawAsync(sql, id, userId);
        }
        catch (PostgresException ex) when(ex.MessageText == "PAYMENT_METHOD_NOT_FOUND")
        {
            throw new NotFoundException("PaymentMethod", id);
        }
    }

    public async Task DeleteAsync(Guid id, Guid userId)
    {
        const string sql = @"SELECT delete_payment_method({0}, {1})";
        try
        {
            await _context.Database.ExecuteSqlRawAsync(sql, id, userId);
        }
        catch (PostgresException ex) when(ex.MessageText == "PAYMENT_METHOD_NOT_FOUND")
        {
            throw new NotFoundException("PaymentMethod", id);
        }
    }

    private static PaymentMethodDto MapToDto(PaymentMethodResult r) => new(r.Id, r.Brand, r.Last4, r.ExpiryMonth,
        r.ExpiryYear, r.IsDefault, r.CreatedAt);
}