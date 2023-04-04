using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_CloseActiveTestingServerThisIndex:Commands
    {
        public override async void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {
                var indexServer = JsonSerializer.Deserialize<Data_ConnectTestingServer>(json);
                if (indexServer != null)
                {
                    await activeServer.ServerBrowser.CloseServer(indexServer.IndexServer);
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Command_CloseActiveTestingServerThisIndex ({client.IP}:{client.Port}) вызвал ошибку: {ex.Message}");
            }
        }
    }
}
