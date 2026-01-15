using Pro.Shared.Dtos;

namespace ToolRent.Services;

public sealed class FakeToolRentApi : IToolRentApi
{
    private readonly List<CategoryDto> _categories = new()
    {
        new CategoryDto(Guid.Parse("11111111-1111-1111-1111-111111111111"), "Power Tools"),
        new CategoryDto(Guid.Parse("22222222-2222-2222-2222-222222222222"), "Hand Tools"),
        new CategoryDto(Guid.Parse("33333333-3333-3333-3333-333333333333"), "Stations"),
    };

    private readonly List<ToolListItemDto> _tools = new();
    private readonly Dictionary<Guid, ToolDetailsDto> _details = new();
    private readonly List<PaymentHistoryItemDto> _payments = new();

    private UserDtos _currentUser = new(Guid.NewGuid(), "admin", "admin@example.com", "Admin");

    public FakeToolRentApi()
    {
        Seed();
    }

    private void Seed()
    {
        var admin = new UserDtos(Guid.NewGuid(), "admin", "admin@example.com", "Admin");
        var john  = new UserDtos(Guid.NewGuid(), "john",  "john@example.com",  "Customer");

        var drillId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var hammerId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");

        _tools.Add(new ToolListItemDto(
            drillId,
            "Cordless Drill",
            49.99f,
            "/images/Drill.jpg",
            "Power Tools",
            "admin",
            "Warsaw"
        ));

        _tools.Add(new ToolListItemDto(
            hammerId,
            "Hammer",
            9.99f,
            "/images/hamer.jpg",
            "Hand Tools",
            "john",
            "Krakow"
        ));

        _details[drillId] = new ToolDetailsDto(
            drillId,
            "Cordless Drill",
            "18V cordless power drill",
            49.99f,
            10,
            "/images/Drill.jpg",
            "Power Tools",
            "admin",
            "Warsaw",
            new List<ReviewDto>
            {
                new(Guid.NewGuid(), 5, "Great product", DateTime.UtcNow.AddDays(-2), john),
                new(Guid.NewGuid(), 3, "Okay", DateTime.UtcNow.AddDays(-10), john)
            }
        );

        _details[hammerId] = new ToolDetailsDto(
            hammerId,
            "Hammer",
            "Heavy duty hammer",
            9.99f,
            20,
            "/images/hamer.jpg",
            "Hand Tools",
            "john",
            "Krakow",
            new List<ReviewDto>()
        );

        _payments.Add(new PaymentHistoryItemDto(
            Guid.NewGuid(),
            DateTime.UtcNow.AddDays(-3),
            49.99m,
            "Completed",
            "Card",
            drillId
        ));
    }

    private static string NormalizeImage(string? fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return "/images/placeholder.jpg";

        if (fileName.StartsWith("/images/", StringComparison.OrdinalIgnoreCase))
            return fileName;

        if (fileName.StartsWith("/", StringComparison.OrdinalIgnoreCase))
            return fileName;

        return "/images/" + fileName.Trim();
    }

    // Auth
    public Task<AuthResponseDto> LoginAsync(LoginRequestDto req)
    {
        _currentUser = req.UsernameOrEmail.Contains("admin", StringComparison.OrdinalIgnoreCase)
            ? new UserDtos(Guid.NewGuid(), "admin", "admin@example.com", "Admin")
            : new UserDtos(Guid.NewGuid(), "john", "john@example.com", "Customer");

        return Task.FromResult(new AuthResponseDto("dev-token", _currentUser));
    }

    public Task RegisterAsync(RegisterRequestDto req) => Task.CompletedTask;

    // Categories
    public Task<IReadOnlyList<CategoryDto>> GetCategoriesAsync()
        => Task.FromResult((IReadOnlyList<CategoryDto>)_categories);

    // Tool Filters
    public Task<ToolFiltersDto> GetToolFiltersAsync()
    {
        var cats = _tools.Select(t => t.CategoryName).Distinct().OrderBy(x => x).ToList();
        var owners = _tools.Select(t => t.OwnerUsername).Distinct().OrderBy(x => x).ToList();
        var locations = _tools.Select(t => t.Location).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().OrderBy(x => x).ToList();

        float? minPrice = _tools.Count == 0 ? null : _tools.Min(t => (float?)t.Price);
        float? maxPrice = _tools.Count == 0 ? null : _tools.Max(t => (float?)t.Price);

        return Task.FromResult(new ToolFiltersDto(cats, owners, locations, minPrice, maxPrice));
    }

    public Task<IReadOnlyList<ToolListItemDto>> GetToolsAsync(string category, string owner, float? minPrice = null, float? maxPrice = null,
        string? location = null, string? q = null)
    {
        throw new NotImplementedException();
    }

    // Tools List
    public Task<IReadOnlyList<ToolListItemDto>> GetToolsAsync(
        string category,
        string owner,
        float? minPrice = null,
        float? maxPrice = null,
        string? location = null
    )
    {
        IEnumerable<ToolListItemDto> q = _tools;

        if (!string.IsNullOrWhiteSpace(category))
            q = q.Where(t => t.CategoryName == category);

        if (!string.IsNullOrWhiteSpace(owner))
            q = q.Where(t => t.OwnerUsername == owner);

        if (!string.IsNullOrWhiteSpace(location))
            q = q.Where(t => string.Equals(t.Location, location, StringComparison.OrdinalIgnoreCase));

        if (minPrice is not null)
            q = q.Where(t => t.Price >= minPrice.Value);

        if (maxPrice is not null)
            q = q.Where(t => t.Price <= maxPrice.Value);

        return Task.FromResult((IReadOnlyList<ToolListItemDto>)q.OrderBy(t => t.Name).ToList());
    }

    public Task<ToolDetailsDto?> GetToolAsync(Guid toolId)
        => Task.FromResult(_details.TryGetValue(toolId, out var d) ? d : null);

    public Task<IReadOnlyList<ToolListItemDto>> GetMyToolsAsync()
    {
        if (_currentUser.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            return Task.FromResult((IReadOnlyList<ToolListItemDto>)_tools.ToList());

        if (_currentUser.Role.Equals("Seller", StringComparison.OrdinalIgnoreCase))
        {
            var mine = _tools.Where(t => t.OwnerUsername == _currentUser.Username).ToList();
            return Task.FromResult((IReadOnlyList<ToolListItemDto>)mine);
        }

        return Task.FromResult((IReadOnlyList<ToolListItemDto>)new List<ToolListItemDto>());
    }

    public Task<ToolDetailsDto> CreateToolAsync(CreateToolRequestDto req)
    {
        var id = Guid.NewGuid();

        var catName = _categories.FirstOrDefault(c => c.Id == req.CategoryId)?.Name ?? "Unknown";
        var owner = _currentUser.Username;

        var img = NormalizeImage(req.ImageFileName);
        var loc = string.IsNullOrWhiteSpace(req.Location) ? "Unknown" : req.Location.Trim();

        var listItem = new ToolListItemDto(
            id,
            req.Name.Trim(),
            req.Price,
            img,
            catName,
            owner,
            loc
        );

        _tools.Add(listItem);

        var details = new ToolDetailsDto(
            id,
            req.Name.Trim(),
            req.Description?.Trim() ?? "",
            req.Price,
            req.Quantity,
            img,
            catName,
            owner,
            loc,
            new List<ReviewDto>()
        );

        _details[id] = details;

        return Task.FromResult(details);
    }

    public Task UpdateToolAsync(Guid toolId, UpdateToolRequestDto req)
    {
        if (!_details.TryGetValue(toolId, out var existing))
            return Task.CompletedTask;

        var catName = _categories.FirstOrDefault(c => c.Id == req.CategoryId)?.Name ?? existing.CategoryName;
        var img = NormalizeImage(req.ImageFileName);
        var loc = string.IsNullOrWhiteSpace(req.Location) ? existing.Location : req.Location.Trim();

        var updatedDetails = existing with
        {
            Name = req.Name.Trim(),
            Description = req.Description?.Trim() ?? "",
            Price = req.Price,
            Quantity = req.Quantity,
            ImagePath = img,
            CategoryName = catName,
            Location = loc
        };
        _details[toolId] = updatedDetails;

        var idx = _tools.FindIndex(t => t.Id == toolId);
        if (idx >= 0)
        {
            var old = _tools[idx];
            _tools[idx] = old with
            {
                Name = req.Name.Trim(),
                Price = req.Price,
                ImagePath = img,
                CategoryName = catName,
                Location = loc
            };
        }

        return Task.CompletedTask;
    }

    public Task DeleteToolAsync(Guid toolId)
    {
        _details.Remove(toolId);
        _tools.RemoveAll(t => t.Id == toolId);
        return Task.CompletedTask;
    }

    // Borrow + Payment + History
    public Task<CreateBorrowResponseDto> CreateBorrowAsync(CreateBorrowRequestDto req)
    {
        var tool = _tools.First(t => t.Id == req.ToolId);
        var borrowId = Guid.NewGuid();
        var Date1 = DateTime.Now;
        var Date2 = DateTime.Now;
        var total = (decimal)tool.Price * req.Quantity;
        return Task.FromResult(new CreateBorrowResponseDto(borrowId, total, Date2, Date1));
    }

    public Task<IReadOnlyList<string>> GetBorrowItemNamesAsync(Guid borrowId)
        => Task.FromResult((IReadOnlyList<string>)new List<string> { "— (fake items)" });

    public Task<PaymentInitiateResponseDto> InitiatePaymentAsync(PaymentInitiateRequestDto req)
    {
        throw new NotImplementedException();
    }

    Task<PaymentConfirmResponseDto> IToolRentApi.ConfirmPaymentAsync(PaymentConfirmRequestDto req)
    {
        throw new NotImplementedException();
    }

    public Task<ReceiptDto> GetReceiptAsync(Guid paymentId)
    {
        throw new NotImplementedException();
    }


    public Task<IReadOnlyList<PaymentHistoryItemDto>> GetPaymentHistoryAsync(DateTime? fromUtc, DateTime? toUtc)
    {
        IEnumerable<PaymentHistoryItemDto> q = _payments;

        if (fromUtc.HasValue) q = q.Where(p => p.Date >= fromUtc.Value);
        if (toUtc.HasValue) q = q.Where(p => p.Date <= toUtc.Value);

        return Task.FromResult((IReadOnlyList<PaymentHistoryItemDto>)q.OrderByDescending(p => p.Date).ToList());
    }

    public Task<CreateReviewResponseDto> CreateReviewAsync(Guid toolId, CreateReviewRequestDto req)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<PendingReviewDto>> GetPendingReviewsAsync()
    {
        throw new NotImplementedException();
    }

    public Task ApproveReviewAsync(Guid reviewId)
    {
        throw new NotImplementedException();
    }

    public Task RejectReviewAsync(Guid reviewId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteReviewAsync(Guid toolId, Guid reviewId)
    {
        throw new NotImplementedException();
    }

    public Task<ReturnDto> CreateReturnAsync(Guid borrowId, CreateReturnRequestDto req)
    {
        throw new NotImplementedException();
    }

    public Task<ReturnDto> GetReturnAsync(Guid borrowId)
    {
        throw new NotImplementedException();
    }

    public Task FinalizeReturnAsync(Guid borrowId, FinalizeReturnRequestDto req)
    {
        throw new NotImplementedException();
    }

    public Task<ReturnDto?> TryGetReturnAsync(Guid borrowId)
    {
        throw new NotImplementedException();
    }
}
