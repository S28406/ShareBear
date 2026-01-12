using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Pro.Client.Helpers;
using Pro.Client.Services;
using Pro.Shared.Dtos;

namespace Pro.Client.Views
{
    public partial class ToolDetailsPage : Page
    {
        private readonly Guid _toolId;
        private ToolDetailsDto? _tool;
        public bool IsAdmin => AppState.CurrentUser?.Role == "Admin";
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
            if (_tool is null)
            {
                MessageBox.Show("Tool not found (maybe it was not saved or the API returned a wrong id).");
                NavigationService?.GoBack();
                return;
            }

            ToolName.Text = _tool.Name;
            CategoryName.Text = $"Category: {_tool.CategoryName}";
            OwnerName.Text = $"Owner: {_tool.OwnerUsername}";
            PriceText.Text = $"Price: ${_tool.Price:F2} / day";
            DescriptionText.Text = _tool.Description ?? "";

            HeroImage.Source = ImageHelper.Resolve(_tool.ImagePath);

            ReviewsList.ItemsSource = _tool.Reviews?
                .OrderByDescending(r => r.Date)
                .ToList();
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
            
            var start = DateTime.Now;
            var end = DateTime.Today.AddDays(1);
            var qty = 1;

            try
            {
                var res = await Api.Instance.CreateBorrowAsync(
                    new CreateBorrowRequestDto(_tool.Id, qty, start, end)
                );
            
                NavigationService?.Navigate(new PaymentConfirmationPage(res.BorrowId, res.Total, start, end));
            
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not start checkout:\n" + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async void LeaveReview_Click(object sender, RoutedEventArgs e)
        {
            if (_tool is null) return;

            if (AppState.CurrentUser is null)
            {
                MessageBox.Show("Please sign in to leave a review.", "Sign in required",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var dlg = new LeaveReviewDialog();
            if (dlg.ShowDialog() != true) return;

            try
            {
                var res = await Api.Instance.CreateReviewAsync(
                    _tool.Id,
                    new CreateReviewRequestDto(dlg.Rating, dlg.Description)
                );

                MessageBox.Show($"Review submitted. Status: {res.Status}", "Thanks!",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not submit review:\n" + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async void DeleteReview_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn || btn.Tag is not Guid reviewId)
                return;

            if (AppState.CurrentUser?.Role != "Admin")
            {
                MessageBox.Show("Only admins can delete reviews.");
                return;
            }

            var confirm = MessageBox.Show("Delete this review permanently?",
                "Confirm delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (confirm != MessageBoxResult.Yes) return;

            try
            {
                await Api.Instance.DeleteReviewAsync(_toolId, reviewId);
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete review:\n" + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}
