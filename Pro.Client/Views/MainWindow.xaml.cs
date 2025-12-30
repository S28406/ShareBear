using System.Windows;
using PRO_;
using PRO.Security;
using ToolRent.Views;
using History = ToolRent.Views.History;

namespace ToolRent
{
    public partial class MainWindow : Window
    {
        public void RefreshHeader()
        {
            UpdateAddProductVisibility();
        }
        public MainWindow()
        {
            InitializeComponent();
            ContentFrame.Navigate(new Views.ToolListPage());
        }
        private void UpdateAddProductVisibility()
        {
            AddProductBtn.Visibility =
                Roles.IsSeller(AppState.CurrentUser) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void GoRegister_Click(object sender, RoutedEventArgs e)
            => ContentFrame.Navigate(new RegPage());

        private void GoAddProduct_Click(object sender, RoutedEventArgs e)
        {
            if (!Roles.IsSeller(AppState.CurrentUser))
            {
                MessageBox.Show("Only sellers can add products.", "Access denied",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            // TODO: Create new page
            ContentFrame.Navigate(new PRO_.Views.AddToolPage());
        }

        private void GoLogin_Click(object sender, RoutedEventArgs e)
            => ContentFrame.Navigate(new LoginPage());
        
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