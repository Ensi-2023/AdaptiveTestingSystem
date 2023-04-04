using AdaptiveTestingSystem.UserApplication.Assets.GUI.Roly._roly_subpage;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.Command
{
    public class Command_SetSubjectListInGeneratorTest:Commands 
    {
        public override void Execut(string json, InternetClient client)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<List<Data_Subject>>(json);
                if (obj == null) return;

                Application.Current.Dispatcher.Invoke(() =>
                {

                    var guiUID = (_Main.Instance.MainBody.Children[0] as View_BodyApplication).Main.Children[0] as GUI_GeneratorTest;
                    if (guiUID != null)
                    {
                        guiUID.SetDataPacket(obj);
                    }
                });
            }
            catch (Exception ex)
            {
                Logger.Error($"Command_SetRolyInformationList.Execut вызывал ошибку: {ex.Message}");
            }
        }
    }
}
