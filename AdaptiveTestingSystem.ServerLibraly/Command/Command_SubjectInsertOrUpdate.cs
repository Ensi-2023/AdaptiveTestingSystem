using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_SubjectInsertOrUpdate:Commands
    {
        public override async void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<Data_Subject>(json);
                if (obj == null) return;
                Data_FirstCommand command = ErrorInsertingClassRoom();


                if (await DBAddingMethod.InsertOrUpdateSubject(obj))
                {

                    int index = 0;

                    if (obj.IsCode == Code.Subject_Update) index = obj.Id_data;
                    if (obj.IsCode == Code.Subject_Insert) index = await DBSearchMethods.SearchIndexSubject(obj.Name_data);

                    var packet = new Data_Subject()
                    {
                        Id_data = index,
                        IsCode = obj.IsCode,
                        Name_data = obj.Name_data
                    };

                    command = new Data_FirstCommand()
                    {
                        Command = "Command_SubjectUpdateData",
                        Json = JsonSerializer.Serialize(packet)
                    };


                }
                else
                {
                    command = ErrorInsertingClassRoom();

                }


                Send(client, activeServer, command);

            }
            catch (Exception ex)
            {
                Logger.Error($"Command_SubjectInsertOrUpdate ({client.IP}:{client.Port}) вызвал ошибку: {ex.Message}");
            }
        }

        private static Data_FirstCommand ErrorInsertingClassRoom()
        {
            Data_FirstCommand command;
            var send = new Data_Subject()
            {
                IsCode = Code.Subject_Error
            };

            command = new Data_FirstCommand()
            {
                Command = "Command_SubjectUpdateData",
                Json = JsonSerializer.Serialize(send)
            };
            return command;
        }
    }
}
