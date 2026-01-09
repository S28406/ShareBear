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

            RefreshHeader(); // show/hide buttons based on role + login
            ContentFrame.Navigate(new ToolListPage());
        }

        public void RefreshHeader()
        {
            UpdateAddProductVisibility();
        }

        private void UpdateAddProductVisibility()
        {
            AddProductBtn.Visibility =
                RoleHelper.IsSellerOrAdmin(AppState.CurrentUser)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
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



// using System.Windows;
// using Pro.Shared.Security;
// using Pro.Client.Helpers;
// using Pro.Client.Views;
// using History = Pro.Client.Views.History;
//
// namespace Pro.Client.Views
// {
//     public partial class MainWindow : Window
//     {
//         public void RefreshHeader()
//         {
//             UpdateAddProductVisibility();
//         }
//         public MainWindow()
//         {
//             InitializeComponent();
//             RefreshHeader();
//             ContentFrame.Navigate(new Views.ToolListPage());
//         }
//         private void UpdateAddProductVisibility()
//         {
//             AddProductBtn.Visibility =
//                 Roles.IsSeller(AppState.CurrentUser) ? Visibility.Visible : Visibility.Collapsed;
//         }
//
//         private void GoRegister_Click(object sender, RoutedEventArgs e)
//             => ContentFrame.Navigate(new RegPage());
//
//         private void GoAddProduct_Click(object sender, RoutedEventArgs e)
//         {
//             if (!Roles.IsSeller(AppState.CurrentUser))
//             {
//                 MessageBox.Show("Only sellers can add products.", "Access denied",
//                     MessageBoxButton.OK, MessageBoxImage.Information);
//                 return;
//             }
//             // TODO: Create new page
//             ContentFrame.Navigate(new AddToolPage());
//         }
//
//         private void GoLogin_Click(object sender, RoutedEventArgs e)
//             => ContentFrame.Navigate(new LoginPage());
//         
//         private void GoHistory_Click(object sender, RoutedEventArgs e)
//         {
//             if (AppState.CurrentUser is null)
//             {
//                 MessageBox.Show("Please sign in to view your purchase history.");
//                 return;
//             }
//
//             ContentFrame.Navigate(new History());
//         }
//     }
// }