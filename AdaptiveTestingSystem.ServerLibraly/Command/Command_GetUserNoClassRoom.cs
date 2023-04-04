using AdaptiveTestingSystem.ServerLibraly.CScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_GetUserNoClassRoom:Commands
    {

        ThreadSendPacket sendPacket;

        public override async void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {

                var obj = JsonSerializer.Deserialize<Data_UserViewer>(json);

                if (obj != null)
                {
                    switch (obj.IsCode)
                    {
                        case Code.ThreadStart: StartSendData(client, activeServer); break;
                        case Code.ThreadNext: sendPacket.CancelWaitSendPacket(); break;
                        case Code.ThreadEnd: StopSend(); break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Command_GetUserNoClassRoom ({client.IP}:{client.Port}) вызвал ошибку: {ex.Message}");
            }
        }

        private void StopSend()
        {
            if (sendPacket != null) { sendPacket.Dispose(); sendPacket = null; }
        }

        private async void StartSendData(ClientObject client, ServerObject activeServer)
        {
            sendPacket = new ThreadSendPacket("Command_SetUserNoClassRoom", client, activeServer);
            var dataViewer = new Data_UserViewer();
            var list = await DBSearchMethods.GetUserNoClassRoom();
            if (list != null)
            {
                dataViewer.UserList = list;
            }
            else
            {
                dataViewer.IsCode = Code.Null;
            }

            sendPacket.StartSend(dataViewer);
        }
    }
}
