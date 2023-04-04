using AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports._gui_subpage.viewmodel;
using System;
using System.Collections.Generic;
using System.Drawing;
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
using System.Windows.Shapes;

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports._gui_subpage.window
{
    /// <summary>
    /// Логика взаимодействия для GUI_ReportPage_2_FullViewUser.xaml
    /// </summary>
    public partial class GUI_ReportPage_2_FullViewUser : Window
    {
        private GUI_Report_Page2 _gui_Report_Page2  { get; set; }

    public ViewResultUserModel _resultUserModel { get; set; }
        public bool IsViewSelect { get; private set; }

        public GUI_ReportPage_2_FullViewUser(GUI_Report_Page2 gui_Report_Page2)
        {
            InitializeComponent();

            DataContext = _resultUserModel = _Main.Instance.MVVM_Manager.ResultViewDetailUserModel;

            _gui_Report_Page2 = gui_Report_Page2;
        }


        private void PTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var obj = (ComboTextBox)sender;
            if (obj != null && _resultUserModel != null)
            {
                _resultUserModel.SetCountView(ParserVariables.GetInt(obj.Text));
            }
        }

        private void DataGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Ensure row was clicked and not empty space
            var row = DataGridHelper.GetRow(sender, e);
            if (row == null) return;

            if (row.IsSelected) 
            {
                row.IsSelected = false;
                var user = row.Item as User;
                _resultUserModel.DeleteSelectUser(user);
            }
        }

        private void searchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _resultUserModel.Search((sender as ComboTextBox).Text);
            }
        }

        private void Header_CloseClick()
        {
            Close();
        }

      

        private async void userList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var obj = sender as DataGrid;

            if (obj == null) return;

            if (obj.SelectedItems.Count > 0)
            {
                ClearSelect.Visibility= Visibility.Visible;
                AppSelectUser.Visibility= Visibility.Visible;
                SelectALL.Visibility= Visibility.Collapsed;
                _resultUserModel.AddSelect(obj.SelectedItems);
                button_manager.Visibility = Visibility.Visible;
                Animation.AnimatedHeight(button_manager, button_manager.ActualHeight, 75, TimeSpan.FromMilliseconds(150));
            }
            else
            {
                ClearSelect.Visibility= Visibility.Collapsed;
                AppSelectUser.Visibility= Visibility.Collapsed;
                SelectALL.Visibility= Visibility.Visible;

                if (IsViewSelect) return;
                Animation.AnimatedHeight(button_manager, button_manager.ActualHeight, 0, TimeSpan.FromMilliseconds(150));
                await Task.Delay(130);
                
            }
        }
        private void AppSelectUser_Click(object sender, RoutedEventArgs e)
        {
            _gui_Report_Page2.SetSelectUser(userList.SelectedItems);
            Close();
        }
        private void ViewSelectUser_Click(object sender, RoutedEventArgs e)
        {
            searchBox.IsEnabled= false;
            IsViewSelect = true;
            ViewSelectUser.Visibility= Visibility.Collapsed;
            ViewAllSelect.Visibility= Visibility.Visible;
            _managerPanel1.Visibility = Visibility.Collapsed;
            _managerPanel2.Visibility = Visibility.Collapsed;
            _resultUserModel.ViewAllSelectUser();
            // modelPage_2.ViewSelect(userList.SelectedItems);
        }

        private void ClearSelect_Click(object sender, RoutedEventArgs e)
        {
            userList.UnselectAll();
            _resultUserModel.AddSelect(userList.SelectedItems);
        }

        private void ViewAllSelect_Click(object sender, RoutedEventArgs e)
        {
            searchBox.IsEnabled = true;
            IsViewSelect = false;
            _resultUserModel.Search("");
            ViewSelectUser.Visibility = Visibility.Visible;
            ViewAllSelect.Visibility = Visibility.Collapsed;

            _managerPanel1.Visibility= Visibility.Visible;
            _managerPanel2.Visibility= Visibility.Visible;


       

        }
             

        private void root_Loaded(object sender, RoutedEventArgs e)
        {
            _resultUserModel.Update += _resultUserModel_Update;
            _resultUserModel.OverlayShowing += _resultUserModel_OverlayShowing; ;
            _resultUserModel.OverlayChangeInformation += _resultUserModel_OverlayChangeInformation; ;
            _resultUserModel.SelectUsersInGrid += _resultUserModel_SelectUsersInGrid;
            _resultUserModel.IsView = true;
            _resultUserModel.OnUpdate();
        }

        private void _resultUserModel_OverlayChangeInformation(string firstValue, string lastValue)
        {
            double c = double.Parse(lastValue) / double.Parse(firstValue);
            double perc = 100 / c;
            percentLoad.Text = $"{Math.Round(perc)}%";
        }

        private void _resultUserModel_OverlayShowing(bool show)
        {
            OverlayShow(show);
        }

        private void OverlayShow(bool show,bool first = false)
        {
            if (show)
            {
                Overlay.Visibility = Visibility.Visible;
                if(first) body.Visibility = Visibility.Collapsed;
            }
            else
            {
                Overlay.Visibility = Visibility.Collapsed;
                body.Visibility = Visibility.Visible;
            }
        }

        private async void _resultUserModel_Update(bool skipCheck)
        {
            OverlayShow(true,true);

            await Task.Delay(250);

            var packet = new Data_StatisticPacket()
            {
                IsCode = Code.ThreadStart
            };

            ThreadManager.Send("Command_StatisticCustom", packet);
        }

        private async void _resultUserModel_SelectUsersInGrid(List<User> users)
        {
            userList.SelectionMode = DataGridSelectionMode.Extended;

            foreach (User _user in users)
            {
                foreach (User item in userList.Items)
                {
                    if (item.Index == _user.Index)
                    {
                        userList.SelectedItems.Add(item);
                    }
                }
            }


            if (userList.SelectedItems.Count > 0)
            {
         
                button_manager.Visibility = Visibility.Visible;
                Animation.AnimatedHeight(button_manager, button_manager.ActualHeight, 75, TimeSpan.FromMilliseconds(150));
            }
            else
            {
                Animation.AnimatedHeight(button_manager, button_manager.ActualHeight, 0, TimeSpan.FromMilliseconds(150));
                await Task.Delay(130);
            }

        }

        private void root_Unloaded(object sender, RoutedEventArgs e)
        {
            _resultUserModel.IsView = false;
            _resultUserModel.Update -= _resultUserModel_Update;
            _resultUserModel.OverlayShowing -= _resultUserModel_OverlayShowing;
            _resultUserModel.OverlayChangeInformation -= _resultUserModel_OverlayChangeInformation;
            _resultUserModel.SelectUsersInGrid -= _resultUserModel_SelectUsersInGrid;
            ThreadManager.CloseActiveThread();
        }

        public void SetData(Data_StatisticCustom obj)
        {
            SetViewModelItem(obj.data_AllUsers);
            ThreadManager.Clear();
        }

        private async void SetViewModelItem(List<Data_AllUserPacket> data_AllUsers)
        {
            await Application.Current.Dispatcher.Invoke(async () =>
            {
                await Task.Factory.StartNew(() =>
                {
                    _resultUserModel.SetCollection(data_AllUsers);
                });
            });
        }

        private void SelectALL_Click(object sender, RoutedEventArgs e)
        {
            userList.SelectAll();   
        }
    }
}
