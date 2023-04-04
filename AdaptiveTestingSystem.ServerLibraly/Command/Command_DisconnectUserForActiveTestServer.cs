using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_DisconnectUserForActiveTestServer:Commands
    {
        public override void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {
                var indexServer = JsonSerializer.Deserialize<Data_ConnectTestingServer>(json);
                if (indexServer != null)
                {
                    activeServer.ServerBrowser.RemoveClient(client, indexServer.IndexServer);
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Command_ConnectToServerThisIndexServer ({client.IP}:{client.Port}) вызвал ошибку: {ex.Message}");
            }
        }

    }
}
