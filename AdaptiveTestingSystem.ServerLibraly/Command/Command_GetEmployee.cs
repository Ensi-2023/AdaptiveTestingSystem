using AdaptiveTestingSystem.ServerLibraly.CScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_GetEmployee:Commands
    {

        ThreadSendPacket sendPacket;

        public override async void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {

                var obj = JsonSerializer.Deserialize<Data_UserList>(json);

                if (obj != null)
                {
                    switch (obj.IsCode)
                    {
                        case Code.ThreadStart: StartSendData(obj, client, activeServer); break;
                        case Code.ThreadNext: sendPacket.CancelWaitSendPacket(); break;
                        case Code.ThreadEnd: StopSend(); break;
                    }
                }



                //var obj = JsonSerializer.Deserialize<Data_UserList>(json);
                //if (obj == null) return;

                //var listEmployee = await DBSearchMethods.GetListEmployee(obj);
                //if (listEmployee != null)
                //{
                //    var packet = new Data_FirstCommand
                //    {
                //        Command = "Command_SetEmployee",
                //        Json = JsonSerializer.Serialize(listEmployee)
                //    };


                //    Send(client, activeServer, packet);

                //}          
            }
            catch (Exception ex)
            {
                Logger.Error($"Command_GetEmployee ({client.IP}:{client.Port}) вызвал ошибку: {ex.Message}");
            }
        }

        private void StopSend()
        {
            if (sendPacket != null) { sendPacket.Dispose(); sendPacket = null; }
        }

        private async void StartSendData(Data_UserList obj, ClientObject client, ServerObject activeServer)
        {
            sendPacket = new ThreadSendPacket("Command_SetEmployee", client, activeServer);

            var listEmployee = await DBSearchMethods.GetListEmployee(obj);
            sendPacket.StartSend(listEmployee);
        }

    }
}
