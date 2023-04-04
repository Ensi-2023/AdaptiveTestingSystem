using AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports._gui_subpage.viewmodel;
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
using System.Windows.Shapes;

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports._gui_subpage.window
{
    /// <summary>
    /// Логика взаимодействия для GUI_ReportPage_2_FullViewUser.xaml
    /// </summary>
    public partial class GUI_ReportPage_2_FullViewUser : Window
    {
        public modelPage_2 modelPage_2 { get; set; }

        public GUI_ReportPage_2_FullViewUser(GUI_Report_Page2 gUI_Report_Page2, viewmodel.modelPage_2 _modelPage_2)
        {
            InitializeComponent();

            DataContext = modelPage_2 = _modelPage_2;
        }

        private void DataGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Ensure row was clicked and not empty space
            var row = DataGridHelper.GetRow(sender, e);
            if (row == null) return;

            if (row.IsSelected) row.IsSelected = false;
        }

        private void searchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Search((sender as ComboTextBox).Text);              
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

                button_manager.Visibility = Visibility.Visible;
                Animation.AnimatedHeight(button_manager, button_manager.ActualHeight, 75, TimeSpan.FromMilliseconds(150));
            }
            else
            {
                Animation.AnimatedHeight(button_manager, button_manager.ActualHeight, 0, TimeSpan.FromMilliseconds(150));
                await Task.Delay(130);
                
            }
        }

        private void AppSelectUser_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ViewSelectUser_Click(object sender, RoutedEventArgs e)
        {
            ViewSelectUser.Visibility= Visibility.Collapsed;
            ViewAllSelect.Visibility= Visibility.Visible;
            modelPage_2.ViewSelect(userList.SelectedItems);
        }

        private void ClearSelect_Click(object sender, RoutedEventArgs e)
        {
            userList.UnselectAll();
        }

        private void ViewAllSelect_Click(object sender, RoutedEventArgs e)
        {
            Search("");
            ViewSelectUser.Visibility = Visibility.Visible;
            ViewAllSelect.Visibility = Visibility.Collapsed;

        }

        private void Search(string text)
        {
            var select = modelPage_2.Search(text, userList.SelectedItems);

            if (select.Count > 0)
            {
                foreach (var item in select)
                {
                    var obj = item as modelPage_2_user;

                    foreach (modelPage_2_user rrr in userList.Items)
                    {
                        if (rrr.Index == obj.Index)
                        {
                            userList.SelectedItem = rrr;
                        }
                    }
                }
            }
        }
    }
}
