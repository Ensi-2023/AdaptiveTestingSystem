using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Xml.Linq;

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Roly._roly_subpage.window.page
{
    /// <summary>
    /// Логика взаимодействия для GUI_page_UpdateRoly.xaml
    /// </summary>
    public partial class GUI_page_UpdateRoly : UserControl
    {
        private GUI_RolyInfViewer gui_RolyInfViewer;
        private int _index;

        public GUI_page_UpdateRoly(int index, GUI_RolyInfViewer gUI_RolyInfViewer)
        {
            InitializeComponent();

            gui_RolyInfViewer= gUI_RolyInfViewer;
            _index = index;
        }

        private void default_Click(object sender, RoutedEventArgs e)
        {
            SetDefault();
        }

        private void SetDefault()
        {
            ReadUserYes.IsChecked = gui_RolyInfViewer.ReadUserYes.IsChecked;
            ReadUserNo.IsChecked = gui_RolyInfViewer.ReadUserNo.IsChecked;

            ReadSotrudYes.IsChecked = gui_RolyInfViewer.ReadSotrudYes.IsChecked;
            ReadSotrudNo.IsChecked = gui_RolyInfViewer.ReadSotrudNo.IsChecked;

            ReadClassdYes.IsChecked = gui_RolyInfViewer.ReadClassdYes.IsChecked;
            ReadClassdNo.IsChecked = gui_RolyInfViewer.ReadClassdNo.IsChecked;

            ReadPredmetYes.IsChecked = gui_RolyInfViewer.ReadPredmetYes.IsChecked;
            ReadPredmetNo.IsChecked = gui_RolyInfViewer.ReadPredmetNo.IsChecked;

            CreateAndViewReportYes.IsChecked = gui_RolyInfViewer.CreateAndViewReportYes.IsChecked;
            CreateAndViewReportNo.IsChecked = gui_RolyInfViewer.CreateAndViewReportNo.IsChecked;

            TestYes.IsChecked = gui_RolyInfViewer.TestYes.IsChecked;
            TestNo.IsChecked = gui_RolyInfViewer.TestNo.IsChecked;

            CreateTestYes.IsChecked = gui_RolyInfViewer.CreateTestYes.IsChecked;
            CreateTestNo.IsChecked = gui_RolyInfViewer.CreateTestNo.IsChecked;

            CreateGroupYes.IsChecked = gui_RolyInfViewer.CreateGroupYes.IsChecked;
            CreateGroupNo.IsChecked = gui_RolyInfViewer.CreateGroupNo.IsChecked;

            ConnectGroupYes.IsChecked = gui_RolyInfViewer.ConnectGroupYes.IsChecked;
            ConnectGroupNo.IsChecked = gui_RolyInfViewer.ConnectGroupNo.IsChecked;

            AddSotrudForPredmetYes.IsChecked = gui_RolyInfViewer.AddSotrudForPredmetYes.IsChecked;
            AddSotrudForPredmetNo.IsChecked = gui_RolyInfViewer.AddSotrudForPredmetNo.IsChecked;

            DeleteSotrudForPredmetYes.IsChecked = gui_RolyInfViewer.DeleteSotrudForPredmetYes.IsChecked;
            DeleteSotrudForPredmetNo.IsChecked = gui_RolyInfViewer.DeleteSotrudForPredmetNo.IsChecked;



            ViewUserYes.IsChecked = gui_RolyInfViewer.ViewUserYes.IsChecked;
            ViewUserNo.IsChecked = gui_RolyInfViewer.ViewUserNo.IsChecked;


            ViewPredmetYes.IsChecked = gui_RolyInfViewer.ViewPredmetYes.IsChecked;
            ViewPredmetNo.IsChecked = gui_RolyInfViewer.ViewPredmetNo.IsChecked;


            ViewSotrudYes.IsChecked = gui_RolyInfViewer.ViewSotrudYes.IsChecked;
            ViewSotrudNo.IsChecked = gui_RolyInfViewer.ViewSotrudNo.IsChecked;



            ViewClassYes.IsChecked = gui_RolyInfViewer.ViewClassYes.IsChecked;
            ViewClassNo.IsChecked = gui_RolyInfViewer.ViewClassNo.IsChecked;


            dataBox.Text = gui_RolyInfViewer.rolyName.Text;


            bool flag = false;
            switch (dataBox.Text.Trim().ToLower())
            {
                case "пользователь": flag = true; break;
                case "системный администратор": flag = true; break;
                case "учитель": flag = true; break;
            }

            if(flag==true) dataBox.IsEnabled= false;


        }
        public void SaveData()
        {
            gui_RolyInfViewer.ReadUserYes.IsChecked = ReadUserYes.IsChecked;
            gui_RolyInfViewer.ReadUserNo.IsChecked = ReadUserNo.IsChecked;

            gui_RolyInfViewer.ReadSotrudYes.IsChecked = ReadSotrudYes.IsChecked;
            gui_RolyInfViewer.ReadSotrudNo.IsChecked = ReadSotrudNo.IsChecked;

            gui_RolyInfViewer.ReadClassdYes.IsChecked = ReadClassdYes.IsChecked;
            gui_RolyInfViewer.ReadClassdNo.IsChecked = ReadClassdNo.IsChecked;

            gui_RolyInfViewer.ReadPredmetYes.IsChecked = ReadPredmetYes.IsChecked;
            gui_RolyInfViewer.ReadPredmetNo.IsChecked = ReadPredmetNo.IsChecked;

            gui_RolyInfViewer.CreateAndViewReportYes.IsChecked = CreateAndViewReportYes.IsChecked;
            gui_RolyInfViewer.CreateAndViewReportNo.IsChecked = CreateAndViewReportNo.IsChecked;

            gui_RolyInfViewer.TestYes.IsChecked = TestYes.IsChecked;
            gui_RolyInfViewer.TestNo.IsChecked = TestNo.IsChecked;

            gui_RolyInfViewer.CreateTestYes.IsChecked = CreateTestYes.IsChecked;
            gui_RolyInfViewer.CreateTestNo.IsChecked = CreateTestNo.IsChecked;

            gui_RolyInfViewer.CreateGroupYes.IsChecked = CreateGroupYes.IsChecked;
            gui_RolyInfViewer.CreateGroupNo.IsChecked = CreateGroupNo.IsChecked;

            gui_RolyInfViewer.ConnectGroupYes.IsChecked = ConnectGroupYes.IsChecked;
            gui_RolyInfViewer.ConnectGroupNo.IsChecked = ConnectGroupNo.IsChecked;

            gui_RolyInfViewer.AddSotrudForPredmetYes.IsChecked = AddSotrudForPredmetYes.IsChecked;
            gui_RolyInfViewer.AddSotrudForPredmetNo.IsChecked = AddSotrudForPredmetNo.IsChecked;

            gui_RolyInfViewer.DeleteSotrudForPredmetYes.IsChecked = DeleteSotrudForPredmetYes.IsChecked;
            gui_RolyInfViewer.DeleteSotrudForPredmetNo.IsChecked = DeleteSotrudForPredmetNo.IsChecked;
            gui_RolyInfViewer.rolyName.Text = dataBox.Text;



            gui_RolyInfViewer.ViewUserYes.IsChecked = ViewUserYes.IsChecked;
            gui_RolyInfViewer.ViewUserNo.IsChecked = ViewUserNo.IsChecked;


            gui_RolyInfViewer.ViewPredmetYes.IsChecked = ViewPredmetYes.IsChecked;
            gui_RolyInfViewer.ViewPredmetNo.IsChecked = ViewPredmetNo.IsChecked;


            gui_RolyInfViewer.ViewSotrudYes.IsChecked = ViewSotrudYes.IsChecked;
            gui_RolyInfViewer.ViewSotrudNo.IsChecked = ViewSotrudNo.IsChecked;



            gui_RolyInfViewer.ViewClassYes.IsChecked = ViewClassYes.IsChecked;
            gui_RolyInfViewer.ViewClassNo.IsChecked = ViewClassNo.IsChecked;


        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SetDefault();
        }

        private void AddNew_Click(object sender, RoutedEventArgs e)
        {
            string name = dataBox.Text.Trim();

            if (name.Trim().Length == 0)
            {
                _Main.Instance._Notification.Add("", "Заполните название роли", TypeNotification.Error);
                return;
            }

            bool ReadUser = ReadUserYes.IsChecked == true ? true : false;
            bool ReadSotrud = ReadSotrudYes.IsChecked == true ? true : false;
            bool ReadClass = ReadClassdYes.IsChecked == true ? true : false;
            bool ReadPredmet = ReadPredmetYes.IsChecked == true ? true : false;
            bool CreateAndViewReport = CreateAndViewReportYes.IsChecked == true ? true : false;
            bool TestReady = TestYes.IsChecked == true ? true : false;
            bool CreateTest = CreateTestYes.IsChecked == true ? true : false;
            bool CreateGroup = CreateGroupYes.IsChecked == true ? true : false;
            bool ConnectGroup = ConnectGroupYes.IsChecked == true ? true : false;
            bool AddSotrudForPredmet = AddSotrudForPredmetYes.IsChecked == true ? true : false;
            bool DeleteSotrudForPredmet = DeleteSotrudForPredmetYes.IsChecked == true ? true : false;

            bool viewUser = ViewUserYes.IsChecked == true ? true : false;
            bool viewPredmet = ViewPredmetYes.IsChecked == true ? true : false;
            bool viewSotrud = ViewSotrudYes.IsChecked == true ? true : false;
            bool viewCLass = ViewClassYes.IsChecked == true ? true : false;


            if (MessageShow.Show("Изменить данные роли ? ", "Редактирование", MessageShow.Type.Question) == true)
            {
              
                var obj = new Data_RolyInf()
                {
                    Name = name,
                    ReadUser = ReadUser,
                    ReadSotrud = ReadSotrud,
                    ReadClass = ReadClass,
                    ReadPredmet = ReadPredmet,
                    CreateAndViewReport = CreateAndViewReport,
                    TestReady = TestReady,
                    CreateTest = CreateTest,
                    CreateGroup = CreateGroup,
                    ConnectGroup = ConnectGroup,
                    AddSotrudForPredmet = AddSotrudForPredmet,
                    DeleteSotrudForPredmet = DeleteSotrudForPredmet,
                    ViewDataSotrud = viewSotrud,
                    ViewDataUser = viewUser,
                    ViewPrepmet = viewPredmet,
                    ViewClass = viewCLass,
                    IsCode = Code.Null,
                    Index = _index
                };

                var command = new Data_FirstCommand()
                {
                    Command = "Command_UpdateDataUserRoly",
                    Json = JsonSerializer.Serialize(obj)
                };

                _Main.Instance.Client.Send(JsonSerializer.Serialize(command));

            }
        }
    }
}
