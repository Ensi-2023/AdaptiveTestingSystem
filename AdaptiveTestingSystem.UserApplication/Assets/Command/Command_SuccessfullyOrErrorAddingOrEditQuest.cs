using AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage.window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.Command
{
    public class Command_SuccessfullyOrErrorAddingOrEditQuest:Commands
    {
        public override void Execut(string json, InternetClient client)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<Data_Code>(json);
                if (obj == null) return;

                Logger.Log($"{obj.IsCode}");

                Application.Current.Dispatcher.Invoke(() =>
                {
                    switch (obj.IsCode)
                         {
                            case Code.Error:
                                            _Main.Instance.OverlayShow(true, TypeOverlay.error, "Генерация вопросов", $"Возникла непредвиденная ошибка, попробуйте отправить данные еще раз", visibleButton: Visibility.Visible);
                                            break;
                             case Code.Successfully:

                                                                    
                                            _Main.Instance.Manager.Back();
                                            _Main.Instance._Notification.Add("Успешно отправленно. Скоро сервер обработает данные");
                                            _Main.Instance.NotificationViewerManagerNotificationViewerManager.Add("Успешно отправленно. Скоро сервер обработает данные", "Вопросы");
                                            break;
                                    }


                                });

            }
            catch (Exception ex)
            {
                Logger.Error($"Command_SuccessfullyOrErrorAddingOrEditQuest.Execut вызывал ошибку: {ex.Message}");
            }
        }
    }
}
