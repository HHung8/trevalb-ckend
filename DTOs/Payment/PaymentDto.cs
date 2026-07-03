namespace TrevalApp.DTOs.Payment;

public record PaymentDto(
    Guid Id, 
    string BookingType, 
    Guid BookingId,
    decimal Amount, 
    string Method, 
    string Status, 
    string? TransactionRef, 
    DateTime? PaidAt
);