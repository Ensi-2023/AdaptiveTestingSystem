using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_DeleteSubject:Commands
    {
        public override async void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<Data_Subject_Delete>(json);
                if (obj == null)
                {
                    return;
                }


                var deleted = await DBSearchMethods.DeleteSubject(obj);
                obj.Subject = null;

                if (deleted)
                {
             
                    obj.IsCode = Code.Subject_Delete;
                }
                else
                {
                    obj.IsCode = Code.Subject_Error;
                }


                Data_FirstCommand data_First = new Data_FirstCommand()
                {
                    Json = JsonSerializer.Serialize(deleted),
                    Command = "Command_DeleteSubject"
                };

                Send(client, activeServer, data_First);
            }
            catch (Exception ex)
            {
                Logger.Error($"Command_DeleteSubject.Execut вызвал ошибку: {ex.Message}");
            }
        }
    }
}
