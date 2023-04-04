using AdaptiveTestingSystem.ServerLibraly.Command.InternalCommand;
using AdaptiveTestingSystem.ServerLibraly.CScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_GetUserList:Commands
    {

        ThreadSendPacket sendPacket;

        public override void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {
                var command = JsonSerializer.Deserialize<Data_UserPacket>(json);
                if (command != null)
                {
                    switch (command.IsCode)
                    {
                        case Code.ThreadStart: StartSendData(command.IsRoleUser, client, activeServer); break;
                        case Code.ThreadNext: sendPacket.CancelWaitSendPacket(); break;
                        case Code.ThreadEnd: StopSend(client, activeServer); break;
                    }
                     

                }
            }
            catch (Exception ex)
            {
             
                Logger.Error($"Command_GetUserList ({client.IP}:{client.Port}) вызвал ошибку: {ex.Message}");
                StopSend(client, activeServer);

            }
        }

        private void StopSend(ClientObject client, ServerObject activeServer)
        {
             //IC_SendDataUser.Stop();

           sendPacket.Dispose();   
        }

        private async void StartSendData(Code isRoleUser, ClientObject client, ServerObject activeServer)
        {
            sendPacket = new ThreadSendPacket("Command_SetUserList", client, activeServer);

            var userList = await DBDataMethod.GetUserList(isRoleUser);
            var packet = new Data_UserViewer()
            {
                IsCode = isRoleUser,
                UserList = userList,
            };

            sendPacket.StartSend(packet);

           // IC_SendDataUser.StartSend(packet, client, activeServer);


        }
    }
}
