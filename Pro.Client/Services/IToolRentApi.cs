using Pro.Shared.Dtos;

namespace ToolRent.Services;

public interface IToolRentApi
{
    // Auth
    Task<AuthResponseDto> LoginAsync(LoginRequestDto req);
    Task RegisterAsync(RegisterRequestDto req);

    // Tools
    Task<ToolFiltersDto> GetToolFiltersAsync();
    Task<IReadOnlyList<ToolListItemDto>> GetToolsAsync(string category, string owner);
    Task<ToolDetailsDto?> GetToolAsync(Guid toolId);

    // Borrow + Payment + History
    Task<CreateBorrowResponseDto> CreateBorrowAsync(CreateBorrowRequestDto req);
    Task<IReadOnlyList<string>> GetBorrowItemNamesAsync(Guid borrowId);
    Task ConfirmPaymentAsync(PaymentConfirmRequestDto req);
    Task<IReadOnlyList<PaymentHistoryItemDto>> GetPaymentHistoryAsync(DateTime? fromUtc, DateTime? toUtc);
}