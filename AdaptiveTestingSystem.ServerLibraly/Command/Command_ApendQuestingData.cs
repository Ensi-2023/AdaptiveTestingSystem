using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_ApendQuestingData:Commands
    {

        byte[] dataPakcet = new byte[0];
        int size;
        Queue<byte[]> QueueByte = new Queue<byte[]>();

        bool startQueueCheck = false;
        bool IsInsert { get; set; } = false;

        public CancellationTokenSource cancelTokenSource;
        public CancellationToken token;
        public override void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {

                var obj = JsonSerializer.Deserialize<Data_SendTesting>(json);

                if (obj != null)
                {
                    switch (obj.IsCode)
                    {
                        case Code.ThreadStart: StartSendPacket(obj); break;
                        case Code.ThreadEnd: CollectDataPacket(client, activeServer); break;
                        case Code.ThreadNext: NextAcceptPacket(obj.Data, client, activeServer); break;
                    };

                }

            }
            catch (Exception ex)
            {
                Logger.Error($"Command_ApendQuestingData ({client.IP}:{client.Port}) вызвал ошибку: {ex.Message}");
                SendMesasge(client, activeServer, true);
            }
        }

        private void CreateToken()
        {
            cancelTokenSource = new CancellationTokenSource();
            token = cancelTokenSource.Token;
        }

        private void CollectDataPacket(ClientObject client, ServerObject activeServer)
        {
            startQueueCheck = false;
            SendMesasge(client, activeServer);

            cancelTokenSource.Cancel();
            cancelTokenSource.Dispose();

            // await CreateTest(client, activeServer);
        }

        private async Task CreateTest(ClientObject client, ServerObject activeServer)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(Encoding.Unicode.GetString(dataPakcet));

            string packet = builder.ToString();
            try
            {
                var obj = JsonSerializer.Deserialize<Data_QuestionEditOrInsert>(packet);

                if (obj != null)
                {
                    await DBAddingMethod.EditQuestion(obj);

                    //SendMesasge(client, activeServer);
                    IsInsert = false;
                    dataPakcet = new byte[0];
                    size = 0;
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Command_ApendQuestingData ({client.IP}:{client.Port}) вызвал ошибку: {ex.Message}");
                dataPakcet = new byte[0];
                size = 0;
                SendMesasge(client, activeServer, true);
            }
        }

        private async void NextAcceptPacket(byte[] data, ClientObject client, ServerObject activeServer)
        {

            QueueByte.Enqueue(data);

            if (startQueueCheck == false)
            {
                startQueueCheck = true;
          

                try
                {
                    await Task.Delay(5000, token);
                }
                catch
                {
;
                }

                await CheckQueue(QueueByte, client, activeServer);
            }

        }

        private async Task CheckQueue(Queue<byte[]> queueByte, ClientObject client, ServerObject activeServer)
        {

            while (QueueByte.Count != 0)
            {
                if (QueueByte.Count == 0) break;

                int step = dataPakcet.Length;
                var data = QueueByte.Dequeue();
                Array.Resize(ref dataPakcet, dataPakcet.Length + data.Length);
                Array.Copy(data, 0, dataPakcet, step, data.Length);
                await Task.Delay(150);

            }
            await CreateTest(client, activeServer);
        }

        private void StartSendPacket(Data_SendTesting obj)
        {
            size = obj.SizePacket;
            dataPakcet = new byte[0];
            QueueByte = new Queue<byte[]>();
            IsInsert = obj.IsEdit;
            CreateToken();
        }

        private void SendMesasge(ClientObject client, ServerObject activeServer, bool isError = false)
        {
            try
            {
                Data_Code code = new Data_Code()
                {
                    IsCode = isError == true ? Code.Error : Code.Successfully
                };

                Data_FirstCommand data = new Data_FirstCommand()
                {
                    Command = "Command_SuccessfullyOrErrorAddingOrEditQuest",
                    Json = JsonSerializer.Serialize(code)
                };

                Send(client, activeServer, data);

            }
            catch (Exception exSend)
            {
                Logger.Error($"Command_ApendQuestingData.SendMesasge выдал критическую ошибку при отправке пакета: {exSend.Message}");
            }
        }
    }
}
