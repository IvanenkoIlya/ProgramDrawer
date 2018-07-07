using MahApps.Metro.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProgramDrawer.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
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
        private readonly string programFileLocation = Path.Combine(Directory.GetCurrentDirectory(), "programs.json");

        private List<ProgramItemBase> _programItems;
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

            EventManager.RegisterClassHandler(typeof(MetroWindow), Keyboard.KeyDownEvent, new KeyEventHandler(KeyDown), true);
            SearchBar.ApplyAnimationClock(MarginProperty,
                new ThicknessAnimation(new Thickness(10, -40, 10, 0), TimeSpan.FromMilliseconds(10)).CreateClock());

            LoadProgramList();

            ProgramItems = CollectionViewSource.GetDefaultView(_programItems) as ListCollectionView;
            ProgramItems.Filter = (x => ((ProgramItemBase)x).ProgramName.ToLower().Contains(_searchString.ToLower()));

            ProgramList.ItemsSource = ProgramItems;

            Application.Current.Exit += SaveProgramList;
        }

        private void LoadProgramList()
        {
            if (File.Exists(programFileLocation))
            {
                using (StreamReader sr = new StreamReader(programFileLocation))
                {
                    _programItems = JsonConvert.DeserializeObject<List<ProgramItemBase>>(sr.ReadToEnd(), 
                        new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.Objects
                        });
                }
            }
            else
            {
                _programItems = new List<ProgramItemBase>();
                    //{
                    //    new SteamProgramItem(105600),
                    //    new ProgramItem("test 0", ""), //this is a super long name so that it overlaps the settings icon
                    //    new ProgramItem("test 1", ""),
                    //    new ProgramItem("test 2", ""),
                    //    new ProgramItem("test 3", ""),
                    //    new ProgramItem("test 4", ""),

                    //};
            }
        }

        public void SaveProgramList(object sender, ExitEventArgs e)
        {
            string json = JsonConvert.SerializeObject(_programItems, Formatting.Indented,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects,
                    TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
                });

            using (StreamWriter sw = new StreamWriter(programFileLocation, false))
            {
                sw.Write(json);
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
                new ProgramItem(addProgramControl.ProgramName,
                                addProgramControl.ProgramLocation,
                                addProgramControl.BannerImageLocation)
            );

            ProgramItems.Refresh();

            AddProgramGrid.Children.Remove(sender as AddProgramControl);
        }
    }
}
