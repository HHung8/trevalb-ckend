
public record PaymentDto(
    Guid Id,
    Guid UserId,
    string BookingType,
    Guid BookingId,
    decimal Amount,
    string Method,
    string Status,
    string? TransactionRef,
    DateTimeOffset? PaidAt,
    DateTimeOffset CreatedAt
    );
    
public record PaymentDetailDto(
    Guid Id,
    Guid UserId,
    string BookingType,
    Guid BookingId,
    decimal Amount,
    string Method,
    string Status,
    string? TransactionRef,
    string? GatewayResponse,
    DateTimeOffset? PaidAt,
    DateTimeOffset CreatedAt
    );
public record CreatePaymentDto(
        string BookingType,   // "tour" | "hotel"
        Guid BookingId,
        decimal Amount,
        string Method
    );
public record ConfirmPaymentDto(
    string TransactionRef,
    string? GatewayResponse
);

public record FailPaymentDto(
    string? GatewayResponse
);

public record PaymentQueryDto(
    int Page = 1,
    int PageSize = 10
);