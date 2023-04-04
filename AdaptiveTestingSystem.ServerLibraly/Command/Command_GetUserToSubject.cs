using AdaptiveTestingSystem.ServerLibraly.CScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_GetUserToSubject:Commands
    {
        ThreadSendPacket sendPacket;
        public override async void Execut(string json, ClientObject client, ServerObject activeServer)
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
                Logger.Error($"Command_GetUserToSubject ({client.IP}:{client.Port}) вызвал ошибку: {ex.Message}");
            }
        }


        private void StopSend()
        {
            if (sendPacket != null) { sendPacket.Dispose(); sendPacket = null; }
        }

        private async void StartSendData(Data_Subject obj, ClientObject client, ServerObject activeServer)
        {
            sendPacket = new ThreadSendPacket("Command_SetUserToSubject", client, activeServer);
            var dataViewer = new Data_UserViewer();
            var list = await DBSearchMethods.GetUserNoSubject(obj.Id_data);
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
