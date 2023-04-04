using AdaptiveTestingSystem.ServerLibraly.Command.InternalCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_GetFilterUserList:Commands
    {
        public override void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {

                var command = JsonSerializer.Deserialize<Data_UserPacket>(json);
                if (command != null)
                {
                    switch (command.IsCode)
                    {
                        case Code.ThreadStart: StartSendData(command, client, activeServer); break;
                        case Code.ThreadNext: IC_SendDataUser.CancelWaitSendPacket(); break;
                        case Code.ThreadEnd: StopSend(client, activeServer); break;
                    }


                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Command_GetUserList ({client.IP}:{client.Port}) вызвал ошибку: {ex.Message}");
            }
        }

        private void StopSend(ClientObject client, ServerObject activeServer)
        {
            IC_SendDataUser.Stop();
        }

        private async void StartSendData(Data_UserPacket command, ClientObject client, ServerObject activeServer)
        {
            var userList = await DBDataMethod.GetFilterUserList(command.IsCode, command.FilterUser);
            var packet = new Data_UserViewer()
            {
                IsCode = command.IsCode,
                UserList = userList,
                FilterUser = command.FilterUser,
                IsFilter = true,
            };

            IC_SendDataUser.StartSend(packet, client, activeServer);
        }
    }
}

