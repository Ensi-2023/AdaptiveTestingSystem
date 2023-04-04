using AdaptiveTestingSystem.Data.JsonData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.Command
{
    public class Command_SetFreeSocket:Commands
    {
        public override void Execut(string json, InternetClient client)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<Data_SendClientSocket>(json);
                if (obj == null) return;

                UPDSendData.Setup(obj.IP, obj.Port);
        
            }
            catch (Exception ex)
            {
                Logger.Error($"Command_SetSubjectList.Execut вызывал ошибку: {ex.Message}");
            }
        }
    }
}
