using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_ClassRoom_UpdateEmployee:Commands
    {
        public override async void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<Data_Klass_UpdateEmployee>(json);
                if (obj == null) return;

                if (await DBDataMethod.EditClassRoomEmployee(obj))
                { 
                    
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Command_ClassRoom_UpdateEmployee ({client.IP}:{client.Port}) вызвал ошибку: {ex.Message}");
            }
        }
    }
}
