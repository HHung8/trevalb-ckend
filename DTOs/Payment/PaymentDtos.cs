
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

public record PaymentMethodDto(
    Guid Id,
    string Brand,
    string Last4,
    int ExpiryMonth,
    int ExpiryYear,
    bool IsDefault,
    DateTimeOffset CreatedAt
    );

public record AddPaymentMethodDto(
        string Brand,
        string Last4,
        int ExpiryMonth,
        int ExpiryYear
    );