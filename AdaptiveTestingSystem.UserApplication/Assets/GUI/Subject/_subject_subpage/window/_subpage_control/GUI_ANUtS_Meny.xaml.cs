using AdaptiveTestingSystem.UserApplication.Assets.GUI.ClassRoom._classRoom_page._classRoom_subPage._classRoom_subPage_control;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.ClassRoom._classRoom_page._classRoom_subPage;
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

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Subject._subject_subpage.window._subpage_control
{
    /// <summary>
    /// Логика взаимодействия для GUI_ANUtS_Meny.xaml
    /// </summary>
    public partial class GUI_ANUtS_Meny : UserControl
    {
        public GUI_ANUtS_Meny(GUI_AddNewUserToSubject gUI_AddNewUserToSubject, int index)
        {
            InitializeComponent();
            _main = gUI_AddNewUserToSubject;
            _indexSubject = index;
        }


        private GUI_AddNewUserToSubject _main;
        private int _indexSubject;
 

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _main.Manager.Next(new GUI_Users_Insert(true, _indexSubject,true));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _main.Manager.Next(new GUI_ANUtS_UView(_indexSubject, _main.MainDataContext));
        }
    }
}
