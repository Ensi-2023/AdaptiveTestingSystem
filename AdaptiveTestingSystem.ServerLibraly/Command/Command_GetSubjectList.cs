using AdaptiveTestingSystem.ServerLibraly.CScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_GetSubjectList:Commands
    {

        ThreadSendPacket sendPacket;
        public override void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<Data_Subject>(json);

                if (obj != null)
                {
                    switch (obj.IsCode)
                    {
                        case Code.ThreadStart: StartSendData(obj, client, activeServer); break;
                        case Code.ThreadNext: sendPacket.CancelWaitSendPacket(); break;
                        case Code.ThreadEnd: StopSend(); break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Command_GetSubjectList вызвал ошибку: {ex.Message}");
            }
        }


        private void StopSend()
        {
            if (sendPacket != null) { sendPacket.Dispose(); sendPacket = null; }
        }

        private async void StartSendData(Data_Subject obj, ClientObject client, ServerObject activeServer)
        {
            sendPacket = new ThreadSendPacket("Command_SetSubjectList", client, activeServer);
            var listEmployee = await DBDataMethod.GetSubjectList();
            sendPacket.StartSend(listEmployee);
        }
    }
}
