using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using Pro.Client.Helpers;
using Pro.Client.Services;
using Pro.Shared.Dtos;

namespace Pro.Client.Views;

public partial class AddToolPage : Page
{
    public AddToolPage()
    {
        InitializeComponent();

        Loaded += async (_, __) =>
        {
            if (!RoleHelper.IsSellerOrAdmin(AppState.CurrentUser))
            {
                MessageBox.Show("Only sellers/admins can add tools.");
                MessageBox.Show(
                    $"DEBUG ADD CHECK\n" +
                    $"User null: {AppState.CurrentUser is null}\n" +
                    $"Role: '{AppState.CurrentUser?.Role ?? "NULL"}'\n" +
                    $"IsSellerOrAdmin: {RoleHelper.IsSellerOrAdmin(AppState.CurrentUser)}",
                    "DEBUG ADD");
                NavigationService?.GoBack();
                return;
            }

            var cats = await Api.Instance.GetCategoriesAsync();
            CategoryComboBox.ItemsSource = cats;
            CategoryComboBox.DisplayMemberPath = "Name";
            CategoryComboBox.SelectedValuePath = "Id";
            if (cats.Count > 0) CategoryComboBox.SelectedIndex = 0;
        };
    }

    private async void CreateButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (CategoryComboBox.SelectedValue is not Guid categoryId || categoryId == Guid.Empty)
                throw new Exception("Pick a category.");

            if (!float.TryParse(PriceTextBox.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out var price))
                throw new Exception("Price must be a number (use dot like 12.5).");

            if (!int.TryParse(QuantityTextBox.Text, out var qty))
                throw new Exception("Quantity must be an integer.");
            
            var location = LocationTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(location))
                throw new Exception("Location is required.");

            var req = new CreateToolRequestDto(
                NameTextBox.Text.Trim(),
                DescriptionTextBox.Text.Trim(),
                price,
                qty,
                categoryId,
                location,
                ImageFileNameTextBox.Text.Trim()
            );

            var created = await Api.Instance.CreateToolAsync(req);

            MessageBox.Show("Tool created!");
            NavigationService?.Navigate(new ToolDetailsPage(created.Id));
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Create tool failed", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}