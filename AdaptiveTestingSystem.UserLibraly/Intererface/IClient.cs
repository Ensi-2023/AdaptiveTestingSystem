using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace AdaptiveTestingSystem.UserLibraly.Intererface
{
    public interface IClient
    {
        public IPAddress Address { get; set; }
        public int Port { get; set; }
        public NetworkStream Stream { get; set; }
        public TcpClient Client { get; set; }
        public bool IsConnect { get; set; }
        public void Connect();
        public void Disconnect(string error);
        public void ReceiveMessage();
        public void SendMessage(string message);
        public Task<bool> CheckConnect(string message);


        public delegate void OnDisconnectToServerHander(string error);
        public event OnDisconnectToServerHander? OnDisconnectToServer;

        public delegate void OnConnectToServerHander();
        public event OnConnectToServerHander? OnConnectToServer;


        delegate void OnServerSendCommandHandler(string command);
        event OnServerSendCommandHandler? OnServerSendCommand;


        delegate void OnServerSendManyMaxSizeHandler(string command);
        event OnServerSendManyMaxSizeHandler? OnServerSendManyMaxSizeCommand;

        delegate void OnServerSendingSizeHandler(string command);
        event OnServerSendingSizeHandler? OnServerSendingSizeCommand;

    }
}
