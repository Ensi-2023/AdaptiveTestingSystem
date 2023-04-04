using AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports._gui_subpage.uc_control;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports._gui_subpage.uc_control.CScript;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports._gui_subpage.viewmodel;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports._gui_subpage.window;
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


namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports._gui_subpage
{
    /// <summary>
    /// Логика взаимодействия для GUI_Report_Page2.xaml
    /// </summary>
    public partial class GUI_Report_Page2 : UserControl
    {

        CreateReportPage_2_Range reportPage_2_Range;
        private GUI_ReportPage_2_RangeDay _countrols;

        public modelPage_2 modelPage_2 { get; set; }

        public bool IsCancelUpload { get; internal set; }

        public GUI_Report_Page2()
        {
            InitializeComponent();
            reportPage_2_Range = new CreateReportPage_2_Range();
            DataContext = modelPage_2 = new modelPage_2();

        }

        private void searchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var select = modelPage_2.Search((sender as ComboTextBox).Text, userList.SelectedItems);

                if (select.Count > 0)
                {
                    foreach (var item in select)
                    { 
                        var obj = item as modelPage_2_user;

                        foreach (modelPage_2_user rrr in userList.Items)
                        {
                            if (rrr.Index == obj.Index)
                            {
                                userList.SelectedItem= rrr;
                            }
                        }
                    }
                }
            }
        }

    

        private void root_Loaded(object sender, RoutedEventArgs e)
        {
            SendToServer();

          
        }

        private void root_Unloaded(object sender, RoutedEventArgs e)
        {
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

        private async void SendToServer()
        {
            IsCancelUpload = false;
            indicator.Visibility = Visibility.Visible;
            body.Visibility = Visibility.Collapsed;
            retryUpload.Visibility = Visibility.Collapsed;
            overlay.Visibility = Visibility.Visible;

            indicator.Value = 0;

            await Task.Delay(250);


            var packet = new Data_StatisticPacket()
            {
                IsCode = Code.ThreadStart
            };

            ThreadManager.Send("Command_StatisticCustom", packet);
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
            Logger.Debug($"User: {custom.data_AllUsers.Count}");
            SetSearchPopupItem(custom.data_AllUsers);
            ThreadManager.Clear();



            _countrols = new uc_control.GUI_ReportPage_2_RangeDay(reportPage_2_Range);

            FilterRangeDataSetUI(_countrols);

            dayRadioButton.IsChecked = true;
        }

        private async void SetSearchPopupItem(List<Data_AllUserPacket> data_AllUsers)
        {
            ConsoleBox.Items.Clear();
            _Main.Instance.OverlayShow(true);

         //   for (int i = 0; i < 5000; i++)
            {

                foreach (var item in data_AllUsers.ToList())
                {
                    ConsoleBox.Items.Add(new PopupItemControl()
                    {
                        Index = item.Index,
                        Caption = $"{item.Name}",
                        HiddenField = item.Gender
                    });

                    modelPage_2.SetUser(item.Index, item.Name, item.Gender, item.DateBirch);
            

                 //   _Main.Instance.OverlayShow(true,TypeOverlay.loading,"Обработка данных",$"Packet: {i+1} из {5000}");

                }


                await Task.Delay(20);

            }
            _Main.Instance.OverlayShow(false);

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var obj = sender as RadioButton;
            if(obj!=null)
            {
                switch (obj.Uid)
                {
                    case "1": _countrols.SetView(GUI_ReportPage_2_RangeDay.ViewData.day); break;
                    case "2": _countrols.SetView(GUI_ReportPage_2_RangeDay.ViewData.month); break;
                    case "3": _countrols.SetView(GUI_ReportPage_2_RangeDay.ViewData.year); break;
                }
            }
        }
        public void FilterRangeDataSetUI(UIElement ui)
        {
            filterRangeData.Children.Clear();
            filterRangeData.Children.Add(ui);
        }

        private async void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var obj = sender as DataGrid;

            if (obj == null) return;

            if (obj.SelectedItems.Count > 0)
            {
                titleCountSelect.Text = obj.SelectedItems.Count.ToString();

                button_cancleSelect.Visibility = Visibility.Visible;
                Animation.AnimatedOpacity(button_cancleSelect, button_cancleSelect.Opacity, 1, TimeSpan.FromMilliseconds(150));
            }
            else
            {
                Animation.AnimatedOpacity(button_cancleSelect, button_cancleSelect.Opacity, 0, TimeSpan.FromMilliseconds(150));
                await Task.Delay(170);
                button_cancleSelect.Visibility = Visibility.Collapsed;
            }
        }

        private void DataGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Ensure row was clicked and not empty space
            var row = DataGridHelper.GetRow(sender, e);
            if (row == null) return;

            if (row.IsSelected) row.IsSelected = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            userList.UnselectAll();
        }

        private void openUserViewer_Click(object sender, RoutedEventArgs e)
        {
            var obj = new GUI_ReportPage_2_FullViewUser(this, modelPage_2);
            obj.ShowDialog();
        }
    }
}
