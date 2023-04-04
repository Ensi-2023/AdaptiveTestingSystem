using AdaptiveTestingSystem.Data.JsonData;
using AdaptiveTestingSystem.ServerLibraly.CScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_GetCLassRoomList:Commands
    {

        ThreadSendPacket sendPacket;
        public override void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {

                var obj = JsonSerializer.Deserialize<Data_Klass>(json);

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
               Logger.Error($"Command_GetCLassRoomList ({client.IP}:{client.Port}) вызвал ошибку: {ex.Message}");
           }
        }

        private void StopSend()
        {
            if (sendPacket != null) { sendPacket.Dispose(); sendPacket = null; }
        }

        private async void StartSendData(ClientObject client, ServerObject activeServer)
        {
            sendPacket = new ThreadSendPacket("Command_SetKlassList", client, activeServer);
            var listEmployee = await DBDataMethod.GetKlassList(false, true);
            sendPacket.StartSend(listEmployee);
        }

    }
}
