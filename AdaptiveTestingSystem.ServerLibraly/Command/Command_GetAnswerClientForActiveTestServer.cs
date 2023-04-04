using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerLibraly.Command  
{
    public class Command_GetAnswerClientForActiveTestServer:Commands
    {
        public override void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {
                var send = JsonSerializer.Deserialize<Data_MultyServerSendAnswer>(json);
                if (send != null)
                {
                    var server = activeServer.ServerBrowser.GetServerByIndex(send.IndexServer);
                    if (server == null) return;
                    send.GUID = client.GuidClient;

                    Data_FirstCommand data_First = new Data_FirstCommand()
                    {
                        Json = JsonSerializer.Serialize(send),
                        Command = "Command_SetAnswerClientForActiveTestServer"
                    };                  

                    activeServer.Send(JsonSerializer.Serialize(data_First), server.CreatorGuid);

                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Command_GetAnswerClientForActiveTestServer ({client.IP}:{client.Port}) вызвал ошибку: {ex.Message}");
            }
        }
    }
}
