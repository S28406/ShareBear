using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using PRO_;
using PRO.Data.Context;
using PRO.Models;

namespace ToolRent.Views
{
    public partial class PaymentConfirmationPage : Page
    {
        private readonly Guid _borrowId;
        private readonly decimal _total; 

        public PaymentConfirmationPage(Guid borrowId, decimal total)
        {
            InitializeComponent();

            _borrowId = borrowId;
            _total = total;

            Loaded += PaymentConfirmationPage_Loaded;
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
            UserText.Text    = $"User: {AppState.CurrentUser.Username}";
            EmailText.Text   = $"Email: {AppState.CurrentUser.Email}";
            CreatedText.Text = $"Now: {DateTime.Now:g}";
            TotalText.Text   = _total.ToString("C2", CultureInfo.CurrentCulture);

            await TryLoadItemsAsync();
            UpdateConfirmEnabled();
        }

        private async Task TryLoadItemsAsync()
        {
            try
            {
                await using var db = new ToolLendingContext();

                var borrow = await db.Borrows
                    .AsNoTracking()
                    .Include(b => b.ProductBorrows)
                        .ThenInclude(pb => pb.Tool)
                    .FirstOrDefaultAsync(b => b.ID == _borrowId);

                if (borrow?.ProductBorrows != null && borrow.ProductBorrows.Any())
                {
                    var names = borrow.ProductBorrows
                        .Where(pb => pb.Tool != null)
                        .Select(pb => pb.Tool.Name)
                        .Distinct()
                        .ToList();

                    ItemsList.ItemsSource = names;
                }
                else
                {
                    ItemsList.ItemsSource = new List<string> { "—" };
                }
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

            if (AppState.CurrentUser is null)
            {
                ShowError("Please sign in.");
                return;
            }

            var method = GetSelectedMethod();
            if (string.IsNullOrEmpty(method))
            {
                ShowError("Please choose a payment method.");
                return;
            }

            var status = GetSuggestedStatus(method);

            try
            {
                await using var db = new ToolLendingContext();

                var owned = await db.Borrows
                    .Include(b => b.User)
                    .AnyAsync(b => b.ID == _borrowId && b.User != null && b.User.ID == AppState.CurrentUser.ID);

                if (!owned)
                {
                    ShowError("Order not found or not accessible.");
                    return;
                }

                var payment = new Payment
                {
                    ID        = Guid.NewGuid(),
                    Date      = DateTime.UtcNow,              
                    Ammount   = (float)_total,                   
                    Status    = status,
                    Method    = method,
                    Orders_ID = _borrowId
                };

                db.Payments.Add(payment);
                await db.SaveChangesAsync();

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
