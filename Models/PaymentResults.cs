namespace TrevalApp.Models;

public class PaymentBasicResult
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string BookingType { get; set; } = null!;
    public Guid BookingId { get; set; }
    public decimal Amount { get; set; }
    public string Method { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string? TransactionRef { get; set; }
    public DateTimeOffset? PaidAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public class PaymentListResult : PaymentBasicResult
{
    public long TotalCount { get; set; }
}

public class PaymentDetailResult : PaymentBasicResult
{
    public string? GatewayResponse { get; set; }
}
