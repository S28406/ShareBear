using System;
using System.IO;
using System.Net.Http;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Pro.Client.Helpers;

public static class ImageHelper
{
    private static readonly HttpClient Http = new HttpClient
    {
        Timeout = TimeSpan.FromSeconds(10)
    };

    public static ImageSource Resolve(string imagePath)
    {
        var placeholder = new BitmapImage(new Uri("pack://application:,,,/Images/placeholder.jpg"));

        try
        {
            if (string.IsNullOrWhiteSpace(imagePath))
                return placeholder;

            Uri uri;

            if (Uri.TryCreate(imagePath, UriKind.Absolute, out var abs))
            {
                uri = abs;
            }
            else
            {
                var baseUrl = Pro.Client.Services.Api.BaseUrl.TrimEnd('/') + "/";
                uri = new Uri(new Uri(baseUrl), imagePath.TrimStart('/'));
            }

            return Make(uri) ?? placeholder;
        }
        catch
        {
            return placeholder;
        }
    }

    private static ImageSource? Make(Uri uri)
    {
        try
        {
            if (uri.Scheme.Equals(Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase) ||
                uri.Scheme.Equals(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase))
            {
                var bytes = Http.GetByteArrayAsync(uri).GetAwaiter().GetResult();

                using var ms = new MemoryStream(bytes);

                var bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
                bmp.StreamSource = ms;
                bmp.EndInit();
                bmp.Freeze();
                return bmp;
            }

            var local = new BitmapImage();
            local.BeginInit();
            local.CacheOption = BitmapCacheOption.OnLoad;
            local.CreateOptions = BitmapCreateOptions.IgnoreImageCache | BitmapCreateOptions.IgnoreColorProfile;
            local.UriSource = uri;
            local.EndInit();
            local.Freeze();
            return local;
        }
        catch
        {
            return null;
        }
    }
}
