using System;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.EntityFrameworkCore;
using PRO.Data.Context;
using PRO.Models;

namespace ToolRent.Views
{
    public partial class ToolDetailsPage : Page
    {
        private readonly Guid _toolId;
        private Tool? _tool;

        public ToolDetailsPage(Guid toolId)
        {
            InitializeComponent();
            _toolId = toolId;
            Loaded += (_, __) => LoadData();
        }

        private void LoadData()
        {
            using var db = new ToolLendingContext();

            _tool = db.Tools
                       .Include(t => t.Category)
                       .Include(t => t.User)
                       .Include(t => t.Reviews).ThenInclude(r => r.User)
                       .FirstOrDefault(t => t.ID == _toolId);

            if (_tool == null) return;

            ToolName.Text        = _tool.Name;
            CategoryName.Text    = _tool.Category != null ? $"Category: {_tool.Category.Name}" : "Category: —";
            OwnerName.Text       = _tool.User != null ? $"Owner: {_tool.User.Username}" : "Owner: —";
            PriceText.Text       = $"Price: ${_tool.Price:F2} / day";
            DescriptionText.Text = _tool.Description ?? "";

            HeroImage.Source = ResolveImage(_tool.ImagePath);

            ReviewsList.ItemsSource = _tool.Reviews?
                                          .OrderByDescending(r => r.Date)
                                          .ToList();
        }

        private static BitmapImage ResolveImage(string? dbPath)
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

            var ph = new BitmapImage();
            ph.BeginInit();
            ph.UriSource = new Uri("pack://application:,,,/Images/placeholder.jpg");
            ph.EndInit();
            return ph;
        }
    }
}
