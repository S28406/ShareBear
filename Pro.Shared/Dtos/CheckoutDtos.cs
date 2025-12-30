namespace Pro.Shared.Dtos;

public record CreateBorrowRequestDto(Guid ToolId, int Quantity);
public record CreateBorrowResponseDto(Guid BorrowId, decimal Total);

public record PaymentConfirmRequestDto(Guid BorrowId, decimal Amount, string Method, string Status);

public record PaymentHistoryItemDto(
    Guid PaymentId,
    DateTime Date,
    decimal Amount,
    string Status,
    string Method,
    Guid OrderId
);