namespace Pro.Shared.Dtos;

public static class PaymentStatuses
{
    public const string Initiated = "Initiated";
    public const string Confirmed = "Confirmed";
    public const string Failed = "Failed";
    public const string Refunded = "Refunded";
}

public static class BorrowStatuses
{
    public const string Pending = "Pending";
    public const string Paid = "Paid";
    public const string Confirmed = "Confirmed";
    public const string Cancelled = "Cancelled";
}

public record PaymentInitiateRequestDto(Guid BorrowId, string? Method);

public record PaymentInitiateResponseDto(
    Guid PaymentId,
    Guid BorrowId,
    DateTime DateUtc,
    decimal Amount,
    string Status,
    string Method
);

public record PaymentConfirmRequestDto(
    Guid PaymentId,
    string Method
);

public record PaymentConfirmResponseDto(
    Guid PaymentId,
    string Status,
    DateTime DateUtc,
    string ReceiptNumber
);

public record ReceiptItemDto(
    string ToolName,
    int Quantity,
    decimal UnitPrice,
    decimal LineTotal
);

public record ReceiptDto(
    string ReceiptNumber,
    DateTime DateUtc,
    Guid PaymentId,
    Guid BorrowId,
    string BuyerUsername,
    string BuyerEmail,
    DateTime StartDate,
    DateTime EndDate,
    IReadOnlyList<ReceiptItemDto> Items,
    decimal Total
);