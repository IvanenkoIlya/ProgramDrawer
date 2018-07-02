using MahApps.Metro;
using System;
using System.Windows;

namespace ProgramDrawer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent("Blue"), ThemeManager.GetAppTheme("BaseDark"));

            base.OnStartup(e);
        }
    }
}
