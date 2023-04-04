
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using AdaptiveTestingSystem.ServerLibraly.Command.InternalCommand;
using AdaptiveTestingSystem.ServerLibraly.CScript;


namespace AdaptiveTestingSystem.ServerLibraly
{
    public class ServerObject : IServer
    {
        #region eventHandler
            #nullable enable
            public event IServer.ClientConnectHandler? ClientConnect;
            public event IServer.ClientDisconnectHandler? ClientDisconnect;
            public event IServer.ServerDisconnectHandler? ServerDisconnect;
            public event IServer.ServerRestartHandler? ServerRestart;
            public event IServer.AuthorizedClientDisconnectedHandler? AuthorizedClientDisconnected;
            public event IServer.AuthorizedClientConnectedHandler? AuthorizedClientConnected;
        #endregion
#nullable disable
        public bool IsRunning { get; set; }
        public TcpListener Listener { get; set; }
        public IPAddress IP { get; set; }
        public int Port { get; set; }
        public List<ClientObject> ListClients { get; set; }
        public List<string> ListAuthorizedClientsToServer { get; set; }
        
        public List<ISocket> ListSockets { get; set; }

        public List<ActiveSocket> Tasks { get; set; }

        public IC_Account IC_Account { get; set; }
        public IC_Socket IC_Socket { get; set; }
        public ServerBrowser ServerBrowser { get; set; }
                     
        private CommandParser _parser;

        /// <summary>
        /// Создание нового сервера
        /// </summary>
        /// <param name="ip">Удаленный адресс сервера</param>
        /// <param name="port">Порт сервера</param>
        public ServerObject(IPAddress ip, int port)
        {
            this.IP= ip;
            this.Port = port;
        }


        #region DefaultVoid
        public async void Start()
        {
            try
            {                
                Logger.Log($"Запускаю прослушивание потока по адресу: ({IP}:{Port})");
                this.Listener = new TcpListener(IP, Port);
                this.Listener.Start();
                this.IsRunning = true;

                _parser = new CommandParser();
                ListClients = new List<ClientObject>();
                ListSockets = new List<ISocket>();
                Tasks = new();
                ListAuthorizedClientsToServer = new List<string>();
                IC_Account = new();
                ServerBrowser = new ServerBrowser(this);
                IC_Socket = new (this);
                
                while (IsRunning)
                {
                    TcpClient tcp = await Listener.AcceptTcpClientAsync();
                    ClientObject clientObject = new(tcp, this, _parser);
                    Thread thread = new(new ThreadStart(clientObject.Process));
                    thread.Start();                 
                }
            }
            catch (SocketException socketError) when (socketError.ErrorCode == 10004)
            {
                if (socketError.ErrorCode == 10004)
                {
                    Logger.Error($"ServerObject.Start вызвал Ошибку#{socketError.ErrorCode}: Принудительное закрытие потока прослушивания. \n[{DateTime.Now}] - Сервер завершает работу");
                }
                else
                {
                    Logger.Error($"ServerObject.Start вызвал ошибку#{socketError.ErrorCode}: {socketError.Message}. \n[{DateTime.Now}] - Сервер завершает работу");
                }

                Disconnect();
            }

            catch (Exception ex)
            { 
                Logger.Error($"ServerObject.Start вызвал ошибку: {ex.Message}");
                Disconnect();
            }
        }
        public async void Send(string message, string guid)
        {
            try
            {
                byte[] data = Encoding.Unicode.GetBytes(message);
                var client = ListClients.FirstOrDefault(obj => obj.GuidClient.ToLower().Trim() == guid.ToLower().Trim());
                if (client != null)
                {
                    await client.Stream.WriteAsync(data);
                }
            }
            catch (Exception ex)
            { 
                Logger.Error($"ServerObject.Send выдал критическую ошибка: {ex.Message}");
            }
        }
        public async void SendAll(string message)
        {
            try
            {
                byte[] data = Encoding.Unicode.GetBytes(message);

                for (int i = 0; i < ListClients.Count; i++)
                {
                    await ListClients[i].Stream.WriteAsync(data);
                }
            }
            catch (Exception ex)
            {
               Logger.Error($"ServerObject.SendAll выдал критическую ошибка: {ex.Message}");
            }
        }
        public async void Disconnect()
        {
            if (IsRunning == false) return;
            Logger.Warning($"Начинаю отключение сервера: ({IP}:{Port})");

            ServerBrowser.Clear();

            
            Logger.Warning($"Закрываю сокеты");
            await CloseAllSocket();
            Tasks.Clear();
            ListSockets.Clear();
            Logger.Warning($"Отключаю БД");
            await ConnectDataBase.Disconnect();
            await Task.Delay(250);
            Logger.Warning($"Отключаю клиентов");
            await CloseClient();
            await Task.Delay(250);
            Logger.Warning($"Отключаю сервер");
            ListAuthorizedClientsToServer.Clear();
            await Task.Delay(250);
            IsRunning = false;
            ListClients.Clear();
            await Task.Delay(250);
            Listener.Stop();
            await Task.Delay(250);
            ServerDisconnect?.Invoke();
        }

        private async Task CloseAllSocket()
        {
            await Task.Run(() =>
            {
                for (int i = 0; i < Tasks.Count; i++)
                {
                    Tasks[i].Stop();
                }
            });
        }

        public async void Restart()
        {
            if (IsRunning == false) return;

      
            Logger.Warning($"Начинаю перезагрузку сервера: ({IP}:{Port})");
            ServerBrowser.Clear();

            ServerDisconnect?.Invoke();

            Logger.Warning($"Закрываю сокеты");
            await CloseAllSocket();
            Tasks.Clear();
            ListSockets.Clear();
            Logger.Warning($"Отключаю БД");
            await ConnectDataBase.Disconnect();
            await Task.Delay(250);
            Logger.Warning($"Отключаю клиентов");
            await CloseClient();
            await Task.Delay(250);
            Logger.Warning($"Отключаю сервер");
            ListAuthorizedClientsToServer.Clear();
            await Task.Delay(250);
            IsRunning = false;
            ListClients.Clear();
            await Task.Delay(250);
            Listener.Stop();
            await Task.Delay(250);


            Logger.Warning($"Перезагрузка...");
            ServerRestart?.Invoke();

        }
        public async Task CloseClient()
        {
            await Task.Run(() =>
            {
                for (int i = 0; i < ListClients.Count; i++)
                {
                    ListClients[i].Close();
                }
            });
        }
        public void AddClient(ClientObject client)
        {
            var check = ListClients.Find(x => x == client);
            if (check == null)
            {
                ListClients.Add(client);
                ClientConnect?.Invoke(client);
            }
        }
        public void RemoveClient(string guid)
        {
            var client = ListClients.FirstOrDefault(obj => obj.GuidClient.ToLower().Trim() == guid.ToLower().Trim());
            if (client != null)
            {
                ListAuthorizedClientsToServer.Remove(ListAuthorizedClientsToServer.FirstOrDefault(obj=>obj.Contains(guid)));
                ListClients.Remove(client);
                ClientDisconnect?.Invoke(client);
                ServerBrowser.DeleteServer(guid, true);
            }
        }
        public async Task<bool> IsCheckAuthorizedClient(string login)
        {
            return await Task<bool>.FromResult(ListAuthorizedClientsToServer.FirstOrDefault(client => client.Contains(login.Trim())) != null);
        }
        public void AddInAuthorizatedUserList(string guid, string login)
        {
            ListAuthorizedClientsToServer.Add($"{guid}@{login}");
            AuthorizedClientConnected?.Invoke($"{guid}@{login}", ListAuthorizedClientsToServer.Count);
        }


        public async Task<string> IsReturnAuthorizedGUID(string login)
        {
            var guid = await Task.FromResult(ListAuthorizedClientsToServer.FirstOrDefault(client => client.Contains(login.Trim())));
            if (guid != null)
            {
                var obj = guid.Split('@');
                if (obj != null) return obj[0];
            }
             return string.Empty;
        }

        public async Task<string> IsReturnAuthorizedLogin(string guid)
        {
            var login = await Task.FromResult(ListAuthorizedClientsToServer.FirstOrDefault(client => client.Contains(guid.Trim())));
            if (login != null)
            {
                var obj = login.Split('@');
                if (obj != null) return obj[1];
            }
            return string.Empty;
        }


        public void DeleteInListAuthorizationGUID(string guid)
        {
            ListAuthorizedClientsToServer.Remove(ListAuthorizedClientsToServer.FirstOrDefault(obj => obj.Contains(guid)));
            AuthorizedClientDisconnected?.Invoke(guid, ListAuthorizedClientsToServer.Count);

            RemoveClient(guid);
        }




        public void ViewSocket()
        {
            for (int i = 0; i < ListSockets.Count; i++)
            {
                var item = ListSockets[i];

                Logger.Log($"Socket: {item.IPAddressAddress}:{item.Port} Connect Count: {item.CountConnect}");

            }
        }


        public void DeleteSocket(int port)
        {
            ///Если есть подключения - отключить
            ///
            var search = ListSockets.Find(item => item.Port == port);
            var searchActive = Tasks.Find(item => item.Port == port);
            ListSockets.Remove(search);
            searchActive.Stop();
        }

        public void DeleteSocket(ActiveSocket activeSocket)
        {
            ///Если есть подключения - отключить
            ///
            Tasks.Remove(activeSocket);
        }


        public void DeleteAllSocket()
        {
            ListSockets.Clear();
        }

        /// <summary>
        /// Создает новый сокет для подключения либо возвращает один из доступных сокетов. Не будет создавать новый сокет, если в каком либо
        /// если доступное место для подключения
        /// </summary>
        /// <param name="_port"> не обязательный параметр, по умолчанию 6754</param>
        /// <returns>Возвращает доступные сокет.</returns>
        public async Task<SocketObject> SetSocket(int _port = 6754)
        {
            SocketObject socketsocket = new SocketObject();
            try
            {

                if (_port == Port) _port++;

                int port = _port;
                IPAddress address = IP;

                if (ListSockets.Count > 0)
                {

                    //for (int i = 0; i < ListSockets.Count; i++)
                    //{

                    //    var item = ListSockets[i];

                    //    if (item.CountConnect < item.MaxConnect)
                    //    {
                    //        socketsocket.Port = item.Port;
                    //        socketsocket.TimeLife = 200;
                    //        socketsocket.CountConnect = item.CountConnect;
                    //        socketsocket.MaxConnect = 10;
                    //        socketsocket.IPAddressAddress = item.IPAddressAddress;

                    //        return socketsocket;
                    //    }

                    //    await Task.Delay(10);
                    //}


                    while (true)
                    {

                        var search = ListSockets.Find(item => item.Port == port);
                        if (search == null)
                        {
                            socketsocket = new SocketObject();
                            socketsocket.Port = port;
                            socketsocket.TimeLife = 900;
                            socketsocket.CountConnect = 0;
                            socketsocket.MaxConnect = 10;
                            socketsocket.IPAddressAddress = address;
                            ListSockets.Add(socketsocket);
                            return socketsocket;
                        }
                        port++;
                        await Task.Delay(10);
                    }


                }
                else
                {
                    socketsocket.Port = port;
                    socketsocket.TimeLife = 900;
                    socketsocket.CountConnect = 0;
                    socketsocket.MaxConnect = 10;
                    socketsocket.IPAddressAddress = address;

                    ListSockets.Add(socketsocket);
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"ServerObject.SetSocket выдал критическую ошибка: {ex.Message}");
            }

            return socketsocket;

        }


        public async Task<SocketObject> GetSocket()
        {
            SocketObject socketsocket = new SocketObject();

            if (ListSockets.Count > 0)
            {

                for (int i = 0; i < ListSockets.Count; i++)
                {

                    var item = ListSockets[i];

                    if (item.CountConnect < item.MaxConnect)
                    {
                        socketsocket.Port = item.Port;
                        socketsocket.TimeLife = 200;
                        socketsocket.CountConnect = item.CountConnect;
                        socketsocket.MaxConnect = 10;
                        socketsocket.IPAddressAddress = item.IPAddressAddress;

                        return socketsocket;
                    }

                    await Task.Delay(10);
                }
            }
            else
            {
                socketsocket.Port = 6754;
                socketsocket.TimeLife = 200;
                socketsocket.CountConnect = 0;
                socketsocket.MaxConnect = 10;
                socketsocket.IPAddressAddress = IP;

                ListSockets.Add(socketsocket);
            }

            return socketsocket;
        }

        #endregion

        #region ModifyVoid

        #endregion
    }
}
