using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_GetFreeSocket:Commands
    {
        public override async void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {
                var socket = await activeServer.IC_Socket.StartSocket();

                var packet = new Data_SendClientSocket()
                { 
                     IP = socket.IPAddressAddress.ToString(), 
                     Port = socket.Port,
                };

                var comman = new Data_FirstCommand()
                {
                    Command = "Command_SetFreeSocket",
                    Json = JsonSerializer.Serialize(packet)
                };

                Send(client, activeServer, comman);

            }
            catch (Exception ex)
            {
                Logger.Error($"Command_GetFreeSocket вызвал ошибку: {ex.Message}");
            }
        }
    }
}
