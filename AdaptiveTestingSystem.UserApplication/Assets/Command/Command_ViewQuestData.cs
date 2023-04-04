using AdaptiveTestingSystem.Data.JsonData;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.Command
{
    public class Command_ViewQuestData:Commands
    {

        byte[] dataPakcet = new byte[0];
        int size;
        Queue<byte[]> QueueByte = new Queue<byte[]>();

        bool startQueueCheck = false;

        private CancellationTokenSource cancelTokenSource;
        private CancellationToken token;


        public override void Execut(string json, InternetClient client)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() => {

                    var obj = JsonSerializer.Deserialize<Data_QuestDataViewer>(json);

                    if (obj != null)
                    {
                        switch (obj.IsCode)
                        {
                            case Code.ThreadStart: StartSendPacket(obj); break;
                            case Code.ThreadEnd: CollectDataPacket(); break;
                            case Code.ThreadNext: 
                                {
                              
                                    NextAcceptPacket(obj.Data);
                                } break;
                        };

                    }
                });
            }

            catch (Exception ex)
            {
                Logger.Error($"Command_ViewQuestData.Execut вызывал ошибку: {ex.Message}");
            }
        }
        private void CollectDataPacket()
        {
            Application.Current.Dispatcher.Invoke(() => {
                startQueueCheck = false;
                cancelTokenSource.Cancel();
                cancelTokenSource.Dispose();
            });

            // await CreateTest(client, activeServer);
        }

        private async Task CreateQuest()
        {
            await Application.Current.Dispatcher.Invoke(async () =>
            {

                StringBuilder builder = new StringBuilder();
                builder.Append(Encoding.Unicode.GetString(dataPakcet));

                string packet = builder.ToString();
                try
                {
                    _Main.Instance.OverlayShow(true, TypeOverlay.loading, "Просмотр данных вопроса", $"Орбаботка данных: {Math.Round(CountingSizePacket(size))} кб из {Math.Round(CountingSizePacket(size))} кб");

                    _Main.Instance.OverlayShow(true, TypeOverlay.loading, "Просмотр данных вопроса", $"Формирую вопрос...");
                    await Task.Delay(250);

                    var obj = JsonSerializer.Deserialize<Data_Question>(packet);

                    if (obj != null)
                    {
                      

                        var guiUID = (_Main.Instance.MainBody.Children[0] as View_BodyApplication).Main.Children[0] as GUI_QuestViewer;

                        dataPakcet = new byte[0];
                        size = 0;

                        if (guiUID == null) return;

                        guiUID.SetTest(obj);

                        _Main.Instance.OverlayShow(false);
                    }
                }
                catch (Exception ex)
                {

                    dataPakcet = new byte[0];
                    size = 0;
                    _Main.Instance.OverlayShow(false);
                    Logger.Error($"Command_ViewQuestData.CreateQuest вызывал ошибку: {ex.Message}");
                    _Main.Instance.NotificationViewerManagerNotificationViewerManager.Add(ex.Message, "Ошибка", TypeNotification.Error);
                    _Main.Instance._Notification.Add("", "Возникла ошибка", TypeNotification.Error);
                    _Main.Instance.Manager.Back();




                }
            });
        }


        private async void NextAcceptPacket(byte[] data)
        {
            await Application.Current.Dispatcher.Invoke(async () => {

                QueueByte.Enqueue(data);

                if (startQueueCheck == false)
                {
                    startQueueCheck = true;
                    _Main.Instance.OverlayShow(true, TypeOverlay.loading, "Загрузка данных", $"Ожидайте загрузки данных...");
                    try
                    {
                        await Task.Delay(5000, token);
                    }
                    catch
                    {
                        Logger.Warning("[Загрузка данных] - Загрузка данных c сервера завершена");
                        Logger.Warning("[Загрузка данных] - Значинаю обработку");
                    }
                    await CheckQueue();
                }
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

        private async Task CheckQueue()
        {
            await Application.Current.Dispatcher.Invoke(async () => {
                while (QueueByte.Count != 0)
                {
                    _Main.Instance.OverlayShow(true, TypeOverlay.loading, "Просмотр данных вопроса", $"Орбаботка данных: {Math.Round(CountingSizePacket(dataPakcet))} кб из {Math.Round(CountingSizePacket(size))} кб");


                    if (QueueByte.Count == 0) break;

                    int step = dataPakcet.Length;
                    var data = QueueByte.Dequeue();
                    Array.Resize(ref dataPakcet, dataPakcet.Length + data.Length);
                    Array.Copy(data, 0, dataPakcet, step, data.Length);
                    await Task.Delay(150);

                }
                await CreateQuest();
            });
        }

        private void StartSendPacket(Data_QuestDataViewer obj)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                size = obj.SizePacket;
                dataPakcet = new byte[0];
                QueueByte = new Queue<byte[]>();
                CreateToken();
            });
        }

        private void CreateToken()
        {
            cancelTokenSource = new CancellationTokenSource();
            token = cancelTokenSource.Token;
        }
    }
}
