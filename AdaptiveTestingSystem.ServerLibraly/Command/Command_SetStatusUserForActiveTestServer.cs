using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_SetStatusUserForActiveTestServer:Commands
    {
        public override void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {
                var status = JsonSerializer.Deserialize<Data_PacketServerStatusUser>(json);
                if (status != null)
                {
                    activeServer.ServerBrowser.StatusClient(client, status.IndexServer, status.IsCode);
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Command_SetStatusUserForActiveTestServer ({client.IP}:{client.Port}) вызвал ошибку: {ex.Message}");
            }
        }
    }
}
