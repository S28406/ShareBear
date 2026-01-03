namespace Pro.Shared.Dtos;

public record CategoryDto(Guid Id, string Name);

public record ToolListItemDto(
    Guid Id,
    string Name,
    float Price,
    string ImagePath,
    string CategoryName,
    string OwnerUsername
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
    IReadOnlyList<ReviewDto> Reviews
);

public record CreateToolRequestDto(
    string Name,
    string Description,
    float Price,
    int Quantity,
    Guid CategoryId,
    string? ImageFileName
);

public record UpdateToolRequestDto(
    string Name,
    string Description,
    float Price,
    int Quantity,
    Guid CategoryId,
    string? ImageFileName
);