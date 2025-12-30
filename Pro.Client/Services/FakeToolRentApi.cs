using Pro.Shared.Dtos;

namespace ToolRent.Services;

public class FakeToolRentApi : IToolRentApi
{
    private readonly List<ToolListItemDto> _tools;
    private readonly Dictionary<Guid, ToolDetailsDto> _details;
    private readonly List<PaymentHistoryItemDto> _payments = new();

    public FakeToolRentApi()
    {
        _tools = new()
        {
            new(Guid.Parse("11111111-1111-1111-1111-111111111111"), "Cordless Drill", 49.99f, "Drill.jpg", "Power Tools", "admin"),
            new(Guid.Parse("22222222-2222-2222-2222-222222222222"), "Hammer", 9.99f, "hamer.jpg", "Hand Tools", "john"),
        };

        var admin = new UserDtos(Guid.NewGuid(), "admin", "admin@example.com", "Admin");
        var john  = new UserDtos(Guid.NewGuid(), "john",  "john@example.com",  "Customer");

        _details = new()
        {
            [_tools[0].Id] = new ToolDetailsDto(
                _tools[0].Id, _tools[0].Name, "18V cordless power drill", _tools[0].Price, 10, _tools[0].ImagePath,
                _tools[0].CategoryName, _tools[0].OwnerUsername,
                new List<ReviewDto>
                {
                    new(Guid.NewGuid(), 5, "Great product", DateTime.UtcNow.AddDays(-2), john),
                    new(Guid.NewGuid(), 3, "Okay", DateTime.UtcNow.AddDays(-10), john)
                }
            ),
            [_tools[1].Id] = new ToolDetailsDto(
                _tools[1].Id, _tools[1].Name, "Heavy duty hammer", _tools[1].Price, 20, _tools[1].ImagePath,
                _tools[1].CategoryName, _tools[1].OwnerUsername,
                new List<ReviewDto>()
            )
        };

        // seed a sample payment
        _payments.Add(new PaymentHistoryItemDto(Guid.NewGuid(), DateTime.UtcNow.AddDays(-3), 49.99m, "Completed", "Card",
            Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")));
    }

    public Task<AuthResponseDto> LoginAsync(LoginRequestDto req)
    {
        // super fake: accept anything non-empty
        var user = req.UsernameOrEmail.Contains("admin", StringComparison.OrdinalIgnoreCase)
            ? new UserDtos(Guid.NewGuid(), "admin", "admin@example.com", "Admin")
            : new UserDtos(Guid.NewGuid(), "john", "john@example.com", "Customer");

        return Task.FromResult(new AuthResponseDto("dev-token", user));
    }

    public Task RegisterAsync(RegisterRequestDto req) => Task.CompletedTask;

    public Task<ToolFiltersDto> GetToolFiltersAsync()
    {
        var cats = _tools.Select(t => t.CategoryName).Distinct().OrderBy(x => x).ToList();
        var owners = _tools.Select(t => t.OwnerUsername).Distinct().OrderBy(x => x).ToList();
        return Task.FromResult(new ToolFiltersDto(cats, owners));
    }

    public Task<IReadOnlyList<ToolListItemDto>> GetToolsAsync(string category, string owner)
    {
        IEnumerable<ToolListItemDto> q = _tools;

        if (!string.Equals(category, "All", StringComparison.OrdinalIgnoreCase))
            q = q.Where(t => t.CategoryName == category);

        if (!string.Equals(owner, "All", StringComparison.OrdinalIgnoreCase))
            q = q.Where(t => t.OwnerUsername == owner);

        return Task.FromResult((IReadOnlyList<ToolListItemDto>)q.ToList());
    }

    public Task<ToolDetailsDto?> GetToolAsync(Guid toolId)
        => Task.FromResult(_details.TryGetValue(toolId, out var d) ? d : null);

    public Task<CreateBorrowResponseDto> CreateBorrowAsync(CreateBorrowRequestDto req)
    {
        var tool = _tools.First(t => t.Id == req.ToolId);
        var borrowId = Guid.NewGuid();
        var total = (decimal)tool.Price * req.Quantity;
        return Task.FromResult(new CreateBorrowResponseDto(borrowId, total));
    }

    public Task<IReadOnlyList<string>> GetBorrowItemNamesAsync(Guid borrowId)
        => Task.FromResult((IReadOnlyList<string>)new List<string> { "— (fake items)" });

    public Task ConfirmPaymentAsync(PaymentConfirmRequestDto req)
    {
        _payments.Add(new PaymentHistoryItemDto(Guid.NewGuid(), DateTime.UtcNow, req.Amount, req.Status, req.Method, req.BorrowId));
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<PaymentHistoryItemDto>> GetPaymentHistoryAsync(DateTime? fromUtc, DateTime? toUtc)
    {
        IEnumerable<PaymentHistoryItemDto> q = _payments;

        if (fromUtc.HasValue) q = q.Where(p => p.Date >= fromUtc.Value);
        if (toUtc.HasValue) q = q.Where(p => p.Date <= toUtc.Value);

        return Task.FromResult((IReadOnlyList<PaymentHistoryItemDto>)q.OrderByDescending(p => p.Date).ToList());
    }
}
