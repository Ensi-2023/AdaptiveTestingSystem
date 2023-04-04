using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace AdaptiveTestingSystem.UserApplication.Assets.Command
{
    public class Command_Authorization : Commands
    {
        public override void Execut(string json, InternetClient client)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<Data_Authoriz>(json);
                switch (obj.IsCode)
                {
                    case Code.Null:

                        Logger.Message("Успешная авторизация");
                        Application.Current.Dispatcher.Invoke(async () =>
                        {

                            _Main.Instance.OverlayShow(true, TypeOverlay.message, "Авторизация успешна...",visibleButton:Visibility.Collapsed);
                            await Task.Delay(450);
                            _Main.Instance.OverlayShow(false, typeOverlay: TypeOverlay.message);
                            _Main.Instance.MyAccount = new Account();
                            _Main.Instance.MyAccount.Disconnect += MyAccount_Disconnect;

                            _Main.Instance.MyAccount.Set(obj.AccessRights, obj.Login, client.GUID);


                            _Main.Instance.SetSingeltonChilden(new View_BodyApplication());

                            _Main.Instance.MVVM_Manager = new MVVM_Manager();

                            //switch(obj.AccessRights.IdRoly)
                            //{
                            //    case 1: _Main.Instance.SetSingeltonChilden(new View_BodyApplication()); break;
                            //    default:
                            //        MessageShow.Show("Пользовательский интерфейс еще не готов.", "Ошибка", MessageShow.Type.Error);                               
                            //        break;
                            //}
                        });

                        break;
                    case Code.LoginIsAuthorized:

                        Logger.Warning("Логин уже авторизован!");
                        _Main.Instance.OverlayShow(true, TypeOverlay.error, "Ошибка авторизации", "Данный логин уже авторизован", visibleButton: Visibility.Visible);


                        break;
                    case Code.InvalidUserNameOrPassword:

                        Logger.Error("Не верный логин или пароль");
                        _Main.Instance.OverlayShow(true, TypeOverlay.error, "Ошибка авторизации", "Не верная связка логин и пароль", visibleButton: Visibility.Visible);


                        break;
                    case Code.RegistrationSuccessful:
                        break;
                    case Code.InvalidLogin:
                        break;
                    case Code.InvalidRegistration:
                        break;
                    case Code.AccountBanned:

                        Logger.Error("Аккаунт забанен.");
                        _Main.Instance.OverlayShow(true, TypeOverlay.error, "Ошибка авторизации", "Аккаунт забанен", visibleButton: Visibility.Visible);

                        break;
                }

            }
            catch (Exception ex)
            {
                Logger.Error($"Command_Authorization.Execut вызвал ошибку: {ex.Message}");
            }
        }



        
        private void MyAccount_Disconnect()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {

                var obj = new Data_Disconnect()
                {
                    GUI = _Main.Instance.MyAccount.GUI
                };

                var send = new Data_FirstCommand()
                {
                    Command = "Command_AuthorizedUserDisconnect",
                    Json = JsonSerializer.Serialize(obj)
                };

                _Main.Instance.Client.Send(JsonSerializer.Serialize(send));
                _Main.Instance.Reconect();




            });
        }
    }
}
