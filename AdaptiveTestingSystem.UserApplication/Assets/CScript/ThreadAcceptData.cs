using AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.CScript
{
    public class ThreadAcceptData
    {

        public delegate void ErrorUploadHandler(string error);
        public event ErrorUploadHandler? ErrorUpload;

        public delegate void FinishUploadHandler(object packet);
        public event FinishUploadHandler? FinishUpload;

        public delegate void StatusUploadHandler((double,double)sendmax,bool collection = false);
        public event StatusUploadHandler? StatusUpload;

        public delegate void StartCollectingPacketHandler((double, double) sendmax);
        public event StartCollectingPacketHandler? StartCollectingPacket;

        public delegate void StopUploadHandler();
        public event StopUploadHandler? StopUploadPacket;

        public ThreadAcceptData()
        { 
        
        }

         bool startQueueCheck = false;
         private CancellationTokenSource cancelTokenSource;
         private CancellationToken token;
         byte[] dataPakcet = new byte[0];
         int size;
         Queue<(string,byte[])> QueueByte = new Queue<(string,byte[])>();
         private double sum;

         public void Accept(Data_Base data)
         {
            
            switch (data.IsCode)
            {
                case Code.ThreadStart: Logger.Debug("Start"); StartSendPacket(data); break;
                case Code.ThreadEnd: Logger.Debug("Upload"); CollectDataPacket(); break;
                case Code.ThreadNext:  NextAcceptPacket(data.PacketIndex,data.Data); break;
                case Code.ThreadStop: StopingAcceptData();  break;
            };
        }

        private void StopingAcceptData()
        {
           
           
                Application.Current.Dispatcher.Invoke(() => {

                    Logger.Warning("[Загрузка данных] - загрузка прервана пользователем");
                    StopUploadPacket?.Invoke();   
               
                    startQueueCheck = false;
                    cancelTokenSource.Cancel();
                    cancelTokenSource.Dispose();
                });
            
        }

        private double CountingSizePacket(byte[] datasize)
        {
            return (double)(datasize.Length) / (1024 * 2);
        }

         private double CountingSizePacket(int size)
        {
            return (double)(size) / (1024 * 2);
        }
         private void CollectDataPacket()
        {
            Application.Current.Dispatcher.Invoke(async() => {

                Logger.Warning("[Загрузка данных] - Загрузка данных c сервера завершена");
                Logger.Warning("[Загрузка данных] - Начинаю обработку");
                StartCollectingPacket?.Invoke((Math.Round(CountingSizePacket(size)), Math.Round(CountingSizePacket(size))));
                await CheckQueue();
                startQueueCheck = false;
                cancelTokenSource.Cancel();
                cancelTokenSource.Dispose();
            });
        }
         private void StartSendPacket(Data_Base obj)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                size = obj.SizePacket;
                dataPakcet = new byte[0];
                startQueueCheck = true;
                sum = 0;
                QueueByte = new Queue<(string,byte[])>();
                CreateToken();
            });
        }
         private void CreateToken()
        {
            cancelTokenSource = new CancellationTokenSource();
            token = cancelTokenSource.Token;
        }


         private async void NextAcceptPacket(string packetIndex, byte[] data)
        {
            await Application.Current.Dispatcher.Invoke(async () =>
            {

                // Logger.Debug($"{packetIndex} | {data.Length}");

                if (ThreadManager.IsStop)
                {
                    return;
                }

                var item = QueueByte.FirstOrDefault(o=>o.Item1.Contains(packetIndex));

                if (item.Item1 == null)
                {
                    QueueByte.Enqueue((packetIndex, data));
                }
                                  

                Application.Current.Dispatcher.Invoke(() =>
                {
                    sum += Math.Round(CountingSizePacket(data));
                    double maxsize = Math.Round(CountingSizePacket(size));
                    if (sum <= maxsize)
                        StatusUpload?.Invoke((sum, maxsize));

                });


                ThreadManager.GoNextPacket();
                await Task.Delay(10);
            });
        }

         private async Task CheckQueue()
        {
            await Application.Current.Dispatcher.Invoke(async () => {
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
            });
        }


         private async Task CreatePacket()
        {
            await Application.Current.Dispatcher.Invoke(async () =>
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
                    size = 0;

                    Logger.Error($"ThreadAcceptData.CreatePacket вызывал ошибку: {ex.Message}");
                    _Main.Instance.NotificationViewerManagerNotificationViewerManager.Add(ex.Message, "Ошибка", TypeNotification.Error);
                    _Main.Instance._Notification.Add("", "Возникла ошибка", TypeNotification.Error);

                    ErrorUpload?.Invoke(ex.Message);
                }
            });
        }     
    }
}
