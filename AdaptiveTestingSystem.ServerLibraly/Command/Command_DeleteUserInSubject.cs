using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_DeleteUserInSubject:Commands
    {
        public override async void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<Data_Subject_UserDelete>(json);
                if (obj == null) return;

                if (await DBSearchMethods.DeleteUserInSubject(obj) == true)
                {
                    Logger.Log($"{client.IP}:{client.Port} - Удаление из таблицы PredmetSotrud успешно");
                };


            }
            catch
            (Exception ex)
            {
                Logger.Error($"Command_DeleteUserInSubject.Execut вызвал ошибку: {ex.Message}");
            }
        }
    }
}
