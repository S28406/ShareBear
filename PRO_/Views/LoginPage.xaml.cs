// ToolRent/Views/LoginPage.xaml.cs
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using PRO_;
using PRO.Data.Context;
using ToolRent.Security;

namespace ToolRent.Views
{
    public partial class LoginPage : Page
    {
        public LoginPage() => InitializeComponent();

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            ErrorText.Visibility = Visibility.Collapsed;

            var input = UserOrEmailBox.Text.Trim();
            var pass  = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(input) || string.IsNullOrWhiteSpace(pass))
            {
                ShowError("Enter your username/email and password.");
                return;
            }

            using var db = new ToolLendingContext();

            var user = await db.Users
                .FirstOrDefaultAsync(u => u.Username == input || u.Email == input);

            if (user == null)
            {
                ShowError("Invalid credentials.");
                return;
            }

            var ok = PasswordHelper.Verify(pass, user.PasswordSalt, user.PasswordHash);
            if (!ok)
            {
                ShowError("Invalid credentials.");
                return;
            }

            AppState.CurrentUser = user;

            // Navigate to your main page (e.g., ToolListPage)
            NavigationService?.Navigate(new ToolListPage());
        }

        private void GoToRegister_Click(object sender, RoutedEventArgs e)
            => NavigationService?.Navigate(new RegPage());

        private void ShowError(string msg)
        {
            ErrorText.Text = msg;
            ErrorText.Visibility = Visibility.Visible;
        }
    }
}