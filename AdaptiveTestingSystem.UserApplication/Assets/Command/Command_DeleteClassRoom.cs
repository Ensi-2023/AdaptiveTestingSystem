using AdaptiveTestingSystem.UserApplication.Assets.GUI.ClassRoom;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Users._user_page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.Command
{
    public class Command_DeleteClassRoom:Commands
    {
        public override void Execut(string json, InternetClient client)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<Data_Klass_Delete>(json);

                if (obj == null) return;

                if (obj.IsCode == Code.ErrorDeleteClassRoom_GUI_User)
                {
                    _Main.Instance.OverlayShow(true, TypeOverlay.error, "Ошибка удаления", $"Удаление завершилось с ошибкой\n{obj.Description}", visibleButton: Visibility.Visible);


                    _Main.Instance.NotificationViewerManagerNotificationViewerManager.Add($"Удаление завершилось с ошибкой\n{obj.Description}", type:TypeNotification.Error);

                    Logger.Log(obj.Description);

                    if (obj.Klasses.Count > 0)
                    {
                        DeleteSelections(obj);
                    }

                }

                if (obj.IsCode == Code.ErrorDeleteClassRoom)
                {
                    Update();

                    _Main.Instance.OverlayShow(false);
                    _Main.Instance.NotificationViewerManagerNotificationViewerManager.Add($"Удаление завершилось с ошибкой\n{obj.Description}", type: TypeNotification.Error);


                    _Main.Instance._Notification.Add("Удаление", "Возникла ошибка, проверьте оповещение", TypeNotification.Error);

                    Logger.Log(obj.Description);
                }


                if (obj.IsCode == Code.SuccessfullDeleteClassRoom_GUI_User)
                {
                    _Main.Instance._Notification.Add("Удаление", "Удаление успешно!", TypeNotification.Message);
                    DeleteSelections(obj);

                }


                if (obj.IsCode == Code.SuccessfullDeleteClassRoom)
                {
                    _Main.Instance._Notification.Add("Удаление", "Удаление успешно!", TypeNotification.Message);

                    Update();

                }

            }
            catch (Exception ex)
            {
                Logger.Error($"Command_DeleteClassRoom.Execut вызвал ошибку: {ex.Message}");
            }
        }

        private void Update()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var guiUID = (_Main.Instance.MainBody.Children[0] as View_BodyApplication).Main.Children[0] as GUI_ClassRoom;
                if (guiUID == null) return;
                guiUID.Update();
            });
        }

        private static void DeleteSelections(Data_Klass_Delete? obj)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var guiUID = (_Main.Instance.MainBody.Children[0] as View_BodyApplication).Main.Children[0] as GUI_Users_Insert;
                if (guiUID == null) return;

                guiUID.DeleteSelected(obj);

            });
        }
    }
}
