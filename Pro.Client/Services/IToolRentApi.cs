using Pro.Shared.Dtos;

namespace ToolRent.Services;

public interface IToolRentApi
{
    // Auth
    Task<AuthResponseDto> LoginAsync(LoginRequestDto req);
    Task RegisterAsync(RegisterRequestDto req);
    
    //Categories
    Task<IReadOnlyList<CategoryDto>> GetCategoriesAsync();

    // Tools
    Task<ToolFiltersDto> GetToolFiltersAsync();
    Task<IReadOnlyList<ToolListItemDto>> GetToolsAsync(
        string category,
        string owner,
        float? minPrice = null,
        float? maxPrice = null,
        string? location = null
    );
    Task<IReadOnlyList<ToolListItemDto>> GetMyToolsAsync();
    Task<ToolDetailsDto?> GetToolAsync(Guid toolId);
    Task<ToolDetailsDto> CreateToolAsync(CreateToolRequestDto req);
    Task UpdateToolAsync(Guid toolId, UpdateToolRequestDto req);
    Task DeleteToolAsync(Guid toolId);


    // Borrow + Payment + History
    Task<CreateBorrowResponseDto> CreateBorrowAsync(CreateBorrowRequestDto req);
    Task<IReadOnlyList<string>> GetBorrowItemNamesAsync(Guid borrowId);
    Task ConfirmPaymentAsync(PaymentConfirmRequestDto req);
    Task<IReadOnlyList<PaymentHistoryItemDto>> GetPaymentHistoryAsync(DateTime? fromUtc, DateTime? toUtc);
}