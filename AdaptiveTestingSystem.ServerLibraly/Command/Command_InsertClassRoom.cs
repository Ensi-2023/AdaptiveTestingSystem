using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_InsertClassRoom:Commands
    {
        public override async void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {
                Data_FirstCommand command;
                var obj = JsonSerializer.Deserialize<Data_Klass>(json);

                if (obj == null)
                {
                    command = ErrorInsertingClassRoom();
                }
                else
                {

                    if (await DBAddingMethod.InsertClassRoom(obj))
                    {
                        var send = new Data_Klass()
                        {
                            IsCode = Code.SuccessfulInsertClassRoom
                        };

                        command = new Data_FirstCommand()
                        {
                            Command = "Command_User_Insert_ClassRoomInsert",
                            Json = JsonSerializer.Serialize<Data_Klass>(send)
                        };

                        Logger.Log($"Command_GUID ({client.IP}:{client.Port}) выдаю GUID:{client.GuidClient}");

                    }
                    else
                    {
                        command = ErrorInsertingClassRoom();

                    }
                }

                activeServer.Send(JsonSerializer.Serialize<Data_FirstCommand>(command), client.GuidClient);

            }
            catch (Exception ex)
            {
                Logger.Error($"Command_InsertClassRoom.Execut вызвал ошибку: {ex.Message}");
            }
        }

        private static Data_FirstCommand ErrorInsertingClassRoom()
        {
            Data_FirstCommand command;
            var send = new Data_Klass()
            {
                IsCode = Code.ErrorInsertClassRoom
            };

            command = new Data_FirstCommand()
            {
                Command = "Command_User_Insert_ClassRoomInsert",
                Json = JsonSerializer.Serialize<Data_Klass>(send)
            };
            return command;
        }
    }
}
