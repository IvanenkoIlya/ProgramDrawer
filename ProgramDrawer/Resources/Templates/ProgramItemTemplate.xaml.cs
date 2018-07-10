using ProgramDrawer.Model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace ProgramDrawer.Resources.Templates
{
    public partial class ProgramItemTemplate : ResourceDictionary
    {
        public virtual void ChangeSettings(object sender, RoutedEventArgs e)
        {
            ((sender as Image).Tag as ProgramItemBase).ChangeProperties(sender, e);
        }

        public virtual void LaunchProgram(object sender, RoutedEventArgs e)
        {
            ((sender as Rectangle).Tag as ProgramItemBase).LaunchProgram(sender, e);
        }
        // To open taskbar settings: Process.Start("ms-settings:taskbar");
    }
}
