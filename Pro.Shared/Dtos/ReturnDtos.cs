namespace Pro.Shared.Dtos;

public record CreateReturnRequestDto(
    string Condition,
    string Damage
);

public record ReturnDto(
    Guid Id,
    DateTime DateUtc,
    string Condition,
    string Damage,
    Guid BorrowId
);

public record FinalizeReturnRequestDto(
    string Status
);