using AdaptiveTestingSystem.ServerLibraly.CScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{
    public class Command_AddMultyServerTesting:Commands
    {

        ThreadSendPacket sendPacket;
        public override async void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<Data_MultyServer>(json);

                if (obj != null)
                {
                    switch (obj.IsCode)
                    {
                        case Code.ThreadStart: StartSendData(obj, client, activeServer); break;
                        case Code.ThreadNext: sendPacket.CancelWaitSendPacket(); break;
                        case Code.ThreadEnd: StopSend(); break;
                    }
                }    
            }
            catch (Exception ex)
            {
                Logger.Error($"Command_AddMultyServerTesting ({client.IP}:{client.Port}) вызвал ошибку: {ex.Message}");
            }
        }

        private void StopSend()
        {
            if (sendPacket != null) { sendPacket.Dispose(); sendPacket = null; }
        }

        private async void StartSendData(Data_MultyServer obj, ClientObject client, ServerObject activeServer)
        {
            sendPacket = new ThreadSendPacket("Command_ConnectAdminTestServer", client, activeServer);
            if (obj != null)
            {
                var indexCreationServer = await activeServer.ServerBrowser.AddServer(obj.IndexTest, obj.IndexCreator, obj.NameTest, client.GuidClient, obj.Password, obj.IsAdaptive, obj.CountQuestForTesting);
                var nameSotrudAndNamePredmet = await DBSearchMethods.GetNameSotrudAndNamePredmet(obj.IndexCreator, obj.IndexTest);
                var packet = new Data_ListPacketServer();

                var listServer = new List<Data_ListMultyServer>()
                    {
                        new Data_ListMultyServer()
                        {
                           IndexCreator = obj.IndexCreator,
                           NameTest = obj.NameTest,
                           Password = obj.Password,
                           IsAdaptive = obj.IsAdaptive,
                           CountUser = 0,
                           IndexServer = indexCreationServer,
                           IndexTest= obj.IndexTest,
                           NameCreator=nameSotrudAndNamePredmet.Item1,
                           NamePredmet = nameSotrudAndNamePredmet.Item2,
                           IsCode = Code.CreateServer,
                           CountQuestForTest = obj.CountQuestForTesting
                        }
                    };

                packet.MultyServer = listServer;
                sendPacket.StartSend(packet);
            }
        }
    }
}



