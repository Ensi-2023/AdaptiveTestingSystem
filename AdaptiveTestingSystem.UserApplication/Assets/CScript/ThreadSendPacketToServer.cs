using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.CScript
{
    public class ThreadSendPacketToServer
    {
        public delegate void ErrorUploadHandler(string error);
        public event ErrorUploadHandler? ErrorUpload;

        public delegate void FinishUploadHandler();
        public event FinishUploadHandler? FinishUpload;

        public delegate void StatusUploadHandler((double, double) sendmax);
        public event StatusUploadHandler? StatusUpload;

        public delegate void StopUploadHandler();
        public event StopUploadHandler? StopUploadPacket;





        private CancellationTokenSource cancelTokenSource;
        private CancellationToken canceltoken;

        private CancellationTokenSource acceptTokenSource;
        private CancellationToken accepttoken;



        private double _maxSize = 0;
        private double _sum = 0;
        private string _identityCommand { get; set; }
        private string _command { get; set; }

        private double CountingSizePacket(byte[] datasize)
        {
            return (double)(datasize.Length) / (1024 * 2);
        }
        public ThreadSendPacketToServer(string identityCommand)
        {          
            _maxSize = 0;
            _sum = 0;
            _identityCommand = identityCommand;
        }

        public async void Dispose()
        {
            CloseThread();

            var sendPacket = new Data_Base()
            {
                IsCode = Code.ThreadStop,
            };

            SendData(sendPacket);
            await Task.Delay(500);
            _command = string.Empty;

        }
        ~ThreadSendPacketToServer()
        {
            // Dispose();
        }

        public void CancelWait()
        {
            try
            {
                acceptTokenSource.Cancel();
                acceptTokenSource.Dispose();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }

        public async void StartSend(object packet)
        {
            SetCancelToken();
            SetAcceptPacket();
            await CreateAndSendPacket(packet);
            CloseThread();
        }
        private void CloseThread(bool isUser = false)
        {

            if (isUser) StopUploadPacket?.Invoke();

            try
            {
                if (cancelTokenSource != null)
                {
                    cancelTokenSource.Cancel();
                    cancelTokenSource.Dispose();
                }

            }
            catch (Exception ex)
            {
                Logger.Error($"ThreadSendPacket - {ex.Message}");
            }

            try
            {
                if (acceptTokenSource != null)
                {
                    acceptTokenSource.Cancel();
                    acceptTokenSource.Dispose();
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"ThreadSendPacket - {ex.Message}");
            }
        }

        private async Task CreateAndSendPacket(object packet)
        {
            string json = JsonSerializer.Serialize(packet);
            byte[] datasize = Encoding.Unicode.GetBytes(json);

            _maxSize = Math.Round(CountingSizePacket(datasize));

            var sendPacket = new Data_Base()
            {
                IsCode = Code.ThreadStart,
                SizePacket = datasize.Length,
                IdentityCommand = _identityCommand,
            };

            SendData(sendPacket);

            await Task.Delay(100);

            List<byte> bytes = new List<byte>();

            for (int i = 0; i < datasize.Length; i++)
            {
                if (cancelTokenSource.IsCancellationRequested)
                    break;

                bytes.Add(datasize[i]);

                if (i == datasize.Length - 1)
                {
                    await Sending(bytes);
                    bytes.Clear();
                    break;
                }

                if (datasize.Length >= 1024 && datasize.Length < 4096)
                {
                    if (bytes.Count >= ((datasize.Length) / 2))
                    {
                        await Sending(bytes);
                        bytes.Clear();
                        continue;
                    }
                }

                if (datasize.Length >= 4096)
                {
                    if (bytes.Count >= 131072)
                    {
                        await Sending(bytes);
                        bytes.Clear();
                    }
                }

            }

            sendPacket = new Data_Base()
            {
                IsCode = Code.ThreadEnd,
            };

            SendData(sendPacket);

            FinishUpload?.Invoke();
        }

        private void SendData(Data_Base data)
        {
            var sendMsg = new Data_FirstCommand()
            {
                Command = _command,
                Json = JsonSerializer.Serialize(data)
            };

            _Main.Instance.Client.Send(JsonSerializer.Serialize(sendMsg));
        }

        private void SetAcceptPacket()
        {
            acceptTokenSource = new CancellationTokenSource();
            accepttoken = acceptTokenSource.Token;
        }

        private void SetCancelToken()
        {
            cancelTokenSource = new CancellationTokenSource();
            canceltoken = cancelTokenSource.Token;
        }

        private async Task Sending(List<byte> bytes)
        {
            Data_Base sendPacket;
  
            var new_data = bytes.ToArray();
            _sum += Math.Round(CountingSizePacket(new_data));

            if (_sum <= _maxSize)
            {
                StatusUpload?.Invoke((_sum, _maxSize));
            }

            sendPacket = new Data_Base()
            {
                IsCode = Code.ThreadNext,
                Data = new_data,
                PacketIndex = Guid.NewGuid().ToString(),
            };

            SendData(sendPacket);

            int countWait = 0;

            while (acceptTokenSource.IsCancellationRequested == false)
            {

                if (acceptTokenSource.IsCancellationRequested)
                    break;


                if (cancelTokenSource.IsCancellationRequested)
                    break;

                if (countWait == 2 || countWait == 4 || countWait == 6 || countWait == 8 || countWait == 12)
                {
                    SendData(sendPacket);
                }

                if (countWait > 12)
                {
                    CloseThread();
                }

                try
                {
                    await Task.Delay(1000, accepttoken);
                }
                catch
                {
                    break;
                }

                countWait++;

            }

            await Task.Delay(10);
            SetAcceptPacket();
        }

    }
}
