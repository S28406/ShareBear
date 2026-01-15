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
        string? location = null,
        string? q = null
    );
    Task<IReadOnlyList<ToolListItemDto>> GetMyToolsAsync();
    Task<ToolDetailsDto?> GetToolAsync(Guid toolId);
    Task<ToolDetailsDto> CreateToolAsync(CreateToolRequestDto req);
    Task UpdateToolAsync(Guid toolId, UpdateToolRequestDto req);
    Task DeleteToolAsync(Guid toolId);


    // Borrow + Payment + History
    Task<CreateBorrowResponseDto> CreateBorrowAsync(CreateBorrowRequestDto req);
    Task<IReadOnlyList<string>> GetBorrowItemNamesAsync(Guid borrowId);
    Task<PaymentInitiateResponseDto> InitiatePaymentAsync(PaymentInitiateRequestDto req);
    Task<PaymentConfirmResponseDto> ConfirmPaymentAsync(Pro.Shared.Dtos.PaymentConfirmRequestDto req);
    Task<ReceiptDto> GetReceiptAsync(Guid paymentId);
    Task<IReadOnlyList<PaymentHistoryItemDto>> GetPaymentHistoryAsync(DateTime? fromUtc, DateTime? toUtc);
    
    //Reviews
    Task<CreateReviewResponseDto> CreateReviewAsync(Guid toolId, CreateReviewRequestDto req);
    Task<IReadOnlyList<PendingReviewDto>> GetPendingReviewsAsync();
    Task ApproveReviewAsync(Guid reviewId);
    Task RejectReviewAsync(Guid reviewId);
    public Task DeleteReviewAsync(Guid toolId, Guid reviewId);
    
    //Reviews
    Task<ReturnDto> CreateReturnAsync(Guid borrowId, CreateReturnRequestDto req);
    Task<ReturnDto> GetReturnAsync(Guid borrowId);
    Task FinalizeReturnAsync(Guid borrowId, FinalizeReturnRequestDto req);
    Task<ReturnDto?> TryGetReturnAsync(Guid borrowId);

}