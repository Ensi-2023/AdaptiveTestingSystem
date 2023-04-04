using AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage._testing_gui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace AdaptiveTestingSystem.UserApplication.Assets.Command
{
   public class Command_StartTesting:Commands
    {
        public override void Execut(string json, InternetClient client)
        {
            // Data_StartTesting

            try
            {
                var obj = JsonSerializer.Deserialize<Data_StartTesting>(json);
                if (obj == null) return;

                Application.Current.Dispatcher.Invoke(() =>
                {
                    switch (obj.IsCode)
                    {
                        case Code.StartTestingUser: UserStart(obj.IsAdaptive); break;
                        case Code.StartTestingForAdmin: AdminStart(); break;
                    }
              
                });
            }
            catch (Exception ex)
            {
                Logger.Error($"Command_StartTesting.Execut вызывал ошибку: {ex.Message}");
            }
        }

        private void UserStart(bool isAdaptive)
        {
            Application.Current.Dispatcher.Invoke(() => { 


            if (UIHelper.IsWindowOpen<GUI_TestReady>())
            {
                foreach (var item in Application.Current.Windows)
                {
                    var window = item as GUI_TestReady;
                    if (window != null)
                    {

                        var userViewer = window.body.Children[0] as GUI_TestingRun;
                        if (userViewer == null) return;
                        userViewer.StartMulty(isAdaptive);
                    }
                }
            }
            });
        }

        private void AdminStart()
        {
            Application.Current.Dispatcher.Invoke(() => { 

                var guiUID = (_Main.Instance.MainBody.Children[0] as View_BodyApplication).Main.Children[0] as GUI_TestingServerAdminPanel;
            if (guiUID != null)
            {
                guiUID.StartTest();
            }
            });
        }
    }
}
