using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_UserToSubject:Commands
    {
        public override async void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<List<Data_UserToSubject>>(json);
                if (await DBAddingMethod.AddUserToSubject(obj))
                {
                    Logger.Log($"{client.IP}:{client.Port} - Успешное добавление.");
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Command_UserToSubject ({client.IP}:{client.Port}) вызвал ошибку: {ex.Message}");
            }
        }
    }
}
