using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_DeleteTest:Commands
    {
        public override async void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<List<Data_Testing>>(json);
                if (obj == null) return;





                await DBSearchMethods.DeleteTest(obj);
              


            }
            catch (Exception ex)
            {
                Logger.Error($"Command_DeleteTest вызвал ошибку: {ex.Message}");
            }
        }
    }
}
