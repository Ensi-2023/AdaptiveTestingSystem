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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI
{
    /// <summary>
    /// Логика взаимодействия для GUI_Authorization.xaml
    /// </summary>
    public partial class GUI_Authorization : UserControl
    {
        public GUI_Authorization()
        {
            InitializeComponent();
            
        }
        private void StartAuthoriz_Click(object sender, RoutedEventArgs e)
        {

            string login = UserLogin.Text;
            string Pass = UserPAssword.Password;
            string error = "";

            if (login.Trim().Length == 0)
            {
                error += "Логин не может быть пустым.\n";
            }

            if (Pass.Trim().Length == 0)
            {
                error += "Пароль не может быть пустым.\n";
            }

            if (error.Trim().Length > 0)
            {
                _Main.Instance._Notification.Add("Авторизация", error, TypeNotification.Error);
                return;
            }

            Authorizations(login, Pass);

        }

        private async void Authorizations(string login, string Pass)
        {
            _Main.Instance.OverlayShow(true);

            var obj = new Data_Authoriz()
            {
                Login = login,
                Password = Pass,
                IsVerified = false
            };



            var send = new Data_FirstCommand()
            {
                Command = "Command_AuthorizationUser",
                Json = JsonSerializer.Serialize(obj)
            };


             await Start(JsonSerializer.Serialize(send));


        }



        private async Task Start(string message)
        {


            _Main.Instance.OverlayShow(true, TypeOverlay.loading,"Подождите...", "Идет авторизация пользователя",visibleButton:Visibility.Collapsed);
            await Task.Delay(500);

            _Main.Instance.Client.Send(message);
        }

        private void Registration_Click(object sender, RoutedEventArgs e)
        {
            _Main.Instance.SetSingeltonChilden(new GUI_Registration());
        }

        private void root_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) StartAuthoriz_Click(null, null);
        }

        private void root_Loaded(object sender, RoutedEventArgs e)
        {
            Logger.Debug("Вход под админом");
            //Authorizations("admin", "admin");
        }
    }
}
