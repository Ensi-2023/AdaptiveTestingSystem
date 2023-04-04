using AdaptiveTestingSystem.UserApplication.Assets.GUI.ClassRoom._classRoom_page._classRoom_subPage._classRoom_subPage_control;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.ClassRoom._classRoom_page._classRoom_subPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage.window;

namespace AdaptiveTestingSystem.UserApplication.Assets.Command
{
    public class Command_SuccessfullyOrErrorAddingTest:Commands
    {
        public override void Execut(string json, InternetClient client)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<Data_Code>(json);
                if (obj == null) return;
               
                    Application.Current.Dispatcher.Invoke(() => {

                        if (UIHelper.IsWindowOpen<GUI_ViewCreateQuestions>())
                        {
                            foreach (var item in Application.Current.Windows)
                            {
                                var window = item as GUI_ViewCreateQuestions;
                                if (window != null)
                                {
                                    Application.Current.Dispatcher.Invoke(() =>
                                    {
                                        switch (obj.IsCode)
                                        {
                                            case Code.Error:
                                                window.OverlayShow(true, TypeOverlay.error, "Генерация теста", $"Возникла непредвиденная ошибка, попробуйте отправить данные еще раз", visibleButton: Visibility.Visible);
                                                break;
                                            case Code.Successfully:

                                            
                                                window.Close();
                                                _Main.Instance.Manager.Back();
                                                _Main.Instance._Notification.Add("Успешно отправленно. Скоро сервер обработает данные");
                                                _Main.Instance.NotificationViewerManagerNotificationViewerManager.Add("Успешно отправленно. Скоро сервер обработает данные", "Тесты");
                                                break;
                                        }

                                                                        
                                    });

                                    break;
                                }
                            }
                        }
                    });
               
            }
            catch (Exception ex)
            {
                Logger.Error($"Command_SuccessfullyOrErrorAddingTest.Execut вызывал ошибку: {ex.Message}");
            }
        }
    }
}
