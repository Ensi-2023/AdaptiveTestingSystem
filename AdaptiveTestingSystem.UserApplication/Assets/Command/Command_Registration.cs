using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AdaptiveTestingSystem.UserApplication.Assets.Command
{
    public class Command_Registration:Commands
    {
        public override void Execut(string json, InternetClient client)
        {
      
            try
            {
                var obj = JsonSerializer.Deserialize<Data_Registration>(json);

                switch (obj.IsCode)
                {
                    case Code.RegistrationSuccessful:
                        StartAuthorization(obj.Login, obj.Password);
                        break;
                    case Code.InvalidLogin:

                        _Main.Instance.OverlayShow(true, TypeOverlay.error, "Ошибка регистрации", "Такой логин уже зарегистрирован", visibleButton: Visibility.Visible);
                        break;

                    case Code.InvalidRegistration:

                        _Main.Instance.OverlayShow(true, TypeOverlay.error, "Ошибка регистрации", "Возникла непредвиденная ошибка. Попробуйте попозже", visibleButton: Visibility.Visible);
                        break;

                    default:
                        throw new Exception($"Код {obj.IsCode} не опознан");
                }


            }
            catch (Exception ex)
            {
                Logger.Error($"Command_Registration.Execut вызвал ошибку: {ex.Message}");
            }
        }


        private void StartAuthorization(string login, string password)
        {
            Application.Current.Dispatcher.Invoke(async() =>
            {
                _Main.Instance.OverlayShow(true, TypeOverlay.message, "Регистрация успешна...");
                await Task.Delay(450);
          
       
            var obj = new Data_Authoriz()
            {
                Login = login,
                Password = password.Trim(),
                IsVerified = false
            };


            var send = new Data_FirstCommand()
            {
                Command = "Command_AuthorizationUser",
                Json = JsonSerializer.Serialize(obj)
            };

              _Main.Instance.Client.Send(JsonSerializer.Serialize(send));

             });

        }
 
    }
}
