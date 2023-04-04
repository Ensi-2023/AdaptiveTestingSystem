using System.Net;
using System.Net.Sockets;

namespace AdaptiveTestingSystem.ServerLibraly.Intererface
{
    public interface IClient
    {
        public string GuidClient { get;  set; }
        public string Name { get; set; }
        public NetworkStream Stream { get;  set; }
        public TcpClient Client { get; set; }
        public IPAddress IP { get;  set; }
        public int Port { get;  set; }
        public void Process();
        public void Close();
        public Task<string> GetMessage(NetworkStream networkStream);
        public void AddConnection(ClientObject clientObject);
        public int CountNotCorrectCommand { get; set; }
    }
}
