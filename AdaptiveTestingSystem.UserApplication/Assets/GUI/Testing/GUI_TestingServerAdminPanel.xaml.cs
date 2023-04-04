using AdaptiveTestingSystem.Control.Windows;
using AdaptiveTestingSystem.Data.JsonData;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage._custom_component;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage._testing_mini_mvvm;
using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing
{
    /// <summary>
    /// Логика взаимодействия для GUI_TestingServerAdminPanel.xaml
    /// </summary>
    /// 


 
    public partial class GUI_TestingServerAdminPanel : UserControl
    {



        public List<Data_MultyServerSendAnswer> data_MultyServerSendAnswers { get; set; }

        static Data_Testing data_Testing { get; set; } = null;
        VM_UserTestingViewer mvvm_UserViewer;

        private Data_ListMultyServer MultyServer { get; set; }
        public bool IsCompiled { get; private set; }

        public bool IsRun { get; set; }

        public GUI_TestingServerAdminPanel(List<Data_ListMultyServer> multyServer)
        {
            InitializeComponent();
            MultyServer = multyServer[0];


            indentificator.Text = MultyServer.IndexServer;
            ServerPassword.Password = MultyServer.Password;

            mvvm_UserViewer = new VM_UserTestingViewer();
            DataContext = mvvm_UserViewer;
        }

        private void searchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var text = ((TextBox)sender).Text;
                mvvm_UserViewer.Search(text);
            }
        }

        private void Row_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Ensure row was clicked and not empty space

            var row = DataGridHelper.GetRow(sender, e);
            if (row == null) return;


            var select = TestingGrid.SelectedItem as MV_UserTesting;
            if (select != null)
            {
                body.Children.Clear();

                body.Visibility= Visibility.Visible;
                Log.Visibility= Visibility.Collapsed;
                var list = data_MultyServerSendAnswers.FindAll(o => o.GUID.Equals(select.GUID));
                body.Children.Add(new ViewUserDataTest(select.GUID, data_Testing, list,this));
            }

        }



        private void DataGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Ensure row was clicked and not empty space
            var row = DataGridHelper.GetRow(sender, e);
            if (row == null) return;

            if (row.IsSelected) row.IsSelected = false;
        }


        internal void Connect(Data_MultyServerClient obj)
        {
            mvvm_UserViewer.AddOneUser(obj);

          //  SendMessage($"({obj.GUID}) {mvvm_UserViewer.GetNameuser(obj.GUID)} Подключился");
        }

        internal async void Connect(List<Data_MultyServerClient> obj)
        {
            await mvvm_UserViewer.SetData(obj);
        }

        internal void DesconnectUser(Data_MultyServerClient obj)
        {
            mvvm_UserViewer.RemoveUser(obj);
          //  SendMessage($"({obj.GUID}) {mvvm_UserViewer.GetNameuser(obj.GUID)} Отключился");
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (data_Testing == null) SendServerForEditServer(MultyServer.IndexTest);
            else
            {
                UploadUser();
            }
        }

        private void UploadUser()
        {
            Data_GiveAllUserInTestServer userInTestServer = new Data_GiveAllUserInTestServer()
            {
                IndexServer = MultyServer.IndexServer
            };

            Data_FirstCommand data = new Data_FirstCommand()
            {
                Command = "Command_GiveAllUserinActiveTestServerThisIndex",
                Json = JsonSerializer.Serialize(userInTestServer)
            };
            _Main.Instance.Client.Send(JsonSerializer.Serialize(data));
        }

        private async void SendServerForEditServer(int index)
        {
            _Main.Instance.OverlayShow(true, TypeOverlay.loading);

            await Task.Delay(500);

            Data_TestingView view = new Data_TestingView()
            {
                Index = index,
            };

            ThreadManager.Send("Command_ViewTestingData", view);

        }

        public void SetStatusUser(Data_MultyServerClient obj)
        {
             mvvm_UserViewer.UpdateStatusUser(obj);
        }

        internal async void SetTest(Data_Testing obj)
        {

            _Main.Instance.OverlayShow(false);

            Data_ConnectTestingServer userInTestServer = new Data_ConnectTestingServer()
            {
                IndexServer = MultyServer.IndexServer
            };

            Data_FirstCommand data = new Data_FirstCommand()
            {
                Command = "Command_AllowConnectingToTesetServer",
                Json = JsonSerializer.Serialize(userInTestServer)
            };
            _Main.Instance.Client.Send(JsonSerializer.Serialize(data));

            await Task.Delay(150);

            data_Testing = obj;
          

            answerrControl.Children.Clear();
            UploadUser(); 
        }

        public void SetAnswerUser(Data_MultyServerSendAnswer send)
        {
           

            data_MultyServerSendAnswers.Add(new Data_MultyServerSendAnswer() {
                GUID= send.GUID,
                IndexQuest = send.IndexQuest,
                NumberAnswer= send.NumberAnswer,
                NumberCorrect= send.NumberCorrect,
                resultTesting = send.resultTesting,
                CountQuest= send.CountQuest,
            });
            LogAnswerControl control = new LogAnswerControl()
            {
                Index = send.IndexQuest,
                NameUser = mvvm_UserViewer.GetNameuser(send.GUID),
                IsCorrect= (send.NumberAnswer == send.NumberCorrect ? true : false),
                CorrectNumber = send.NumberCorrect.ToString(),
                AnswerNumber = send.NumberAnswer.ToString(),
                HorizontalAlignment= HorizontalAlignment.Stretch,
                AVGScore = send.resultTesting.avg.ToString()
            };

            answerrControl.Children.Add(control);
        }

        private async void startTesting_Click(object sender, RoutedEventArgs e)
        {

            var start = await CheckallUpload();

            if (start.Item1)
            {

                Data_ConnectTestingServer userInTestServer = new Data_ConnectTestingServer()
                {
                    IndexServer = MultyServer.IndexServer
                };

                Data_FirstCommand data = new Data_FirstCommand()
                {
                    Command = "Command_StartTestingUser",
                    Json = JsonSerializer.Serialize(userInTestServer)
                };
                _Main.Instance.Client.Send(JsonSerializer.Serialize(data));

              
            }
            else
            {
                _Main.Instance._Notification.Add("",$"{start.Item2}",TypeNotification.Error);
            }
        }

        private async Task<(bool,string)> CheckallUpload()
        {
            if (TestingGrid.Items.Count == 0) return (false, "Нет пользователей");
            for (int i = 0; i < TestingGrid.Items.Count; i++)
            {
                var item = TestingGrid.Items[i] as MV_UserTesting;
                if (item != null)
                {
                    if (item.IsCode != Code.UploadSuccessfull) return (false,"Не все пользователи загрузили данные");
                }

                await Task.Delay(20);
            }

            return (true, "");
        }

        internal void StartTest()
        {
            //  SendMessage("Тестирование запущено");
            IsCompiled = false;
            startTesting.Visibility = Visibility.Collapsed;
            Log.Visibility = Visibility.Visible;
            data_MultyServerSendAnswers = new List<Data_MultyServerSendAnswer>();
            IsRun = true;
            Task.Factory.StartNew(async() => { await CheckCompiledTest(); });
        }

        private async Task CheckCompiledTest()
        {
            await Application.Current.Dispatcher.Invoke(async () => 
            {
                while (IsRun)
                {

                    if (IsRun == false) return;

                    List<string> ListCompiled = new List<string>();
                    int count = 0;

                    foreach (var item in TestingGrid.Items)
                    {
                        if (TestingGrid.Items.Count == 0) return;                        
                        var check = item as MV_UserTesting;
                        if (check == null) continue;


                        Logger.Debug($"{check.IsCode}");
                        if (check.IsCode == Code.TestingCompleted)
                        {
                            var guid = ListCompiled.Find(o => o.Equals(check.GUID));
                            if (guid == null)
                            {
                                ListCompiled.Add(check.GUID);
                                count++;
                            }
                        }

                    }

                    if (count == mvvm_UserViewer.UserCollectionViewer.Count) 
                    {
                        TestCompiled();  
                        return;
                    }
                    await Task.Delay(1000);
                }
                
            });
        }

        private void TestCompiled()
        {
            Logger.Debug("Test compiled");

            IsCompiled = true;

            Data_ConnectTestingServer userInTestServer = new Data_ConnectTestingServer()
            {
                IndexServer = MultyServer.IndexServer
            };

            Data_FirstCommand data = new Data_FirstCommand()
            {
                Command = "Command_CloseActiveTestingServerThisIndex",
                Json = JsonSerializer.Serialize(userInTestServer)
            };
            _Main.Instance.Client.Send(JsonSerializer.Serialize(data));
        }

 

        private void deleteTest_Click(object sender, RoutedEventArgs e)
        {
            if (MessageShow.Show("Закрыть сервер ? Он будет удален.","",MessageShow.Type.Question) == true)
            {
                DeleteServer();
            }
        }

        private void DeleteServer()
        {
            Data_ConnectTestingServer userInTestServer = new Data_ConnectTestingServer()
            {
                IndexServer = MultyServer.IndexServer
            };

            Data_FirstCommand data = new Data_FirstCommand()
            {
                Command = "Command_CloseActiveTestingServerThisIndex",
                Json = JsonSerializer.Serialize(userInTestServer)
            };
            data_Testing = null;
            IsRun = false;
            IsCompiled = false;
            _Main.Instance.Client.Send(JsonSerializer.Serialize(data));
            _Main.Instance.Manager.Home();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scrollviewer = sender as ScrollViewer;
            if (e.Delta > 0)
                scrollviewer.LineUp();
            else
                scrollviewer.LineDown();
        }

        internal void Back()
        {
            body.Visibility = Visibility.Collapsed;
            body.Children.Clear();
            Log.Visibility = Visibility.Visible;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            ThreadManager.CloseActiveThread();

            if (IsRun == true)
            {
                DeleteServer();
                return;
            }

            if (IsCompiled) { data_Testing = null; IsRun = false; }


        }

        private void UserPAssword_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
    }
}
