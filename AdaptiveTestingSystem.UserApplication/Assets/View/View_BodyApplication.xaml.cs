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

namespace AdaptiveTestingSystem.UserApplication.Assets.View
{
    /// <summary>
    /// Логика взаимодействия для View_BodyApplication.xaml
    /// </summary>
    public partial class View_BodyApplication : UserControl
    {
      
        public View_BodyApplication()
        {
            InitializeComponent();
   
            _Main.Instance.Manager = new PManager(Main);
            _Main.Instance.Manager.InformationPage += PManager_InformationPage;
            Loaded += View_BodyApplication_Loaded;


            _Main.Instance.NotificationViewerManagerNotificationViewerManager.SetNotificationButton(NotificationOpen);
        }

        private void View_BodyApplication_Loaded(object sender, RoutedEventArgs e)
        {
       

            var card = new AccountInformationCard()
            {
                RolyName = _Main.Instance.MyAccount.NameRoly,
                AccountName = $"{_Main.Instance.MyAccount.Surname} {_Main.Instance.MyAccount.Firstname}",
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            card.Click += Card_Click;
            card.AccountExit += Card_AccountExit;
            card.AccountExitProgramm += Card_AccountExitProgramm;
            card.AccountViewProfil += Card_AccountViewProfil;

      

            titleAccount.Children.Clear();
            titleAccount.Children.Add(card);


            if (_Main.Instance.Manager != null) _Main.Instance.Manager.SetFirstPage(new GUI.Meny.GUI_MainMeny());
         
        }


        private async Task Start()
        {
            while (true)
            {
                if (_Main.Instance.Manager == null) continue;

                foreach (var item in _Main.Instance.Manager.Get.ToList())
                {
                    Logger.Debug($"PageManager: {item.Uid}");
                }

                await Task.Delay(1000);
            }
        }


        private void PManager_InformationPage(int count, string title)
        {
            TitlesButton.Content = title;
            BackPage.ToolTip = _Main.Instance.Manager.GetInfoOpenPage();
            if (count > 1)
            {
                BackPage.Visibility = Visibility.Visible;
            }
            else
                BackPage.Visibility = Visibility.Collapsed;
        }

        private void Meny_Click(object sender, RoutedEventArgs e)
        {
              if (_Main.Instance.Manager != null) _Main.Instance.Manager.Home(); 
        }

        public void SetUI(UIElement ui)
        {
            Application.Current.Dispatcher.InvokeAsync(async () => {
                await Task.Delay(1);
                this.Main.Children.Clear();
                this.Main.Children.Add(ui);
            });
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scrollviewer = sender as ScrollViewer;
            if (e.Delta > 0)
                scrollviewer.LineUp();
            else
                scrollviewer.LineDown();
        }

   
        private void Card_AccountViewProfil()
        {
            _Main.Instance.Manager.Next(new GUI.Users._user_page.GUI_User_Viewer());
        }

        private void Card_AccountExitProgramm()
        {
            if (MessageShow.Show("Выйти из программы?", "Выход", MessageShow.Type.Question) == true)
            {
                Environment.Exit(0);
            }
        }

        private void Card_AccountExit()
        {
            if (MessageShow.Show("Выйти из профиля?", "Выход", MessageShow.Type.Question) == true)
            {
                _Main.Instance.MyAccount.Dispose();
            }
        }

        private void Card_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void BackPage_Click(object sender, RoutedEventArgs e)
        {
             if (_Main.Instance.Manager != null) _Main.Instance.Manager.Back();

           

        }

        private void SettingOpen_Click(object sender, RoutedEventArgs e)
        {
            if (_Main.Instance.Manager != null) _Main.Instance.Manager.Next(new GUI.Setting.GUI_Setting());
        }


        public void SetDialogHost(UIElement control)
        {
            DIalogHost_Child.Children.Clear();
            DIalogHost_Child.Children.Add(control);

            VisibleDoalogHost(true);
        }

        public void VisibleDoalogHost(bool visible)
        {
            if (visible)
                DialogHost.Visibility = Visibility.Visible;
            else
                DialogHost.Visibility = Visibility.Collapsed;
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            VisibleDoalogHost(false);
            DIalogHost_Child.Children.Clear();
        }

        private void NotificationOpen_Click(object sender, RoutedEventArgs e)
        {
            _Main.Instance.NotificationOverlay.Open();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _Main.Instance.NotificationViewerManagerNotificationViewerManager.Add("Test");
        }
    }
}
