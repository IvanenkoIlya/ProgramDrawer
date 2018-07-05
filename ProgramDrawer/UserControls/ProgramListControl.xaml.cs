using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                ProgramItems.Refresh();

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

            ProgramItems = CollectionViewSource.GetDefaultView(_programItems) as ListCollectionView;
            ProgramItems.Filter = (x => ((ProgramItem)x).ProgramName.ToLower().Contains(_searchString.ToLower()));

            ProgramList.ItemsSource = ProgramItems;
        }

        private void PopulateProgramList()
        {
            if (File.Exists(""))
            {
                // TODO Load existing list from file
            }
            else
            {
                _programItems = new List<ProgramItem>
                {
                    new ProgramItem() { ProgramName = "test0" }, //this is a super long name so that it overlaps the settings icon
                    new ProgramItem() { ProgramName = "test1" },
                    new ProgramItem() { ProgramName = "test2" },
                    new ProgramItem() { ProgramName = "test3" },
                    new ProgramItem() { ProgramName = "test4" }
                };
            }
        }

        private new void KeyDown(object sender, KeyEventArgs e)
        {
            if (!(Keyboard.FocusedElement is TextBox) || Keyboard.FocusedElement == SearchBar)
            {
                if (e.Key == Key.Escape)
                    SearchBar.Text = "";
                Keyboard.Focus(SearchBar);
            }
            else
            {
                SearchBar.ApplyAnimationClock(MarginProperty,
                        new ThicknessAnimation(new Thickness(10, -40, 10, 0), TimeSpan.FromMilliseconds(350)) { EasingFunction = new SineEase() }.CreateClock());
            }
        }

        private void CreateNewProgramItem(object sender, MouseButtonEventArgs e)
        {
            AddProgramControl addProgramControl = new AddProgramControl();

            addProgramControl.Cancel += OnCancel;
            addProgramControl.Save += OnSave;

            AddProgramGrid.Children.Add(addProgramControl);
        }

        private void OnCancel(object sender, EventArgs e)
        {
            AddProgramGrid.Children.Remove(sender as AddProgramControl);
        }

        private void OnSave(object sender, EventArgs e)
        {
            AddProgramControl addProgramControl = (sender as AddProgramControl);

            // TODO Animate
            _programItems.Add(
                new ProgramItem()
                {
                    ProgramName = addProgramControl.ProgramName,
                    ImageLocation = addProgramControl.BannerImageLocation,
                    ProgramLocation = addProgramControl.ProgramLocation
                });
            ProgramItems.Refresh();

            AddProgramGrid.Children.Remove(sender as AddProgramControl);
        }
    }
}
