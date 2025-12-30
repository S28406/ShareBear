namespace Pro.Shared.Dtos;

public sealed class ToolFiltersDto
{
    public List<string> Categories { get; set; } = new();
    public List<string> Owners { get; set; } = new();
    
    public ToolFiltersDto() { }

    public ToolFiltersDto(List<string> categories, List<string> owners)
    {
        Categories = categories ?? new();
        Owners = owners ?? new();
    }
}