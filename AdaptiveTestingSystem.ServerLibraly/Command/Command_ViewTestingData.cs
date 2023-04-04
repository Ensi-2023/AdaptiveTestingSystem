using AdaptiveTestingSystem.ServerLibraly.CScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static AdaptiveTestingSystem.DLL.CScript.ControlEnum;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_ViewTestingData : Commands
    {
   
        ThreadSendPacket sendPacket;

        public override void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<Data_TestingView>(json);

                if (obj != null)
                {
                    switch (obj.IsCode)
                    {
                        case Code.ThreadStart: StartSendData(obj.Index, client, activeServer); break;
                        case Code.ThreadNext: sendPacket.CancelWaitSendPacket(); break;
                        case Code.ThreadEnd: StopSend(); break;
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.Error($"Command_ViewTestingData ({client.IP}:{client.Port}) вызвал ошибку: {ex.Message}");
            }
        }

        private void StopSend()
        {
            if (sendPacket != null) { sendPacket.Dispose(); sendPacket = null; }
        }

        private async void StartSendData(int index, ClientObject client, ServerObject activeServer)
        {
            sendPacket = new ThreadSendPacket("Command_ApendTestingData", client, activeServer);
            var data = await DBSearchMethods.GetSearchTestByIndex(index);
            sendPacket.StartSend(data);
        }
    }
}
