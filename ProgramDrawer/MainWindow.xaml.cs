using MahApps.Metro.Controls;
using Microsoft.Win32;
using ProgramDrawer.UserControls;
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
    public partial class MainWindow : MetroWindow
    {
        private readonly int ANIMATION_TIME = 350;

        WinForms.NotifyIcon notifyIcon;

        private bool _closing = false;
        private bool _first = true;
        public double DesktopHeight { get; private set; } = SystemParameters.WorkArea.Height;


        private SteamBannerDownloader sbd;
        private string _steamDirectory;
        public string SteamDirectory
        {
            get
            {
                if (_steamDirectory == null)
                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam"))
                    {
                        if (key != null)
                        {
                            _steamDirectory = key.GetValue("SteamPath").ToString().Replace("/", @"\");
                        }
                    }
                        
                return _steamDirectory;
            }
        }

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
                    da.Completed += (sender, args) =>
                    {
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

            notifyIcon = SetupNotifyIcon();

            Left = SystemParameters.WorkArea.Width;

            EventManager.RegisterClassHandler(typeof(Window), Keyboard.KeyDownEvent, new KeyEventHandler(KeyDown), true);

            MainContentControl.Content = new ProgramListControl();
            
            // To open taskbar settings: Process.Start("ms-settings:taskbar");
            // To start steam app: Process.Start($@"steam://rungameid/105600"); 
        }

        private WinForms.NotifyIcon SetupNotifyIcon()
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
                        //Keyboard.Focus(SearchBar);
                    }
                    _closing = false;
                }
            };

            return ni;
        }

        private new void KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Escape)
            //    //SearchBar.Text = "";
            //else
                //Keyboard.Focus(SearchBar);
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
