using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PRO.Data.Context;

namespace PRO_
{
    public partial class MainWindow : Window
    {
        private readonly ToolLendingContext _context;

        public MainWindow(ToolLendingContext context)
        {
            InitializeComponent();
            _context = context;

            LoadFilters();
            LoadTools();
        }

        private void LoadFilters()
        {
            // Populate Brand and Ownership filters
            var brands = _context.Tools
                .Select(t => t.Brand)
                .Distinct()
                .OrderBy(b => b)
                .ToList();

            var ownerships = _context.Tools
                .Select(t => t.Ownership)
                .Distinct()
                .OrderBy(o => o)
                .ToList();

            BrandFilterComboBox.Items.Add("All");
            foreach (var brand in brands)
                BrandFilterComboBox.Items.Add(brand);
            BrandFilterComboBox.SelectedIndex = 0;

            OwnershipFilterComboBox.Items.Add("All");
            foreach (var ownership in ownerships)
                OwnershipFilterComboBox.Items.Add(ownership);
            OwnershipFilterComboBox.SelectedIndex = 0;
        }

        private void LoadTools()
        {
            var selectedBrand = BrandFilterComboBox.SelectedItem?.ToString();
            var selectedOwnership = OwnershipFilterComboBox.SelectedItem?.ToString();

            var tools = _context.Tools.AsQueryable();

            if (!string.IsNullOrEmpty(selectedBrand) && selectedBrand != "All")
                tools = tools.Where(t => t.Brand == selectedBrand);

            if (!string.IsNullOrEmpty(selectedOwnership) && selectedOwnership != "All")
                tools = tools.Where(t => t.Ownership == selectedOwnership);

            ToolsListBox.ItemsSource = tools.ToList();
        }

        private void OnFilterChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
                LoadTools();
        }
    }
}
