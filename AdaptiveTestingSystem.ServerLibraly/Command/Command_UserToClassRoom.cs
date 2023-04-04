using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_UserToClassRoom:Commands
    {
        public override async void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<List<Data_UserToClass>>(json);
                if (await DBAddingMethod.AddUserToClassRoom(obj))
                {
                    Logger.Log($"{client.IP}:{client.Port} - Успешное добавление пользователей в класс.");
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Command_UserToClassRoom ({client.IP}:{client.Port}) вызвал ошибку: {ex.Message}");
            }
        }
    }
}
