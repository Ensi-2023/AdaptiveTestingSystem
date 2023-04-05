using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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


namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports._gui_subpage
{
    /// <summary>
    /// Логика взаимодействия для GUI_Report_Page1.xaml
    /// </summary>
    public partial class GUI_Report_Page1 : UserControl
    {
        public bool IsCancelUpload { get; private set; }

        public bool IsNoMouseScroll { get; set; }

        public GUI_Report_Page1()
        {
            InitializeComponent();
        }

        public void SetData(Data_StatisticGeneral data)
        {
            _Main.Instance.IsEnabled = true;
            Body.Visibility= Visibility.Visible;
            overlay.Visibility = Visibility.Collapsed;
            allTestData.SetData(data.AllScoreTests);
            allClassRoomData.SetData(data.ClassroomScore_generals);
            fiveClassRoom.SetData(data.AverageScores5ClassRoom);
            subject_3.SetData(data.MostTested3Subjects);
            lineDiagramOneUser.SetData(data.OneMostActiveUser);
            lineDiagramMultyUser.SetData(data.MostActiveUsers);
            ThreadManager.Clear();
        }

        private void root_Loaded(object sender, RoutedEventArgs e)
        {
            SendToServer();
        }

        private void root_Unloaded(object sender, RoutedEventArgs e)
        {
            ThreadManager.CloseActiveThread();
        }


        public void SendInfo(double size, double maxsize)
        {
            if (size != 0)
            {
                double c = maxsize / size;
                double perc = 100 / c;
                indicator.IsIndeterminate = false;
                indicator.Value = perc;
            }
            percUpload.Visibility = Visibility.Visible;

 

        }

        private async void SendToServer()
        {
            IsCancelUpload = false;
            indicator.Visibility = Visibility.Visible;
            Body.Visibility = Visibility.Collapsed;
            retryUpload.Visibility = Visibility.Collapsed;
            overlay.Visibility = Visibility.Visible;

            indicator.Value = 0;

            await Task.Delay(250);


            var packet = new Data_StatisticPacket()
            {
                IsCode = Code.ThreadStart
            };

            ThreadManager.Send("Command_StatisticGeneral", packet);
        }

        private void percUpload_MouseEnter(object sender, MouseEventArgs e)
        {
            percUpload.Visibility= Visibility.Collapsed;
            buttonCloseThread.Visibility= Visibility.Visible;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            if (IsCancelUpload) return;
            percUpload.Visibility = Visibility.Visible;
            buttonCloseThread.Visibility = Visibility.Collapsed;
        }

        private void buttonCloseThread_Click(object sender, RoutedEventArgs e)
        {
            SetError();
        }

        private void retryUpload_Click(object sender, RoutedEventArgs e)
        {
            SendToServer();
        }

        public async void SetError()
        {
            _Main.Instance.IsEnabled = true;

            indicator.Visibility = Visibility.Collapsed;
            percUpload.Visibility = Visibility.Collapsed;
            buttonCloseThread.Visibility = Visibility.Collapsed;
            IsCancelUpload = true;

            await Task.Delay(250);

            ThreadManager.CloseActiveThread();

            await Task.Delay(250);

            retryUpload.Visibility = Visibility.Visible;   
        }

        private void GUI_MostActiveOneUser_MouseEnter(object sender, MouseEventArgs e)
        {
            IsNoMouseScroll = true;

        }

        private void GUI_MostActiveOneUser_MouseLeave(object sender, MouseEventArgs e)
        {
            IsNoMouseScroll = false;
        }       
    }
}
