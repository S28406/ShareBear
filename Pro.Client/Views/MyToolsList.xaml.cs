using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Pro.Client.Helpers;
using Pro.Client.Services;

namespace Pro.Client.Views;

public partial class MyToolsPage : Page
{
    public MyToolsPage()
    {
        InitializeComponent();

        Loaded += async (_, __) => await LoadMyToolsAsync();
    }

    private async Task LoadMyToolsAsync()
    {
        try
        {
            var tools = await Api.Instance.GetMyToolsAsync();

            ToolListPanel.Items.Clear();

            foreach (var tool in tools)
            {
                var card = new Border
                {
                    Margin = new Thickness(10),
                    Padding = new Thickness(8),
                    Width = 240,
                    Background = Brushes.White,
                    CornerRadius = new CornerRadius(8),
                    BorderBrush = (Brush)new BrushConverter().ConvertFrom("#E7EDF4")!,
                    BorderThickness = new Thickness(1),
                    Cursor = Cursors.Hand,
                    Tag = tool.Id
                };

                var stack = new StackPanel();
                card.Child = stack;

                stack.Children.Add(new Image
                {
                    Height = 120,
                    Stretch = Stretch.UniformToFill,
                    Source = ImageHelper.Resolve(tool.ImagePath)
                });

                stack.Children.Add(new TextBlock
                {
                    Text = tool.Name,
                    FontWeight = FontWeights.Bold,
                    FontSize = 14,
                    Margin = new Thickness(0, 6, 0, 0),
                    Foreground = (Brush)new BrushConverter().ConvertFrom("#0D141C")!
                });

                stack.Children.Add(new TextBlock
                {
                    Text = $"${tool.Price:F2} / day",
                    FontSize = 12,
                    Foreground = (Brush)new BrushConverter().ConvertFrom("#48739D")!
                });

                var btnRow = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(0, 8, 0, 0)
                };

                var editBtn = new Button
                {
                    Content = "Edit",
                    Width = 80,
                    Height = 28,
                    Margin = new Thickness(0, 0, 8, 0),
                    Background = (Brush)new BrushConverter().ConvertFrom("#E7EDF4")!,
                    Foreground = (Brush)new BrushConverter().ConvertFrom("#0D141C")!,
                    FontWeight = FontWeights.Bold
                };
                editBtn.Click += (_, __) =>
                    NavigationService?.Navigate(new EditToolPage(tool.Id));

                var delBtn = new Button
                {
                    Content = "Delete",
                    Width = 80,
                    Height = 28,
                    Background = (Brush)new BrushConverter().ConvertFrom("#FDE2E2")!,
                    Foreground = (Brush)new BrushConverter().ConvertFrom("#8A1F1F")!,
                    FontWeight = FontWeights.Bold
                };
                delBtn.Click += async (_, __) => await DeleteToolAsync(tool.Id, tool.Name);

                btnRow.Children.Add(editBtn);
                btnRow.Children.Add(delBtn);

                stack.Children.Add(btnRow);

                card.MouseLeftButtonUp += (_, __) =>
                    NavigationService?.Navigate(new ToolDetailsPage((Guid)card.Tag));

                ToolListPanel.Items.Add(card);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Failed to load your tools:\n" + ex.Message, "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task DeleteToolAsync(Guid toolId, string toolName)
    {
        var confirm = MessageBox.Show(
            $"Delete '{toolName}'?\nThis cannot be undone.",
            "Confirm delete",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (confirm != MessageBoxResult.Yes)
            return;

        try
        {
            await Api.Instance.DeleteToolAsync(toolId);
            await LoadMyToolsAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Delete failed:\n" + ex.Message, "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
        if (NavigationService?.CanGoBack == true) NavigationService.GoBack();
        else NavigationService?.Navigate(new ToolListPage());
    }

    private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        => await LoadMyToolsAsync();
}
