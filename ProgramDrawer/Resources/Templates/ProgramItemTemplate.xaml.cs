using ProgramDrawer.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace ProgramDrawer.Resources.Templates
{
    public partial class ProgramItemTemplate : ResourceDictionary
    {
        public virtual void ChangeSettings(object sender, RoutedEventArgs e)
        {
            ((sender as Image).Tag as ProgramItem).ChangeSettings(sender, e);
        }

        public virtual void LaunchProgram(object sender, RoutedEventArgs e)
        {
            ((sender as Rectangle).Tag as ProgramItem).LaunchProgram(sender, e);
        }
    }
}
