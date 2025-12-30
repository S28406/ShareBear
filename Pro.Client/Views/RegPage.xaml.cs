using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using Pro.Client.Services;
using Pro.Shared.Dtos;

namespace ToolRent.Views
{
    public partial class RegPage : Page
    {
        public RegPage() => InitializeComponent();

        private async void Register_Click(object sender, RoutedEventArgs e)
        {
            ErrorText.Visibility = Visibility.Collapsed;

            var username = UsernameBox.Text.Trim();
            var email = EmailBox.Text.Trim();
            var pass = PasswordBox.Password;
            var confirm = ConfirmBox.Password;

            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(pass))
            {
                ShowError("All fields are required.");
                return;
            }

            if (!Regex.IsMatch(email, @"^\S+@\S+\.\S+$"))
            {
                ShowError("Email is invalid.");
                return;
            }

            if (pass.Length < 6)
            {
                ShowError("Password must be at least 6 characters.");
                return;
            }

            if (pass != confirm)
            {
                ShowError("Passwords do not match.");
                return;
            }

            try
            {
                await Api.Instance.RegisterAsync(new RegisterRequestDto(username, email, pass));
                MessageBox.Show("Account created. Please sign in.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService?.Navigate(new LoginPage());
            }
            catch (System.Exception ex)
            {
                ShowError("Registration failed: " + ex.Message);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService?.CanGoBack == true) NavigationService.GoBack();
            else NavigationService?.Navigate(new ToolListPage());
        }

        private void GoToLogin_Click(object sender, RoutedEventArgs e)
            => NavigationService?.Navigate(new LoginPage());

        private void ShowError(string msg)
        {
            ErrorText.Text = msg;
            ErrorText.Visibility = Visibility.Visible;
        }
    }
}
