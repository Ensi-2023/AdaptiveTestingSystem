using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows.Threading;

namespace AdaptiveTestingSystem.ServerLibraly.CScript
{

    public record class BServer
    {
        public List<ClientObject> Client = new List<ClientObject>();
        public string IndexServer { get; set; }
        public int IndexTest { get; set; }
        public string NameTest { get; set; }
        public int IndexCreator { get; set; }
        public bool IsAdaptive { get; set; }
        public Code IsCode { get; set; } = Code.WaitClient;
        public int CountCheck { get; set; } = 0;
        public string Password { get; set; } = string.Empty;
        public string CreatorGuid { get; set; } = string.Empty;
        public bool IsConnecting { get; internal set; } = false;

        public int CountQuesting { get; set; } = 0;
    }

    public class ServerBrowser
    {
        private int _timerTime;
        private DispatcherTimer _timeUpdate;
        private ObservableCollection<BServer> bServers { get; set; }
        private ServerObject activeServer { get; set; }

        public ServerBrowser(ServerObject server) 
        {
            activeServer = server;
            bServers = new ObservableCollection<BServer>();
            SetupTimer();
        }

        public void Send(string gui, Data_FirstCommand command)
        {
            activeServer.Send(JsonSerializer.Serialize(command), gui);
        }
        public void Clear()
        {
            _timeUpdate.Stop();
            bServers.Clear();
        }

        private void SetupTimer()
        {
            if (_timeUpdate != null) _timeUpdate.Stop();

            _timerTime = 20;
            _timeUpdate = new DispatcherTimer();
            _timeUpdate.Tick += new EventHandler(timeReconnectr_Tick);
            _timeUpdate.Interval = TimeSpan.FromSeconds(1);
            _timeUpdate.Start();
        }

        private async void timeReconnectr_Tick(object sender, EventArgs e)
        {

            if (_timerTime <= 0)
            {
                _timerTime = 20;
                await CheckServerStatus();
            }

            _timerTime--;

        }

        private async Task CheckServerStatus()
        {
            if(bServers.Count==0) return;

            for (int i = 0; i < bServers.Count; i++)
            {
                if (bServers.Count == 0) return;

                var bServer = bServers[i];

                if (bServer.IsCode == Code.WaitClient)
                {
                    bServer.CountCheck++;
                    if (bServer.CountCheck > 100)
                    {
                        bServers.Remove(bServer);
                        continue;
                    }
                }

                if (bServer.IsCode == Code.StartingTest) continue;

                if (bServer.IsCode == Code.StopServer)
                {
                    bServers.Remove(bServer);
                    continue;
                }                
                
                await Task.Delay(30);
            }
        }

        public void StatusServer(Code code, string indexServer)
        {
            var item = bServers.FirstOrDefault(o => o.IndexServer.Equals(indexServer));
            if (item != null)
            {
                item.IsCode = code;
            }
        }

        public void AllowConnectToServer(string indexServer)
        {
            var item = bServers.FirstOrDefault(o => o.IndexServer.Equals(indexServer));
            if (item != null)
            {
                item.IsConnecting = true;   
            }
        }

        public void SendDataInClient(ClientObject client, string indexServer,(int,int) data)
        {
            var item = bServers.FirstOrDefault(o => o.IndexServer.Equals(indexServer));
            if (item != null)
            {
                var clientData = new Data_MultyServerClient()
                {
                    GUID = client.GuidClient,
                    IsCode = Code.TestingRun,
                    IndexServer = item.IndexServer,
                    IndexAnswer = data.Item2,
                    IndexQuest =data.Item1
                };


                var dataPacket = new Data_FirstCommand()
                {
                    Command = "Command_SetAnswerClientForActiveTestServer",
                    Json = JsonSerializer.Serialize(clientData)
                };

                Send(item.CreatorGuid, dataPacket);
            }
        }

        public async void SendAllStart(string indexServer)
        {
            var item = bServers.FirstOrDefault(o => o.IndexServer.Equals(indexServer));
            if (item != null)
            { 
                item.IsCode = Code.StartingTest;

                foreach (var client in item.Client)
                {
                    if (client.GuidClient == item.CreatorGuid) continue;

                    var clientData = new Data_MultyServerClient()
                    {
                        GUID = client.GuidClient,
                        IsCode = Code.StartingTest,
                        IndexServer = item.IndexServer,IsAdaptive = item.IsAdaptive,
                    };

                    CreateClientData(client, clientData);


                    await Task.Delay(30);
                }
            }
        }

        public void StatusClient(ClientObject client, string indexServer, Code isCode)
        {
            var item = bServers.FirstOrDefault(o => o.IndexServer.Equals(indexServer));
            if (item != null)
            {
                var search = item.Client.Find (o=> o == client);
                if (search != null) 
                {
                    search.IsCode = isCode;


                    var clientData = new Data_MultyServerClient()
                    {
                        GUID = client.GuidClient,
                        IsCode = isCode,
                        IndexServer = item.IndexServer
                    };


                    var data = new Data_FirstCommand()
                    {
                        Command = "Command_SetStatusClientForConnectTestServer",
                        Json = JsonSerializer.Serialize(clientData)
                    };

                    Send(item.CreatorGuid, data);

                }
            }
        }

        public async void AddClient(ClientObject client, Data_ConnectTestingServer indexServer)
        {
            var item = bServers.FirstOrDefault(o => o.IndexServer.Equals(indexServer.IndexServer));
            if (item != null)
            {
                if (item.IsCode == Code.StartingTest)
                {
                    var clientData = new Data_MultyServerClient()
                    {
                        GUID = client.GuidClient,
                        IsCode = Code.ErrorCnnectToServer_ServerStart,
                    };

                    CreateClientData(client, clientData);

                    return;
                }
                else
                {
                    if (client.GuidClient == item.CreatorGuid)
                    {
                        await ConnectToAdmin(client, item);
                        client.IsCode = Code.ConnectedToServer;
                        var searchAdmin = item.Client.FirstOrDefault(o => o.GuidClient == client.GuidClient);
                        if (searchAdmin != null) return;
                        item.Client.Add(client);
                        return;

                    }


                    if (item.IsConnecting == false)
                    {
                        var clientData = new Data_MultyServerClient()
                        {
                            GUID = client.GuidClient,
                            IsCode = Code.ErrorCnnectToServer_ServerNotStart,
                        };

                        CreateClientData(client, clientData);

                        return;
                    }

                    if (item.Password.Trim() != string.Empty)
                    {
                        if (Encryption.verifyMd5Hash(item.Password,indexServer.Hash))
                        {
                            AddNewClient(client, item);
                        }
                        else
                        {
                            var clientData = new Data_MultyServerClient()
                            {
                                GUID = client.GuidClient,
                                IsCode = Code.ErrorCnnectToServer_NotCorrectPassword,
                            };

                            CreateClientData(client, clientData);
                        }
                    }
                    else
                    {
                        AddNewClient(client, item);
                    }
                }
            }
            else
            {
                var clientData = new Data_MultyServerClient()
                {
                    GUID = client.GuidClient,
                    IsCode = Code.ErrorCnnectToServer_NotCorrectServerIndex,
                };

                CreateClientData(client, clientData);
            }
        }

        private async Task ConnectToAdmin(ClientObject client, BServer? item)
        {
            var nameSotrudAndNamePredmet = await DBSearchMethods.GetNameSotrudAndNamePredmet(item.IndexCreator, item.IndexTest);
        
            var packet = new Data_ListPacketServer()
            {
                IsCode = Code.NoThreadStart
            };

            var listServer = new List<Data_ListMultyServer>()
                    {
                        new Data_ListMultyServer()
                        {
                           IndexCreator = item.IndexCreator,
                           NameTest = item.NameTest,
                           Password = item.Password,
                           IsAdaptive = item.IsAdaptive,
                           CountUser = item.Client.Count,
                           IndexServer = item.IndexServer,
                           IndexTest= item.IndexTest,
                           NameCreator=nameSotrudAndNamePredmet.Item1,
                           NamePredmet = nameSotrudAndNamePredmet.Item2,
                           IsCode = Code.CreateServer,
                           CountQuestForTest = item.CountQuesting
                        }
                    };

            packet.MultyServer = listServer;
        
            CreateClientData(client, packet, "Command_ConnectAdminTestServer");
        }

        private async void AddNewClient(ClientObject client, BServer? item)
        {
            client.IsCode = Code.ConnectedToServer;
            item.Client.Add(client);

            var clientData = new Data_MultyServerClient()
            {
                GUID = client.GuidClient,
                IsCode = Code.ConnectedToServer,
                IndexServer = item.IndexServer,
                IndexTest = item.IndexTest,
                NameTest = item.NameTest,
                IsAdaptive = item.IsAdaptive,
                CountQuest = item.CountQuesting
            };

            CreateClientData(client, clientData);

            clientData.IsCode = Code.NewClientConnect;
            var loginUser = await activeServer.IsReturnAuthorizedLogin(client.GuidClient);

            string sql = $" select (UT.SurnameUser+ ' ' + ut.LastnameUser+ ' '+ut.MiddlenameUser) as fio from UserTable UT " +
                        $" where ID_User = (Select ID_User from LoginUserTable where Login_User = '{loginUser}')";

            clientData.NameClient = await DBDataMethod.SQLCommandReturnFirstStringNoTransaction(sql);
            CreateClientData(client, clientData, item.CreatorGuid);

        }

        private void CreateClientData(ClientObject client, Data_MultyServerClient clientData,string admin = "")
        {
  
            var data = new Data_FirstCommand()
            {
                Command = "Command_ConnectToTestingServer",
                Json = JsonSerializer.Serialize(clientData)
            };

            if (admin.Trim() == string.Empty) 
                Send(client.GuidClient, data);
            else
                Send(admin, data);
        }

        private void CreateClientData(ClientObject client, Data_ListPacketServer clientData,string command = "Command_SetAllMultyTest")
        {

            var data = new Data_FirstCommand()
            {
                Command =command,
                Json = JsonSerializer.Serialize(clientData)
            };

            Send(client.GuidClient, data);
        }


        public void RemoveClient(ClientObject client, string indexServer) 
        {
            var item = bServers.FirstOrDefault(o => o.IndexServer.Equals(indexServer));
            if (item != null)
            {
                client.IsCode = Code.Null;

                var clientData = new Data_MultyServerClient()
                {
                    GUID = client.GuidClient,
                    IsCode = Code.RemoveConnect,
                };

                CreateClientData(client, clientData, item.CreatorGuid);
                item.Client.Remove(client);
            }
        }
        public ObservableCollection<BServer> GetAllServer()
        {
            return bServers;
        }


        public BServer? GetServerByIndex(string index)
        {
           var item = bServers.FirstOrDefault(o => o.IndexServer.Equals(index));
           return item;
        }

        public async void DeleteServer(string indexServer)
        {
            var item = bServers.FirstOrDefault(o => o.IndexServer.Equals(indexServer));
            if (item != null)
            {
                await CloseServer(item.IndexServer);
                bServers.Remove(item);
            }
        }

        public void DeleteServer(string guidClient, bool userRemove = true)
        {
            var item = bServers.FirstOrDefault(o => o.CreatorGuid.Equals(guidClient));
            if (item != null)
            {
             
                bServers.Remove(item);
            }
        }


        public async Task<string> AddServer(int indexTest, int indexCreator, string nameTest,string creatorGUID,string password = "", bool isAdaptive = false, int countQuestForTesting = 0)
        {
            string indexServer = "";
            if (bServers.Count > 0)
            {
                indexServer = await GetFreeIndex();
            }
            else
                indexServer = CreateIndex();


            Logger.Debug($"Server create: {indexServer}"); 

            var server = new BServer()
            {
                Client = new List<ClientObject>() { },
                IndexTest = indexTest,
                IndexCreator = indexCreator,
                NameTest = nameTest,
                IndexServer = indexServer,
                IsAdaptive = isAdaptive,
                Password = password,
                CreatorGuid = creatorGUID,
                CountQuesting = countQuestForTesting

            };

            bServers.Add(server);

            return indexServer;
        }

        private async Task<string> GetFreeIndex()
        {     
       
            while(true)
            {
                string str = CreateIndex();
                var item = bServers.FirstOrDefault(o => o.IndexServer.Equals(str));
                if (item == null)
                {
                    return str;
                }
                await Task.Delay(20);
            }
        }

        private string CreateIndex()
        {
            Random random = new Random();
            return $"{random.Next(0, 9)}{random.Next(0, 9)}{random.Next(0, 9)}{random.Next(0, 9)}{random.Next(0, 9)}{random.Next(0, 9)}{random.Next(0, 9)}{random.Next(0, 9)}";
        }

        public async void StartTesting(string indexServer)
        {
            var item = bServers.FirstOrDefault(o => o.IndexServer.Equals(indexServer));
            if (item != null)
            {
                item.IsCode = Code.StartingTest;
                AdminStartServer(item.CreatorGuid);
                await Task.Delay(30);
                Data_StartTesting data = new Data_StartTesting()
                {
                    IsCode = Code.StartTestingUser,
                    IsAdaptive = item.IsAdaptive
                };

                var packet = new Data_FirstCommand()
                {
                    Command = "Command_StartTesting",
                    Json = JsonSerializer.Serialize(data)
                };

                foreach (var client in item.Client)
                {
                    Send(client.GuidClient, packet);
                    await Task.Delay(30);
                }


               
            }
        }

        private void AdminStartServer(string creatorGuid)
        {
            Data_StartTesting data = new Data_StartTesting()
            {
                IsCode = Code.StartTestingForAdmin
            };

            var packet = new Data_FirstCommand()
            {
                Command = "Command_StartTesting",
                Json = JsonSerializer.Serialize(data)
            };

            Send(creatorGuid, packet);

        }

        public async Task CloseServer(string indexServer)
        {
            var item = bServers.FirstOrDefault(o => o.IndexServer.Equals(indexServer));
            if (item != null)
            {
                Data_StartTesting data = new Data_StartTesting()
                {
                    IsCode = Code.RemoveConnectedClient
                };


                var packet = new Data_FirstCommand()
                {
                    Command = "Command_ConnectToTestingServer",
                    Json = JsonSerializer.Serialize(data)
                };

                foreach (var client in item.Client.ToList())
                {
                    Send(client.GuidClient, packet);
                    await Task.Delay(30);
                }

                await Task.Delay(100);
                bServers.Remove(item);
            }
        }
    }
}
