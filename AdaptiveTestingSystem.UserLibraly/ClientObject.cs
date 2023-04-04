#nullable disable
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace AdaptiveTestingSystem.UserLibraly
{
    public class ClientObject : IClient
    {
        public IPAddress Address { get; set; }
        public int Port { get; set; }
        public NetworkStream Stream { get; set; }
        public TcpClient Client { get; set; }
        public bool IsConnect { get; set; }

        public event IClient.OnDisconnectToServerHander? OnDisconnectToServer;
        public event IClient.OnConnectToServerHander? OnConnectToServer;
        public event IClient.OnServerSendCommandHandler? OnServerSendCommand;

        public event IClient.OnServerSendingSizeHandler? OnServerSendingSizeCommand;
        public event IClient.OnServerSendManyMaxSizeHandler? OnServerSendManyMaxSizeCommand;

        CancellationTokenSource cancelTokenSource;
        CancellationToken token;




        CancellationTokenSource senadDataCancelTokenSource;
        CancellationToken token_2;

        public ClientObject(IPAddress address, int port)
        {
            Address = address;
            Port = port;
            cancelTokenSource = new CancellationTokenSource();
            token = cancelTokenSource.Token;

            senadDataCancelTokenSource= new CancellationTokenSource();
            token_2 = senadDataCancelTokenSource.Token;
        }

        [STAThread]
        public async void Connect()
        {
            Client = new TcpClient();
            IsConnect = false;
            try
            {
                await Client.ConnectAsync(Address, Port);
                IsConnect = true;

                Stream = Client.GetStream();
                Thread thread = new Thread(new ThreadStart(ReceiveMessage));
                thread.Start();
                SetGUID();
                OnConnectToServer?.Invoke();
          
            }
            catch (Exception ex)
            {           
                Disconnect(ex.Message);
            }
        }


        private void SetGUID()
        {
            Data_GiveGuid guid = new Data_GiveGuid()
            {
                Identity = "asdsad!sdffd///..sads343423fdASDASD545353....asdsa/??sddsdasd..,,asdzxc"
            };

            Data_FirstCommand command = new Data_FirstCommand()
            {
                Json = JsonSerializer.Serialize(guid),
                Command = "Command_GUID"
            };

            string message = JsonSerializer.Serialize<Data_FirstCommand>(command);
            SendMessage(message);
        }


        public async Task<bool> CheckConnect(string message)
        {
            try
            {
                byte[] bytes = Encoding.Unicode.GetBytes(message);
                await Stream.WriteAsync(bytes);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error($"ClientObject.CheckConnect вызвал ошибку: {ex.Message}");
                return false;
            }
        }

        public void Disconnect(string error)
        {
            if (Stream != null)
                Stream.Close();
            if (Client != null)
                Client.Close();

            IsConnect = false;
            OnDisconnectToServer?.Invoke(error);
        }

        public void Close()
        {   
            cancelTokenSource.Cancel();
            cancelTokenSource.Dispose();

            cancelTokenSource = new CancellationTokenSource();
            token = cancelTokenSource.Token;
        }

        public async void ReceiveMessage()
        {
            try
            {
                while (IsConnect)
                {
             
                    byte[] bytes = new byte[262144];
                    StringBuilder builder = new StringBuilder();
                    int bit = 0;
                    do
                    {
                        bit = await Stream.ReadAsync(bytes,token);
                        builder.Append(Encoding.Unicode.GetString(bytes, 0, bit));
                    }
                    while (Stream.DataAvailable);

                    string message = builder.ToString();
                    if (message.Trim().Length == 0)
                    {
                        if (await CheckConnect("test") == false)
                        {
                            break;
                        }
                    }
                    else
                    {
                        OnServerSendCommand?.Invoke(message);
                        //Parser message ClientScript.Parse(message, this);
                    }

                }

                Disconnect("");
            }
            catch (SocketException ex) when (ex.ErrorCode == 10004)
            {
                Disconnect(ex.Message);
                Logger.Error($"ClientObject.ReceiveMessage вызвал ошибку #{ex.ErrorCode} Разрыв соединения:{ex.Message}");
            }
            catch (Exception ex)
            {
                Disconnect(ex.Message);
                Logger.Error($"ClientObject.ReceiveMessage вызвал ошибку: {ex.Message}");
            }

        }

        public void CancelSend()
        { 
            cancelTokenSource.Cancel();
            cancelTokenSource.Dispose();

            cancelTokenSource = new CancellationTokenSource();
            token = cancelTokenSource.Token;
        }


        public void CancelSendData()
        {
            try
            {
                senadDataCancelTokenSource.Cancel();
                senadDataCancelTokenSource.Dispose();
            } catch { }
        }

        public async void SendMessage(string message)
        {
            try
            {                                            
                byte[] data = Encoding.Unicode.GetBytes(message);
                await Stream.WriteAsync(data);
            }
            catch (Exception ex)
            {
                Logger.Error($"ClientObject.SendMessage вызвал ошибку: {ex.Message}");
            }
        }


        public async void SendManyMessage(String text)
        {
            try
            {
                senadDataCancelTokenSource = new CancellationTokenSource();
                token_2 = cancelTokenSource.Token;

                var stream = Client.GetStream();
                byte[] data = Encoding.Unicode.GetBytes(text);
                await stream.WriteAsync(data, token_2);
            }
            catch (Exception ex)
            {
                Logger.Error($"ClientObject.SendManyMessage вызвал ошибку: {ex.Message}");
            }

        }
    }
}

