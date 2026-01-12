namespace Pro.Shared.Dtos;

public record CategoryDto(Guid Id, string Name);

public record ToolListItemDto(
    Guid Id,
    string Name,
    float Price,
    string ImagePath,
    string CategoryName,
    string OwnerUsername,
    string Location
);

public record CreateReviewRequestDto(int Rating, string Description);

public record CreateReviewResponseDto(Guid ReviewId, string Status);

public record PendingReviewDto(
    Guid ReviewId,
    Guid ToolId,
    string ToolName,
    Guid BorrowId,
    int Rating,
    string Description,
    DateTime Date,
    UserDtos User
);

public record ReviewDto(
    Guid Id,
    int Rating,
    string Description,
    DateTime Date,
    UserDtos User
);

public record ToolDetailsDto(
    Guid Id,
    string Name,
    string Description,
    float Price,
    int Quantity,
    string ImagePath,
    string CategoryName,
    string OwnerUsername,
    string Location,
    IReadOnlyList<ReviewDto> Reviews
);

public record CreateToolRequestDto(
    string Name,
    string Description,
    float Price,
    int Quantity,
    Guid CategoryId,
    string Location,
    string? ImageFileName
);

public record UpdateToolRequestDto(
    string Name,
    string Description,
    float Price,
    int Quantity,
    Guid CategoryId,
    string Location,
    string? ImageFileName
);