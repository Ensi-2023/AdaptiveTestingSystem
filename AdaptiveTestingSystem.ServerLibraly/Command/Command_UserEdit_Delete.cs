using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_UserEdit_Delete:Commands
    {
        public override async void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            var obj = JsonSerializer.Deserialize<Data_UserEdit_Delete>(json);
            if (obj != null)
            {
                var indexUser = obj.IndexUser;

                if (await DBSearchMethods.UserAccessVerification(indexUser, "ReadUser"))
                {

                    var list = new List<Data_DeleteUser>
                    {
                        new Data_DeleteUser()
                        {
                            Id = obj.IndexDeletedUser
                        }
                    };

                    await DBSearchMethods.DeleteUser(list);

                    SendClient(client, activeServer, Code.User_Delete_Сompleted);
                } 
                else
                    SendClient(client, activeServer, Code.User_Delete_WithoutAccess);


            }
            else
            {
                SendClient(client,activeServer,Code.User_Delete_Error);
            }
        }


        private void SendClient(ClientObject client, ServerObject activeServer, Code code = Code.User_Edit_Save)
        {
            try
            {
                Data_UserEdit_Delete data = new Data_UserEdit_Delete()
                {
                    IsCode = code,
                };

                Data_FirstCommand command = new Data_FirstCommand()
                {
                    Command = "Command_UserEdit_Delete",
                    Json = JsonSerializer.Serialize(data)
                };

                activeServer.Send(JsonSerializer.Serialize(command), client.GuidClient);
            }
            catch (Exception ex)
            {
                Logger.Error($"Command_UserEdit_Delete ({client.IP}:{client.Port}) вызвал ошибку: {ex.Message}");
            }
        }
    }
}
