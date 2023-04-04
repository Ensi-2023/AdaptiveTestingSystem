using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerLibraly.Command.InternalCommand
{
    public class IC_Account
    {
 
        public async Task<bool> Banned(string login,ServerObject server)
        {
            bool banned = await DBSearchMethods.BannedAccount(login);

            if (banned)
            {
                string guid = await server.IsReturnAuthorizedGUID(login);
                if (guid != string.Empty)
                {
                    var ob = new Data_Disconnect()
                    {
                        GUI = guid,
                        IsCode = Code.AccountBanned
                    };

                    var comm = new Data_FirstCommand()
                    {
                        Command = "DisconnectClass",
                        Json = JsonSerializer.Serialize(ob)
                    };

                    server.DeleteInListAuthorizationGUID(guid);
                    server.Send(JsonSerializer.Serialize(comm), guid);
                }
            }

            return banned;
        }

        public async Task<bool> UnBanned(string login)
        {
            bool unbanned = await DBSearchMethods.UnBannedAccount(login);
            return unbanned;
        }

    }
}
