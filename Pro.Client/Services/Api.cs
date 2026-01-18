using System;
using System.IO;
using ToolRent.Services;

namespace Pro.Client.Services;

public static class Api
{
    // public const string DefaultBaseUrl = "http://localhost:5262";
    public const string DefaultBaseUrl = "https://sharebear.onrender.com";
    public static string BaseUrl { get; private set; } = DefaultBaseUrl;
    public static IToolRentApi Instance { get; private set; } = new HttpToolRentApi(DefaultBaseUrl);
    static Api()
    {
        Init();
    }
    public static void Init()
    {
        var urlFromFile = TryReadBaseUrlFromFile();

        var finalUrl = string.IsNullOrWhiteSpace(urlFromFile)
            ? DefaultBaseUrl
            : urlFromFile!;

        BaseUrl = NormalizeBaseUrl(finalUrl);
        Instance = new HttpToolRentApi(BaseUrl);
    }
    private static string? TryReadBaseUrlFromFile()
    {
        try
        {
            var path = Path.Combine(AppContext.BaseDirectory, "api-url.txt");
            if (!File.Exists(path))
                return null;
                
            var text = File.ReadAllText(path).Trim();
            return string.IsNullOrWhiteSpace(text) ? null : text;
        }
        catch
        {
            return null;
        }
    }
    private static string NormalizeBaseUrl(string url)
    {
        url = url.Trim();
        while (url.EndsWith("/"))
            url = url.Substring(0, url.Length - 1);
        return url;
    }
}
