using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.Command
{
    public class Command_DisconnectClass:Commands
    {
        public override void Execut(string json, InternetClient client)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<Data_Disconnect>(json);
                if (obj == null) return;

                switch (obj.IsCode)
                {
                    case Code.Null:
                        break;

                    case Code.Delete:
                        {

                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                _Main.Instance.MyAccount.Dispose();


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

                                Logger.Error("Вы были отключены. Ваш аккаунт был удален.");
                                MessageShow.Show("Вы были отключены.\nВаш аккаунт был удален.", "Авторизация", MessageShow.Type.Error);


                           
                                
                                
                           


                        });

                        }
                        break;

                    case Code.LoginIsAuthorized:

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            _Main.Instance.MyAccount.Dispose();

                            foreach (var item in Application.Current.Windows)
                            {
                                var window = item as _Main;
                                var noti = item as Notification;
                                if (window != null || noti!=null)
                                {
                                    continue;
                                }

                           (item as Window).Close();
                            }

                            Logger.Error("Вы были отключены. Похоже кто то авторизовался с вашей учетной записи");
                            MessageShow.Show("Вы были отключены.\nПохоже кто то авторизовался с вашей учетной записи", "Авторизация", MessageShow.Type.Error);



                       


                        });

                        break;
                    case Code.InvalidUserNameOrPassword:
                        break;
                    case Code.RegistrationSuccessful:
                        break;
                    case Code.InvalidLogin:
                        break;
                    case Code.InvalidRegistration:
                        break;
                    case Code.AccountBanned:

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            _Main.Instance.MyAccount.Dispose();
                            Logger.Error("Ваш аккаунт был забанен");
                            MessageShow.Show("Ваш аккаунт был забанен", "Авторизация", MessageShow.Type.Error);
                        });

                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Command_DisconnectClass.Execut вызвал ошибку: {ex.Message}");
            }
        }
    }
}
