using AdaptiveTestingSystem.Control.Windows;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports._gui_subpage.CScript;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports._gui_subpage.uc_control;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports._gui_subpage.uc_control.CScript;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports._gui_subpage.viewmodel;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports._gui_subpage.window;
using System;
using System.Collections;
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


namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports._gui_subpage
{
    /// <summary>
    /// Логика взаимодействия для GUI_Report_Page2.xaml
    /// </summary>
    public partial class GUI_Report_Page2 : UserControl
    {
        ViewResultUserModel _resultUserModel;
        CreateReportPage_2_Range reportPage_2_Range;
        private GUI_ReportPage_2_RangeDay _countrols;
    
        public bool IsCancelUpload { get; internal set; }

        public GUI_Report_Page2()
        {
            InitializeComponent();
            reportPage_2_Range = new CreateReportPage_2_Range();
 
            DataContext = _resultUserModel = _Main.Instance.MVVM_Manager.ResultUserModel;


        }

        private void searchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _resultUserModel.Search((sender as ComboTextBox).Text);              
            }
        }
   

        private void root_Loaded(object sender, RoutedEventArgs e)
        {
            _resultUserModel.Update += _resultUserModel_Update; 
            _resultUserModel.OverlayShowing += _resultUserModel_OverlayShowing; 
            _resultUserModel.OverlayChangeInformation += _resultUserModel_OverlayChangeInformation;
            _resultUserModel.SelectUsersInGrid += _resultUserModel_SelectUsersInGrid;  
            _resultUserModel.IsView = true;
            _resultUserModel.OnUpdate();
        }

        public void _resultUserModel_SelectUsersInGrid(List<User> users)
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
                titleCountSelect.Text = userList.SelectedItems.Count.ToString();

                button_cancleSelect.Visibility = Visibility.Visible;
                Animation.AnimatedOpacity(button_cancleSelect, button_cancleSelect.Opacity, 1, TimeSpan.FromMilliseconds(150));
            }
            else
            {
                button_cancleSelect.Visibility = Visibility.Collapsed;
            }

        }

        private void _resultUserModel_OverlayChangeInformation(string firstValue, string lastValue)
        {
            _Main.Instance.OverlayShow(true, TypeOverlay.loading, title: $"Пользователи", subtitle: $"Обработано: {firstValue} из {lastValue}");

        }

        private void _resultUserModel_OverlayShowing(bool show)
        {
            _Main.Instance.OverlayShow(show);
        }

        private async void _resultUserModel_Update(bool skipCheck)
        {
            _Main.Instance.OverlayShow(!skipCheck, TypeOverlay.loading, title: $"Данные", subtitle: "загрузка...", visibleButton: Visibility.Visible);

            await Task.Delay(250);

            var packet = new Data_StatisticCustom()
            {
                IsCode = Code.ThreadStart,
                IsSearch = false
            };

            ThreadManager.Send("Command_StatisticCustom", packet);
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

        private void retryUpload_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonCloseThread_Click(object sender, RoutedEventArgs e)
        {

        }

        public void SendInfo(double size, double maxsize)
        {
            if (size != 0)
            {
                double c = maxsize / size;
                double perc = 100 / c;
                indicator.Value = perc;
                indicator.IsIndeterminate = false;
            }
            percUpload.Visibility = Visibility.Visible;
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
        private void percUpload_MouseEnter(object sender, MouseEventArgs e)
        {
            percUpload.Visibility = Visibility.Collapsed;
            buttonCloseThread.Visibility = Visibility.Visible;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            if (IsCancelUpload) return;
            percUpload.Visibility = Visibility.Visible;
            buttonCloseThread.Visibility = Visibility.Collapsed;
        }


        public void SetData(Data_StatisticCustom custom)
        {
            _Main.Instance.IsEnabled = true;
            body.Visibility = Visibility.Visible;
            overlay.Visibility = Visibility.Collapsed;
            if (custom.IsSearch == false)
            {
                SetViewModelItem(custom.data_AllUsers);
                _countrols = new uc_control.GUI_ReportPage_2_RangeDay(reportPage_2_Range);
                FilterRangeDataSetUI(_countrols);
                dayRadioButton.IsChecked = true;
            }
            else
            {
                Logger.Debug("lala");
            }

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

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var obj = sender as RadioButton;
            if(obj!=null)
            {
                switch (obj.Uid)
                {
                    case "1": _countrols.SetView(ViewData.day); break;
                    case "2": _countrols.SetView(ViewData.month); break;
                    case "3": _countrols.SetView(ViewData.year); break;
                }
            }
        }
        public void FilterRangeDataSetUI(UIElement ui)
        {
            filterRangeData.Children.Clear();
            filterRangeData.Children.Add(ui);
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var obj = sender as DataGrid;

            if (obj == null) return;
         

            if (obj.SelectedItems.Count > 0)
            {
                _resultUserModel.AddSelect(obj.SelectedItems);
                titleCountSelect.Text = obj.SelectedItems.Count.ToString();
                button_cancleSelect.Visibility = Visibility.Visible;
                Animation.AnimatedOpacity(button_cancleSelect, button_cancleSelect.Opacity, 1, TimeSpan.FromMilliseconds(150));
            }
            else
            {
                button_cancleSelect.Visibility = Visibility.Collapsed;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            userList.UnselectAll();
            _resultUserModel.AddSelect(userList.SelectedItems);
        }

        private void openUserViewer_Click(object sender, RoutedEventArgs e)
        {
            _resultUserModel.IsView = false;
            var obj = new GUI_ReportPage_2_FullViewUser(this);
            obj.ShowDialog();
            _resultUserModel.IsView = true;
        }

        public void SetSelectUser(IList selectedItems)
        {
            _resultUserModel.AddSelect(selectedItems);
            _resultUserModel.ViewAllSelectUser();
        }

        private void CreateCustomReport_Click(object sender, RoutedEventArgs e)
        {
            var filterData = AddDataInReport.Create(_countrols.CreateReport());
            var selectItem = userList.SelectedItems;
            if (selectItem.Count == 0)
            {
                _Main.Instance._Notification.Add("", "Выберите хотя бы 1 пользователя", TypeNotification.Error);
                return;
            }


            if (selectItem.Count > 30)
            {
                _Main.Instance._Notification.Add("", "Вы выбрали слишком много пользователей, макс.: 30",TypeNotification.Error);
                return;
            }

            if (selectItem.Count > 10)
            {
                if (MessageShow.Show("Вы действительно хотите отобразить на графике больше 10 записей?","",MessageShow.Type.Question) == false)
                {
                    return;
                }
            }

            List<Data_AllUserPacket> selectUser = GetUserSelectData();
            Data_StatisticCustom data = new Data_StatisticCustom()
            {
                customReportFilter = new Data_CustomReportFilter()
                {
                    DayEnd = filterData.DayEnd,
                    DayStart = filterData.DayStart,
                    MonthEnd = filterData.MonthEnd,
                    MonthStart = filterData.MonthStart,
                    ViewData = filterData.ViewData,
                    YearEnd = filterData.YearEnd,
                    YearStart = filterData.YearStart,
                },
                data_AllUsers = selectUser,
                IsSearch = true
 
           };
            _Main.Instance.OverlayShow(true);

            ThreadManager.Send("Command_StatisticCustom", data);

        }
 

        private List<Data_AllUserPacket> GetUserSelectData()
        {
            List < Data_AllUserPacket > list = new List<Data_AllUserPacket>();
            foreach (User item in userList.SelectedItems)
            {
                list.Add(new Data_AllUserPacket() 
                {
                    Index = item.Index,
                    DateBirch  = item.Datebirch,
                    Gender= item.Gender,    
                    Name = item.FIO
                });
            }

            return list;
        }
    }
}
