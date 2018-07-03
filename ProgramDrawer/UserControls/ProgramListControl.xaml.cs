using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProgramDrawer.UserControls
{
    /// <summary>
    /// Interaction logic for ProgramListControl.xaml
    /// </summary>
    public partial class ProgramListControl : UserControl
    {
        private List<ProgramItem> _programItems;
        public ListCollectionView ProgramItems;

        private string _searchString = "";

        public string SearchString
        {
            get { return _searchString; }
            set
            {
                if (value == _searchString)
                    return;

                _searchString = value;
                ProgramItems.Refresh();

                if (_searchString == "")
                {
                    //SearchBar.ApplyAnimationClock(TextBox.MarginProperty,
                    //    new ThicknessAnimation(new Thickness(10, -40, 10, 0), TimeSpan.FromMilliseconds(350)) { EasingFunction = new SineEase() }.CreateClock());
                    return;
                }
                else
                {
                    //SearchBar.ApplyAnimationClock(TextBox.MarginProperty,
                    //    new ThicknessAnimation(new Thickness(10, 10, 10, 0), TimeSpan.FromMilliseconds(350)) { EasingFunction = new SineEase() }.CreateClock());
                    return;
                }
            }
        }

        public ProgramListControl()
        {
            InitializeComponent();
        }
    }
}
