using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_GetAllTest:Commands
    {
        public override async void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {
                var list = await DBDataMethod.GetTestList();

                var comman = new Data_FirstCommand()
                {
                    Command = "Command_SetAllTest",
                    Json = JsonSerializer.Serialize(list)
                };

                Send(client, activeServer, comman);

            }
            catch (Exception ex)
            {
                Logger.Error($"Command_GetAllTest ({client.IP}:{client.Port}) вызвал ошибку: {ex.Message}");
            }
        }
    }
}
