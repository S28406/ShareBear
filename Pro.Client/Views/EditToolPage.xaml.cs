using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Pro.Client.Services;
using Pro.Shared.Dtos;

namespace Pro.Client.Views;

public partial class EditToolPage : Page
{
    private readonly Guid _toolId;
    private ToolDetailsDto? _tool;

    public EditToolPage(Guid toolId)
    {
        InitializeComponent();
        _toolId = toolId;

        Loaded += async (_, __) => await LoadAsync();
    }

    private async System.Threading.Tasks.Task LoadAsync()
    {
        ErrorText.Visibility = Visibility.Collapsed;

        _tool = await Api.Instance.GetToolAsync(_toolId);
        if (_tool is null)
        {
            MessageBox.Show("Tool not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            NavigationService?.GoBack();
            return;
        }

        var cats = await Api.Instance.GetCategoriesAsync();
        CategoryComboBox.ItemsSource = cats;

        NameTextBox.Text = _tool.Name;
        DescriptionTextBox.Text = _tool.Description ?? "";
        PriceTextBox.Text = _tool.Price.ToString(CultureInfo.InvariantCulture);
        QuantityTextBox.Text = _tool.Quantity.ToString(CultureInfo.InvariantCulture);

        var match = cats.FirstOrDefault(c => string.Equals(c.Name, _tool.CategoryName, StringComparison.OrdinalIgnoreCase));
        if (match is not null)
            CategoryComboBox.SelectedValue = match.Id;
        else if (cats.Count > 0)
            CategoryComboBox.SelectedIndex = 0;

        ImageFileNameTextBox.Text = Path.GetFileName(_tool.ImagePath ?? "");
    }

    private async void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        ErrorText.Visibility = Visibility.Collapsed;

        try
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
                throw new Exception("Name is required.");

            if (CategoryComboBox.SelectedValue is not Guid categoryId || categoryId == Guid.Empty)
                throw new Exception("Pick a category.");

            if (!float.TryParse(PriceTextBox.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out var price))
                throw new Exception("Price must be a number (use dot like 12.5).");

            if (!int.TryParse(QuantityTextBox.Text, out var qty))
                throw new Exception("Quantity must be an integer.");

            if (price < 0) throw new Exception("Price must be >= 0.");
            if (qty < 0) throw new Exception("Quantity must be >= 0.");

            var req = new UpdateToolRequestDto(
                NameTextBox.Text.Trim(),
                DescriptionTextBox.Text.Trim(),
                price,
                qty,
                categoryId,
                ImageFileNameTextBox.Text.Trim()
            );

            await Api.Instance.UpdateToolAsync(_toolId, req);

            MessageBox.Show("Tool updated!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigationService?.Navigate(new ToolDetailsPage(_toolId));
        }
        catch (Exception ex)
        {
            ErrorText.Text = ex.Message;
            ErrorText.Visibility = Visibility.Visible;
        }
    }

    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
        if (NavigationService?.CanGoBack == true) NavigationService.GoBack();
        else NavigationService?.Navigate(new MyToolsPage());
    }
}
