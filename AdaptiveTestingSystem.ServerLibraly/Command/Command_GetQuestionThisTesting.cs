using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_GetQuestionThisTesting:Commands
    {
        public override async void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {

                var obj = JsonSerializer.Deserialize<Data_Testing>(json);
                if (obj == null) return;

                var list = await DBDataMethod.GetTestingQuestionList(obj.Index);

                var comman = new Data_FirstCommand()
                {
                    Command = "Command_SetQuestionThisTesting",
                    Json = JsonSerializer.Serialize(list)
                };

                Send(client, activeServer, comman);

            }
            catch (Exception ex)
            {
                Logger.Error($"Command_GetSubjectList вызвал ошибку: {ex.Message}");
            }
        }
    }
}
