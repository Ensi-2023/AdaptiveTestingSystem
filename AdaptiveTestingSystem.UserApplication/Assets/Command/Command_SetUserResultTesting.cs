using AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage._testing_gui;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.Command
{
    public class Command_SetUserResultTesting:Commands
    {
        public override void Execut(string json, InternetClient client)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<Data_ResultTesting>(json);
                if (obj == null) return;

                Application.Current.Dispatcher.Invoke(() =>
                {
                    _Main.Instance.MyAccount.CountAssessment2 = obj.CountAssessment2;
                    _Main.Instance.MyAccount.CountAssessment3 = obj.CountAssessment3;
                    _Main.Instance.MyAccount.CountAssessment4 = obj.CountAssessment4;
                    _Main.Instance.MyAccount.CountAssessment5 = obj.CountAssessment5;


                    if (UIHelper.IsWindowOpen<GUI_TestReady>())
                    {
                        foreach (var item in Application.Current.Windows)
                        {
                            var window = item as GUI_TestReady;
                            if (window != null)
                            {

                                var userViewer = window.body.Children[0] as GUI_TestingRun;
                                if (userViewer == null) return;
                                userViewer.UploadDataTest(obj);
                            }
                        }
                    }


                });
            }
            catch (Exception ex)
            {
                Logger.Error($"Command_SetUserResultTesting.Execut вызывал ошибку: {ex.Message}");
            }
        }
    }
}
