using AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage.window;
using AdaptiveTestingSystem.UserApplication.Assets.MVVM.Model;
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

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Meny
{
    /// <summary>
    /// Логика взаимодействия для GUI_MainMeny.xaml
    /// </summary>
    public partial class GUI_MainMeny : UserControl
    {
        public GUI_MainMeny()
        {
            InitializeComponent();

            AccessRights();

        }

        private void AccessRights()
        {
            if (_Main.Instance.MyAccount.AllRoly) 
                return;
            else
                roly.Visibility= Visibility.Collapsed;


            dbTitleBorder.Visibility= Visibility.Collapsed;
            dbTitleInfo.Visibility= Visibility.Collapsed;      
            testingBorder.Visibility= Visibility.Collapsed;
            testingTitle.Visibility= Visibility.Collapsed;

            if (_Main.Instance.MyAccount.ViewClass ||
                _Main.Instance.MyAccount.ViewPrepmet ||
                _Main.Instance.MyAccount.ViewDataSotrud ||
                _Main.Instance.MyAccount.ViewDataUser)
            {
                dbTitleBorder.Visibility = Visibility.Visible;
                dbTitleInfo.Visibility = Visibility.Visible;
            }


            if (_Main.Instance.MyAccount.ViewDataUser)
            {
                BUser.Visibility = Visibility.Visible;
                customuUser.Visibility = Visibility.Visible;
            }
            else
            {
                BUser.Visibility = Visibility.Collapsed;
                customuUser.Visibility = Visibility.Collapsed;
            }

            if (_Main.Instance.MyAccount.ViewDataSotrud)
            {
                uUser.Visibility= Visibility.Visible;
            }
            else
            {
                uUser.Visibility = Visibility.Collapsed;
            }
            
            if (_Main.Instance.MyAccount.ViewClass)
            {
                classRoom.Visibility= Visibility.Visible;
            }
            else
            {
                classRoom.Visibility = Visibility.Collapsed;
            }   
            
            if (_Main.Instance.MyAccount.ViewPrepmet)
            {
                subject.Visibility= Visibility.Visible;
            }
            else
            {
                subject.Visibility = Visibility.Collapsed;
            }

            if (_Main.Instance.MyAccount.CreateAndViewReport)
            {
                Report.Visibility= Visibility.Visible;
            }
            else
            {
                Report.Visibility = Visibility.Collapsed;
            }



            if (_Main.Instance.MyAccount.TestReady ||
                _Main.Instance.MyAccount.ConnectGroup ||
                _Main.Instance.MyAccount.CreateGroup ||
                _Main.Instance.MyAccount.CreateTest ||
                _Main.Instance.MyAccount.CreateAndViewReport)
            {
                testingBorder.Visibility = Visibility.Visible;
                testingTitle.Visibility = Visibility.Visible;
            }

            if (_Main.Instance.MyAccount.TestReady)
            {
                testingStart.Visibility = Visibility.Visible;
                testingRandom.Visibility = Visibility.Visible;
            }
            else
            {
                testingStart.Visibility = Visibility.Collapsed;
                testingRandom.Visibility = Visibility.Collapsed;
            }


            if (_Main.Instance.MyAccount.ConnectGroup)
            {
                fastConnectToGroupTest.Visibility = Visibility.Visible;
                viewGroupTest.Visibility = Visibility.Visible;
            }
            else
            {
                fastConnectToGroupTest.Visibility = Visibility.Collapsed;
                viewGroupTest.Visibility = Visibility.Collapsed;
            }

            if(_Main.Instance.MyAccount.CreateGroup)
                createGroupTest.Visibility= Visibility.Visible;
            else
                createGroupTest.Visibility=Visibility.Collapsed;

            if(_Main.Instance.MyAccount.CreateTest)
                viewAllTest.Visibility= Visibility.Visible;
            else
                viewAllTest.Visibility=Visibility.Collapsed;
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scrollviewer = sender as ScrollViewer;
            if (e.Delta > 0)
                scrollviewer.LineUp();
            else
                scrollviewer.LineDown();
        }

      
        private void BUser_Click(object sender, RoutedEventArgs e)
        {
            _Main.Instance.Manager.Next(new GUI.Users.GUI_Users(Code.GUI_User));
        }

        private void uUser_Click(object sender, RoutedEventArgs e)
        {
            _Main.Instance.Manager.Next(new GUI.Users.GUI_Users(Code.GUI_Staff));
        }

        private void customuUser_Click(object sender, RoutedEventArgs e)
        {
            _Main.Instance.Manager.Next(new GUI.Users.GUI_Users(Code.GUI_UserModify));
        }

        private void classRoom_Click(object sender, RoutedEventArgs e)
        {
            _Main.Instance.Manager.Next(new GUI.ClassRoom.GUI_ClassRoom());
        }

        private void subject_Click(object sender, RoutedEventArgs e)
        {
            _Main.Instance.Manager.Next(new GUI.Subject.GUI_Subject());
        }

        private void roly_Click(object sender, RoutedEventArgs e)
        {
            _Main.Instance.Manager.Next(new GUI.Roly.GUI_Roly());
        }

        private void settingSoft_Click(object sender, RoutedEventArgs e)
        {
            _Main.Instance.Manager.Next(new GUI.Setting.GUI_Setting());
        }

        private void viewAllTest_Click(object sender, RoutedEventArgs e)
        {
            _Main.Instance.Manager.Next(new GUI.Testing.GUI_AllTestViewer());
        }

        private void testingStart_Click(object sender, RoutedEventArgs e)
        {
            _Main.Instance.Manager.Next(new GUI.Testing.GUI_TakeTheTest()); 
        }

        private void testingRandom_Click(object sender, RoutedEventArgs e)
        {
            _Main.Instance.UI_TestReady = new GUI_TestReady();
            _Main.Instance.UI_TestReady.Uid = (Guid.NewGuid().ToString());


            if (_Main.Instance.MVVM_Manager.TestingModel != null)
            {
                var search = _Main.Instance.MVVM_Manager.TestingModel.ReturnRandomNameAndIndexTest();

                if (search.Item1.Trim() == string.Empty && search.Item2 == 0)
                {
                    Data_FirstCommand data = new Data_FirstCommand()
                    {
                        Command = "Command_ReturnRandomNameAndIndexTest",
                        Json = ""
                    };

                    _Main.Instance.Client.Send(JsonSerializer.Serialize(data));

                    Logger.Debug($"DB");
                }
                else
                {

                    Logger.Debug($"Application: {search.Item1} {search.Item2} ");
                    _Main.Instance.UI_TestReady.SetData(search.Item1, search.Item2);
                    _Main.Instance.Hide();
                    _Main.Instance.UI_TestReady.ShowDialog();
                }
            }


        }

        private void viewGroupTest_Click(object sender, RoutedEventArgs e)
        {
            _Main.Instance.Manager.Next(new GUI.Testing.GUI_TestingServerBrowser()); 
        }

        private void createGroupTest_Click(object sender, RoutedEventArgs e)
        {
             _Main.Instance.Manager.Next(new GUI.Testing.GUI_MultyTestingServerCreate());
        }

        private void fastConnectToGroupTest_Click(object sender, RoutedEventArgs e)
        {
            var wind = new GUI_FastConnect();
            wind.ShowDialog();
        }

        private void Report_Click(object sender, RoutedEventArgs e)
        {
            _Main.Instance.Manager.Next(new GUI.Reports.GUI_Statistic()); 
        }

      
    }
}
