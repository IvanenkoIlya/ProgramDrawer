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
            ThemeManager.ChangeAppStyle(Current, 
                ThemeManager.GetAccent(ProgramDrawer.Properties.Settings.Default.AccentColor), 
                ThemeManager.GetAppTheme(ProgramDrawer.Properties.Settings.Default.BaseColor));

            base.OnStartup(e);
        }
    }
}
