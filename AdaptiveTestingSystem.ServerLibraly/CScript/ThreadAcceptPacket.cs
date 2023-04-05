using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace AdaptiveTestingSystem.ServerLibraly.CScript
{
    public class ThreadAcceptPacket
    {
        public delegate void ErrorUploadHandler(string error);
        public event ErrorUploadHandler? ErrorUpload;

        public delegate void FinishUploadHandler(object packet);
        public event FinishUploadHandler? FinishUpload;

        public delegate void StatusUploadHandler((double, double) sendmax, bool collection = false);
        public event StatusUploadHandler? StatusUpload;

        public delegate void StopUploadHandler();
        public event StopUploadHandler? StopUploadPacket;

        public delegate void GoHextPacket();
        public event GoHextPacket? GoHext;

        public ThreadAcceptPacket()
        {
        
            TimerTime = 60;
            TimerNoAcceptPacket = new DispatcherTimer();
            TimerNoAcceptPacket.Tick += new EventHandler(timeReconnectr_Tick);
            TimerNoAcceptPacket.Interval = TimeSpan.FromSeconds(1);
            TimerNoAcceptPacket.Start();

        }

        private void timeReconnectr_Tick(object? sender, EventArgs e)
        {
            TimerTime--;
            if (TimerTime <= 0)
            {        
                StopingAcceptData();
            }
        }

        private CancellationTokenSource cancelTokenSource;
        private CancellationToken token;
        byte[] dataPakcet = new byte[0];
        Queue<(string, byte[])> QueueByte = new Queue<(string, byte[])>();

        private DispatcherTimer TimerNoAcceptPacket;

        public int TimerTime { get; private set; }

        public void Accept(Data_Base data)
        {

            switch (data.IsCode)
            {
                case Code.ThreadStart: StartSendPacket(data); break;
                case Code.ThreadEnd:  CollectDataPacket(); break;
                case Code.ThreadNext: NextAcceptPacket(data.PacketIndex, data.Data); break;
                case Code.ThreadStop: StopingAcceptData(); break;
            };
        }

        private void StopingAcceptData()
        {
            TimerNoAcceptPacket.Stop();
            StopUploadPacket?.Invoke();
            cancelTokenSource.Cancel();
            cancelTokenSource.Dispose();
        }
        private async void CollectDataPacket()
        {
            await CheckQueue();
            TimerNoAcceptPacket.Stop();
            cancelTokenSource.Cancel();
            cancelTokenSource.Dispose();
        }
        private void StartSendPacket(Data_Base obj)
        {
            dataPakcet = new byte[0];
            QueueByte = new Queue<(string, byte[])>();
            CreateToken();
        }
        private void CreateToken()
        {
            cancelTokenSource = new CancellationTokenSource();
            token = cancelTokenSource.Token;
        }


        private async void NextAcceptPacket(string packetIndex, byte[] data)
        {
            if (cancelTokenSource.IsCancellationRequested)
            {
                return;
            }

            var item = QueueByte.FirstOrDefault(o => o.Item1.Contains(packetIndex));

            if (item.Item1 == null)
            {
                QueueByte.Enqueue((packetIndex, data));
            }

            GoHext?.Invoke();
            TimerTime = 60;
            await Task.Delay(10);      
        }

        private async Task CheckQueue()
        {
            while (QueueByte.Count != 0)
            {
                if (QueueByte.Count == 0) break;

                int step = dataPakcet.Length;
                var data = QueueByte.Dequeue();
                Array.Resize(ref dataPakcet, dataPakcet.Length + data.Item2.Length);
                Array.Copy(data.Item2, 0, dataPakcet, step, data.Item2.Length);
                await Task.Delay(5);
            }
            await CreatePacket();
        }


        private async Task CreatePacket()
        {
          
                StringBuilder builder = new StringBuilder();
                builder.Append(Encoding.Unicode.GetString(dataPakcet));
                string packet = builder.ToString();
                try
                {
                    await Task.Delay(50);
                    FinishUpload?.Invoke(packet);
                }
                catch (Exception ex)
                {
                    dataPakcet = new byte[0];
                    Logger.Error($"ThreadAcceptData.CreatePacket вызывал ошибку: {ex.Message}");
                    ErrorUpload?.Invoke(ex.Message);
                }
            }
    }
}
