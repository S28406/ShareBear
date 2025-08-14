using System.Windows;

namespace ToolRent
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ContentFrame.Navigate(new Views.ToolListPage());
        }
    }
}