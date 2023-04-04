using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_DeleteQuestionFromTest:Commands
    {
        public override async void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<Data_DeleteQuestions>(json);
                if (obj == null) return;


                foreach(var item in obj.Questions)
                {
                    await DBDataMethod.SQLCommandNoTransaction($"delete from Answer Where ID_Questions = {item.Index}");
                    await DBDataMethod.SQLCommandNoTransaction($"delete from Questions Where ID_Quest = {item.Index}");    
                }

            }
            catch (Exception ex)
            {
                Logger.Error($"Command_DeleteQuestionFromTest вызвал ошибку: {ex.Message}");
            }
        }
    }
}
