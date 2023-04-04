using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_UserEdit:Commands
    {
        public async override void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            var obj = JsonSerializer.Deserialize<Data_UserEdit>(json);
            try
            {
                if (obj != null)
                {

                    if (obj.IsEditPassword)
                    {

                        if (await DBSearchMethods.VertySysAdminPassword(obj.OldPassword, await DBSearchMethods.ReturnLogin(obj.IndexUser)))
                        {
                            await DBDataMethod.EditUserPassword(obj.NewPassword, obj.IndexUser.ToString());
                        }
                        else
                        {                  
                            SendClient(client, activeServer, Code.User_Edit_PasswordError);
                            return;
                        }
                        
        
                    }


                    if (obj.IsEditGender) 
                    {
                        await DBDataMethod.EditUserData("GenderUser", obj.Gender, obj.IndexUser.ToString());
                    }
                 
                    if (obj.IsEditDateBirch) 
                    {
                        await DBDataMethod.EditUserData("DatebirchUser", obj.DateBirch, obj.IndexUser.ToString());
                    }
                    if (obj.IsEditFIO) 
                    {
                        var mas = obj.FIO.Split(' ');
                        if (mas.Length > 2)
                        {
                            await DBDataMethod.EditUserData("SurnameUser", mas[0], obj.IndexUser.ToString());
                            await DBDataMethod.EditUserData("LastnameUser", mas[1], obj.IndexUser.ToString());
                            await DBDataMethod.EditUserData("MiddlenameUser", mas[2], obj.IndexUser.ToString());
                        } 
                        else 
                        {                         
                            await DBDataMethod.EditUserData("SurnameUser", mas[0], obj.IndexUser.ToString());
                            await DBDataMethod.EditUserData("LastnameUser", mas[1], obj.IndexUser.ToString());
                            await DBDataMethod.EditUserData("MiddlenameUser","NULL", obj.IndexUser.ToString());
                        }                      
                    }

                    SendClient(client, activeServer);

                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Command_UserEdit ({client.IP}:{client.Port}) вызвал ошибку: {ex.Message}");

                SendClient(client, activeServer, Code.User_Edit_Error);

            }
        }

        private static void SendClient(ClientObject client, ServerObject activeServer,Code code= Code.User_Edit_Save)
        {
            try
            {

                Data_UserEdit_Save data = new Data_UserEdit_Save()
                {
                    IsCode = code,
                };

                Data_FirstCommand command = new Data_FirstCommand()
                {
                    Command = "Command_UserEdit",
                    Json=JsonSerializer.Serialize(data)
                };

                activeServer.Send(JsonSerializer.Serialize<Data_FirstCommand>(command), client.GuidClient);
            }
            catch (Exception ex) 
            {
                Logger.Error($"Command_UserEdit ({client.IP}:{client.Port}) вызвал ошибку: {ex.Message}");
            }
        }
    }
}
