using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace AdaptiveTestingSystem.UserApplication.Client
{
    public class InternetClient
    {

#nullable disable

        private ClientObject clientObject { get; set; }
        public IPAddress Address { get; set; }
        public int Port { get; set; }
        public int ReconnectCount = 0;
        public int TimeToReconnect = 15;
        public string GUID { get; set; }

        public void CancelSend() => clientObject.CancelSend();
        public void CancelSendData() => clientObject.CancelSendData();

        public InternetClient(IPAddress address, int port,int reconnect)
        {

            if (clientObject != null)
            {
                clientObject.OnConnectToServer -= ClientObject_OnConnectToServer;
                clientObject.OnDisconnectToServer -= ClientObject_OnDisconnectToServer;
                clientObject.OnServerSendCommand -= ClientObject_OnServerSendCommand;
                clientObject=null;
            }
            
            
            this.Address = address;
            this.Port = port;
            this.ReconnectCount = reconnect;

            Logger.Warning($"Создаю подключение к адресу: {address}:{port}");
            clientObject = new ClientObject(address, port);
            clientObject.OnConnectToServer += ClientObject_OnConnectToServer;
            clientObject.OnDisconnectToServer += ClientObject_OnDisconnectToServer;
            clientObject.OnServerSendCommand += ClientObject_OnServerSendCommand;
        }

        [STAThread]
        public void Start()
        {
            try
            {               
                var thread = new Thread(new ThreadStart(clientObject.Connect));
                thread.Start();
            }
            catch (Exception ex)
            {        
                clientObject.Disconnect($"{ex.Message}");
            }
        }

        public void Send(string message)
        {
            Application.Current.Dispatcher.Invoke(async () => 
            {
                await Task.Factory.StartNew(() => clientObject.SendMessage(message));
            });
        }


        public void SendMany(string message)
        {
            Application.Current.Dispatcher.Invoke(async () =>
            {
                await Task.Factory.StartNew(() => clientObject.SendManyMessage(message));
            });
        }


        public void CloseConnect()
        {
            clientObject.Close();
        }

        public void Disconnect()
        {
            clientObject.Disconnect("Сервер завершает работу");


            clientObject.OnConnectToServer -= ClientObject_OnConnectToServer;
            clientObject.OnDisconnectToServer -= ClientObject_OnDisconnectToServer;
            clientObject.OnServerSendCommand -= ClientObject_OnServerSendCommand;
        }

        private async void ClientObject_OnServerSendCommand(string command)
        {
            //Logger.Debug($"Новая команда для обработки: {command}");
            await Task.Factory.StartNew(() => CommandsManadgment.Parse(command, this));     
        }

        private void ClientObject_OnDisconnectToServer(string error)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
               

                Logger.Error($"Сервер завершил работу: {error}");
                _Main.Instance.OverlayShow(false);
                _Main.Instance.CloseAllWindow();
                ReconnectCount++;

                if (error == "The operation was canceled.")
                {
                    return;
                }

                _Main.Instance.SetSingeltonChilden(new Assets.GUI.GUI_ErrorConnectToServer(error));
        
            });
        }

        private void ClientObject_OnConnectToServer()
        {
            Logger.Message($"Клиент подключен!");
            ReconnectCount = 0;


            Application.Current.Dispatcher.Invoke(() => 
            {
                _Main.Instance.SetSingeltonChilden(new GUI_Authorization());
            });
        }

     
    }
}
