using AdaptiveTestingSystem.ServerLibraly.CScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{


   // var stat = await DBSearchMethods.GetGeneralStatistic();

    public class Command_StatisticGeneral:Commands
    {

        ThreadSendPacket sendPacket;

        private CancellationTokenSource cancelTokenSource;
        private CancellationToken token;

        private CancellationTokenSource acceptTokenSource;
        private CancellationToken accepttoken;


        public override void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<Data_StatisticPacket>(json);

                if (obj != null)
                {
                    switch (obj.IsCode)
                    {
                        case Code.ThreadStart: StartSendData(client, activeServer); break;
                        case Code.ThreadNext: sendPacket.CancelWaitSendPacket(); break;
                        case Code.ThreadEnd: StopSend(); break;
                    }
                }






                //var obj = JsonSerializer.Deserialize<Data_StatisticPacket>(json);
                //if (obj == null) return;

        
                //switch (obj.IsCode)
                //{
                //    case Code.ThreadStart: CreateTransmissionThread(client, activeServer); break;
                //    case Code.ThreadNext: CancelWaitSendPacket(); break;
                //    case Code.ThreadEnd: CloseThread(); return;
                //}

            }
            catch (Exception ex)
            {
                Logger.Error($"Command_StatisticGeneral вызвал ошибку: {ex.Message}");
            }
        }

        private void StopSend()
        {
            if (sendPacket != null) { sendPacket.Dispose(); sendPacket = null; }
        }

        private async void StartSendData( ClientObject client, ServerObject activeServer)
        {
            sendPacket = new ThreadSendPacket("Command_StatisticGeneral", client, activeServer);
            var listEmployee = await DBSearchMethods.GetGeneralStatistic();
            sendPacket.StartSend(listEmployee);
        }









        public void UserAcceptPacket()
        {
            acceptTokenSource = new CancellationTokenSource();
            accepttoken = acceptTokenSource.Token;
        }

        public void CancelWaitSendPacket()
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

        private void CloseThread()
        {
            CancelThread();
        }

        private async void CreateTransmissionThread(ClientObject client, ServerObject activeServer)
        {
            SetToken();
            UserAcceptPacket();
            var stat = await DBSearchMethods.GetGeneralStatistic();
            await CreateAndSendPacket(stat, client, activeServer);
            CloseThread();
        }

        private async Task CreateAndSendPacket(Data_StatisticGeneral stat, ClientObject client, ServerObject activeServer)
        {
            string json = JsonSerializer.Serialize(stat);
            byte[] datasize = Encoding.Unicode.GetBytes(json);

            var sendPacket = new Data_StatisticPacket()
            {
                IsCode = Code.ThreadStart,
                SizePacket = datasize.Length
            };

            SendData(sendPacket, client, activeServer);

            await Task.Delay(500);
            List<byte> bytes = new List<byte>();

            for (int i = 0; i < datasize.Length; i++)
            {

                if (cancelTokenSource.IsCancellationRequested)
                    break;

                bytes.Add(datasize[i]);

                if (i == datasize.Length - 1)
                {
                    await Sending(bytes, client, activeServer);
                    bytes.Clear();
                    break;
                }

                if (bytes.Count >= ((datasize.Length/2)/2))
                {
                    await Sending(bytes, client, activeServer);
                    bytes.Clear();
                }

            }

            sendPacket = new Data_StatisticPacket()
            {
                IsCode = Code.ThreadEnd,
            };

            SendData(sendPacket, client, activeServer);

        }

        private async Task Sending(List<byte> bytes, ClientObject client, ServerObject activeServer)
        {
            Data_StatisticPacket sendPacket;

            var new_data = bytes.ToArray();
            sendPacket = new Data_StatisticPacket()
            {
                IsCode = Code.ThreadNext,
                Data = new_data,
                PacketIndex=Guid.NewGuid().ToString(),  
            };

            SendData(sendPacket, client, activeServer);

            int countWait = 0;

            while (acceptTokenSource.IsCancellationRequested == false)
            {

                if (acceptTokenSource.IsCancellationRequested)
                    break;


                if (cancelTokenSource.IsCancellationRequested)
                    break;

                if (countWait == 2 || countWait == 4 || countWait == 6)
                {
                    SendData(sendPacket, client, activeServer);
                }

                if (countWait > 8)
                {
                    CancelThread();
                }

                try
                {
                    await Task.Delay(2000, accepttoken);
                }
                catch
                {
                    break;
                }

                countWait++;

            }

            await Task.Delay(10);
            UserAcceptPacket();
        }


        private void SendData(Data_StatisticPacket data, ClientObject client, ServerObject activeServer)
        {
            var sendMsg = new Data_FirstCommand()
            {
                Command = "Command_StatisticGeneral",
                Json = JsonSerializer.Serialize(data)
            };

            Send(client, activeServer, sendMsg);
        }

        private void SetToken()
        {
            cancelTokenSource = new CancellationTokenSource();
            token = cancelTokenSource.Token;
        }

        private void CancelThread()
        {
            try
            {
                cancelTokenSource.Cancel();
                cancelTokenSource.Dispose();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }
    }
}
