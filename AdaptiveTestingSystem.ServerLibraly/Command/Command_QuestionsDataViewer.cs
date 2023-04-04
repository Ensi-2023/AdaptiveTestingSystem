using AdaptiveTestingSystem.Data.JsonData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_QuestionsDataViewer:Commands
    {
        public override async void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<Data_QuestDataViewer>(json);
                if (obj != null)
                {
                    var packet = await DBSearchMethods.GetQuestData(obj.Index);
                    if (packet != null)
                    {
                        GetQuestToUserApplication(packet, client, activeServer);
                    }
                }
            }
            catch(Exception ex) 
            {
                Logger.Error($"Command_ApendTestingData ({client.IP}:{client.Port}) вызвал ошибку: {ex.Message}");
            }   
        }

        private async void GetQuestToUserApplication(Data_Question data, ClientObject client, ServerObject activeServer)
        {
            string json = JsonSerializer.Serialize(data);
            byte[] datasize = Encoding.Unicode.GetBytes(json);

            var sendPacket = new Data_QuestDataViewer()
            {
                IsCode = Code.ThreadStart,
                SizePacket = datasize.Length
            };

            SendData(sendPacket, client, activeServer);

            await Task.Delay(500);
            List<byte> bytes = new List<byte>();

            for (int i = 0; i < datasize.Length; i++)
            {

                if (client.cancelTokenSource.IsCancellationRequested)
                    break;

                bytes.Add(datasize[i]);

                if (i == datasize.Length - 1)
                {
                    await Sending(bytes, client, activeServer);
                    bytes.Clear();
                    break;
                }

                if (bytes.Count >= 150000)
                {
                    await Sending(bytes, client, activeServer);
                    bytes.Clear();
                }

            }


            sendPacket = new Data_QuestDataViewer()
            {
                IsCode = Code.ThreadEnd,
            };

            SendData(sendPacket, client, activeServer);

        }


        private async Task Sending(List<byte> bytes, ClientObject client, ServerObject activeServer)
        {
            Data_QuestDataViewer sendPacket;

            var new_data = bytes.ToArray();
            sendPacket = new Data_QuestDataViewer()
            {
                IsCode = Code.ThreadNext,
                Data = new_data
            };

            SendData(sendPacket, client, activeServer);
            await Task.Delay(120);
        }

        private void SendData(Data_QuestDataViewer data, ClientObject client, ServerObject activeServer)
        {
            var sendMsg = new Data_FirstCommand()
            {
                Command = "Command_ViewQuestData",
                Json = JsonSerializer.Serialize(data)
            };

            Send(client, activeServer, sendMsg);
        }
    }
}
