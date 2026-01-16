using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Pro.Client.Services;
using Pro.Shared.Dtos;

namespace Pro.Client.Views
{
    public partial class History : Page
    {
        public string ReturnStatus { get; set; } = "—";
        private ObservableCollection<PurchaseRowVm> _rows = new();

        public History()
        {
            InitializeComponent();

            GridPayments.ItemsSource = _rows;

            FromPicker.SelectedDate = DateTime.Today.AddDays(-90);
            ToPicker.SelectedDate = DateTime.Today;

            Loaded += async (_, __) => await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var fromUtc = ToUtcStartOfDay(FromPicker.SelectedDate);
                var toUtc = ToUtcEndOfDay(ToPicker.SelectedDate);

                var status = (StatusBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "All";
                var search = (SearchBox.Text ?? "").Trim();

                var list = await Api.Instance.GetPaymentHistoryAsync(fromUtc, toUtc);

                if (!string.Equals(status, "All", StringComparison.OrdinalIgnoreCase))
                    list = list.Where(p => string.Equals(p.Status, status, StringComparison.OrdinalIgnoreCase)).ToList();

                if (!string.IsNullOrWhiteSpace(search))
                    list = ApplySearch(list, search);

                var borrowIds = list.Select(p => p.OrderId).Distinct().ToList();

                var returnedMap = new System.Collections.Generic.Dictionary<Guid, bool>();

                var tasks = borrowIds.Select(async id =>
                {
                    try
                    {
                        var ret = await Api.Instance.TryGetReturnAsync(id);
                        return (id, returned: ret is not null);
                    }
                    catch
                    {
                        return (id, returned: false);
                    }
                });

                var results = await Task.WhenAll(tasks);
                foreach (var r in results)
                    returnedMap[r.id] = r.returned;

                var vm = list
                    .OrderByDescending(p => p.Date)
                    .Select(p => new PurchaseRowVm
                    {
                        Id = p.PaymentId,
                        Date = p.Date,
                        Amount = p.Amount,
                        Status = p.Status ?? "—",
                        Method = p.Method ?? "—",
                        OrderId = p.OrderId,
                        ReturnStatus = (returnedMap.TryGetValue(p.OrderId, out var isReturned) && isReturned)
                            ? "Returned"
                            : "—"
                    })
                    .ToList();

                _rows = new ObservableCollection<PurchaseRowVm>(vm);
                GridPayments.ItemsSource = _rows;

                UpdateSummary();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load purchase history:\n" + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public class PurchaseRowVm
        {
            public Guid Id { get; set; }
            public DateTime Date { get; set; }
            public decimal Amount { get; set; }
            public string Status { get; set; } = "—";
            public string Method { get; set; } = "—";
            public Guid OrderId { get; set; }

            public string ReturnStatus { get; set; } = "—";

            public bool IsReturnAllowed =>
                string.Equals(Status, PaymentStatuses.Confirmed, StringComparison.OrdinalIgnoreCase)
                && !string.Equals(ReturnStatus, "Returned", StringComparison.OrdinalIgnoreCase);

            public string AmountDisplay => Amount.ToString("C2", CultureInfo.CurrentCulture);
        }
        
        private static List<PaymentHistoryItemDto> ApplySearch(
            IReadOnlyList<PaymentHistoryItemDto> items, string search)
        {
            var terms = search.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            return items.Where(p =>
            {
                foreach (var raw in terms)
                {
                    var term = raw.Trim();

                    if (Guid.TryParse(term, out var guid))
                    {
                        if (p.OrderId == guid || p.PaymentId == guid) continue;
                        return false;
                    }

                    if (TryParseLocalDate(term, out var localDate))
                    {
                        var startUtc = ToUtcStartOfDay(localDate);
                        var endUtc = ToUtcEndOfDay(localDate);
                        if (startUtc.HasValue && endUtc.HasValue)
                        {
                            if (p.Date < startUtc.Value || p.Date > endUtc.Value) return false;
                            continue;
                        }
                    }

                    if (decimal.TryParse(term, NumberStyles.Any, CultureInfo.CurrentCulture, out var amtDec) ||
                        decimal.TryParse(term, NumberStyles.Any, CultureInfo.InvariantCulture, out amtDec))
                    {
                        if (Math.Abs(p.Amount - amtDec) < 0.005m) continue;
                        return false;
                    }

                    if ((p.Method ?? "").Contains(term, StringComparison.OrdinalIgnoreCase)) continue;
                    if ((p.Status ?? "").Contains(term, StringComparison.OrdinalIgnoreCase)) continue;
                    if (p.OrderId.ToString().Contains(term, StringComparison.OrdinalIgnoreCase)) continue;

                    return false;
                }

                return true;
            }).ToList();
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
            ToPicker.SelectedDate = DateTime.Today;
            StatusBox.SelectedIndex = 0;
            SearchBox.Text = "";
            await LoadDataAsync();
        }

        private async void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) await LoadDataAsync();
        }

        private void Details_Click(object sender, RoutedEventArgs e)
        {
            if (((FrameworkElement)sender).DataContext is PurchaseRowVm row)
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

        private static DateTime? ToUtcStartOfDay(DateTime? localDate)
        {
            if (!localDate.HasValue) return null;
            var local = DateTime.SpecifyKind(localDate.Value.Date, DateTimeKind.Local);
            return local.ToUniversalTime();
        }
        private static DateTime? ToUtcEndOfDay(DateTime? localDate)
        {
            if (!localDate.HasValue) return null;
            var localEnd = DateTime.SpecifyKind(localDate.Value.Date.AddDays(1).AddTicks(-1), DateTimeKind.Local);
            return localEnd.ToUniversalTime();
        }
        private static DateTime? ToUtcStartOfDay(DateTime localDate) => ToUtcStartOfDay((DateTime?)localDate);
        private static DateTime? ToUtcEndOfDay(DateTime localDate) => ToUtcEndOfDay((DateTime?)localDate);

        private static bool TryParseLocalDate(string s, out DateTime date)
        {
            return DateTime.TryParse(s, CultureInfo.CurrentCulture, DateTimeStyles.None, out date) ||
                   DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
        }
        private async void Return_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((FrameworkElement)sender).DataContext is not PurchaseRowVm row)
                    return;

                if (!string.Equals(row.Status, PaymentStatuses.Confirmed, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Only confirmed payments can be returned.", "Return",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var dlg = new ReturnDialog { Owner = Window.GetWindow(this) };
                if (dlg.ShowDialog() != true) return;

                await Api.Instance.CreateReturnAsync(
                    row.OrderId,
                    new CreateReturnRequestDto(dlg.Condition, dlg.Damage));

                MessageBox.Show("Return submitted", "Return",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Return failed:\n" + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    public class PurchaseRowVm
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