using AdaptiveTestingSystem.UserApplication.Assets.GUI.Users._user_page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.Command
{
    public class Command_SetUserInformationByIndex:Commands
    {
        public override void Execut(string json, InternetClient client)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<Data_UserList>(json);
                if(obj!=null)
                {
                    User user = new User()
                    {
                        FIO = obj.Name,
                        Datebirch = obj.DateBirch,
                        Gender = obj.Gender,
                        Index = obj.Id,
                        Login = obj.Login,
                        Role = obj.Role 
                    };


                    Application.Current.Dispatcher.Invoke(() => {

                        _Main.Instance.OverlayShow(false);
                        var guiUID = (_Main.Instance.MainBody.Children[0] as View_BodyApplication).Main.Children[0] as GUI_User_Viewer;
                        if (guiUID != null)
                        {
                            guiUID.SetDateUser(user);
                            return;
                        }


               
                    });




                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Command_SetUserInformationByIndex.Execut вызывал ошибку: {ex.Message}");
            }
        }
    }
}
