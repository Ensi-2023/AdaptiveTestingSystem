using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_CheckForPresencePredmet:Commands
    {
        public override async void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {

                var list = await DBDataMethod.GetSubjectList();

                var comman = new Data_FirstCommand()
                {
                    Command = "Command_SetSubjectListInGeneratorTest",
                    Json = JsonSerializer.Serialize(list)
                };

                Send(client, activeServer, comman);

            }
            catch (Exception ex)
            {
                Logger.Error($"Command_CheckForPresencePredmet вызвал ошибку: {ex.Message}");
            }
        }
    }
}
