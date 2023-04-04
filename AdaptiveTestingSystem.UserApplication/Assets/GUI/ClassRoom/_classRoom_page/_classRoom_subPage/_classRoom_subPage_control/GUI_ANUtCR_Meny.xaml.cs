using AdaptiveTestingSystem.UserApplication.Assets.GUI.Users._user_page;
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

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.ClassRoom._classRoom_page._classRoom_subPage._classRoom_subPage_control
{
    /// <summary>
    /// Логика взаимодействия для GUI_ANUtCR_Meny.xaml
    /// </summary>
    public partial class GUI_ANUtCR_Meny : UserControl
    {

        private GUI_AddNewUserToClassRoom _main;
        private int _indexClassRoom;
        public GUI_ANUtCR_Meny(GUI_AddNewUserToClassRoom gUI_AddNewUserToClassRoom, int index)
        {
            InitializeComponent();
            _main=gUI_AddNewUserToClassRoom;
            _indexClassRoom=index;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _main.Manager.Next(new GUI_Users_Insert(true, _indexClassRoom));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _main.Manager.Next(new GUI_ANUtCR_UView(_indexClassRoom, _main.MainDataContext));
        }
    }
}
