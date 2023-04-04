using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerApplication.Server
{
    public class APServer: IAPServer
    {

        public event IAPServer.ErrorConnectHandler? ErrorConnect;
        public event IAPServer.StartingToConnectHandler? StartingToConnect;
        public event IAPServer.ServerRunningHandler? ServerRunning;
#nullable disable
        public ServerObject Server { get; set; }
        public AppSettings Settings { get; set; }
        public Thread ListenThread { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsRunning => Server.IsRunning;

        public APServer(AppSettings setting)
        {
            this.Settings = setting;
        }

        public async void Start()
        {
            try
            {
                StartingToConnect?.Invoke();
   
                Logger.Message($"Начинаю запуск сервера: {Settings.IP}:{Settings.Port}");
                Logger.Message($"Подключаюсь к базе данных: {DBSettings.DBServer}: {DBSettings.DBase}");
                if (await ConnectDataBase.Connect())
                {
                    Server = new ServerObject(Settings.IP, Settings.Port);

                    Server.ClientConnect += Server_ClientConnect;
                    Server.ClientDisconnect += Server_ClientDisconnect;
                    Server.ServerDisconnect += Server_ServerDisconnect;
                    Server.ServerRestart += Server_ServerRestart;
                    Server.AuthorizedClientDisconnected += Server_AuthorizedClientDisconnected;
                    Server.AuthorizedClientConnected += Server_AuthorizedClientConnected;


                    ListenThread = new Thread(new ThreadStart(Server.Start));
                    ListenThread.Start();

                    Logger.Message($"Сервер успешно запущен по адресу: {Settings.IP}:{Settings.Port}");
                    ServerRunning?.Invoke();
                }
                else
                {
                    ErrorMessage = $"Ошибка подключения к базе данных: {DBSettings.DBServer}: {DBSettings.DBase}";
                    Logger.Error(ErrorMessage);
                    ErrorConnect?.Invoke(ErrorMessage);
                }                
            }
            catch (Exception ex)
            {
                ErrorMessage = $"APServer.Start вызвал ошибку: {ex.Message}";

                if (Server != null) Server.Disconnect(); 
                Logger.Error(ErrorMessage);
                ErrorConnect?.Invoke(ErrorMessage);
            }
        }

        private void Server_AuthorizedClientConnected(string message, int countAuthorized)
        {
         
        }

        private void Server_AuthorizedClientDisconnected(string guid, int countAuthorized)
        {
           
        }

        private async void Server_ServerRestart()
        {
            await Task.Delay(1000);
            Start();
        }

        private void Server_ServerDisconnect()
        {
            Main.Instance.OFFServer();


            Server.ClientConnect -= Server_ClientConnect;
            Server.ClientDisconnect -= Server_ClientDisconnect;
            Server.ServerDisconnect -= Server_ServerDisconnect;
            Server.ServerRestart -= Server_ServerRestart;
            Server.AuthorizedClientDisconnected -= Server_AuthorizedClientDisconnected;
            Server.AuthorizedClientConnected -= Server_AuthorizedClientConnected;


        }

        private void Server_ClientDisconnect(ClientObject client)
        {
            Logger.Log($"Отключение: {client.GuidClient} | {client.Name} | {client.IP}");
        }

        private void Server_ClientConnect(ClientObject client)
        {
            Logger.Log($"Подключение: {client.GuidClient} | {client.Name} | {client.IP}");
        }

        public async void BannedAccount(string login)
        {
            if (await Server.IC_Account.Banned(login, Server))
            {
                Logger.Message($"BannedAccount: Аккаунт {login} успешно забанен");
            }
        }
        public async void UnBannedAccount(string login)
        {
            if (await Server.IC_Account.UnBanned(login))
            {
                Logger.Message($"UnBannedAccount: Аккаунт {login} успешно разбанен");
            }
        }

        public void Stop()
        {
            if (Server == null) return;
            Server.Disconnect();
        }

        public void Restart()
        {
            if (Server == null) return;
            Server.Restart();
        }

       
    }
}
