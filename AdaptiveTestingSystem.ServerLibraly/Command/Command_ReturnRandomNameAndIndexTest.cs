using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_ReturnRandomNameAndIndexTest:Commands
    {
        public override async void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {

                (string,int) list = await DBSearchMethods.GetRandomNameAndIndexTest();

                var comman = new Data_FirstCommand()
                {
                    Command = "Command_SetRandomNameAndIndexTest",
                    Json = "{"+$"{list.Item1},{list.Item2}"+"}"
                };

                Send(client, activeServer, comman);
 

            }
            catch (Exception ex)
            {
                Logger.Error($"Command_ReturnRandomNameAndIndexTest вызвал ошибку: {ex.Message}");
            }
        }
    }
}
