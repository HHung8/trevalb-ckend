namespace TrevalApp.DTOs.Payment;

public record CreatePaymentDto(
    string BookingType, 
    Guid BookingId, 
    string Method
);