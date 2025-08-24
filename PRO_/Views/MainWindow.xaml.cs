using System.Windows;
using PRO_;
using PRO.Models;
using ToolRent.Views;
using History = ToolRent.Views.History;

namespace ToolRent
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ContentFrame.Navigate(new Views.ToolListPage());
        }
        
        private void GoRegister_Click(object sender, RoutedEventArgs e)
            => ContentFrame.Navigate(new RegPage());

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