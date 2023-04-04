using AdaptiveTestingSystem.Data;
using AdaptiveTestingSystem.UserApplication.Assets.MVVM.Model;
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
using System.Windows.Shapes;

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage.window
{
    /// <summary>
    /// Логика взаимодействия для GUI_FastConnect.xaml
    /// </summary>
    public partial class GUI_FastConnect : Window
    {
        public GUI_FastConnect()
        {
            InitializeComponent();
        }

        private void cancelConnect_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void connectToServer_Click(object sender, RoutedEventArgs e)
        {
            string hash = ServerPAssword.Password.Trim() == string.Empty ? string.Empty : Encryption.getMd5Hash(ServerPAssword.Password);

            string index = IndexServer.Text.Trim();


            if (index == string.Empty)
            {
                _Main.Instance._Notification.Add("", "Заполните индекс сервера", TypeNotification.Error);
                return;
            }

            Overlay.Visibility = Visibility.Visible;


            var connectTestingServer = new Data_ConnectTestingServer()
            {
                IndexServer = index,
                Hash = hash
            };

            Data_FirstCommand data = new Data_FirstCommand()
            {
                Command = "Command_ConnectToServerThisIndexServer",
                Json = JsonSerializer.Serialize(connectTestingServer),
            };

            _Main.Instance.Client.Send(JsonSerializer.Serialize(data));

        }

        private void Header_CloseClick()
        {
            Close();
        }
    }
}
