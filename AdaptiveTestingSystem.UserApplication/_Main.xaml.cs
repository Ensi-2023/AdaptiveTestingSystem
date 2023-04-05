#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AdaptiveTestingSystem.UserApplication.Assets.CScript;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing;
using AdaptiveTestingSystem.UserApplication.Client;

namespace AdaptiveTestingSystem.UserApplication
{
   
    public partial class _Main : Window
    {

        //MVVM Public
        public MVVM_Manager MVVM_Manager { get; set; }



        //Static 
        public static _Main Instance = null;
        public static CancellationTokenSource CancellationTokenSource;

        //public
        public Notification _Notification { get; private set; }
        public AppSettings Settings { get; set; }
        public InternetClient Client { get; set; }
        public Account MyAccount { get; set; }
        public PManager Manager { get; set; }
        public ThemeManager Theme { get; private set; }
        public CustomPropertyChanged customPropertyChanged { get; set; }

        public GUI_TestReady UI_TestReady { get; set; }

        public ThreadAcceptData threadAcceptData { get; set; }

        public NotificationViewerManager NotificationViewerManagerNotificationViewerManager { get; set; }

        public Key KeyPress { get; private set; }

        //public CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        //CancellationToken token = cancelTokenSource.Token;
        //private



        public _Main()
        {
            InitializeComponent();
            ConsoleContol.ShowConsole();
            Instance = this;
            SetVariable();


            this.Loaded += _Main_Loaded;
            this.Closed += _Main_Closed;
            
            this.Opacity = 0;
        }

        public void ShowMessage(string message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                _Notification.Add(message);
            });
        }

        public void ShowMessage(string message, string title)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                _Notification.Add(title, message);
            });
        }

        public void ShowMessage(string message, string title, TypeNotification type)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                _Notification.Add(title, message, type);
            });
        }



        public void ShowWindow()
        {
            Animation.AnimatedOpacity(this, 0, 1, TimeSpan.FromMilliseconds(150));
        }

        private void _Main_Loaded(object sender, RoutedEventArgs e)
        {
            StartConnectToServer();         
        }

      
        public void StartConnectToServer()
        {
            SetSingeltonChilden(new Assets.GUI.GUI_Loading());
        }

        private void _Main_Closed(object sender, EventArgs e)
        {
            if (_Notification != null) _Notification.Close();
        }


        public async void Reconect()
        {
           


            Client.CloseConnect();

            Client = null;
                 
           
            StartConnectToServer();
        }

        public void SetSingeltonChilden(UIElement value)
        {

            Application.Current.Dispatcher.Invoke(() =>
            {
                if (Instance == null) return;
                Instance.MainBody.Children.Clear();
                Instance.MainBody.Children.Add(value);
          
            });
        }

        public void CloseAllWindow()
        {
            foreach (var item in Application.Current.Windows)
            {
                var window = item as _Main;
                var noti = item as Notification;
                if (window != null || noti != null)
                {
                    continue;
                }

                (item as Window).Close();
            }
        }

        private void SetVariable()
        {

            // UI_TestReady = new GUI_TestReady();

            customPropertyChanged = new CustomPropertyChanged();
            threadAcceptData = new ThreadAcceptData();
            _Notification = new Notification();
            //_Notification.Show();

            Theme = new ThemeManager();
            Theme.ThemeChanged += Theme_ThemeChanged;
            NotificationViewerManagerNotificationViewerManager = new NotificationViewerManager(NotificationOverlay);
        }

        private void Theme_ThemeChanged(string themeStyle)
        {
            Settings.ThemeSet(themeStyle);
        }

        private void FormMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void FormMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal) State(WindowState.Maximized, true); else State(WindowState.Normal, false);
        }

        private void State(WindowState state, bool maxmin)
        {
            WindowState = state;
            if (maxmin) root.Margin = new Thickness(0, 0, 0, 0); else root.Margin = new Thickness(10);
        }

        private void FormClose_Click(object sender, RoutedEventArgs e)
        {
            if (MessageShow.Show("Выйти из программы?", "Выход", MessageShow.Type.Question) == true)
            {
                Environment.Exit(0);
            }
        }

        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.Opacity = .7;
                this.Cursor = Cursors.Cross;
                this.DragMove();
                this.Opacity = 1;
                this.Cursor = Cursors.Arrow;
            }

        }

        public void OverlayShow(bool show, TypeOverlay typeOverlay = TypeOverlay.nullable, string title = "", string subtitle = "", Visibility visibleButton = Visibility.Collapsed, bool awaiter = false)
        {
            Application.Current.Dispatcher.Invoke(async () =>
            {
                if (Instance == null) return;

                if (awaiter == true) await Task.Delay(250);

                MainBody.IsEnabled = !show;
                this.Overlay.Visibility = show == true ? Visibility.Visible:Visibility.Collapsed;
                this.Overlay.Title = title;
                this.Overlay.SubTitle = subtitle;
                this.Overlay.TOverlay = typeOverlay;
                this.Overlay.ButtonVisible = visibleButton;


                if (title.Trim() == string.Empty && subtitle.Trim() == string.Empty) return;

                if (typeOverlay == TypeOverlay.loading)
                {
                    Logger.Warning($"[{title}] - {subtitle}");
                }

                if (typeOverlay == TypeOverlay.message)
                {
                    Logger.Message($"[{title}] - {subtitle}");
                }

                if (typeOverlay == TypeOverlay.error)
                {
                    Logger.Error($"[{title}] - {subtitle}");
                }

            });
        }

        private void Overlay_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.Overlay.Visibility == Visibility.Collapsed) MainBody.IsEnabled = true;
        }

        private void root_KeyDown(object sender, KeyEventArgs e)
        {
  
            
            
        
        }

        private void Header_MaximizeClick()
        {
            if (WindowState == WindowState.Normal) State(WindowState.Maximized, true); else State(WindowState.Normal, false);
        }

        private void Header_MinimizeClick()
        {
            WindowState = WindowState.Minimized;
        }

        private void Header_CloseClick()
        {
            if (MessageShow.Show("Выйти из программы?", "Выход", MessageShow.Type.Question) == true)
            {
                Close();
            }
        }

        private void Overlay_OverlayThreadStop()
        {
            ThreadManager.CloseActiveThread();
        }

        private void root_KeyUp(object sender, KeyEventArgs e)
        {
         
        }

        private void root_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            KeyPress = e.Key;

            if (e.Key == Key.Escape)
            {
                if (this.Overlay.Visibility == Visibility.Visible && this.Overlay.TOverlay == TypeOverlay.error)
                {
                    OverlayShow(false);
                }
            }         

        }

        private void root_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            KeyPress = Key.NoName;
        }
    }
}
