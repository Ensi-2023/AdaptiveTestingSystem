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
using System.Windows.Threading;

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI
{
    /// <summary>
    /// Логика взаимодействия для GUI_ErrorConnectToServer.xaml
    /// </summary>
    public partial class GUI_ErrorConnectToServer : UserControl
    {
        public GUI_ErrorConnectToServer(string error)
        {
            InitializeComponent();
            ErrorMessage.Text = error;
            SetTimer();
        }


        private DispatcherTimer timeReconnectr;

        int timeToReconnect = 0;

      

        private void SetTimer()
        {
            timeToReconnect = _Main.Instance.Client.TimeToReconnect;
            timeReconnectr = new DispatcherTimer();
            timeReconnectr.Tick += new EventHandler(timeReconnectr_Tick);
            timeReconnectr.Interval = TimeSpan.FromSeconds(1);
            timeReconnectr.Start();
        }

        private void RecconectButton_Click(object sender, RoutedEventArgs e)
        {
            Reconnect();
        }

        private void Reconnect()
        {
            if (_Main.Instance.Client.ReconnectCount > 5)
            {
                timeReconnectr.Stop();
                MessageShow.Show("Истекли все попытки подключения, программа закрывается");
                Environment.Exit(0);
            }



            timeReconnectr.Stop();
            if (_Main.Instance == null) return;
            _Main.Instance.StartConnectToServer();


        }


        private void timeReconnectr_Tick(object sender, EventArgs e)
        {

            timeToReconnect--;
            SekReconnect.Content = string.Format("{0} сек. (Попытка: {1})", timeToReconnect.ToString(), _Main.Instance.Client.ReconnectCount);

            if (timeToReconnect == 0)
            {

                if (_Main.Instance.Client.ReconnectCount > 5)
                {
                    timeReconnectr.Stop();

                    MessageShow.Show("Истекли все попытки подключения, программа закрывается");
                    Environment.Exit(0);
                }
                else
                {

                    timeReconnectr.Stop();
                    if (_Main.Instance == null) return;
                    _Main.Instance.StartConnectToServer();
                }
            }
        }

        private void root_Unloaded(object sender, RoutedEventArgs e)
        {
            timeReconnectr.Stop();
        }

        private void SettingtButton_Click(object sender, RoutedEventArgs e)
        {
            if (_Main.Instance == null) return;
            _Main.Instance.SetSingeltonChilden(new GUI.GUI_StartupEditSetting());
        }
    }
}
