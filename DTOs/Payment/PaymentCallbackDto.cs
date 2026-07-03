namespace TrevalApp.DTOs.Payment;

public record PaymentCallbackDto(
    string TransactionRef, 
    string Status, 
    string? GatewayData
);