using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Animation;

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
                //ProgramItems.Refresh();

                if (_searchString == "")
                {
                    SearchBar.ApplyAnimationClock(MarginProperty,
                        new ThicknessAnimation(new Thickness(10, -40, 10, 0), TimeSpan.FromMilliseconds(350)) { EasingFunction = new SineEase() }.CreateClock());
                    return;
                }
                else
                {
                    SearchBar.ApplyAnimationClock(MarginProperty,
                        new ThicknessAnimation(new Thickness(10, 10, 10, 0), TimeSpan.FromMilliseconds(350)) { EasingFunction = new SineEase() }.CreateClock());
                    return;
                }
            }
        }

        public ProgramListControl()
        {
            DataContext = this;
            InitializeComponent();

            EventManager.RegisterClassHandler(typeof(Window), Keyboard.KeyDownEvent, new KeyEventHandler(KeyDown), true);
            SearchBar.ApplyAnimationClock(MarginProperty,
                new ThicknessAnimation(new Thickness(10, -40, 10, 0), TimeSpan.FromMilliseconds(10)).CreateClock());

            PopulateProgramList();
        }

        private void PopulateProgramList()
        {
            if (File.Exists(""))
            {
                // TODO Load existing list from file
            }
            else
            {
                
            }
        }

        private new void KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                SearchBar.Text = "";
            else
                Keyboard.Focus(SearchBar);
        }

        private void CreateNewProgramItem(object sender, MouseButtonEventArgs e)
        {
            // TODO Create a new Program Item
        }
    }
}
