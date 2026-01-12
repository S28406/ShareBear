namespace Pro.Shared.Dtos;

public record CreateBorrowRequestDto(
    Guid ToolId,
    int Quantity,
    DateTime StartDate,
    DateTime EndDate
);

public record CreateBorrowResponseDto(
    Guid BorrowId,
    decimal Total,
    DateTime StartDate,
    DateTime EndDate
);

public record PaymentHistoryItemDto(
    Guid PaymentId,
    DateTime Date,
    decimal Amount,
    string Status,
    string Method,
    Guid OrderId
);