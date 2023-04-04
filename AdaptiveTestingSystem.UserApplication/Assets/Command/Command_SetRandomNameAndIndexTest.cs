using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.Command
{
    public class Command_SetRandomNameAndIndexTest:Commands
    {
        public override void Execut(string json, InternetClient client)
        {
            try
            {
                var obj = json.Replace("{","").Replace("}","");
                var mas = obj.Split(',');

                if (mas[0] == string.Empty && int.Parse(mas[1]) == 0) return;
                Application.Current.Dispatcher.Invoke(() =>
                {

                    Logger.Debug($"DB: {mas[0]} {mas[1]} ");

                    _Main.Instance.UI_TestReady.SetData(mas[0], int.Parse(mas[1]));
                    _Main.Instance.Hide();
                    _Main.Instance.UI_TestReady.ShowDialog();
                });
            }
            catch (Exception ex)
            {
                Logger.Error($"Command_SetRandomNameAndIndexTest.Execut вызывал ошибку: {ex.Message}");
            }
        }
    }
}
