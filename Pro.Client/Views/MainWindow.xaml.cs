using System.Windows;
using System.Windows.Input;
using Pro.Client.Helpers;
using Pro.Client.Services;
using History = Pro.Client.Views.History;

namespace Pro.Client.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            RefreshHeader();
            ContentFrame.Navigate(new ToolListPage());
        }

        public void RefreshHeader()
        {
            UpdateAddProductVisibility();
            UpdateAuthButtons();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
            => RunSearch();

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                RunSearch();
        }

        private void RunSearch()
        {
            var q = SearchTextBox.Text?.Trim();
            ContentFrame.Navigate(new ToolListPage(q));
        }
        
        private void UpdateAddProductVisibility()
        {
            var canManage = RoleHelper.IsSellerOrAdmin(AppState.CurrentUser);

            AddProductBtn.Visibility = canManage ? Visibility.Visible : Visibility.Collapsed;
            MyToolsBtn.Visibility = canManage ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateAuthButtons()
        {
            var loggedIn = AppState.CurrentUser is not null && !string.IsNullOrWhiteSpace(AppState.Token);

            RegisterBtn.Visibility = loggedIn ? Visibility.Collapsed : Visibility.Visible;
            LoginBtn.Visibility = loggedIn ? Visibility.Collapsed : Visibility.Visible;

            LogoutBtn.Visibility = loggedIn ? Visibility.Visible : Visibility.Collapsed;
            HistoryBtn.Visibility = loggedIn ? Visibility.Visible : Visibility.Collapsed;
        }
        private void GoMyTools_Click(object sender, RoutedEventArgs e)
        {
            if (!RoleHelper.IsSellerOrAdmin(AppState.CurrentUser))
            {
                MessageBox.Show("Only sellers/admins can manage tools.", "Access denied",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            ContentFrame.Navigate(new MyToolsPage());
        }
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            AppState.CurrentUser = null;
            AppState.Token = null;

            RefreshHeader();
            ContentFrame.Navigate(new ToolListPage());
        }
        private void GoRegister_Click(object sender, RoutedEventArgs e)
            => ContentFrame.Navigate(new RegPage());

        private void GoLogin_Click(object sender, RoutedEventArgs e)
            => ContentFrame.Navigate(new LoginPage());

        private void GoAddProduct_Click(object sender, RoutedEventArgs e)
        {
            if (!RoleHelper.IsSellerOrAdmin(AppState.CurrentUser))
            {
                MessageBox.Show("Only sellers/admins can add products.", "Access denied",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            ContentFrame.Navigate(new AddToolPage());
        }

        private void GoHistory_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new History());
        }
    }
}