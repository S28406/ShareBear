using System;
using System.Diagnostics;
using System.IO;
using System.Net.Cache;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Pro.Client.Services;
using Pro.Shared.Dtos;

namespace Pro.Client.Views
{
    public partial class ToolListPage : Page
    {
        public ToolListPage()
        {
            InitializeComponent();
            Loaded += async (_, __) =>
            {
                await LoadFiltersAsync();
                await LoadToolsAsync();
            };
        }

        private async Task LoadFiltersAsync()
        {
            CategoryFilterComboBox.Items.Clear();
            UserFilterComboBox.Items.Clear();

            CategoryFilterComboBox.Items.Add("All");
            UserFilterComboBox.Items.Add("All");

            var filters = await Api.Instance.GetToolFiltersAsync();

            foreach (var c in filters.Categories) CategoryFilterComboBox.Items.Add(c);
            foreach (var o in filters.Owners) UserFilterComboBox.Items.Add(o);

            CategoryFilterComboBox.SelectedIndex = 0;
            UserFilterComboBox.SelectedIndex = 0;
        }

        private async Task LoadToolsAsync()
        {
            string cat = CategoryFilterComboBox.SelectedItem?.ToString() ?? "All";
            string owner = UserFilterComboBox.SelectedItem?.ToString() ?? "All";

            if (cat == "All") cat = "";
            if (owner == "All") owner = "";
            
            var tools = await Api.Instance.GetToolsAsync(cat, owner);

            ToolListPanel.Items.Clear();

            foreach (var tool in tools)
            {
                var card = new Border
                {
                    Margin = new Thickness(10),
                    Padding = new Thickness(8),
                    Width = 200,
                    Background = Brushes.White,
                    CornerRadius = new CornerRadius(8),
                    BorderBrush = (Brush)new BrushConverter().ConvertFrom("#E7EDF4")!,
                    BorderThickness = new Thickness(1),
                    Cursor = Cursors.Hand,
                    Tag = tool.Id
                };

                var stack = new StackPanel();
                card.Child = stack;

                stack.Children.Add(new Image
                {
                    Height = 120,
                    Stretch = Stretch.UniformToFill,
                    Source = ResolveImage(tool.ImagePath)
                });

                stack.Children.Add(new TextBlock
                {
                    Text = tool.Name,
                    FontWeight = FontWeights.Bold,
                    FontSize = 14,
                    Margin = new Thickness(0, 6, 0, 0),
                    Foreground = (Brush)new BrushConverter().ConvertFrom("#0D141C")!
                });

                stack.Children.Add(new TextBlock
                {
                    Text = $"${tool.Price:F2} / day",
                    FontSize = 12,
                    Foreground = (Brush)new BrushConverter().ConvertFrom("#48739D")!
                });

                card.MouseLeftButtonUp += (_, __) =>
                    NavigationService?.Navigate(new ToolDetailsPage((Guid)card.Tag));

                ToolListPanel.Items.Add(card);
            }
        }

        // private static ImageSource ResolveImage(string? dbPath)
        // {
        //     try
        //     {
        //         var baseDir = AppContext.BaseDirectory;
        //         var raw = (dbPath ?? "").Replace('/', '\\').Trim();
        //
        //         string fullPath;
        //         if (Path.IsPathRooted(raw)) fullPath = raw;
        //         else
        //         {
        //             var p1 = Path.Combine(baseDir, raw);
        //             var p2 = Path.Combine(baseDir, "Images", Path.GetFileName(raw ?? ""));
        //             fullPath = File.Exists(p1) ? p1 : p2;
        //         }
        //
        //         if (File.Exists(fullPath))
        //         {
        //             var bmp = new BitmapImage();
        //             bmp.BeginInit();
        //             bmp.CacheOption = BitmapCacheOption.OnLoad;
        //             bmp.UriSource = new Uri(fullPath, UriKind.Absolute);
        //             bmp.EndInit();
        //             return bmp;
        //         }
        //     }
        //     catch { }
        //
        //     return new BitmapImage(new Uri("pack://application:,,,/Images/placeholder.jpg"));
        // }

        // private static BitmapImage MakeBitmap(Uri uri)
        // {
        //     var bmp = new BitmapImage();
        //     bmp.BeginInit();
        //     bmp.UriSource = uri;
        //     bmp.CacheOption = BitmapCacheOption.OnLoad;
        //     bmp.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
        //     bmp.EndInit();
        //     bmp.Freeze(); // safe for WPF
        //     return bmp;
        // }
        
        private static ImageSource ResolveImage(string? imagePath)
        {
            var baseUrl = Pro.Client.Services.Api.BaseUrl; // we’ll add this const
            var path = string.IsNullOrWhiteSpace(imagePath) ? "/images/placeholder.jpg" : imagePath;

            // if server ever returns "Drill.jpg", normalize it
            if (!path.StartsWith("/")) path = "/images/" + path;

            var full = new Uri(new Uri(baseUrl + "/"), path.TrimStart('/'));
            return MakeBitmap(full);
        }
        
        private static BitmapImage MakeBitmap(Uri uri)
        {
            var bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.CacheOption = BitmapCacheOption.OnLoad; // fully load into memory
            bmp.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            bmp.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            bmp.UriSource = uri;
            bmp.EndInit();
            // bmp.Freeze(); // now safe
            return bmp;
        }

        private async void Filter_Changed(object sender, RoutedEventArgs e)
        {
            if (IsLoaded) await LoadToolsAsync();
        }
    }
}
