using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_SendResultTestig:Commands
    {
        public override async void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<Data_TestRun>(json);

                if (obj != null)
                {
                    await DBAddingMethod.AddToResultTable(obj);
                }

            }
            catch (Exception ex)
            {
                Logger.Error($"Command_SendResultTestig вызвал ошибку: {ex.Message}");
            }
        }
    }
}
