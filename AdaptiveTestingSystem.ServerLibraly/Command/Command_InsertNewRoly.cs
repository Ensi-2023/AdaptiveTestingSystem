using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_InsertNewRoly:Commands
    {
        public override async void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<Data_RolyInf>(json);

                if (obj != null)
                {


                    if (await DBAddingMethod.InserNewRoly(obj))
                    {

                        obj = new Data_RolyInf();
                        obj.IsCode = Code.Roly_Add_Successfull;
                    }
                    else
                    {
                        obj = new Data_RolyInf();
                        obj.IsCode = Code.Roly_NameError;
                    }


                    var comman = new Data_FirstCommand()
                    {
                        Command = "Command_InfAddNewRoly",
                        Json = JsonSerializer.Serialize(obj)
                    };

                    Send(client, activeServer, comman);


                }

            }
            catch(Exception ex) { Logger.Error($"Command_InsertNewRoly вызвал ошибку: {ex.Message}"); }
        }
    }
}
