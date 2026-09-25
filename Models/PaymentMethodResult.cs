namespace TrevalApp.Models;

public class PaymentMethodResult
{
    public Guid Id { get; set; }
    public string Brand { get; set; } = null!;
    public string Last4 { get; set; } = null!;
    public int ExpiryMonth { get; set; }
    public int ExpiryYear { get; set; }
    public bool IsDefault { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}