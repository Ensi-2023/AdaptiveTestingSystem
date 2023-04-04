using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_DeleteUserInClassRoom:Commands
    {
        public override async void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<Data_Klass_UserDelete>(json);
                if (obj == null) return;

                if (await DBSearchMethods.DeleteUserInClassRoom(obj)==true)
                {
                    Logger.Log($"{client.IP}:{client.Port} - Удаление из таблицы Klass_User успешно");
                };


            }   
            catch 
            (Exception ex)
            {
                Logger.Error($"Command_DeleteUserInClassRoom.Execut вызвал ошибку: {ex.Message}");
            }
        }
    }
}
