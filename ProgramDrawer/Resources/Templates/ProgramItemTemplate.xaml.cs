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

        private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Console.WriteLine("Edit click test");
        }
    }
}
