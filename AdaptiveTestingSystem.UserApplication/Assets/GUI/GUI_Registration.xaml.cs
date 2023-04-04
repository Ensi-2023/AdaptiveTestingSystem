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

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI
{
    /// <summary>
    /// Логика взаимодействия для GUI_Registration.xaml
    /// </summary>
    public partial class GUI_Registration : UserControl
    {
        public GUI_Registration()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _Main.Instance.SetSingeltonChilden(new GUI_Authorization());
        }

        private async void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            var sur = SurnameUser.Text;
            var last = LastnameUser.Text;
            var midd = MiddlenameUser.Text;
            var dateB = DatebirchUser.Text;
            var gender = GenderUser.Text;

            var login = UserLogin.Text;
            var pass = UserPassword.Password;
            var passVerty = UserVertyPassword.Password;


            var error = "";

            if (ParserVariables.CountSymbol(sur) == 0)
            {
                error += "Не заполнено поле 'Фамилия'\n";
            }

            if (ParserVariables.CountSymbol(last) == 0)
            {
                error += "Не заполнено поле 'Имя'\n";
            }

            if (ParserVariables.CountSymbol(dateB) == 0)
            {
                error += "Не заполнено поле 'Дата рождения'\n";
            }
            else
            {
                if (ParserVariables.GetDate(dateB) > DateTime.Now)
                {
                    error += "Дата не может быть больше сегодняшней\n";
                }
            }

            if (ParserVariables.CountSymbol(gender) == 0)
            {
                error += "Не заполнено поле 'Пол'\n";
            }

            if (ParserVariables.CountSymbol(login) == 0)
            {
                error += "Не заполнено поле 'Логин'\n";
            }

            if (ParserVariables.CountSymbol(pass) == 0 || ParserVariables.CountSymbol(passVerty) == 0)
            {
                error += "Не заполнено поле 'Пароль' или 'Повторите пароль'\n";
            }
            else
            {
                if (pass != passVerty)
                {
                    error += "Пароли не совпадают!\n";
                }
            }

            if (ParserVariables.CountSymbol(error) > 0)
            {
                MessageShow.Show(error);
                return;
            }


  
            var obj = new Data_Registration()
            {
                Login = login,
                Password = pass,
                DatebirchUser = dateB,
                GenderUser = gender,
                LastnameUser = last,
                MiddlenameUser = midd,
                SurnameUser = sur,
                IsCode = 0
            };

            var send = new Data_FirstCommand()
            {
                Command = "Command_Registration",
                Json = JsonSerializer.Serialize(obj)
            };

            await Start(JsonSerializer.Serialize(send));
        }

        private async Task Start(string message)
        {
    
           _Main.Instance.OverlayShow(true, TypeOverlay.loading, "Подождите...", "Идет регистрация пользователя",visibleButton:Visibility.Collapsed);
            await Task.Delay(500);
            _Main.Instance.Client.Send(message);
   
        }
    }
}
