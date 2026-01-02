using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Pro.Client.Helpers;

public static class ImageHelper
{
    public static ImageSource Resolve(string imagePath)
    {
        var placeholder = new BitmapImage(new Uri("pack://application:,,,/Images/placeholder.jpg"));
    
        if (string.IsNullOrWhiteSpace(imagePath))
            return placeholder;
    
        if (Uri.TryCreate(imagePath, UriKind.Absolute, out var abs))
            return Make(abs);

        var baseUrl = Pro.Client.Services.Api.BaseUrl;
        var full = new Uri(new Uri(baseUrl + "/"), imagePath.TrimStart('/'));
        return Make(full);
    }

    
    private static BitmapImage Make(Uri uri)
    {
        var bmp = new BitmapImage();
        bmp.BeginInit();
        bmp.CacheOption = BitmapCacheOption.OnLoad;
        bmp.CreateOptions = BitmapCreateOptions.IgnoreImageCache
                            | BitmapCreateOptions.IgnoreColorProfile;
        bmp.UriSource = uri;
        bmp.EndInit();
        // bmp.Freeze();
        return bmp;
    }
}