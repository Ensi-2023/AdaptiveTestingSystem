using AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.Command
{
    public class Command_SetAnswerClientForActiveTestServer:Commands
    {
        public override void Execut(string json, InternetClient client)
        {
            try
            {
                var send = JsonSerializer.Deserialize<Data_MultyServerSendAnswer>(json);
                if (send != null)
                {
                    Application.Current.Dispatcher.Invoke(() => {
                   var guiUID = (_Main.Instance.MainBody.Children[0] as View_BodyApplication).Main.Children[0] as GUI_TestingServerAdminPanel;
                    if (guiUID != null)
                    {
                        guiUID.SetAnswerUser(send);
                    }
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Command_SetAnswerClientForActiveTestServer вызвал ошибку: {ex.Message}");
            }
        }
    }
}
