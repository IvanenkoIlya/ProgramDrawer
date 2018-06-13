using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Drawing = System.Drawing;
using WinForms = System.Windows.Forms;

namespace ProgramDrawer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WinForms.NotifyIcon notifyIcon;

        private SteamBannerDownloader sbd;
        private bool _closing = false;
        private bool _first = true;
        private string _steamDirectory;
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

                if(_searchString == "")
                {
                    SearchBar.ApplyAnimationClock(TextBox.MarginProperty,
                        new ThicknessAnimation(new Thickness(10, -40, 10, 0), TimeSpan.FromMilliseconds(350)) { EasingFunction = new SineEase() }.CreateClock());
                    return;
                }
                else
                {
                    SearchBar.ApplyAnimationClock(TextBox.MarginProperty,
                        new ThicknessAnimation(new Thickness(10, 10, 10, 0), TimeSpan.FromMilliseconds(350)) { EasingFunction = new SineEase() }.CreateClock());
                    return;
                }
            }
        }

        public string SteamDirectory
        {
            get
            {
                if (_steamDirectory == null)
                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam"))
                        _steamDirectory = key.GetValue("SteamPath").ToString().Replace("/", @"\");
                return _steamDirectory;
            }
        }

        public double DesktopHeight { get; private set; } = SystemParameters.WorkArea.Height;

        private bool _isDrawerOpen = false;
        public bool IsDrawerOpen
        {
            get
            {
                return _isDrawerOpen;
            }

            set
            {
                if (value == _isDrawerOpen)
                    return;

                _isDrawerOpen = value;

                if(_isDrawerOpen)
                {
                    Visibility = Visibility.Visible;
                    ApplyAnimationClock(Window.LeftProperty,
                        new DoubleAnimation(SystemParameters.WorkArea.Width - 480, TimeSpan.FromMilliseconds(350)) { EasingFunction = new SineEase() }.CreateClock());
                    return;
                }
                else
                {
                    DoubleAnimation da = new DoubleAnimation(SystemParameters.WorkArea.Width, TimeSpan.FromMilliseconds(350)) { EasingFunction = new SineEase() };
                    da.Completed += (sender, args) => {
                        Visibility = Visibility.Collapsed;
                        _closing = false;
                    };
                    ApplyAnimationClock(Window.LeftProperty, da.CreateClock());

                    return;
                }
            }
        }

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();

            SetupResourceColors();
            notifyIcon = CreateNotifyIcon();

            Left = SystemParameters.WorkArea.Width;

            EventManager.RegisterClassHandler(typeof(Window), Keyboard.KeyDownEvent, new KeyEventHandler(KeyDown), true);
            SearchBar.ApplyAnimationClock(TextBox.MarginProperty,
                        new ThicknessAnimation(new Thickness(10, -40, 10, 0), TimeSpan.FromMilliseconds(10)).CreateClock());

            _programItems = new List<ProgramItem>();

            sbd = new SteamBannerDownloader(this, GetInstalledSteamAppIds(SteamDirectory));
            
            _programItems.Add(new ProgramItem() { ProgramName = "test0" }); //this is a super long name so that it overlaps the settings icon
            _programItems.Add(new ProgramItem() { ProgramName = "test1" });
            _programItems.Add(new ProgramItem() { ProgramName = "test2" });
            _programItems.Add(new ProgramItem() { ProgramName = "test3" });
            _programItems.Add(new ProgramItem() { ProgramName = "test4" });
            _programItems.Add(new ProgramItem() { ProgramName = "test5" });
            _programItems.Add(new ProgramItem() { ProgramName = "test6" });
            _programItems.Add(new ProgramItem() { ProgramName = "test7" });

            ProgramItems = CollectionViewSource.GetDefaultView(_programItems) as ListCollectionView;
            ProgramItems.Filter = (x => ((ProgramItem)x).ProgramName.ToLower().Contains(_searchString.ToLower()));

            ProgramList.ItemsSource = ProgramItems;
            
            // To open taskbar settings: Process.Start("ms-settings:taskbar");
            // To start steam app: Process.Start($@"steam://rungameid/105600"); 
        }

        private void SetupResourceColors()
        {
            SolidColorBrush drawerColorDark = new SolidColorBrush();
            SolidColorBrush drawerColorLight = new SolidColorBrush();

            Color windowsColor = SystemParameters.WindowGlassColor;

            drawerColorLight.Color = windowsColor;
            drawerColorDark.Color = Color.FromRgb((byte)(windowsColor.R * 0.7), (byte)(windowsColor.G * 0.7), (byte)(windowsColor.B * 0.7));

            Application.Current.Resources["DrawerBackgroundColorLight"] = drawerColorLight;
            Application.Current.Resources["DrawerBackgroundColorDark"] = drawerColorDark;

            using(Drawing.Bitmap bmp = new Drawing.Bitmap(@".\Resources\Icons\settings-default.png"))
            {
                Drawing.Color color = Drawing.Color.FromArgb(drawerColorLight.Color.R, drawerColorLight.Color.G, drawerColorLight.Color.B);
                for (int x = 0; x < bmp.Width; x++)
                {
                    for(int y = 0; y < bmp.Height; y++)
                    {
                        if( bmp.GetPixel(x,y).A != 0)
                        {
                            bmp.SetPixel(x, y, color);
                        }
                    }
                }

                bmp.Save(@".\Resources\Icons\settings.png", Drawing.Imaging.ImageFormat.Png);
                Application.Current.Resources["SettingsIcon"] = new BitmapImage(new Uri(@".\Resources\Icons\settings.png", UriKind.Relative));
            }
        }

        private WinForms.NotifyIcon CreateNotifyIcon()
        {
            WinForms.ContextMenu cm = new WinForms.ContextMenu();
            cm.MenuItems.Add(new WinForms.MenuItem("&Close", (sender, e) => { Close(); }));

            WinForms.NotifyIcon ni = new WinForms.NotifyIcon
            {
                Text = "Program Drawer",
                Icon = new System.Drawing.Icon(Application.GetResourceStream(new Uri("/Resources/Icons/SystemTrayIcon.ico", UriKind.Relative)).Stream),
                Visible = true,
                ContextMenu = cm
            };
            ni.MouseDown += (sender, args) =>
            {
                if (args.Button == WinForms.MouseButtons.Left)
                {
                    if(!_closing)
                    {
                        Activate();
                        IsDrawerOpen = !IsDrawerOpen;
                        Keyboard.Focus(SearchBar);
                    }
                    _closing = false;
                }
            };

            return ni;
        }

        private new void KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                SearchBar.Text = "";
            else
                Keyboard.Focus(SearchBar);
        }

        protected override void OnDeactivated(EventArgs e)
        {
            if (!_first)
            {
                _closing = true;
                IsDrawerOpen = false;

                base.OnDeactivated(e);
            }
            else
                _first = false;
            
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            notifyIcon.Visible = false; 

            base.OnClosing(e);
        }

        private List<int> GetOwnedSteamAppIds(string steamUserName)
        {
            // TODO Steam Web API stuff

            return new List<int>();
        }

        private List<int> GetInstalledSteamAppIds(string steamInstallLocation)
        {
            return Directory.EnumerateFiles(steamInstallLocation + @"\steamapps")
                .Where(f => Regex.Match(Path.GetFileName(f), @"appmanifest_(\d*).acf").Success) // Could likely optimize into one linq statement?
                .Select(f => Int32.Parse(Regex.Match(Path.GetFileName(f), @"appmanifest_(\d*).acf").Groups[1].Value))
                .ToList();
        }
    }
}
