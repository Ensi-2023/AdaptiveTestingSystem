using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_NewUserInsert:Commands
    {
        public override async void Execut(string json, ClientObject client, ServerObject activeServer)
        {

            try
            {
                var obj = JsonSerializer.Deserialize<Data_NewUserInsert>(json);
                if (obj == null)
                {
                    return;
                }

                if (await DBSearchMethods.IsCheckLoginAndPassword(obj.Login, obj.Password) == false)
                {
                    var reg = await DBAddingMethod.AddNewUserOrStaff(obj);
                    if (reg != null)
                    {
                        Send(client, activeServer, SuccessfulRegistration(reg));
                    }
                }
                else
                {
                    Send(client, activeServer, SendInvalid(obj.IsCode));
                }           
            }
            catch(Exception ex)
            {
                Logger.Error($"Command_NewUserInsert.Execut вызвал ошибку: {ex.Message}");
            }

        }


        private Data_FirstCommand SendInvalid(Code code)
        {

            if (code == Code.NewUserClassRoomInsert)
            {
                code = Code.NewUserClassRoomError;
            }
            else
            {
                code = Code.InvalidLogin;
            }

            var reg = new Data_NewUserInsert()
            {
                IsCode = code
            };

            var command = new Data_FirstCommand()
            {
                Command = "Command_NewUserInsert",
                Json = JsonSerializer.Serialize(reg)
            };

            return command;
        }
        private Data_FirstCommand SuccessfulRegistration(Data_NewUserInsert obj)
        {
            var command = new Data_FirstCommand()
            {
                Command = "Command_NewUserInsert",
                Json = JsonSerializer.Serialize(obj)
            };

            return command;
        }
    }
}
