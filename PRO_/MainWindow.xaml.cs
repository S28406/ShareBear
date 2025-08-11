using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PRO.Models;
using PRO.Data.Context;

namespace ToolRent
{
    public partial class MainWindow : Window
    {
        private readonly ToolLendingContext _context;

        public MainWindow()
        {
            InitializeComponent();
            _context = new ToolLendingContext();

            LoadFilters();
            LoadTools();
        }

        private void LoadFilters()
        {
            CategoryFilterComboBox.Items.Add("All");
            foreach (var category in _context.Categories.OrderBy(c => c.Name))
            {
                CategoryFilterComboBox.Items.Add(category.Name);
            }
            CategoryFilterComboBox.SelectedIndex = 0;

            UserFilterComboBox.Items.Add("All");
            foreach (var user in _context.Users.OrderBy(u => u.Username))
            {
                UserFilterComboBox.Items.Add(user.Username);
            }
            UserFilterComboBox.SelectedIndex = 0;
        }

        private void LoadTools()
        {
            string selectedCategory = CategoryFilterComboBox.SelectedItem?.ToString();
            string selectedUser = UserFilterComboBox.SelectedItem?.ToString();

            var tools = _context.Tools
                .Where(t =>
                    (selectedCategory == "All" || t.Category.Name == selectedCategory) &&
                    (selectedUser == "All" || t.User.Username == selectedUser))
                .ToList();

            ToolListPanel.Items.Clear();
            foreach (var tool in tools)
            {
                var card = new StackPanel
                {
                    Margin = new Thickness(10),
                    Width = 180
                };

                var image = new Image
                {
                    Height = 100,
                    Width = 180,
                    Stretch = Stretch.UniformToFill,
                    Source = ResolveImage(tool.ImagePath)
                };
                card.Children.Add(image);

                card.Children.Add(new TextBlock
                {
                    Text = tool.Name,
                    FontWeight = FontWeights.Bold,
                    FontSize = 14,
                    Margin = new Thickness(0, 5, 0, 0)
                });

                card.Children.Add(new TextBlock
                {
                    Text = $"${tool.Price:F2}/day",
                    FontSize = 12,
                    Foreground = Brushes.SteelBlue
                });

                ToolListPanel.Items.Add(card);
            }
        }

        private static ImageSource ResolveImage(string? dbPath)
        {
            try
            {
                var baseDir = AppContext.BaseDirectory;
                var raw = (dbPath ?? "").Replace('/', '\\').Trim();

                string fullPath;
                if (Path.IsPathRooted(raw))
                {
                    fullPath = raw;
                }
                else
                {
                    var p1 = Path.Combine(baseDir, raw);
                    var p2 = Path.Combine(baseDir, "Images", Path.GetFileName(raw));
                    fullPath = File.Exists(p1) ? p1 : p2;
                }

                if (File.Exists(fullPath))
                {
                    var bmp = new BitmapImage();
                    bmp.BeginInit();
                    bmp.CacheOption = BitmapCacheOption.OnLoad;
                    bmp.UriSource = new Uri(fullPath, UriKind.Absolute);
                    bmp.EndInit();
                    return bmp;
                }
            }
            catch
            {
                // ignored
            }

            return new BitmapImage(new Uri("pack://application:,,,/Images/placeholder.jpg"));
        }

        private void Filter_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
            {
                LoadTools();
            }
        }
    }
}
// using System;
// using System.IO;
// using System.Linq;
// using System.Windows;
// using System.Windows.Controls;
// using System.Windows.Media.Imaging;
// using PRO.Models;
// using PRO.Data.Context;
//
// namespace ToolRent
// {
//     public partial class MainWindow : Window
//     {
//         private readonly ToolLendingContext _context;
//
//         public MainWindow()
//         {
//             InitializeComponent();
//             _context = new ToolLendingContext();
//
//             LoadFilters();
//             LoadTools();
//         }
//
//         private void LoadFilters()
//         {
//             CategoryFilterComboBox.Items.Add("All");
//             foreach (var category in _context.Categories.OrderBy(c => c.Name))
//             {
//                 CategoryFilterComboBox.Items.Add(category.Name);
//             }
//             CategoryFilterComboBox.SelectedIndex = 0;
//
//             UserFilterComboBox.Items.Add("All");
//             foreach (var user in _context.Users.OrderBy(u => u.Username))
//             {
//                 UserFilterComboBox.Items.Add(user.Username);
//             }
//             UserFilterComboBox.SelectedIndex = 0;
//         }
//
//         private void LoadTools()
//         {
//             string selectedCategory = CategoryFilterComboBox.SelectedItem?.ToString();
//             string selectedUser = UserFilterComboBox.SelectedItem?.ToString();
//
//             var tools = _context.Tools
//                 .Where(t =>
//                     (selectedCategory == "All" || t.Category.Name == selectedCategory) &&
//                     (selectedUser == "All" || t.User.Username == selectedUser))
//                 .ToList();
//
//             ToolListPanel.Items.Clear();
//             foreach (var tool in tools)
//             {
//                 var card = new StackPanel
//                 {
//                     Margin = new Thickness(10),
//                     Width = 180
//                 };
//                 
//                 var image = new Image
//                 {
//                     Height = 100,
//                     Width = 180,
//                     Stretch = System.Windows.Media.Stretch.UniformToFill
//                 };
//                 try
//                 {
//                     if (!string.IsNullOrEmpty(tool.ImagePath) && File.Exists(tool.ImagePath))
//                     {
//                         image.Source = new BitmapImage(new Uri(tool.ImagePath));
//                     }
//                     else
//                     {
//                         image.Source = new BitmapImage(new Uri("D:\\Projects\\PRO_\\PRO_\\Images\\placeholder.jpg"));
//                     }
//                 }
//                 catch
//                 {
//                     image.Source = new BitmapImage(new Uri("D:\\Projects\\PRO_\\PRO_\\Images\\placeholder.jpg"));
//                 }
//                 card.Children.Add(image);
//
//                 card.Children.Add(new TextBlock
//                 {
//                     Text = tool.Name,
//                     FontWeight = FontWeights.Bold,
//                     FontSize = 14,
//                     Margin = new Thickness(0, 5, 0, 0)
//                 });
//
//                 card.Children.Add(new TextBlock
//                 {
//                     Text = $"${tool.Price:F2}/day",
//                     FontSize = 12,
//                     Foreground = System.Windows.Media.Brushes.SteelBlue
//                 });
//
//                 ToolListPanel.Items.Add(card);
//             }
//         }
//
//         private void Filter_Changed(object sender, SelectionChangedEventArgs e)
//         {
//             if (IsLoaded)
//             {
//                 LoadTools();
//             }
//         }
//     }
// }