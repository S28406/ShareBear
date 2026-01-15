using System.Windows;

namespace Pro.Client.Views;

public partial class ReturnDialog : Window
{
    public string Condition { get; private set; } = "";
    public string Damage { get; private set; } = "None";

    public ReturnDialog()
    {
        InitializeComponent();
        ConditionBox.Text = "Looks good.";
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    private void Submit_Click(object sender, RoutedEventArgs e)
    {
        var dmg = (DamageBox.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content?.ToString() ?? "None";
        var cond = (ConditionBox.Text ?? "").Trim();

        if (string.IsNullOrWhiteSpace(cond))
        {
            MessageBox.Show("Please write condition notes.", "Validation",
                MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        Damage = dmg.Trim();
        Condition = cond;

        DialogResult = true;
        Close();
    }
}