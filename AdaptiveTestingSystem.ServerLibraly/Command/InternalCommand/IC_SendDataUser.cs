using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace AdaptiveTestingSystem.ServerLibraly.Command.InternalCommand
{
    public class IC_SendDataUser
    {
        static private CancellationTokenSource cancelTokenSource;
        static private CancellationToken token;

        static private CancellationTokenSource acceptTokenSource;
        static private CancellationToken accepttoken;

        private static void SetToken()
        {
            cancelTokenSource = new CancellationTokenSource();
            token = cancelTokenSource.Token;
        }

        public static void CancelWaitSendPacket()
        {
            try
            {
                Logger.Debug("cancel");

                acceptTokenSource.Cancel();
                acceptTokenSource.Dispose();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }

        private static void CancelThread()
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
        private static void CloseThread()
        {
            CancelThread();
        }
        public static async void StartSend(Data_UserViewer packet, ClientObject client, ServerObject activeServer)
        {
            SetToken();
            UserAcceptPacket();
            await CreateAndSendPacket(packet, client, activeServer);
            CloseThread();
        }

        private static async Task Sending(List<byte> bytes, ClientObject client, ServerObject activeServer)
        {
            Data_UserPacket sendPacket;

            var new_data = bytes.ToArray();
            sendPacket = new Data_UserPacket()
            {
                IsCode = Code.ThreadNext,
                Data = new_data,
                PacketIndex = Guid.NewGuid().ToString(),
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


        public static void UserAcceptPacket()
        {
            acceptTokenSource = new CancellationTokenSource();
            accepttoken = acceptTokenSource.Token;
        }

        private static async Task CreateAndSendPacket(Data_UserViewer stat, ClientObject client, ServerObject activeServer)
        {
            string json = JsonSerializer.Serialize(stat);
            byte[] datasize = Encoding.Unicode.GetBytes(json);

            var sendPacket = new Data_UserPacket()
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

                if (datasize.Length >= 1024 && datasize.Length < 4096 )
                {
                    if (bytes.Count >= ((datasize.Length) / 2))
                    {
                        await Sending(bytes, client, activeServer);
                        bytes.Clear();
                        continue;
                    }
                }

                if (datasize.Length >= 4096)
                {
                    if (bytes.Count >= ((datasize.Length /2) / 2))
                    {
                        await Sending(bytes, client, activeServer);
                        bytes.Clear();
                    }
                }

            }

            sendPacket = new Data_UserPacket()
            {
                IsCode = Code.ThreadEnd,
            };

            SendData(sendPacket, client, activeServer);

        }

        private static void SendData(Data_UserPacket data, ClientObject client, ServerObject activeServer)
        {
            var sendMsg = new Data_FirstCommand()
            {
                Command = "Command_SetUserList",
                Json = JsonSerializer.Serialize(data)
            };

            Send(client, activeServer, sendMsg);
        }



        public static void Send(ClientObject client, ServerObject activeServer, Data_FirstCommand command)
        {
            activeServer.Send(JsonSerializer.Serialize(command), client.GuidClient);
        }

        public static void Stop()
        {
            CancelThread();
        }
    }   
}
