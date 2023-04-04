using AdaptiveTestingSystem.Data.JsonData;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Roly._roly_mini_mvvm;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Roly._roly_subpage.window;
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

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Roly._roly_subpage
{
    /// <summary>
    /// Логика взаимодействия для GUI_RolyInfViewer.xaml
    /// </summary>
    public partial class GUI_RolyInfViewer : UserControl
    {
        MV_User_Roly mV_User_Roly;

        private int _index { get; set; }
        public GUI_RolyInfViewer(int indexRoly,string name)
        {
            InitializeComponent();
            mV_User_Roly = new MV_User_Roly();

            _index = indexRoly;
            rolyName.Text = name;

            DataContext = mV_User_Roly;
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var text = ((TextBox)sender).Text;
                mV_User_Roly.Search(text);
            }
        }

        private void Row_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Ensure row was clicked and not empty space

            var row = DataGridHelper.GetRow(sender, e);
            if (row == null) return;
            mV_User_Roly.ViewInformation(row.Item);
        }



        private void DataGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Ensure row was clicked and not empty space
            var row = DataGridHelper.GetRow(sender, e);
            if (row == null) return;

            if (row.IsSelected) row.IsSelected = false;
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private async void Update()
        {
            _Main.Instance.OverlayShow(true, TypeOverlay.loading, title: "Загрузка данных роли..");

            await Task.Delay(250);

            Data_Roly data = new Data_Roly()
            {
                Index = _index,
                IsCode = Code.ThreadStart
            };

            ThreadManager.Send("Command_GetUserInRoly",data);

   
        }

        internal async void SetDataPacket(Data_RPacket obj)
        {                      
            GetDataScope(obj.RolyData);

            if(mV_User_Roly!=null) await mV_User_Roly.SetData(obj.Users);
        }


        public void GetDataScope(Data_RolyInf obj)
        {
        
            if (obj.ReadUser)
            {
                ReadUserYes.IsChecked = true;
            }
            else ReadUserNo.IsChecked = true;

            if (obj.ReadSotrud)
            {
                ReadSotrudYes.IsChecked = true;
            }
            else ReadSotrudNo.IsChecked = true;

            if (obj.ReadClass)
            {
                ReadClassdYes.IsChecked = true;
            }
            else ReadClassdNo.IsChecked = true;

            if (obj.ReadPredmet)
            {
                ReadPredmetYes.IsChecked = true;
            }
            else ReadPredmetNo.IsChecked = true;
            if (obj.CreateAndViewReport)
            {
                CreateAndViewReportYes.IsChecked = true;
            }
            else CreateAndViewReportNo.IsChecked = true;
            if (obj.TestReady)
            {
                TestYes.IsChecked = true;
            }
            else TestNo.IsChecked = true;
            if (obj.CreateTest)
            {
                CreateTestYes.IsChecked = true;
            }
            else CreateTestNo.IsChecked = true;
            if (obj.CreateGroup)
            {
                CreateGroupYes.IsChecked = true;
            }
            else CreateGroupNo.IsChecked = true;
            if (obj.ConnectGroup)
            {
                ConnectGroupYes.IsChecked = true;
            }
            else ConnectGroupNo.IsChecked = true;




            if (obj.AddSotrudForPredmet)
            {
                AddSotrudForPredmetYes.IsChecked = true;
            }
            else AddSotrudForPredmetNo.IsChecked = true;

            if (obj.DeleteSotrudForPredmet)
            {
                DeleteSotrudForPredmetYes.IsChecked = true;
            }
            else DeleteSotrudForPredmetNo.IsChecked = true;



            if(obj.ViewDataUser) 
                ViewUserYes.IsChecked=true;
            else
                ViewUserNo.IsChecked=true;

            if(obj.ViewPrepmet)
                ViewPredmetYes.IsChecked=true;
            else
                ViewPredmetNo.IsChecked=true;

            if(obj.ViewDataSotrud)
                ViewSotrudYes.IsChecked=true;
            else
                ViewSotrudNo.IsChecked=true;


            if (obj.ViewClass)
                ViewClassYes.IsChecked = true;
            else
                ViewClassNo.IsChecked = true;
        }

        private async void UserGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var obj = sender as DataGrid;
            if (obj.SelectedItems.Count > 0)
            {

                commandButtonPanel.Visibility = Visibility.Visible;
                Animation.AnimatedOpacity(commandButtonPanel, commandButtonPanel.Opacity, 1, TimeSpan.FromMilliseconds(150));
            }
            else
            {
                Animation.AnimatedOpacity(commandButtonPanel, commandButtonPanel.Opacity, 0, TimeSpan.FromMilliseconds(150));
                await Task.Delay(170);
                commandButtonPanel.Visibility = Visibility.Collapsed;

            }
        }

        private void selectionClear_Click(object sender, RoutedEventArgs e)
        {
            UserGrid.UnselectAll();
        }

        private void rolyChanger_Click(object sender, RoutedEventArgs e)
        {
            if (UserGrid.SelectedItems.Count > 0)
            {
                var list = UserGrid.SelectedItems;

                var wind = new GUI_RolyChanger(list, _index);
                if (wind.ShowDialog() == true)
                {
                    Update();
                }
            }
        }

        private async void deleteRoly_Click(object sender, RoutedEventArgs e)
        {
            string name = rolyName.Text.Trim().ToLower();
            bool flag = false;
            switch (name)
            {
                case "пользователь": flag = true; break;
                case "системный администратор": flag = true; break;
                case "учитель": flag = true; break;
            }

            if (flag)
            {
                MessageShow.Show($"Не возможно удалить системную роль: {rolyName.Text}","Ошибка",MessageShow.Type.Error);
                return;
            }


            if (MessageShow.Show($"Удалить роль: {rolyName.Text}", "Удаление", MessageShow.Type.Question) == true)
            {
                _Main.Instance.OverlayShow(true);
                var delete = new Data_Roly()
                {
                    Index = _index,
                };

                var packet = new Data_FirstCommand()
                {
                    Command = "Command_DeleteRoly",
                    Json = JsonSerializer.Serialize(delete)
                };

                _Main.Instance.Client.Send(JsonSerializer.Serialize(packet));
                await Task.Delay(100);
                _Main.Instance.OverlayShow(false);
                _Main.Instance.Manager.Back();
            }

        }

        private void editRoly_Click(object sender, RoutedEventArgs e)
        {
            var wind = new GUI_AddNewRolyUser(_index,this);
            wind.ShowDialog();
        }

        private void updateDB_Click(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            ThreadManager.CloseActiveThread();
        }
    }
}
