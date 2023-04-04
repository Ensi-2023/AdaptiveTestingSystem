using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_UpdateDataUserRoly:Commands
    {
        public override async void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<Data_RolyInf>(json);
                if (obj == null) return;
                if (await DBAddingMethod.UpdateDataRoly(obj)==true)
                {

                    obj = new Data_RolyInf();
                    obj.IsCode = Code.Roly_Update_Successfull;
                }
                else
                {
                    obj = new Data_RolyInf();
                    obj.IsCode = Code.Roly_Update_NameError;
                }


                var comman = new Data_FirstCommand()
                {
                    Command = "Command_InfAddNewRoly",
                    Json = JsonSerializer.Serialize(obj)
                };

                Send(client, activeServer, comman);

            }
            catch (Exception ex)
            {
                Logger.Error($"Command_UpdateDataUserRoly вызвал ошибку: {ex.Message}");
            }
        }
    }
}
