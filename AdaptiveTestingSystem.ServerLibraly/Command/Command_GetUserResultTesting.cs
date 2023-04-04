using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_GetUserResultTesting:Commands
    {
        public override async void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<Data_ResultTesting>(json);
                if (obj != null)
                {
                    var packet = await DBSearchMethods.GetUserResultTesting(obj.Index);
                    var comman = new Data_FirstCommand()
                    {
                        Command = "Command_SetUserResultTesting",
                        Json = JsonSerializer.Serialize(packet)
                    };

                    Send(client, activeServer, comman);

                }
    
            }
            catch (Exception ex)
            {
                Logger.Error($"Command_GetUserResultTesting ({client.IP}:{client.Port}) вызвал ошибку: {ex.Message}");
            }
        }
    }
}
