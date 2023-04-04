using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_GetUserInformationByIndex:Commands
    {
        public override async void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<Data_UserList>(json);
                if (obj == null) return;
                var user = await DBSearchMethods.GetUserInformationByIndex(obj.Id);

                if (user != null)
                {
                    var packet = new Data_FirstCommand
                    {
                        Command = "Command_SetUserInformationByIndex",
                        Json = JsonSerializer.Serialize(user)
                    };


                    Send(client, activeServer, packet);
                }
               
            }
            catch (Exception ex)
            {
                Logger.Error($"Command_UserToClassRoom ({client.IP}:{client.Port}) вызвал ошибку: {ex.Message}");
            }
        }
    }
}
