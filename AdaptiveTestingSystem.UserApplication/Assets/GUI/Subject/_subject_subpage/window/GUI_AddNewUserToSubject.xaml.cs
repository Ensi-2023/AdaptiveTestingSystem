
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Subject._subject_subpage.window._subpage_control;
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

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Subject._subject_subpage.window
{
    /// <summary>
    /// Логика взаимодействия для GUI_AddNewUserToSubject.xaml
    /// </summary>
    public partial class GUI_AddNewUserToSubject : Window
    {
        public PManager Manager { get; set; }

        private object mainDataContext;
        public object MainDataContext { get { return mainDataContext; } }

        public GUI_AddNewUserToSubject(int index, object dataContext)
        {
            InitializeComponent();
            mainDataContext = dataContext;

            Manager = new PManager(body);
            Manager.InformationPage += Manager_InformationPage;
            Manager.SetFirstPage(new GUI_ANUtS_Meny(this, index));
        }

        private void Manager_InformationPage(int count, string title)
        {
            TitlesButton.Content = title;
            BackPage.ToolTip = Manager.GetInfoOpenPage();
            if (count > 1)
            {
                BackPage.Visibility = Visibility.Visible;
            }
            else
                BackPage.Visibility = Visibility.Collapsed;
        }


        private void Header_CloseClick()
        {
            Close();
        }

        private void Meny_Click(object sender, RoutedEventArgs e)
        {
            Manager.Home();
        }

        private void BackPage_Click(object sender, RoutedEventArgs e)
        {
            Manager.Back();
        }
    }
}
