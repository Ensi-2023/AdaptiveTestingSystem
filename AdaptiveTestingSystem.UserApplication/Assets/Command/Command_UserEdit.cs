using AdaptiveTestingSystem.UserApplication.Assets.GUI.Users._user_page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.Command
{
    public class Command_UserEdit:Commands
    {
        public override void Execut(string json, InternetClient client)
        {
            var obj = JsonSerializer.Deserialize<Data_UserEdit_Save>(json);
            if (obj == null) return;

            switch(obj.IsCode) 
            {
                case Code.User_Edit_Save:
                    _Main.Instance.OverlayShow(false,awaiter:true);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        var userViewer = (_Main.Instance.MainBody.Children[0] as View_BodyApplication).Main.Children[0] as GUI_User_Viewer;
                        if (userViewer == null) return;
                        userViewer.SaveData();


                    });



                    break;
                case Code.User_Edit_Error:
                    _Main.Instance.OverlayShow(true, TypeOverlay.error, "Ошибка при сохранении", "Возникла непредвиденная ошибка. Попробуйте попозже", visibleButton: Visibility.Visible);
                    break;

                case Code.User_Edit_PasswordError:
                    _Main.Instance.OverlayShow(true, TypeOverlay.error, "Ошибка при изменении пароля", "Старый пароль не верен.", visibleButton: Visibility.Visible);
                    break;

            }
        }
    }
}
