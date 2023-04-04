using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Users._user_page.window
{

    public partial class GUI_User_Insert_ClassRoomInsert : Window
    {
        private bool _update = false;
        private int _indexClassRoom = 0;
        private string _nameClassRoom = "";
        public GUI_User_Insert_ClassRoomInsert()
        {
            InitializeComponent();
        }


        public GUI_User_Insert_ClassRoomInsert(bool update,int indexClassRoom,string nameCRoom)
        {
            InitializeComponent();
            _update = update;
            _indexClassRoom= indexClassRoom;
            _nameClassRoom= nameCRoom;

            nameClassRoom.Text = _nameClassRoom;
        }

        private void HeaderControl_CloseClick()
        {       
            Close();
        }

        private void insertClassRoom_Click(object sender, RoutedEventArgs e)
        {
            var nameclassroom = nameClassRoom.Text.Trim();
            if (nameclassroom.Length > 0)
            {

                if (nameclassroom == _nameClassRoom) Close();                
                
                Overlay(visibleLoad:true,isenable:false);


                Data_Klass qq = new Data_Klass()
                {
                    Name = nameclassroom,
                    Id = _indexClassRoom
                };



                Data_FirstCommand command = new Data_FirstCommand()
                {
                    Command = "Command_InsertClassRoom",
                    Json = JsonSerializer.Serialize(qq)
                };

                _Main.Instance.Client.Send(JsonSerializer.Serialize(command));

               
            }
            else
            {
                Overlay("Заполните текстовое поле");
                SystemSounds.Beep.Play();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="visibleLoad"></param>
        public void Overlay(string message="", bool visibleLoad=false,bool isenable=true)
        {
            loadProgressBar.Visibility = visibleLoad == true ? Visibility.Visible : Visibility.Collapsed;
            errorMessage.Visibility= message.Trim().Length==0 ? Visibility.Collapsed : Visibility.Visible;
            errorMessage.Text = message;

            errorMessage.IsEnabled = isenable;
            insertClassRoom.IsEnabled = isenable;
            cancelClassRoom.IsEnabled= isenable;

        }

        private void cancelClassRoom_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
