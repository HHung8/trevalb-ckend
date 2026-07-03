namespace TrevalApp.DTOs.Payment;

public record PaymentResponseDto(
    Guid PaymentId, 
    string Status, 
    string? PaymentUrl, 
    string? Message
);