using AdaptiveTestingSystem.Data.JsonData;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.ClassRoom._classRoom_page._classRoom_subPage._classRoom_subPage_control;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage._testing_gui;
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
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;


namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing
{
    /// <summary>
    /// Логика взаимодействия для GUI_TestReady.xaml
    /// </summary>
    public partial class GUI_TestReady : Window
    {
        static public GUI_TestReady Instance;

        Data_MultyServerClient data_MultyServer;

        public bool IsOnline { get; private set; }
        private int IndexTest { get; set; }
        private string NameTest { get; set; }

        public string GUID { get; set; }
        public int CountQuestTest { get; set; }

        private bool IsOffline { get; set; }
        public GUI_TestReady()
        {
            InitializeComponent(); 
            Instance = this;
        }

        public GUI_TestReady(Data_MultyServerClient serverClient)
        {
            InitializeComponent();
            Instance = this;
            IsOnline = true;
            data_MultyServer = serverClient;




    
        }

        public GUI_TestReady(Data_Testing test)
        {
            InitializeComponent();
            Instance = this;
            IsOffline = true;
            SetUI(new GUI_TestingRun(test));
        }

        public void SetData(string nameTest, int indexTest)
        {
            IndexTest = indexTest;
            NameTest = nameTest;

        }

        public void SetData(string nameTest, int indexTest, int countQuest)
        {
            IndexTest = indexTest;
            NameTest = nameTest;
            CountQuestTest = countQuest;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsOffline) return;
            if (IsOnline)
            {
                SetUI(new GUI_TestingRun(data_MultyServer));
                return;
            }
            SetUI(new _testing_subpage._testing_gui.GUI_TestingUserMeny(IndexTest, NameTest, CountQuestTest));
        }

        public void SetUI(UIElement element)
        {
            body.Children.Clear();
            body.Children.Add(element);
        }

        public bool SetInfo(double size, double maxsize)
        {
            var userViewer = body.Children[0] as GUI_TestingRun;
            if (userViewer == null) return false;
            userViewer.SendInfo(size, maxsize);
            if(_Main.Instance.UI_TestReady!=null) return true; else return false;
        }

        public void SetInfo(bool isind = false,Visibility visibilityTitle = Visibility.Visible)
        {
            var userViewer = body.Children[0] as GUI_TestingRun;
            if (userViewer == null) return;
            userViewer.SendInfo(isind, visibilityTitle);
   
        }

        internal void SetTest(Data_Testing obj)
        {
            var userViewer = body.Children[0] as GUI_TestingRun;
            if (userViewer == null) return;
            userViewer.SendData(obj);
        }

        public void SetError()
        {
            var userViewer = body.Children[0] as GUI_TestingRun;
            if (userViewer == null) return;
            userViewer.SetError();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _Main.Instance.Show();
            _Main.Instance.UI_TestReady = null;


            if (IsOnline)
            {
                if (data_MultyServer != null)
                {

                    Data_ConnectTestingServer packet = new Data_ConnectTestingServer()
                    {
                        IndexServer = data_MultyServer.IndexServer
                    };

                    Data_FirstCommand data = new Data_FirstCommand()
                    {
                        Command = "Command_DisconnectUserForActiveTestServer",
                        Json = JsonSerializer.Serialize(packet)
                    };

                    _Main.Instance.Client.Send(JsonSerializer.Serialize(data));
                }
            }


            var userViewer = body.Children[0] as GUI_TestingRun;
            if (userViewer == null) return;
            if (userViewer.IsUpload)
            {
                Data_TestingView view = new Data_TestingView()
                {
                    IsCode = Code.ThreadEnd
                };

                Data_FirstCommand data = new Data_FirstCommand()
                {
                    Command = "Command_ViewTestingData",
                    Json = JsonSerializer.Serialize(view)
                };

                _Main.Instance.Client.Send(JsonSerializer.Serialize(data));
                Logger.Debug("Send cancel Upload");
            }

            

        }
    }
}
