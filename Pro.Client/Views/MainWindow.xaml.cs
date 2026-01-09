using System.Windows;
using Pro.Client.Helpers;
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
        }

        private void UpdateAddProductVisibility()
        {
            var canManage = RoleHelper.IsSellerOrAdmin(AppState.CurrentUser);

            AddProductBtn.Visibility = canManage ? Visibility.Visible : Visibility.Collapsed;
            MyToolsBtn.Visibility = canManage ? Visibility.Visible : Visibility.Collapsed;
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
            if (AppState.CurrentUser is null)
            {
                MessageBox.Show("Please sign in to view your purchase history.");
                return;
            }

            ContentFrame.Navigate(new History());
        }
    }
}