using AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.Command
{
    public class Command_ClientManagerForActiveTestingServer:Commands
    {
        public override void Execut(string json, InternetClient client)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<List<Data_MultyServerClient>>(json);
                if (obj == null) return;

                Application.Current.Dispatcher.Invoke(async () =>
                {
                    await Task.Factory.StartNew(() => 
                    {
                        Application.Current.Dispatcher.Invoke(() => 
                        {
                          

                            var guiUID = (_Main.Instance.MainBody.Children[0] as View_BodyApplication).Main.Children[0] as GUI_TestingServerAdminPanel;
                            if (guiUID != null)
                            {
                                guiUID.Connect(obj);
                                return;
                            }
                        });
                    });
                });


            }
            catch (Exception ex)
            {
                Logger.Error($"Command_SetSubjectList.Execut вызывал ошибку: {ex.Message}");
            }
        }
    }
}
