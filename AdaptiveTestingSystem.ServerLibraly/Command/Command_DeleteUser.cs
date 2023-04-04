using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_DeleteUser:Commands
    {
        public override async void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {
                var list = JsonSerializer.Deserialize<List<Data_DeleteUser>>(json);
                if (list == null) return;

                foreach (var item in list)
                {
                    string login = await DBSearchMethods.ReturnLogin(item.Id);   
                    await CloseAuthorization(activeServer,login);
                }

                await DBSearchMethods.DeleteUser(list);
            } 
            catch(Exception ex)
            {
                Logger.Error($"Command_DeleteUser.Execut вызвал ошибку: {ex.Message}");
            }
        }

        private static async Task CloseAuthorization(ServerObject activeServer,string login)
        {
            if (await activeServer.IsCheckAuthorizedClient(login))
            {

                var guid = await activeServer.IsReturnAuthorizedGUID(login);
                var disc = new Data_Disconnect()
                {
                    GUI = guid,
                    IsCode = Code.Delete
                };


                var comm = new Data_FirstCommand()
                {
                    Command = "Command_DisconnectClass",
                    Json = JsonSerializer.Serialize(disc)
                };

                activeServer.Send(JsonSerializer.Serialize(comm), guid);
                activeServer.DeleteInListAuthorizationGUID(guid);
            }
        }
    }
}
