using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.EntityFrameworkCore;
using PRO.Data.Context;
using PRO.Models;

namespace ToolRent.Views
{
    public partial class ToolListPage : Page
    {
        private readonly ToolLendingContext _context = new ToolLendingContext();

        public ToolListPage()
        {
            InitializeComponent();
            Loaded += (_, __) => { LoadFilters(); LoadTools(); };
        }

        private void LoadFilters()
        {
            CategoryFilterComboBox.Items.Clear();
            CategoryFilterComboBox.Items.Add("All");
            foreach (var category in _context.Categories.OrderBy(c => c.Name))
                CategoryFilterComboBox.Items.Add(category.Name);
            CategoryFilterComboBox.SelectedIndex = 0;

            UserFilterComboBox.Items.Clear();
            UserFilterComboBox.Items.Add("All");
            foreach (var user in _context.Users.OrderBy(u => u.Username))
                UserFilterComboBox.Items.Add(user.Username);
            UserFilterComboBox.SelectedIndex = 0;
        }

        private void LoadTools()
        {
            string cat = CategoryFilterComboBox.SelectedItem?.ToString() ?? "All";
            string owner = UserFilterComboBox.SelectedItem?.ToString() ?? "All";

            var q = _context.Tools
                            .Include(t => t.Category)
                            .Include(t => t.User)
                            .AsQueryable();

            if (cat != "All")   q = q.Where(t => t.Category.Name == cat);
            if (owner != "All") q = q.Where(t => t.User.Username == owner);

            var tools = q.ToList();

            ToolListPanel.Items.Clear();

            foreach (var tool in tools)
            {
                var card = new Border
                {
                    Margin = new Thickness(10),
                    Padding = new Thickness(8),
                    Width = 200,
                    Background = Brushes.White,
                    CornerRadius = new CornerRadius(8),
                    BorderBrush = (Brush)new BrushConverter().ConvertFrom("#E7EDF4")!,
                    BorderThickness = new Thickness(1),
                    Cursor = Cursors.Hand,
                    Tag = tool.ID
                };

                var stack = new StackPanel();
                card.Child = stack;

                var image = new Image
                {
                    Height = 120,
                    Stretch = Stretch.UniformToFill,
                    Source = ResolveImage(tool.ImagePath)
                };
                stack.Children.Add(image);

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

                card.MouseLeftButtonUp += (_, __) =>
                {
                    NavigationService?.Navigate(new ToolDetailsPage((Guid)card.Tag));
                };

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
                    fullPath = raw;
                else
                {
                    var p1 = Path.Combine(baseDir, raw);
                    var p2 = Path.Combine(baseDir, "Images", Path.GetFileName(raw ?? ""));
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
            catch { }

            return new BitmapImage(new Uri("pack://application:,,,/Images/placeholder.jpg"));
        }

        private void Filter_Changed(object sender, RoutedEventArgs e)
        {
            if (IsLoaded) LoadTools();
        }
    }
}
