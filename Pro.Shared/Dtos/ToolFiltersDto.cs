namespace Pro.Shared.Dtos;

public sealed class ToolFiltersDto
{
    public List<string> Categories { get; set; } = new();
    public List<string> Owners { get; set; } = new();
    
    public List<string> Locations { get; set; } = new();
    public float? MinPrice { get; set; }
    public float? MaxPrice { get; set; }
    
    public ToolFiltersDto() { }

    public ToolFiltersDto(List<string> categories, List<string> owners, List<string> locations, float? minPrice, float? maxPrice)
    {
        Categories = categories ?? new();
        Owners = owners ?? new();
        Locations = locations ?? new();
        MinPrice = minPrice;
        MaxPrice = maxPrice;
    }
}