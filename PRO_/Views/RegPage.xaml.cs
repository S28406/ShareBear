// ToolRent/Views/RegPage.xaml.cs
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using PRO.Data.Context;
using PRO.Models;
using ToolRent.Security;

namespace ToolRent.Views
{
    public partial class RegPage : Page
    {
        public RegPage() => InitializeComponent();

        private async void Register_Click(object sender, RoutedEventArgs e)
        {
            ErrorText.Visibility = Visibility.Collapsed;
            var username = UsernameBox.Text.Trim();
            var email    = EmailBox.Text.Trim();
            var pass     = PasswordBox.Password;
            var confirm  = ConfirmBox.Password;

            // Basic validation
            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(email)    ||
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

            using var db = new ToolLendingContext();

            // Uniqueness
            var exists = await db.Users
                .AnyAsync(u => u.Username == username || u.Email == email);

            if (exists)
            {
                ShowError("Username or Email already in use.");
                return;
            }

            // Hash + store
            var (hash, salt) = PasswordHelper.Hash(pass);

            var user = new User
            {
                ID = Guid.NewGuid(),
                Username = username,
                Email = email,
                PasswordHash = hash,
                PasswordSalt = salt,
                Role = "Customer"
            };

            db.Users.Add(user);
            await db.SaveChangesAsync();

            MessageBox.Show("Account created. Please sign in.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigationService?.Navigate(new LoginPage());
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
