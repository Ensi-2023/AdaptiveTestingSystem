#nullable disable
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AdaptiveTestingSystem.ServerLibraly
{
    public class ClientObject : IClient
    {
        #region public
        public Code IsCode { get; set; } = Code.Null;
        public string GuidClient { get; set ; }
        public string Name { get; set; }
        public NetworkStream Stream { get ; set; }
        public IPAddress IP { get; set ; }
        public int Port { get; set; }
        public TcpClient Client { get; set; }
        public ServerObject ServerObject { get; set; }

        public CancellationTokenSource cancelTokenSource;
        public CancellationToken token;

        #endregion

        #region private
        private CommandParser _parser { get; set; }
        public int CountNotCorrectCommand { get; set; } = 3;

        private Queue<int> messageQuere = new Queue<int>();
        #endregion

        public void Close()
        {
            CancelThread();

            if (Stream != null)
                Stream.Close();
            if (Client != null)
                Client.Close();
        }

        public async Task<string> GetMessage(NetworkStream networkStream)
        {
            byte[] data = new byte[262144];
            StringBuilder sb = new StringBuilder();
            do
            {
               // await Task.Delay(0);
                messageQuere.Enqueue(await networkStream.ReadAsync(data));
                if (messageQuere.Count > 0)
                {
                    sb.Append(Encoding.Unicode.GetString(data, 0, messageQuere.Dequeue()));
                }

            }
            while (networkStream.DataAvailable);
            return sb.ToString();
            
        }


        public void CancelThread()
        {
            cancelTokenSource.Cancel();
            cancelTokenSource.Dispose();

            cancelTokenSource = new CancellationTokenSource();
            token = cancelTokenSource.Token;
        }
        public async void Process()
        {
            this.Stream = Client.GetStream();
            try
            {
                var _parser = new CommandParser();
                cancelTokenSource= new CancellationTokenSource();
                token = cancelTokenSource.Token;

                while (ServerObject.IsRunning)
                {
                    var message = await GetMessage(Stream);
                    _parser.Parse(message,this, ServerObject);
                    await Task.Delay(10);
                }
            }
            catch (Exception ex)
            {
                 Logger.Error($"ClientObject ({IP}:{Port}) вызвал ошибку: {ex.Message}");
                 ServerObject.RemoveClient(this.GuidClient);
            }
            finally 
            {
                Close();
            }
        }

        public ClientObject(TcpClient tcpClient, ServerObject server, CommandParser parser)
        { 
          this.Client = tcpClient;
          this.ServerObject = server;
        //  this._parser = parser;
          this.Client.ReceiveTimeout = 3000;
        
          this.Client.NoDelay = true;
          this.GuidClient = Guid.NewGuid().ToString();
          this.IP = (((System.Net.IPEndPoint)tcpClient.Client.RemoteEndPoint).Address);
          this.Port = (((System.Net.IPEndPoint)tcpClient.Client.RemoteEndPoint).Port);
          this.AddConnection(this);
        }


        /// <summary>
        /// Создаём подключение и передаём его серверу
        /// </summary>
        /// <param name="clientObject"></param>
        public void AddConnection(ClientObject clientObject)
        {
            ServerObject.AddClient(clientObject);
        }

    }
}
