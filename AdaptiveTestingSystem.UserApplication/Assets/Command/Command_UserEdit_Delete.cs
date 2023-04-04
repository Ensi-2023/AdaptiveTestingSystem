using AdaptiveTestingSystem.Data.JsonData;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Users._user_page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.Command
{
    public class Command_UserEdit_Delete:Commands
    {
        public override void Execut(string json, InternetClient client)
        {
            var obj = JsonSerializer.Deserialize<Data_UserEdit_Delete>(json);
            if (obj!= null)
            {
                switch (obj.IsCode)
                {
                    case Code.User_Delete_Error:
                        _Main.Instance.OverlayShow(true, TypeOverlay.error, "Ошибка при удалении", "Возникла непредвиденная ошибка. Попробуйте попозже", visibleButton: Visibility.Visible);
                        break;
                    case Code.User_Delete_WithoutAccess:
                        _Main.Instance.OverlayShow(true, TypeOverlay.error, "Ошибка при удалении", "У вас нет прав на это.", visibleButton: Visibility.Visible);

                        break;
                    case Code.User_Delete_Сompleted:

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            _Main.Instance._Notification.Add("Удаление", "Пользователь удален", TypeNotification.Message);
                            _Main.Instance.Manager.Back();
                        });
                        break;
                }
            }
        }
    }
}
