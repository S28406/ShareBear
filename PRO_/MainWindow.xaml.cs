using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PRO.Data.Context;

namespace PRO_;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly ToolLendingContext _context;

    public MainWindow(ToolLendingContext context)
    {
        InitializeComponent();
        _context = context;

        LoadData();
    }

    private void LoadData()
    {
        UsersListBox.ItemsSource = _context.Users.ToList();
        ToolsListBox.ItemsSource = _context.Tools.ToList();
    }
}