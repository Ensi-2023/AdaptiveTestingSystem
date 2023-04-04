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

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Roly._roly_subpage.window.page
{
    /// <summary>
    /// Логика взаимодействия для GUI_page_AddNewRoly.xaml
    /// </summary>
    public partial class GUI_page_AddNewRoly : UserControl
    {
        public GUI_page_AddNewRoly()
        {
            InitializeComponent();

        }
        private void SetDefault()
        {
            ReadUserYes.IsChecked = false;
            ReadUserNo.IsChecked = true;

            ReadSotrudYes.IsChecked = false;
            ReadSotrudNo.IsChecked = true;

            ReadClassdYes.IsChecked = false;
            ReadClassdNo.IsChecked = true;

            ReadPredmetYes.IsChecked = false;
            ReadPredmetNo.IsChecked = true;

            CreateAndViewReportYes.IsChecked = false;
            CreateAndViewReportNo.IsChecked = true;

            TestYes.IsChecked = true;
            TestNo.IsChecked = false;

            CreateTestYes.IsChecked = false;
            CreateTestNo.IsChecked = true;

            CreateGroupYes.IsChecked = false;
            CreateGroupNo.IsChecked = true;

            ConnectGroupYes.IsChecked = true;
            ConnectGroupNo.IsChecked = false;

            AddSotrudForPredmetYes.IsChecked = false;
            AddSotrudForPredmetNo.IsChecked = true;

            DeleteSotrudForPredmetYes.IsChecked = false;
            DeleteSotrudForPredmetNo.IsChecked = true;
            dataBox.Text = string.Empty;
                      
            ViewUserYes.IsChecked = true;
          
            ViewUserNo.IsChecked = false;
         
            ViewPredmetYes.IsChecked = true;
            ViewPredmetNo.IsChecked = false;
                  
            ViewSotrudYes.IsChecked = false;
          
            ViewSotrudNo.IsChecked = true;

            ViewClassYes.IsChecked = false;

            ViewClassNo.IsChecked = true;


        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SetDefault();
        }

        private void default_Click(object sender, RoutedEventArgs e)
        {
            SetDefault();
        }

        private void AddNew_Click(object sender, RoutedEventArgs e)
        {
            string name = dataBox.Text.Trim();

            if (name.Trim().Length == 0)
            {
                _Main.Instance._Notification.Add("","Заполните название роли",TypeNotification.Error);
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


            if (MessageShow.Show("Добавить новую роль ? ", "Добавление", MessageShow.Type.Question) == true)
            {
                GUI_AddNewRolyUser.Instance.OverlayShow(true, TypeOverlay.loading, "Идет добавление");


                var obj = new Data_RolyInf()
                {
                     Name = name,
                     ReadUser= ReadUser,
                     ReadSotrud= ReadSotrud,
                     ReadClass= ReadClass,
                     ReadPredmet= ReadPredmet,
                     CreateAndViewReport=CreateAndViewReport,
                     TestReady= TestReady,
                     CreateTest= CreateTest,
                     CreateGroup= CreateGroup,
                     ConnectGroup= ConnectGroup,
                     AddSotrudForPredmet= AddSotrudForPredmet,
                     DeleteSotrudForPredmet= DeleteSotrudForPredmet,
                     ViewDataSotrud = viewSotrud,
                     ViewDataUser = viewUser,
                     ViewPrepmet = viewPredmet,
                     ViewClass= viewCLass,

                     IsCode = Code.Null
                };

                var command = new Data_FirstCommand()
                {
                    Command = "Command_InsertNewRoly",
                    Json = JsonSerializer.Serialize(obj)
                };

                _Main.Instance.Client.Send(JsonSerializer.Serialize(command));

            }


        }
    }
}
