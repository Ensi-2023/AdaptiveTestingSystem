
using System.Net;
using System.Net.Sockets;

namespace AdaptiveTestingSystem.ServerLibraly.Intererface
{
    public interface IServer
    {
        public List<ClientObject> ListClients { get; set; }
        public List<string> ListAuthorizedClientsToServer { get; set; }
        public bool IsRunning { get; set; }
        public TcpListener Listener { get; set; }
        public IPAddress IP { get; set; }
        public int Port { get; set; }
        public void Start();
        public void Send(string message, string guid);
        public void SendAll(string message);
        public void Restart();
        public void Disconnect();
        public void AddClient(ClientObject client);
        public void RemoveClient(string guid);
        public Task CloseClient();
        public Task<bool> IsCheckAuthorizedClient(string login);
        public void AddInAuthorizatedUserList(string guid, string login);
        public Task<string> IsReturnAuthorizedGUID(string login);
        public void DeleteInListAuthorizationGUID(string guid);


        public delegate void ClientConnectHandler(ClientObject client);
        public event ClientConnectHandler? ClientConnect;

        public delegate void ClientDisconnectHandler(ClientObject client);
        public event ClientDisconnectHandler? ClientDisconnect;


        public delegate void ServerDisconnectHandler();
        public event ServerDisconnectHandler? ServerDisconnect;


        public delegate void ServerRestartHandler();
        public event ServerRestartHandler? ServerRestart;


        public delegate void AuthorizedClientDisconnectedHandler(string guid, int countAuthorized);
        public event AuthorizedClientDisconnectedHandler? AuthorizedClientDisconnected;


        public delegate void AuthorizedClientConnectedHandler(string message,int countAuthorized);
        public event AuthorizedClientConnectedHandler? AuthorizedClientConnected;

    }
}
