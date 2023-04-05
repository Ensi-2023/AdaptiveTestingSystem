using AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage.window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace AdaptiveTestingSystem.UserApplication.Assets.CScript
{
    public class ThreadSendingTestToServer
    {

        static double _sizes = 0;
        static string _maxsize = string.Empty;
        static CancellationTokenSource cancelTokenSource;
        static CancellationToken Token;

        public static void CancelThread()
        {

            ThreadManager.CloseActiveThread();  

            if (cancelTokenSource == null) return;
            try
            {
                cancelTokenSource.Cancel();
                cancelTokenSource.Dispose();
            } catch { }
        }


        public static async void StartSendEditOrInsertQuest(Data_QuestionEditOrInsert question,bool isInsert = false)
        {
            cancelTokenSource = new CancellationTokenSource();
            Token = cancelTokenSource.Token;

            try
            {
                await Application.Current.Dispatcher.Invoke(async () =>
                {
                    _Main.Instance.OverlayShow(true, TypeOverlay.loading, "Генерация вопроса", $"Начинаю отправку данных",visibleButton:Visibility.Collapsed);
                    await Task.Delay(250);
                    _Main.Instance.OverlayShow(true, TypeOverlay.loading, "Генерация вопроса", $"Отправка ожидайте...", visibleButton: Visibility.Collapsed);
                    string json = JsonSerializer.Serialize(question);
                    byte[] datasize = Encoding.Unicode.GetBytes(json);
                    _maxsize = Math.Round(CountingSizePacket(datasize)).ToString();
                    var sendPacket = new Data_SendTesting()
                    {
                        IsCode = Code.ThreadStart,
                        SizePacket = datasize.Length,
                        IsEdit = isInsert
                    };

       
                    ThreadManager.Send("Command_ApendQuestingData", sendPacket);
                    _Main.Instance.OverlayShow(true, TypeOverlay.loading, "Генерация вопроса", $"Отправляю: {0} кб из {_maxsize} кб", visibleButton: Visibility.Visible);


                    await Task.Delay(500);

                    List<byte> bytes = new List<byte>();

                    for (int i = 0; i < datasize.Length; i++)
                    {

                        if (cancelTokenSource.IsCancellationRequested)
                            break;

                        bytes.Add(datasize[i]);

                        if (i == datasize.Length - 1)
                        {
                            await Send(bytes, "Command_ApendQuestingData");
                            bytes.Clear();
                            break;
                        }

                        if (bytes.Count >= 150000)
                        {
                            await Send(bytes, "Command_ApendQuestingData");
                            bytes.Clear();
                        }

                    }

                    //sendPacket = new Data_SendTesting()
                    //{
                    //    IsCode = Code.ThreadEnd,
                    //};

                    //Send(sendPacket, "Command_ApendQuestingData");


                    ThreadManager.CloseActiveThread();  
                    _maxsize = string.Empty;
                    _sizes = 0;
                });
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _Main.Instance.OverlayShow(true, TypeOverlay.error, "Генерация вопроса", $"Вознакла ошибка:\n{ex.Message}", visibleButton: Visibility.Visible);
                });
            }
        }

        private static void Send(Data_SendTesting data,string command = "Command_ApendTestingData")
        {
                    
            var sendMsg = new Data_FirstCommand()
            {
                Command = command,
                Json = JsonSerializer.Serialize(data)
            };

            _Main.Instance.Client.SendMany(JsonSerializer.Serialize(sendMsg));
        }
        public static async void SendTest(Data_Testing data, CancellationTokenSource cancelTokenSource, CancellationToken token, bool isEdit)
        {
            try
            {
               await Application.Current.Dispatcher.Invoke(async() =>
                {
                    GUI_ViewCreateQuestions.Instanse.OverlayShow(true, TypeOverlay.loading, "Генерация теста", $"Начинаю отправку данных", visibleButton: Visibility.Collapsed);
                    await Task.Delay(250);
                    GUI_ViewCreateQuestions.Instanse.OverlayShow(true, TypeOverlay.loading, "Генерация теста", $"Отправка ожидайте...", visibleButton: Visibility.Collapsed);
                    string json = JsonSerializer.Serialize(data);
                    byte[] datasize = Encoding.Unicode.GetBytes(json);
                    _maxsize = Math.Round(CountingSizePacket(datasize)).ToString();
                   
                    var sendPacket = new Data_SendTesting()
                    {
                        IsCode = Code.ThreadStart,
                        SizePacket = datasize.Length,
                        IsEdit = isEdit
                    };

            
                   ThreadManager.Send("Command_ApendTestingData", sendPacket);

                    GUI_ViewCreateQuestions.Instanse.OverlayShow(true, TypeOverlay.loading, "Генерация теста", $"Отправляю: {0} кб из {_maxsize} кб",visibleButton:Visibility.Visible);


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

                        if (bytes.Count >= 150000)
                        {
                            await Send(bytes);
                            bytes.Clear();
                        }

                    }

                    //sendPacket = new Data_SendTesting()
                    //{
                    //    IsCode = Code.ThreadEnd,
                    //};
                    //Send(sendPacket);

                    ThreadManager.CloseActiveThread();

           
                    _maxsize = string.Empty;
                    _sizes = 0;
                });


            }  
            catch(Exception ex) 
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    GUI_ViewCreateQuestions.Instanse.OverlayShow(true, TypeOverlay.error, "Генерация теста", $"Вознакла ошибка:\n{ex.Message}", visibleButton: Visibility.Visible);

                });
            }
        }

        private static double CountingSizePacket(byte[] datasize)
        {
            return (double)(datasize.Length) / (1024 * 2);
        }


        private static async Task Send(List<byte> bytes,string command = "Command_ApendTestingData")
        {
            Data_SendTesting sendPacket;
        
            var new_data = bytes.ToArray();
            _sizes += CountingSizePacket(new_data);

            if(command == "Command_ApendTestingData")
                GUI_ViewCreateQuestions.Instanse.OverlayShow(true, TypeOverlay.loading, "Генерация теста", $"Отправлено: {Math.Round(_sizes)} кб из {_maxsize} кб");
            else
                _Main.Instance.OverlayShow(true, TypeOverlay.loading, "Генерация вопроса", $"Отправлено: {Math.Round(_sizes)} кб из {_maxsize} кб");

            sendPacket = new Data_SendTesting()
            {
                IsCode = Code.ThreadNext,
                Data = new_data
            };

            Send(sendPacket, command);
            await Task.Delay(120);
        }
    }
    
}
