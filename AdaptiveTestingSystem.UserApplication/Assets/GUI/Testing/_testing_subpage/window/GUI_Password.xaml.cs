using AdaptiveTestingSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage.window
{
    /// <summary>
    /// Логика взаимодействия для GUI_Password.xaml
    /// </summary>
    public partial class GUI_Password : Window
    {
        private string hash = "";
        public GUI_Password(string password)
        {
            InitializeComponent();
            hash = password;
      
        }

        private void Header_CloseClick()
        {
            Close();
        }

        private void connectPassword_Click(object sender, RoutedEventArgs e)
        {
            if (Encryption.verifyMd5Hash(ServerPAssword.Password, hash))
            {
                DialogResult = true;
                Close();
            }
            else
            {
                MessageShow.Show("Пароль невенер","Ошибка",MessageShow.Type.Error);
            }
        }

        private void root_Loaded(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
