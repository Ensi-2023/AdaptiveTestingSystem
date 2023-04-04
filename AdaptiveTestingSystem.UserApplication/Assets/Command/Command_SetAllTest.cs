using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.Command
{
    public class Command_SetAllTest:Commands
    {
        public override void Execut(string json, InternetClient client)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<List<Data_Testing>>(json);
                if (obj == null) return;
                Application.Current.Dispatcher.Invoke(async () =>
                {
                    await Task.Factory.StartNew(() => _Main.Instance.MVVM_Manager.TestingModel.SetCollection(obj));
                });
            }
            catch (Exception ex)
            {
                Logger.Error($"Command_SetAllTest.Execut вызывал ошибку: {ex.Message}");
            }
        }
    }
}
