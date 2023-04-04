using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_DeleteClassRoom:Commands
    {
        public override async void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<Data_Klass_Delete>(json);
                if (obj == null)
                {
                    return;
                }

    

                var deleted = await DBSearchMethods.DeleteClassRoom(obj);

                if (deleted.Klasses.Count > 0)
                {
                    if (deleted.IsCode == Code.GUI_User) deleted.IsCode = Code.ErrorDeleteClassRoom_GUI_User;
                    if (deleted.IsCode == Code.Delete) deleted.IsCode = Code.ErrorDeleteClassRoom;
                    
                }
                else
                {
                    if (deleted.IsCode == Code.GUI_User) deleted.IsCode = Code.SuccessfullDeleteClassRoom_GUI_User;
                    if (deleted.IsCode == Code.Delete) deleted.IsCode = Code.SuccessfullDeleteClassRoom;
      
                }


                Data_FirstCommand data_First = new Data_FirstCommand()
                {
                    Json = JsonSerializer.Serialize(deleted),
                    Command = "Command_DeleteClassRoom"
                };

                Send(client, activeServer, data_First);
            }
            catch(Exception ex) 
            {
                Logger.Error($"Command_DeleteClassRoom.Execut вызвал ошибку: {ex.Message}");
            }
        }
    }
}
