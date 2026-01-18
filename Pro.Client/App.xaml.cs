using System.Windows;
using Pro.Client.Services;

namespace Pro.Client;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        Api.Init();
    }
}