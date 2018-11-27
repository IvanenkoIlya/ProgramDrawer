using AnimatedListView;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using Newtonsoft.Json;
using ProgramDrawer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
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
        
        public ObservableCollectionView<ProgramItemBase> ProgramItems;

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

            ProgramItems = new ObservableCollectionView<ProgramItemBase>();
            LoadProgramList();
            ProgramItems.Filter = (x => ((ProgramItemBase)x).ProgramName.ToLower().Contains(_searchString.ToLower()));
            ProgramItems.SortDescriptions.Add(new SortDescription("Favorite", ListSortDirection.Descending));
            ProgramItems.SortDescriptions.Add(new SortDescription("ProgramName", ListSortDirection.Ascending));

            ProgramList.ItemsSource = ProgramItems;

            Application.Current.Exit += SaveProgramList;
        }

        private void LoadProgramList()
        {
            if (File.Exists(programFileLocation))
            {
                using (StreamReader sr = new StreamReader(programFileLocation))
                {
                    ObservableCollection<ProgramItemBase> items = JsonConvert.DeserializeObject<ObservableCollection<ProgramItemBase>>(sr.ReadToEnd(), 
                        new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.Objects
                        });

                    foreach (ProgramItemBase item in items)
                        ProgramItems.Add(item);
                }
            }
            else
            {
                //TODO This needs to be moved out elsewhere
                string steamDirectory = "";

                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam"))
                {
                    if (key != null)
                    {
                        steamDirectory = key.GetValue("SteamPath").ToString().Replace("/", @"\");
                    }
                }

                if(steamDirectory != "")
                {
                    Dictionary<int,string> appIDs = GetInstalledSteamAppIds(steamDirectory);
                    foreach(KeyValuePair<int,string> keyValue in appIDs)
                    {
                        string programName = GetSteamAppNameFromAcf(keyValue.Value);

                        ProgramItems.Add(new SteamProgramItem(keyValue.Key, programName));
                    }
                }
            }
        }

        public void SaveProgramList(object sender, ExitEventArgs e)
        {
            ObservableCollection<ProgramItemBase> items = new ObservableCollection<ProgramItemBase>();

            foreach (ProgramItemBase item in ProgramItems)
                items.Add(item);

            string json = JsonConvert.SerializeObject(items, Formatting.Indented,
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
            EditableProgramItemControl addProgramControl = new EditableProgramItemControl(new ProgramItem("Default name", ""));

            addProgramControl.Cancel += OnCancel;
            addProgramControl.Save += OnSave;

            AddProgramGrid.Children.Add(addProgramControl);
        }

        private void OnCancel(object sender, EventArgs e)
        {
            AddProgramGrid.Children.Remove(sender as EditableProgramItemControl);
        }

        private void OnSave(object sender, EventArgs e)
        {
            EditableProgramItemControl addProgramControl = (sender as EditableProgramItemControl);

            ProgramItems.Add(addProgramControl.ProgramItem);
            
            AddProgramGrid.Children.Remove(sender as EditableProgramItemControl);
        }

        private Dictionary<int,string> GetInstalledSteamAppIds(string steamInstallLocation)
        {
            List<string> acfFiles = Directory.EnumerateFiles(steamInstallLocation + @"\steamapps")
                .Where(f => Regex.Match(Path.GetFileName(f), @"appmanifest_(\d*).acf").Success).ToList();

            Dictionary<int, string> result = new Dictionary<int, string>();

            foreach(string file in acfFiles)
            {
                result.Add(Int32.Parse(Regex.Match(Path.GetFileName(file), @"appmanifest_(\d*).acf").Groups[1].Value), file);
            }

            result.Remove(228980); // This is Steamworks Common Redistributables, not actually a valid game

            return result;
        }

        private string GetSteamAppNameFromAcf(string file)
        {
            using (StreamReader sr = new StreamReader(file))
            {
                string line;

                while((line = sr.ReadLine()) != null)
                {
                    if (line.Contains("\"name\""))
                    {
                        string[] parts = line.Split('\"');
                        return parts[3];
                    }
                }
            }

            return "";
        }
    }
}
