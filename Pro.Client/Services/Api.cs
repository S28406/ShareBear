using ToolRent.Services;

namespace Pro.Client.Services;

public static class Api
{
    
    public const string BaseUrl = "http://localhost:5262";
    public static IToolRentApi Instance { get; set; } =
        new HttpToolRentApi(BaseUrl);
}