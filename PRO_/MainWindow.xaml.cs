using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
                    Stretch = System.Windows.Media.Stretch.UniformToFill
                };
                try
                {
                    if (!string.IsNullOrEmpty(tool.ImmagePath) && File.Exists(tool.ImmagePath))
                    {
                        image.Source = new BitmapImage(new Uri(tool.ImmagePath));
                    }
                    else
                    {
                        image.Source = new BitmapImage(new Uri("D:\\Projects\\PRO_\\PRO_\\Immages\\placeholder.jpg"));
                    }
                }
                catch
                {
                    image.Source = new BitmapImage(new Uri("D:\\Projects\\PRO_\\PRO_\\Immages\\placeholder.jpg"));
                }
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
                    Foreground = System.Windows.Media.Brushes.SteelBlue
                });

                ToolListPanel.Items.Add(card);
            }
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