using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_ChangerRolyUser:Commands
    {
        public override async void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<Data_UpdateUserRoly>(json);
                if (obj == null) return;

                await DBSearchMethods.ChangedDataUserRoly(obj);

          
            }
            catch (Exception ex)
            {
                Logger.Error($"Command_ChangerRolyUser вызвал ошибку: {ex.Message}");
            }
        }
    }
}
