using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_GiveAllUserinActiveTestServerThisIndex:Commands
    {
        public override async void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<Data_GiveAllUserInTestServer>(json);
                if (obj != null)
                {
                    var server = activeServer.ServerBrowser.GetServerByIndex(obj.IndexServer);
                    if (server == null) return;
                    List < Data_MultyServerClient > list = new List<Data_MultyServerClient>();
                    foreach (var cl in server.Client)
                    {
                        if (cl.GuidClient == server.CreatorGuid) continue;


                        var loginUser = await activeServer.IsReturnAuthorizedLogin(cl.GuidClient);

                        string sql = $" select (UT.SurnameUser+ ' ' + ut.LastnameUser+ ' '+ut.MiddlenameUser) as fio from UserTable UT " +
                                    $" where ID_User = (Select ID_User from LoginUserTable where Login_User = '{loginUser}')";

                       string nameClient = await DBDataMethod.SQLCommandReturnFirstStringNoTransaction(sql);

                        list.Add(new Data_MultyServerClient() 
                        {
                            GUID= cl.GuidClient,
                            IsCode = cl.IsCode,
                            NameClient = nameClient
                        });

                    }

                    var comman = new Data_FirstCommand()
                    {
                        Command = "Command_ClientManagerForActiveTestingServer",
                        Json = JsonSerializer.Serialize(list)
                    };

                    Send(client, activeServer, comman);

                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Command_GiveAllUserinActiveTestServerThisIndex ({client.IP}:{client.Port}) вызвал ошибку: {ex.Message}");
            }
        }
    }
}
