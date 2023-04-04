using AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.Command
{
    public class Command_SetStatusClientForConnectTestServer:Commands
    {
        public override void Execut(string json, InternetClient client)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<Data_MultyServerClient>(json);
                if (obj == null) return;

                Application.Current.Dispatcher.Invoke(() =>
                {

                    var guiUID = (_Main.Instance.MainBody.Children[0] as View_BodyApplication).Main.Children[0] as GUI_TestingServerAdminPanel;
                    if (guiUID != null)
                    {
                        guiUID.SetStatusUser(obj);
                    }
                });
            }
            catch (Exception ex)
            {
                Logger.Error($"Command_SetStatusClientForConnectTestServer.Execut вызывал ошибку: {ex.Message}");
            }
            
        }
    }
}
