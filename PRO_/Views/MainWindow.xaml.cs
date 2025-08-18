using System.Windows;
using ToolRent.Views;

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
    }
}