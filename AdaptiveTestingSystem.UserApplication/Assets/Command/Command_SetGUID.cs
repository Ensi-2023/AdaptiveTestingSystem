using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.Command
{
    public class Command_SetGUID:Commands
    {
        public override void Execut(string json, InternetClient client)
        {
            try
            {
                var guid = JsonSerializer.Deserialize<Data_GiveGuid>(json);
                if (guid != null)
                {
                    client.GUID = guid.GUID;
                    Logger.Message($"GUID: {guid.GUID} успешно присвоен");
                }
            }
            catch
            {
                throw new Exception("Ошибка присвоениея GUID");
            }
        }

    }
}
