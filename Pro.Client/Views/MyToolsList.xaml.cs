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

    private Brush B(string hex) => (Brush)new BrushConverter().ConvertFrom(hex)!;

    private T Res<T>(string key) where T : class
        => (Resources[key] as T)
           ?? (Application.Current?.TryFindResource(key) as T)
           ?? throw new InvalidOperationException($"Missing resource: {key}");

    
    private async Task LoadMyToolsAsync()
    {
        try
        {
            var tools = await Api.Instance.GetMyToolsAsync();
            ToolListPanel.Items.Clear();

            var cardBg = Res<Brush>("CardBg");
            var borderC = Res<Brush>("BorderC");
            var textSecondary = Res<Brush>("TextSecondary");

            Style? secondaryBtnStyle = Resources["SecondaryButton"] as Style;
            Style? primaryBtnStyle = Resources["PrimaryButton"] as Style;

            foreach (var tool in tools)
            {
                var card = new Border
                {
                    Margin = new Thickness(10),
                    Padding = new Thickness(10),
                    Width = 240,
                    Background = cardBg,
                    CornerRadius = new CornerRadius(14),
                    BorderBrush = borderC,
                    BorderThickness = new Thickness(1),
                    Cursor = Cursors.Hand,
                    Tag = tool.Id
                };

                var stack = new StackPanel();
                card.Child = stack;

                var imgWrap = new Border
                {
                    CornerRadius = new CornerRadius(10),
                    ClipToBounds = true,
                    BorderBrush = borderC,
                    BorderThickness = new Thickness(1),
                    Background = B("#0E1627")
                };

                imgWrap.Child = new Image
                {
                    Height = 120,
                    Stretch = Stretch.UniformToFill,
                    Source = ImageHelper.Resolve(tool.ImagePath)
                };

                stack.Children.Add(imgWrap);

                stack.Children.Add(new TextBlock
                {
                    Text = tool.Name,
                    FontWeight = FontWeights.Bold,
                    FontSize = 14,
                    Margin = new Thickness(0, 10, 0, 0),
                    Foreground = B("#E5E7EB"),
                    TextWrapping = TextWrapping.Wrap
                });

                stack.Children.Add(new TextBlock
                {
                    Text = $"${tool.Price:F2} / day",
                    FontSize = 12,
                    Margin = new Thickness(0, 4, 0, 0),
                    Foreground = textSecondary
                });

                var btnRow = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(0, 10, 0, 0)
                };

                var editBtn = new Button
                {
                    Content = "Edit",
                    Width = 96,
                    Height = 34,
                    Margin = new Thickness(0, 0, 10, 0),
                    FontWeight = FontWeights.SemiBold,
                    Cursor = Cursors.Hand
                };

                if (secondaryBtnStyle != null) editBtn.Style = secondaryBtnStyle;
                else
                {
                    editBtn.Background = B("#17233A");
                    editBtn.Foreground = B("#E5E7EB");
                    editBtn.BorderBrush = borderC;
                    editBtn.BorderThickness = new Thickness(1);
                }

                editBtn.Click += (_, __) =>
                    NavigationService?.Navigate(new EditToolPage(tool.Id));

                var delBtn = new Button
                {
                    Content = "Delete",
                    Width = 96,
                    Height = 34,
                    FontWeight = FontWeights.SemiBold,
                    Cursor = Cursors.Hand
                };

                // Dark-danger look (still clearly “delete”)
                if (primaryBtnStyle != null)
                {
                    delBtn.Background = B("#3B1B1B");
                    delBtn.Foreground = B("#FCA5A5");
                    delBtn.BorderBrush = B("#7F1D1D");
                    delBtn.BorderThickness = new Thickness(1);
                }
                else
                {
                    delBtn.Background = B("#3B1B1B");
                    delBtn.Foreground = B("#FCA5A5");
                    delBtn.BorderBrush = B("#7F1D1D");
                    delBtn.BorderThickness = new Thickness(1);
                }

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
