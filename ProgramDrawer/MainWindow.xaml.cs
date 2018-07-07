using MahApps.Metro.Controls;
using Microsoft.Win32;
using ProgramDrawer.Model;
using ProgramDrawer.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using WinForms = System.Windows.Forms;

namespace ProgramDrawer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow, INotifyPropertyChanged
    {
        public double DesktopHeight { get; private set; } = SystemParameters.WorkArea.Height;

        private bool _first = true;
        private bool _closing = false;
        WinForms.NotifyIcon notifyIcon;

        private bool _lockDrawer = false;

        public void LockDrawer()
        {
            _lockDrawer = true;
        }

        public void UnlockDrawer()
        {
            _lockDrawer = false;
        }

        private Dictionary<string, UserControl> mainContents;

        private bool _isDrawerOpen = false;
        public bool IsDrawerOpen
        {
            get { return _isDrawerOpen; }

            set
            {
                if (value == _isDrawerOpen)
                    return;

                _isDrawerOpen = value;
                OnPropertyChanged("IsDrawerOpen");
            }
        }

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();

            notifyIcon = SetupNotifyIcon();
            mainContents = new Dictionary<string, UserControl>();

            Left = SystemParameters.WorkArea.Width-480;

            AllowsTransparency = true;
            MyFlyout.ClosingFinished += (sender, e) => { _closing = false; };

            mainContents.Add("ProgramList", new ProgramListControl());
            mainContents.Add("Settings", new SettingsControl());

            MainContentControl.Content = mainContents["ProgramList"];
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
                        if(!_lockDrawer)
                            IsDrawerOpen = !IsDrawerOpen;
                    }
                    _closing = false;
                }
            };

            return ni;
        }

        protected override void OnDeactivated(EventArgs e)
        {
            if (!_lockDrawer)
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
        }
        
        protected override void OnClosing(CancelEventArgs e)
        {
            notifyIcon.Visible = false;

            base.OnClosing(e);
        }

        private void SetStartup()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            rk.SetValue("ProgramDrawer", System.Windows.Forms.Application.ExecutablePath);
        }

        #region Property Changed event handler
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private bool toggle;
        private void UniversalTestButtonClick(object sender, RoutedEventArgs e)
        {
            if (toggle)
            {
                MainContentControl.Content = mainContents["ProgramList"];
            }   
            else
            {
                MainContentControl.Content = mainContents["Settings"];
            }
            toggle = !toggle;
        }

        private void ToggleLockClick(object sender, RoutedEventArgs e)
        {
            if(_lockDrawer)
            {
                UnlockDrawer();
            } else
            {
                LockDrawer();
            }
        }
    }
}
