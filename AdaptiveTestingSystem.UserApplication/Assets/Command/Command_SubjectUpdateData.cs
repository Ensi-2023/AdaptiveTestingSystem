using AdaptiveTestingSystem.UserApplication.Assets.GUI.Users._user_page.window;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Users._user_page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Subject._subject_subpage.window;

namespace AdaptiveTestingSystem.UserApplication.Assets.Command
{
    public class Command_SubjectUpdateData:Commands
    {
        public override void Execut(string json, InternetClient client)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<Data_Subject>(json);
               
                if (obj == null) return;

                if (obj.IsCode == Code.Subject_Insert || obj.IsCode == Code.Subject_Update)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        _Main.Instance._Notification.Add("Данные предмета сохранены");

                        _Main.Instance.NotificationViewerManagerNotificationViewerManager.Add($"Данные предмета сохранены", type: TypeNotification.Message);


                    });

                    Application.Current.Dispatcher.Invoke(() => {

                        if (UIHelper.IsWindowOpen<GUI_Subject_Changer>())
                        {
                            foreach (var item in Application.Current.Windows)
                            {
                                var window = item as GUI_Subject_Changer;
                                if (window != null)
                                {
                                    Application.Current.Dispatcher.Invoke(() =>
                                    {

                                        if (obj.IsCode == Code.Subject_Update) window.Update(obj.Id_data, obj.Name_data);                                        
                                        window.Close();
                                    });

                                    break;
                                }
                            }
                        }
                    });
                }

                if (obj.IsCode == Code.Subject_Error)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        _Main.Instance._Notification.Add("", "Ошибка сохранения, возможно такой предмет уже сущуствует", TypeNotification.Error);
                        _Main.Instance.NotificationViewerManagerNotificationViewerManager.Add($"Ошибка сохранения, возможно такой предмет уже сущуствует", type: TypeNotification.Error);

                        Application.Current.Dispatcher.Invoke(() => {

                            if (UIHelper.IsWindowOpen<GUI_Subject_Changer>())
                            {
                                foreach (var item in Application.Current.Windows)
                                {
                                    var window = item as GUI_Subject_Changer;
                                    if (window != null)
                                    {
                                        Application.Current.Dispatcher.Invoke(() =>
                                        {
                                            window.Overlay("Ошибка сохранения, возможно такой предмет уже сущуствует");
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
                Logger.Error($"Command_SubjectUpdateData.Execut вызвал ошибку: {ex.Message}");
            }
        }
    }
}
