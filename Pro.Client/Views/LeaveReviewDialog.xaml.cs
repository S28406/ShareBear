using System;
using System.Windows;
using System.Windows.Controls;

namespace Pro.Client.Views;

public partial class LeaveReviewDialog : Window
{
    public int Rating { get; private set; } = 5;
    public string Description { get; private set; } = "";

    public LeaveReviewDialog()
    {
        InitializeComponent();
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }

    private void Submit_Click(object sender, RoutedEventArgs e)
    {
        var item = RatingBox.SelectedItem as ComboBoxItem;
        var ratingStr = item?.Content?.ToString() ?? "5";
        Rating = int.TryParse(ratingStr, out var r) ? r : 5;

        Description = (DescBox.Text ?? "").Trim();
        if (string.IsNullOrWhiteSpace(Description))
        {
            MessageBox.Show("Please write a short description.", "Validation",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        DialogResult = true;
    }
}