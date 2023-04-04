using AdaptiveTestingSystem.UserApplication.Assets.GUI.Users._user_page;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Users._user_page.window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.Command
{
    public class Command_User_Insert_ClassRoomInsert:Commands
    {
        public override void Execut(string json, InternetClient client)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<Data_Klass>(json);

                if (obj == null) return;
             
                if(obj.IsCode == Code.SuccessfulInsertClassRoom)
                {
                    Application.Current.Dispatcher.Invoke(() => 
                    {
                        _Main.Instance._Notification.Add("Данные класса сохранены");

                        _Main.Instance.NotificationViewerManagerNotificationViewerManager.Add($"Данные класса сохранены", type: TypeNotification.Message);


                    });

                    Application.Current.Dispatcher.Invoke(() => {

                    if (UIHelper.IsWindowOpen<GUI_User_Insert_ClassRoomInsert>())
                    {
                        foreach (var item in Application.Current.Windows)
                        { 
                            var window = item as GUI_User_Insert_ClassRoomInsert;
                            if (window != null)
                            {
                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    window.Close();
                                });

                                break;
                            }
                        }
                        }
                    });


                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        var guiUID = (_Main.Instance.MainBody.Children[0] as View_BodyApplication).Main.Children[0] as GUI_Users_Insert;
                        if (guiUID == null) return;



                       // Logger.Debug($"guiUID.student.IsChecked: {guiUID.student.IsChecked}");
                        var klass = new Data_Klass()
                        {
                            IsCheking = guiUID.student.IsChecked==true?false:true
                        };

                        var send = new Data_FirstCommand()
                        {
                            Command = "Command_GetKlassInfo",
                            Json = JsonSerializer.Serialize(klass)
                        };


                        _Main.Instance.Client.Send(JsonSerializer.Serialize(send));

                    });


                }

                if (obj.IsCode == Code.ErrorInsertClassRoom)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        _Main.Instance._Notification.Add("","Ошибка сохранения, возможно такой класс уже сущуствует",TypeNotification.Error);
                        _Main.Instance.NotificationViewerManagerNotificationViewerManager.Add($"Ошибка сохранения, возможно такой класс уже сущуствует", type: TypeNotification.Error);

                        Application.Current.Dispatcher.Invoke(() => {

                            if (UIHelper.IsWindowOpen<GUI_User_Insert_ClassRoomInsert>())
                            {
                                foreach (var item in Application.Current.Windows)
                                {
                                    var window = item as GUI_User_Insert_ClassRoomInsert;
                                    if (window != null)
                                    {
                                        Application.Current.Dispatcher.Invoke(() =>
                                        {
                                            window.Overlay("Ошибка сохранения, возможно такой класс уже сущуствует");
                                        });

                                        break;
                                    }
                                }
                            }
                        });


                    });
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Command_User_Insert_ClassRoomInsert.Execut вызвал ошибку: {ex.Message}");
            }
        }
    }
}
