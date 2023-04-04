using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.ComponentModel;
using System.Timers;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace AdaptiveTestingSystem.ServerLibraly.CScript
{

    public class SendLogin
    {
        public string Login { get; set; }
    }
    public class ActiveSocket
    {

        private System.Timers.Timer aTimer;
        private int TimerCount = 0;

        CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        CancellationToken token;
        public int Port { get; set; }
        public string Login { get; private set; } = "";

        ServerObject serverObject;
        Task _Task;
        public ActiveSocket(ServerObject server)
        {
            serverObject = server;
            token = cancelTokenSource.Token;
        }

        public void Start(SocketObject socket)
        {
            Port = socket.Port;

            TimerCount = socket.TimeLife;

            SetTimer();

            _Task = Task.Run(async() => 
            {
                await ReceiveMessageAsync(socket);
            }, token);
        }

        private void SetTimer()
        {
            aTimer = new System.Timers.Timer(1000);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            TimerCount--;

            if (TimerCount == 0)
            {
                Stop();
            }
        }


        async Task ReceiveMessageAsync(SocketObject socket)
        {



            var list = new TcpListener(new IPEndPoint(socket.IPAddressAddress, socket.Port));
            list.Start();



            try
            {

                using var tcpClient = await list.AcceptTcpClientAsync(token);

                using (var stream = tcpClient.GetStream())
                {



                    while (token.IsCancellationRequested == false)
                    {

                        if (token.IsCancellationRequested)  // проверяем наличие сигнала отмены задачи
                        {
                            break;     //  выходим из метода и тем самым завершаем задачу
                        }

                      

                        byte[] bytes = new byte[2048];
                        StringBuilder builder = new StringBuilder();
                        int bit = 0;
                        do
                        {
                            bit = await stream.ReadAsync(bytes);
                            builder.Append(Encoding.Unicode.GetString(bytes, 0, bit));
                        }
                        while (stream.DataAvailable);

                        if (this.Login.Trim().Length == 0)
                        {
                            var json = JsonSerializer.Deserialize<SendLogin>(builder.ToString());
                            if (json != null)
                            {
                                this.Login = json.Login;
                                Logger.Debug(this.Login);
                            }

                            continue;
                        }                      
                        else
                        {
                            Logger.Debug($"{Login} Connect: {Port}");

                            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Data_Testing));
                            // десериализуем объект

                            TextReader text = new StringReader(builder.ToString());

                            var testing = xmlSerializer.Deserialize(text) as Data_Testing;
                                                 

                            if (testing != null)
                            {
                               
                                await Task.Factory.StartNew(async () => { await DBAddingMethod.AddNewTesting(testing); SendMesasge();  });
                            }

                        
                            break;
                        }
                    }


                }

                Disposed(list);
            }
            catch (OperationCanceledException exe)
            {
                Logger.Message($"Сокет {socket.IPAddressAddress}:{socket.Port} - закрыт. Операция была отменена");
                
                Disposed(list);
            }
            catch (Exception ex)
            {
                Logger.Error($"ActiveSocket.ReceiveMessageAsync выдал критическую ошибка: {ex.Message}");
                SendMesasge(true);
                Disposed(list);
            }








            //using UdpClient receiver = new UdpClient(new IPEndPoint(socket.IPAddressAddress, socket.Port));

            //while (token.IsCancellationRequested==false)
            //{

            //    if (token.IsCancellationRequested)  // проверяем наличие сигнала отмены задачи
            //    {                
            //        break;     //  выходим из метода и тем самым завершаем задачу
            //    }
            //    try
            //    {
            //        // получаем данные
            //        var result = await receiver.ReceiveAsync(token);
            //        TimerCount = 200;
            //        var message = Encoding.UTF8.GetString(result.Buffer);
            //        // выводим сообщение
            //        Logger.Debug($"{socket.IPAddressAddress}:{socket.Port}: {message}");
            //    }

            //    catch (OperationCanceledException exe)
            //    {
            //        Logger.Message($"Сокет {socket.IPAddressAddress}:{socket.Port} - закрыт. Операция была отменена");
            //    }
            //    catch (Exception ex)
            //    {
            //        Logger.Error($"ActiveSocket.ReceiveMessageAsync выдал критическую ошибка: {ex.Message}");
            //    }

            //    finally
            //    {
            //        receiver.Close();
            //        serverObject.DeleteSocket(this);
            //        _Task.Dispose();
            //    }

        }

        private void Disposed(TcpListener list)
        {
            list.Stop();
            serverObject.DeleteSocket(this);
            _Task.Dispose();
        }

        private async void SendMesasge(bool isError=false)
        {
            try
            {
                var guid = await serverObject.IsReturnAuthorizedGUID(Login);
                if (guid != null)
                {

                    Data_Code code = new Data_Code()
                    {
                        IsCode = isError==true?Code.Error:Code.Successfully
                    };

                    Data_FirstCommand data = new Data_FirstCommand()
                    {
                        Command = "Command_SuccessfullyOrErrorAddingTest",
                        Json = JsonSerializer.Serialize(code)
                    };

                    serverObject.Send(JsonSerializer.Serialize(data), guid);
                }
            }
            catch (Exception exSend)
            {
                Logger.Error($"ActiveSocket.SendMesasge выдал критическую ошибку при отправке пакета: {exSend.Message}");
            }
        }
        public void Stop() 
        {
            cancelTokenSource.Cancel();
            if (aTimer != null)
            {
                aTimer.Stop();
                aTimer.Dispose();
            }
        }
    }
}
