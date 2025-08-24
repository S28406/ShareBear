using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.EntityFrameworkCore;
using PRO_;
using PRO.Data.Context;

namespace ToolRent.Views
{
    public partial class History : Page
    {
        private ObservableCollection<PurchaseRowVM> _rows = new();

        public History()
        {
            InitializeComponent();

            GridPayments.ItemsSource = _rows;

            FromPicker.SelectedDate = DateTime.Today.AddDays(-90);
            ToPicker.SelectedDate   = DateTime.Today;

            Loaded += async (_, __) => await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                if (AppState.CurrentUser is null)
                {
                    MessageBox.Show("Please sign in to view your purchase history.", "Sign in required",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var userId = AppState.CurrentUser.ID;

                var from = FromPicker.SelectedDate?.Date;
                DateTime? to = ToPicker.SelectedDate?.Date;           
                if (to.HasValue) to = to.Value.AddDays(1).AddTicks(-1);

                var status = (StatusBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "All";
                var search = (SearchBox.Text ?? "").Trim();

                await using var db = new ToolLendingContext();

                var query = db.Payments
                    .AsNoTracking()
                    .AsSplitQuery()
                    .Include(p => p.Borrow).ThenInclude(b => b.User)
                    .Where(p => p.Borrow != null && p.Borrow.User != null && p.Borrow.User.ID == userId);

                if (from.HasValue) query = query.Where(p => p.Date >= from.Value);
                if (to.HasValue)   query = query.Where(p => p.Date <= to.Value);

                if (!string.Equals(status, "All", StringComparison.OrdinalIgnoreCase))
                    query = query.Where(p => p.Status == status);

                if (!string.IsNullOrEmpty(search))
                {
                    if (Guid.TryParse(search, out var orderId))
                        query = query.Where(p => p.Orders_ID == orderId);
                    else
                        query = query.Where(p => EF.Functions.ILike(p.Method, $"%{search}%")); // if not Npgsql, use .Contains(...)
                }

                var list = await query
                    .OrderByDescending(p => p.Date)
                    .Select(p => new PurchaseRowVM
                    {
                        Id      = p.ID,
                        Date    = p.Date,
                        Amount  = (decimal)p.Ammount,
                        Status  = p.Status ?? "—",
                        Method  = p.Method ?? "—",
                        OrderId = p.Orders_ID
                    })
                    .ToListAsync();

                _rows = new(list);
                GridPayments.ItemsSource = _rows;

                UpdateSummary();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load purchase history:\n" + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateSummary()
        {
            var cnt = _rows.Count;
            var total = _rows.Sum(r => r.Amount);
            SummaryText.Text = $"{cnt} purchase{(cnt == 1 ? "" : "s")} • Total {total.ToString("C2", CultureInfo.CurrentCulture)}";

            var cut = DateTime.UtcNow.AddDays(-30);
            var last30 = _rows.Where(r => r.Date.ToUniversalTime() >= cut).Sum(r => r.Amount);
            FooterText.Text = $"Last 30 days: {last30.ToString("C2", CultureInfo.CurrentCulture)}";
        }

        private async void ApplyFilters_Click(object sender, RoutedEventArgs e) => await LoadDataAsync();

        private async void ResetFilters_Click(object sender, RoutedEventArgs e)
        {
            FromPicker.SelectedDate = DateTime.Today.AddDays(-90);
            ToPicker.SelectedDate   = DateTime.Today;
            StatusBox.SelectedIndex = 0;
            SearchBox.Text = "";
            await LoadDataAsync();
        }

        private void Details_Click(object sender, RoutedEventArgs e)
        {
            if (((FrameworkElement)sender).DataContext is PurchaseRowVM row)
            {
                var sb = new StringBuilder();
                sb.AppendLine($"Date:      {row.Date:yyyy-MM-dd HH:mm}");
                sb.AppendLine($"Amount:    {row.Amount.ToString("C2", CultureInfo.CurrentCulture)}");
                sb.AppendLine($"Status:    {row.Status}");
                sb.AppendLine($"Method:    {row.Method}");
                sb.AppendLine($"Order ID:  {row.OrderId}");
                MessageBox.Show(sb.ToString(), "Payment Details", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }

    public class PurchaseRowVM
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } = "—";
        public string Method { get; set; } = "—";
        public Guid OrderId { get; set; }

        public string AmountDisplay => Amount.ToString("C2", CultureInfo.CurrentCulture);
    }

    public class BoolToVisibilityInverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is bool b && b ? Visibility.Collapsed : Visibility.Visible;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
