using System.Windows;
using System.Windows.Controls;
using Pro.Client;
using Pro.Client.Services;
using Pro.Shared.Dtos;

namespace ToolRent.Views
{
    public partial class LoginPage : Page
    {
        public LoginPage() => InitializeComponent();

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            ErrorText.Visibility = Visibility.Collapsed;

            var input = UserOrEmailBox.Text.Trim();
            var pass = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(input) || string.IsNullOrWhiteSpace(pass))
            {
                ShowError("Enter your username/email and password.");
                return;
            }

            try
            {
                var auth = await Api.Instance.LoginAsync(new LoginRequestDto(input, pass));
                AppState.Token = auth.Token;
                AppState.CurrentUser = auth.User;

                // Refresh header buttons if you want
                if (Application.Current.MainWindow is ToolRent.MainWindow mw)
                    mw.RefreshHeader();

                NavigationService?.Navigate(new ToolListPage());
            }
            catch
            {
                ShowError("Invalid credentials.");
            }
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