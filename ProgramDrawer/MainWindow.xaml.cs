using MahApps.Metro.Controls;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using WinForms = System.Windows.Forms;

namespace ProgramDrawer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow, INotifyPropertyChanged
    {
        public double DesktopHeight { get; private set; } = SystemParameters.WorkArea.Height;

        private bool _closing = false;
        private bool _first = true;
        WinForms.NotifyIcon notifyIcon;

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
                OnPropertyChanged("IsDrawerOpen");
            }
        }

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();

            notifyIcon = SetupNotifyIcon();

            Left = SystemParameters.WorkArea.Width-480;

            AllowsTransparency = true;
            MyFlyout.ClosingFinished += (sender, e) => { _closing = false; };

            //MainContentControl.Content = new ProgramListControl();
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
                    }
                    _closing = false;
                }
            };

            return ni;
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

        #region Property Changed event handler
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
