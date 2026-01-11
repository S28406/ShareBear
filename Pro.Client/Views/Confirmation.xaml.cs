using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Pro.Client;
using Pro.Client.Services;
using Pro.Shared.Dtos;

namespace Pro.Client.Views
{
    public partial class PaymentConfirmationPage : Page
    {
        private readonly Guid _borrowId;
        private readonly decimal _total;
        private readonly DateTime _start;
        private readonly DateTime _end;

        public PaymentConfirmationPage(Guid borrowId, decimal total, DateTime start, DateTime end)
        {
            InitializeComponent();
            _borrowId = borrowId;
            _total = total;
            _start = start;
            _end = end;

            BookingDatesText.Text = $"Booking: {_start:yyyy-MM-dd} → {_end:yyyy-MM-dd}";
            TotalText.Text = $"{_total:0.00}";
        }

        private async void PaymentConfirmationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (AppState.CurrentUser is null)
            {
                MessageBox.Show("Please sign in to confirm a payment.", "Sign in required",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService?.GoBack();
                return;
            }

            OrderIdText.Text = $"Order ID: {_borrowId}";
            UserText.Text = $"User: {AppState.CurrentUser.Username}";
            EmailText.Text = $"Email: {AppState.CurrentUser.Email}";
            CreatedText.Text = $"Now: {DateTime.Now:g}";
            TotalText.Text = _total.ToString("C2", CultureInfo.CurrentCulture);

            await TryLoadItemsAsync();
            UpdateConfirmEnabled();
        }

        private async Task TryLoadItemsAsync()
        {
            try
            {
                var names = await Api.Instance.GetBorrowItemNamesAsync(_borrowId);
                ItemsList.ItemsSource = names.Count > 0 ? names : new List<string> { "—" };
            }
            catch
            {
                ItemsList.ItemsSource = new List<string> { "—" };
            }
        }

        private string GetSelectedMethod()
            => (MethodBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "";

        private string GetSuggestedStatus(string method)
            => string.Equals(method, "Cash", StringComparison.OrdinalIgnoreCase) ? "Pending" : "Completed";

        private void MethodBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var method = GetSelectedMethod();
            StatusPreview.Text = string.IsNullOrEmpty(method) ? "—" : GetSuggestedStatus(method);
            UpdateConfirmEnabled();
        }

        private void AgreeBox_Changed(object sender, RoutedEventArgs e) => UpdateConfirmEnabled();

        private void UpdateConfirmEnabled()
        {
            ConfirmBtn.IsEnabled = AgreeBox.IsChecked == true && !string.IsNullOrEmpty(GetSelectedMethod());
        }

        private void ShowError(string msg)
        {
            ErrorText.Text = msg;
            ErrorText.Visibility = Visibility.Visible;
        }

        private void HideError()
        {
            ErrorText.Visibility = Visibility.Collapsed;
            ErrorText.Text = string.Empty;
        }

        private async void Confirm_Click(object sender, RoutedEventArgs e)
        {
            HideError();
            if (AppState.CurrentUser is null) { ShowError("Please sign in."); return; }

            var method = GetSelectedMethod();
            if (string.IsNullOrEmpty(method)) { ShowError("Please choose a payment method."); return; }

            var status = GetSuggestedStatus(method);

            try
            {
                await Api.Instance.ConfirmPaymentAsync(new PaymentConfirmRequestDto(_borrowId, _total, method, status));
                MessageBox.Show("Payment confirmed.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService?.Navigate(new History());
            }
            catch (Exception ex)
            {
                ShowError("Failed to confirm payment: " + ex.Message);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService?.CanGoBack == true) NavigationService.GoBack();
        }
    }
}
