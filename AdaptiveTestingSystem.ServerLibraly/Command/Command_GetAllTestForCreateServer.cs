using AdaptiveTestingSystem.ServerLibraly.CScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_GetAllTestForCreateServer:Commands
    {
        ThreadSendPacket sendPacket;

        public override async void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {

                var obj = JsonSerializer.Deserialize<Data_AllTestForSB>(json);

                if (obj != null)
                {
                    switch (obj.IsCode)
                    {
                        case Code.ThreadStart: StartSendData( client, activeServer); break;
                        case Code.ThreadNext: sendPacket.CancelWaitSendPacket(); break;
                        case Code.ThreadEnd: StopSend(); break;
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.Error($"Command_GetAllTestForCreateServer ({client.IP}:{client.Port}) вызвал ошибку: {ex.Message}");
            }
        }

        private void StopSend()
        {
            if (sendPacket != null) { sendPacket.Dispose(); sendPacket = null; }
        }

        private async void StartSendData(ClientObject client, ServerObject activeServer)
        {
            sendPacket = new ThreadSendPacket("Command_SetAllTestForCreateServer", client, activeServer);

            var listEmployee = await DBSearchMethods.GetAllTestForCreateServer();
            sendPacket.StartSend(listEmployee);
        }

    }
}


