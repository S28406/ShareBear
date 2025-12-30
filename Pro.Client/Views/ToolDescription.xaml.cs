using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Pro.Client;
using Pro.Client.Services;
using Pro.Shared.Dtos;

namespace Pro.Client.Views
{
    public partial class ToolDetailsPage : Page
    {
        private readonly Guid _toolId;
        private ToolDetailsDto? _tool;

        public ToolDetailsPage(Guid toolId)
        {
            InitializeComponent();
            _toolId = toolId;

            Loaded += async (_, __) =>
            {
                await LoadDataAsync();
                BackButton.IsEnabled = NavigationService?.CanGoBack ?? false;
            };
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService?.CanGoBack == true) NavigationService.GoBack();
            else NavigationService?.Navigate(new ToolListPage());
        }

        private async System.Threading.Tasks.Task LoadDataAsync()
        {
            _tool = await Api.Instance.GetToolAsync(_toolId);
            if (_tool == null) return;

            ToolName.Text = _tool.Name;
            CategoryName.Text = $"Category: {_tool.CategoryName}";
            OwnerName.Text = $"Owner: {_tool.OwnerUsername}";
            PriceText.Text = $"Price: ${_tool.Price:F2} / day";
            DescriptionText.Text = _tool.Description ?? "";

            HeroImage.Source = ResolveImage(_tool.ImagePath);

            ReviewsList.ItemsSource = _tool.Reviews?
                .OrderByDescending(r => r.Date)
                .ToList();
        }

        private static BitmapImage ResolveImage(string? dbPath)
        {
            try
            {
                var baseDir = AppContext.BaseDirectory;
                var raw = (dbPath ?? "").Replace('/', '\\').Trim();

                string fullPath;
                if (Path.IsPathRooted(raw)) fullPath = raw;
                else
                {
                    var p1 = Path.Combine(baseDir, raw);
                    var p2 = Path.Combine(baseDir, "Images", Path.GetFileName(raw ?? ""));
                    fullPath = File.Exists(p1) ? p1 : p2;
                }

                if (File.Exists(fullPath))
                {
                    var bmp = new BitmapImage();
                    bmp.BeginInit();
                    bmp.CacheOption = BitmapCacheOption.OnLoad;
                    bmp.UriSource = new Uri(fullPath, UriKind.Absolute);
                    bmp.EndInit();
                    return bmp;
                }
            }
            catch { }

            var ph = new BitmapImage();
            ph.BeginInit();
            ph.UriSource = new Uri("pack://application:,,,/Images/placeholder.jpg");
            ph.EndInit();
            return ph;
        }

        private async void RentButtonClick(object sender, RoutedEventArgs e)
        {
            if (_tool == null) return;

            if (AppState.CurrentUser is null)
            {
                MessageBox.Show("Please sign in to rent this tool.", "Sign in required",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                var res = await Api.Instance.CreateBorrowAsync(new CreateBorrowRequestDto(_tool.Id, 1));
                NavigationService?.Navigate(new PaymentConfirmationPage(res.BorrowId, res.Total));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not start checkout:\n" + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
