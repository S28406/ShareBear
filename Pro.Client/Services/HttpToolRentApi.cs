using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Pro.Shared.Dtos;
using ToolRent.Services;

namespace Pro.Client.Services;

public sealed class HttpToolRentApi : IToolRentApi
{
    private readonly HttpClient _http;
    private string? _token;

    public HttpToolRentApi(string baseUrl)
    {
        _http = new HttpClient { BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/") };
    }

    public void SetToken(string? token) => _token = token;
    private void ApplyAuth()
    {
        _http.DefaultRequestHeaders.Authorization = null;

        var token = _token ?? AppState.Token;
        if (!string.IsNullOrWhiteSpace(token))
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    // Auth
    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto req)
    {
        var resp = await _http.PostAsJsonAsync("api/auth/login", req);
        resp.EnsureSuccessStatusCode();

        var dto = await resp.Content.ReadFromJsonAsync<AuthResponseDto>()
                  ?? throw new Exception("Empty login response");

        _token = dto.Token;
        return dto;
    }

    public async Task RegisterAsync(RegisterRequestDto req)
    {
        var resp = await _http.PostAsJsonAsync("api/auth/register", req);
        resp.EnsureSuccessStatusCode();
    }

    // Categories
    public async Task<IReadOnlyList<CategoryDto>> GetCategoriesAsync()
    {
        ApplyAuth();
        return await _http.GetFromJsonAsync<List<CategoryDto>>("api/categories") ?? new();
    }
    // Tools
    public async Task<ToolFiltersDto> GetToolFiltersAsync()
        => await _http.GetFromJsonAsync<ToolFiltersDto>("api/tools/filters")
           ?? new ToolFiltersDto();

    public async Task<IReadOnlyList<ToolListItemDto>> GetToolsAsync(
        string category,
        string owner,
        float? minPrice = null,
        float? maxPrice = null,
        string? location = null,
        string? q = null
    )
    {
        if (string.Equals(category, "All", StringComparison.OrdinalIgnoreCase)) category = "";
        if (string.Equals(owner, "All", StringComparison.OrdinalIgnoreCase)) owner = "";

        var qs = new List<string>();

        if (!string.IsNullOrWhiteSpace(category))
            qs.Add($"category={Uri.EscapeDataString(category)}");

        if (!string.IsNullOrWhiteSpace(owner))
            qs.Add($"owner={Uri.EscapeDataString(owner)}");

        if (minPrice is not null)
            qs.Add($"minPrice={minPrice.Value.ToString(CultureInfo.InvariantCulture)}");

        if (maxPrice is not null)
            qs.Add($"maxPrice={maxPrice.Value.ToString(CultureInfo.InvariantCulture)}");

        if (!string.IsNullOrWhiteSpace(location))
            qs.Add($"location={Uri.EscapeDataString(location)}");

        if (!string.IsNullOrWhiteSpace(q))
            qs.Add($"search={Uri.EscapeDataString(q.Trim())}");

        var url = "api/tools" + (qs.Count > 0 ? "?" + string.Join("&", qs) : "");
        return await _http.GetFromJsonAsync<List<ToolListItemDto>>(url) ?? new();
    }

    
    public async Task<ToolDetailsDto?> GetToolAsync(Guid toolId)
    {
        var resp = await _http.GetAsync($"api/tools/{toolId}");

        if (resp.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;

        resp.EnsureSuccessStatusCode();

        return await resp.Content.ReadFromJsonAsync<ToolDetailsDto>();
    }
    public async Task<IReadOnlyList<ToolListItemDto>> GetMyToolsAsync()
    {
        ApplyAuth();
        return await _http.GetFromJsonAsync<List<ToolListItemDto>>("api/tools/mine") ?? new();
    }
    public async Task<ToolDetailsDto> CreateToolAsync(CreateToolRequestDto req)
    {
        ApplyAuth();
        var res = await _http.PostAsJsonAsync("api/tools", req);
        res.EnsureSuccessStatusCode();

        var body = await res.Content.ReadFromJsonAsync<ToolDetailsDto>();
        if (body is not null && body.Id != Guid.Empty) return body;

        var location = res.Headers.Location?.ToString();
        if (string.IsNullOrWhiteSpace(location))
            throw new InvalidOperationException("Create succeeded but server returned no body and no Location header.");

        var tool = await _http.GetFromJsonAsync<ToolDetailsDto>(location);
        return tool ?? throw new InvalidOperationException("Failed to load created tool from Location.");
    }

    public async Task UpdateToolAsync(Guid toolId, UpdateToolRequestDto req)
    {
        ApplyAuth();
        var resp = await _http.PutAsJsonAsync($"api/tools/{toolId}", req);
        resp.EnsureSuccessStatusCode();
    }

    public async Task DeleteToolAsync(Guid toolId)
    {
        ApplyAuth();
        var resp = await _http.DeleteAsync($"api/tools/{toolId}");
        resp.EnsureSuccessStatusCode();
    }

    // Borrow + Payment + History
    public async Task<PaymentInitiateResponseDto> InitiatePaymentAsync(PaymentInitiateRequestDto req)
    {
        ApplyAuth();
        var resp = await _http.PostAsJsonAsync("api/payments/initiate", req);
        resp.EnsureSuccessStatusCode();

        return (await resp.Content.ReadFromJsonAsync<PaymentInitiateResponseDto>())
               ?? throw new Exception("Empty initiate payment response");
    }
    
    public async Task<PaymentConfirmResponseDto> ConfirmPaymentAsync(Pro.Shared.Dtos.PaymentConfirmRequestDto req)
    {
        ApplyAuth();
        var resp = await _http.PostAsJsonAsync("api/payments/confirm", req);
        resp.EnsureSuccessStatusCode();

        return (await resp.Content.ReadFromJsonAsync<PaymentConfirmResponseDto>())
               ?? throw new Exception("Empty confirm payment response");
    }
    
    public async Task<ReceiptDto> GetReceiptAsync(Guid paymentId)
    {
        ApplyAuth();
        var resp = await _http.GetAsync($"api/payments/{paymentId}/receipt");
        resp.EnsureSuccessStatusCode();

        return (await resp.Content.ReadFromJsonAsync<ReceiptDto>())
               ?? throw new Exception("Empty receipt response");
    }
    public async Task<CreateBorrowResponseDto> CreateBorrowAsync(CreateBorrowRequestDto req)
    {
        ApplyAuth();
        var resp = await _http.PostAsJsonAsync("api/borrows", req);
        resp.EnsureSuccessStatusCode();

        return (await resp.Content.ReadFromJsonAsync<CreateBorrowResponseDto>())
               ?? throw new Exception("Empty create borrow response");
    }

    public async Task<IReadOnlyList<string>> GetBorrowItemNamesAsync(Guid borrowId)
    {
        ApplyAuth();
        return await _http.GetFromJsonAsync<List<string>>($"api/borrows/{borrowId}/items") ?? new List<string>();
    }

    public async Task<IReadOnlyList<PaymentHistoryItemDto>> GetPaymentHistoryAsync(DateTime? fromUtc, DateTime? toUtc)
    {
        ApplyAuth();
        var qs = new List<string>();
        if (fromUtc is not null) qs.Add($"fromUtc={Uri.EscapeDataString(fromUtc.Value.ToString("O"))}");
        if (toUtc is not null) qs.Add($"toUtc={Uri.EscapeDataString(toUtc.Value.ToString("O"))}");

        var url = "api/payments/history" + (qs.Count > 0 ? "?" + string.Join("&", qs) : "");
        return await _http.GetFromJsonAsync<List<PaymentHistoryItemDto>>(url) ?? new List<PaymentHistoryItemDto>();
    }
    
    // Reviews
    public async Task<CreateReviewResponseDto> CreateReviewAsync(Guid toolId, CreateReviewRequestDto req)
    {
        ApplyAuth();
        var resp = await _http.PostAsJsonAsync($"api/tools/{toolId}/reviews", req);
        resp.EnsureSuccessStatusCode();

        return (await resp.Content.ReadFromJsonAsync<CreateReviewResponseDto>())
               ?? throw new Exception("Empty create review response");
    }

    public async Task<IReadOnlyList<PendingReviewDto>> GetPendingReviewsAsync()
    {
        ApplyAuth();
        return await _http.GetFromJsonAsync<List<PendingReviewDto>>("api/reviews/pending") ?? new();
    }

    public async Task ApproveReviewAsync(Guid reviewId)
    {
        ApplyAuth();
        var resp = await _http.PostAsync($"api/reviews/{reviewId}/approve", content: null);
        resp.EnsureSuccessStatusCode();
    }

    public async Task RejectReviewAsync(Guid reviewId)
    {
        ApplyAuth();
        var resp = await _http.PostAsync($"api/reviews/{reviewId}/reject", content: null);
        resp.EnsureSuccessStatusCode();
    }
    
    public async Task DeleteReviewAsync(Guid toolId, Guid reviewId)
    {
        ApplyAuth();
        var resp = await _http.DeleteAsync($"api/tools/{toolId}/reviews/{reviewId}");

        if (!resp.IsSuccessStatusCode)
        {
            var body = await resp.Content.ReadAsStringAsync();
            var details = string.IsNullOrWhiteSpace(body) ? "(no response body)" : body;
            throw new InvalidOperationException($"{(int)resp.StatusCode} {resp.ReasonPhrase}: {details}");
        }
    }


}
