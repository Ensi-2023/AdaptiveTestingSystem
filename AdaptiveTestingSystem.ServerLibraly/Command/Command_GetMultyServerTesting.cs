using AdaptiveTestingSystem.ServerLibraly.CScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{


    public class Command_GetMultyServerTesting:Commands
    {

        ThreadSendPacket sendPacket;

        public override async void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {

                var obj = JsonSerializer.Deserialize<Data_ListPacketServer>(json);

                if (obj != null)
                {
                    switch (obj.IsCode)
                    {
                        case Code.ThreadStart: StartSendData(client, activeServer); break;
                        case Code.ThreadNext: sendPacket.CancelWaitSendPacket(); break;
                        case Code.ThreadEnd: StopSend(); break;
                    }
                }




               
                 
            }
            catch (Exception ex)
            {
                Logger.Error($"Command_GetMultyServerTesting ({client.IP}:{client.Port}) вызвал ошибку: {ex.Message}");
            }
        }


        private void StopSend()
        {
            if (sendPacket != null) { sendPacket.Dispose(); sendPacket = null; }
        }

        private async void StartSendData(ClientObject client, ServerObject activeServer)
        {
            sendPacket = new ThreadSendPacket("Command_SetAllMultyTest", client, activeServer);

            var allServer = activeServer.ServerBrowser.GetAllServer();
            if (allServer != null)
            {
                var packet = new Data_ListPacketServer()
                {
               
                };

                List<Data_ListMultyServer> List = new List<Data_ListMultyServer>();

                foreach (var item in allServer)
                {
                    if (item.IsCode != Code.WaitClient) { continue; }
                    var nameSotrudAndNamePredmet = await DBSearchMethods.GetNameSotrudAndNamePredmet(item.IndexCreator, item.IndexTest);
                    var itemServer = new Data_ListMultyServer()
                    {
                        CountUser = item.Client.Count,
                        IndexCreator = item.IndexCreator,
                        IndexServer = item.IndexServer,
                        IndexTest = item.IndexTest,
                        IsAdaptive = item.IsAdaptive,
                        NameCreator = nameSotrudAndNamePredmet.Item1,
                        NamePredmet = nameSotrudAndNamePredmet.Item2,
                        NameTest = item.NameTest,
                        Password = item.Password.Trim() == string.Empty ? string.Empty : Encryption.getMd5Hash(item.Password),

                    };
                    List.Add(itemServer);
                    await Task.Delay(20);
                }

                packet.MultyServer = List;
                sendPacket.StartSend(packet);
            }


        
         
        }
    }
}



//var allServer = activeServer.ServerBrowser.GetAllServer();
//if (allServer != null)
//{

//    var packet = new Data_ListPacketServer()
//    {
//        IsAdmin = false,
//    };


//    List<Data_ListMultyServer> List = new List<Data_ListMultyServer>();

//    foreach (var item in allServer)
//    {
//        if (item.IsCode != Code.WaitClient) { continue; }
//        var nameSotrudAndNamePredmet = await DBSearchMethods.GetNameSotrudAndNamePredmet(item.IndexCreator, item.IndexTest);
//        var itemServer = new Data_ListMultyServer()
//        {
//            CountUser = item.Client.Count,
//            IndexCreator = item.IndexCreator,
//            IndexServer = item.IndexServer,
//            IndexTest = item.IndexTest,
//            IsAdaptive = item.IsAdaptive,
//            NameCreator = nameSotrudAndNamePredmet.Item1,
//            NamePredmet = nameSotrudAndNamePredmet.Item2,
//            NameTest = item.NameTest,
//            Password = item.Password.Trim() == string.Empty ? string.Empty : Encryption.getMd5Hash(item.Password),

//        };

//        List.Add(itemServer);
//        await Task.Delay(20);
//    }

//    packet.MultyServer = List;

//    var comman = new Data_FirstCommand()
//    {
//        Command = "Command_SetAllMultyTest",
//        Json = JsonSerializer.Serialize(packet)
//    };

//    Send(client, activeServer, comman);
//}