using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Xml.Serialization;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage.window;
using System.Net.Http;


//Не работает

namespace AdaptiveTestingSystem.UserApplication.Assets.CScript
{

    public class SendLogin
    { 
       public string Login { get; set; }
    }
    public class UPDSendData
    {
     
        private static UdpClient sender = new UdpClient();
        private static IPEndPoint endPoint;


        private static TcpClient tcpClient;

        public async static void Setup(string ip,int port)
        {
            try
            {

                endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                //sender.Connect(endPoint);

                tcpClient = new TcpClient();
                await tcpClient.ConnectAsync(endPoint);

                if (tcpClient.Connected)
                {
                   await Application.Current.Dispatcher.Invoke(async() => {
                        GUI_ViewCreateQuestions.Instanse.OverlayShow(true, TypeOverlay.loading, "Генерация теста", $"Порт {ip}:{port} открыт, ожидаю отправки данных.");
                        await Task.Delay(250);
                        GUI_ViewCreateQuestions.Instanse.StartSendToDB();
                    });
              
                 
                    Logger.Message($"Порт {ip}:{port} открыт, ожидаю отправки данных.");
                }
                 
            }
            catch (Exception ex)
            {
                Logger.Error($"UPDSendData.Setup вызывал ошибку: {ex.Message}");
            }
        }


        static string maxsize;
        public async static void SendInfo(Data_Testing data, CancellationTokenSource cancelTokenSource, CancellationToken token)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    GUI_ViewCreateQuestions.Instanse.OverlayShow(true, TypeOverlay.loading, "Генерация теста", $"Начинаю отправку данных");

                });

               // await Task.Delay(250);
              //  string xmlMessage = MySerializer<Data_Testing>.Serialize(data);
                

               // // Сериализуем объект

               // Application.Current.Dispatcher.Invoke(() =>
               // {
               //     GUI_ViewCreateQuestions.Instanse.OverlayShow(true, TypeOverlay.loading, "Генерация теста", $"Сериализуем объект");

               // });
                await Task.Delay(250);

                // Отправляем информацию о файле
               await Application.Current.Dispatcher.Invoke(async () =>
               {
                   GUI_ViewCreateQuestions.Instanse.OverlayShow(true, TypeOverlay.loading, "Генерация теста", $"Отправка ожидайте...");


                   string json = JsonSerializer.Serialize(data);


                   byte[] datasize = Encoding.Unicode.GetBytes(json);
                   maxsize = Math.Round(CountingSizePacket(datasize)).ToString();

                   GUI_ViewCreateQuestions.Instanse.OverlayShow(true, TypeOverlay.loading, "Генерация теста", $"Отправляю: {0} мб из {maxsize} мб");


                   var sendPacket = new Data_SendTesting()
                   {
                       IsCode = Code.ThreadStart,
                       SizePacket = datasize.Length
                   };

                   var sendMsg = new Data_FirstCommand()
                   {
                       Command = "Command_ApendTestingData",
                       Json = JsonSerializer.Serialize(sendPacket)
                   };

                   _Main.Instance.Client.SendMany(JsonSerializer.Serialize(sendMsg));

                   await Task.Delay(500);

             

                   List<byte> bytes = new List<byte>();
                   for (int i = 0; i < datasize.Length; i++)
                   {
                     
                       if (cancelTokenSource.IsCancellationRequested)
                           break;



                       bytes.Add(datasize[i]);

                       if (i == datasize.Length - 1)
                       {
                           await Send(bytes);
                           bytes.Clear();
                           break;
                       }

                                          

                       if (bytes.Count >= 400000)
                       {
                           await Send(bytes);
                           bytes.Clear();
                       }
                      
                   }


                   Logger.Debug($"list: {bytes.Count}  array : {datasize.Length}");

                   sendPacket = new Data_SendTesting()
                   {
                       IsCode = Code.ThreadEnd,
                   };

                   sendMsg = new Data_FirstCommand()
                   {
                       Command = "Command_ApendTestingData",
                       Json = JsonSerializer.Serialize(sendPacket)
                   };

                   _Main.Instance.Client.SendMany(JsonSerializer.Serialize(sendMsg));

                   maxsize = string.Empty;
                   sizes = 0;
                   //  _Main.Instance.Client.SendMany(JsonSerializer.Serialize(packet));

               });


    

                //SendLogin send = new SendLogin()
                //{
                //    Login = _Main.Instance.MyAccount.Login,
                //};


                //using (var nstream = tcpClient.GetStream())
                //{
                //    await SendMessage(JsonSerializer.Serialize(send), token, nstream);
                //    await Task.Delay(2500);
                //    //отправляем длину сообщения            
                //    await SendMessage(xmlMessage, token, nstream);
                //}
           

                //tcpClient.Close();
                //tcpClient.Dispose();


            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() => 
                {
                    GUI_ViewCreateQuestions.Instanse.OverlayShow(true, TypeOverlay.error, "Генерация теста", $"Вознакла ошибка:\n{ex.Message}",visibleButton:Visibility.Visible);

                });
             }

        }

        private static double CountingSizePacket(byte[] datasize)
        {
            return (double)(datasize.Length) / (1024 * 2);
        }

        static double sizes = 0;

        private static async Task Send(List<byte> bytes)
        {
            Data_SendTesting sendPacket;
            //byte[] bytes1 = new byte[bytes.Count];
            //for (int h = 0; h < bytes.Count; h++)
            //{
            //    bytes1[h] = bytes[h];
            //}


            var new_data = bytes.ToArray();
            sizes += CountingSizePacket(new_data);

            GUI_ViewCreateQuestions.Instanse.OverlayShow(true, TypeOverlay.loading, "Генерация теста", $"Отправляю: {Math.Round(sizes)} мб из {maxsize} мб");

            sendPacket = new Data_SendTesting()
            {
                IsCode = Code.ThreadNext,
                Data = new_data
            };


            var dataPacket = new Data_FirstCommand()
            {
                Command = "Command_ApendTestingData",
                Json = JsonSerializer.Serialize(sendPacket)
            };
        
            _Main.Instance.Client.SendMany(JsonSerializer.Serialize(dataPacket));
            await Task.Delay(100);
        }

        private static async Task SendMessage(string xmlMessage, CancellationToken token, NetworkStream nstream)
        {
            //отправляем сообщение
            byte[] byteArray = Encoding.Unicode.GetBytes(xmlMessage);
            await nstream.WriteAsync(byteArray, token);
        }
    }
}
