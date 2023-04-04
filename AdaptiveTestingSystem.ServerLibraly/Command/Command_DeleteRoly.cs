using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_DeleteRoly:Commands
    {
        public override async void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<Data_Roly>(json);

                if (obj == null) return;

                string index = await DBDataMethod.SQLCommandReturnFirstStringNoTransaction("select ID_Roly From Roly R where R.NameRoly LIKE 'пользователь'");

                string sql = $"UPDATE Position SET ID_Roly={index} where ID_Roly = {obj.Index}";
                await DBDataMethod.SQLCommandNoTransaction(sql);

                sql = $"DELETE FROM Roly WHERE ID_Roly = {obj.Index}";
                await DBDataMethod.SQLCommandNoTransaction(sql);


            }
            catch (Exception ex)
            {
                Logger.Error($"Command_DeleteRoly вызвал ошибку: {ex.Message}");
            }
        }
    }
}
