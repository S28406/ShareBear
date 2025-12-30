using System.Configuration;
using System.Data;
using System.Windows;
using PRO_.Data.Seeder;
using PRO.Data.Context;
using ToolRent;

namespace PRO_
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var context = new ToolLendingContext();
            DbSeeder.Seed(context);
            var mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}