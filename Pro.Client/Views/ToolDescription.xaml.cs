using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.EntityFrameworkCore;
using PRO_;
using PRO.Data.Context;
using PRO.Models;

namespace ToolRent.Views
{
    public partial class ToolDetailsPage : Page
    {
        private readonly Guid _toolId;
        private Tool? _tool;

        public ToolDetailsPage(Guid toolId)
        {
            InitializeComponent();
            _toolId = toolId;
            Loaded += (_, __) =>
            {
                LoadData();
                BackButton.IsEnabled = NavigationService?.CanGoBack ?? false;
            };
        }

        private void BackButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (NavigationService?.CanGoBack == true)
                NavigationService.GoBack();
            else
                NavigationService?.Navigate(new ToolListPage());
        }

        private void LoadData()
        {
            using var db = new ToolLendingContext();

            _tool = db.Tools
                .Include(t => t.Category)
                .Include(t => t.User)
                .Include(t => t.Reviews).ThenInclude(r => r.User)
                .FirstOrDefault(t => t.ID == _toolId);

            if (_tool == null) return;

            ToolName.Text        = _tool.Name;
            CategoryName.Text    = _tool.Category != null ? $"Category: {_tool.Category.Name}" : "Category: —";
            OwnerName.Text       = _tool.User != null ? $"Owner: {_tool.User.Username}" : "Owner: —";
            PriceText.Text       = $"Price: ${_tool.Price:F2} / day";
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
                if (Path.IsPathRooted(raw))
                    fullPath = raw;
                else
                {
                    var p1 = Path.Combine(baseDir, raw);
                    var p2 = Path.Combine(baseDir, "Images", Path.GetFileName(raw ?? ""));
                    fullPath = System.IO.File.Exists(p1) ? p1 : p2;
                }

                if (System.IO.File.Exists(fullPath))
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

        // private void RentButtonClick(object sender, RoutedEventArgs e)
        // {
        //     if (_tool == null) return;
        //
        //     if (AppState.CurrentUser is null)
        //     {
        //         MessageBox.Show("Please sign in to rent this tool.", "Sign in required",
        //             MessageBoxButton.OK, MessageBoxImage.Information);
        //         return;
        //     }
        //
        //     try
        //     {
        //         using var db = new ToolLendingContext();
        //
        //         // 1) Create a minimal Borrow (aka “order”) for the current user
        //         var borrow = new Borrow
        //         {
        //             ID = Guid.NewGuid(),
        //             Users_ID = AppState.CurrentUser.ID,  // <-- if your FK is UserID, rename to UserID
        //             // Optional fields – remove if your model doesn’t have them:
        //             // CreatedAt = DateTime.UtcNow,
        //             // Status = "Pending"
        //         };
        //         db.Borrows.Add(borrow);
        //
        //         // 2) Add one line item: this tool, qty = 1 (adjust names to your model)
        //         var line = new ProductBorrow
        //         {
        //             ID = Guid.NewGuid(),
        //             Orders_ID = borrow.ID,   // <-- if your FK is BorrowId, rename
        //             Tools_ID   = _tool.ID,    // <-- if your FK is ToolId, rename
        //             Quantity  = 1,
        //             // Optional: if your model tracks price per day on the line:
        //             // PricePerDay = _tool.Price
        //         };
        //         db.ProductBorrows.Add(line);
        //
        //         db.SaveChanges();
        //
        //         // 3) Compute total (default: 1 day)
        //         var total = (decimal)_tool.Price; // change if you have days/fees
        //
        //         // 4) Navigate to confirmation
        //         NavigationService?.Navigate(new PaymentConfirmationPage(borrow.ID, total));
        //     }
        //     catch (Exception ex)
        //     {
        //         MessageBox.Show("Could not start checkout:\n" + ex.Message, "Error",
        //             MessageBoxButton.OK, MessageBoxImage.Error);
        //     }
        // }
        private void RentButtonClick(object sender, RoutedEventArgs e)
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
        using var db = new ToolLendingContext();

        // IMPORTANT: re-fetch entities in THIS context (don't reuse objects from other contexts)
        var user = db.Users.FirstOrDefault(u => u.ID == AppState.CurrentUser.ID);
        var tool = db.Tools.FirstOrDefault(t => t.ID == _tool.ID);

        if (user is null || tool is null)
        {
            MessageBox.Show("User or tool not found.", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        // 1) Create the order (Borrow). Use NAVIGATION property to avoid FK-name mismatches.
        var borrow = new Borrow
        {
            ID = Guid.NewGuid(),
            User = user,
            // If your model has required fields, set sane defaults here:
            // CreatedAt = DateTime.UtcNow,
            // Status = "Pending",
            // StartDateUtc = DateTime.UtcNow,
            // EndDateUtc   = DateTime.UtcNow.AddDays(1),
        };

        // 2) Add the line item
        var line = new ProductBorrow
        {
            ID = Guid.NewGuid(),
            Order = borrow,
            Tool   = tool,
            Quantity = 1,
            // PricePerDay = tool.Price, // uncomment if required
        };

        db.Borrows.Add(borrow);
        db.ProductBorrows.Add(line);

        db.SaveChanges();

        // 3) Compute total (1 day by default)
        var total = (decimal)tool.Price;

        // 4) Go to confirmation
        NavigationService?.Navigate(new PaymentConfirmationPage(borrow.ID, total));
    }
    catch (DbUpdateException ex)
    {
        // Show the REAL DB reason (e.g., FK violation, NOT NULL)
        var msg = ex.InnerException?.Message ?? ex.Message;
        MessageBox.Show("Could not start checkout:\n" + msg, "Error",
            MessageBoxButton.OK, MessageBoxImage.Error);
    }
    catch (Exception ex)
    {
        MessageBox.Show("Could not start checkout:\n" + ex.Message, "Error",
            MessageBoxButton.OK, MessageBoxImage.Error);
    }
}
    }
}