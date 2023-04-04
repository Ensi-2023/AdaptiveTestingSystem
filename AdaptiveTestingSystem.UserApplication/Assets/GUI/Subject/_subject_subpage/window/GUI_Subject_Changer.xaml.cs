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
using static AdaptiveTestingSystem.DLL.CScript.ControlEnum;

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Subject._subject_subpage.window
{
    /// <summary>
    /// Логика взаимодействия для GUI_Subject_Changer.xaml
    /// </summary>
    public partial class GUI_Subject_Changer : Window
    {
        private bool _update = false;
        private MVVM.Model.Subject _item;
        private int _indexSubject = 0;
        private string _nameSubject = "";


        public GUI_Subject_Changer()
        {
            InitializeComponent();
    
        }

        public GUI_Subject_Changer(MVVM.Model.Subject item)
        {
            InitializeComponent();
            _update= true;
            _item = item;

            nameSubject.Text = item.Name;
        }
        private void HeaderControl_CloseClick()
        {
            Close();
        }

        private void insertSubject_Click(object sender, RoutedEventArgs e)
        {
            var namesubject = nameSubject.Text.Trim();
            if (namesubject.Length > 0)
            {

                if (namesubject == _nameSubject) Close();

                Overlay(visibleLoad: true, isenable: false);


                Data_Subject qq = new Data_Subject()
                {
                   Id_data = _item==null? _indexSubject: _item.Index,
                   Name_data = namesubject,                  
                   IsCode = _update==true?Code.Subject_Update:Code.Subject_Insert,
                };



                Data_FirstCommand command = new Data_FirstCommand()
                {
                    Command = "Command_SubjectInsertOrUpdate",
                    Json = JsonSerializer.Serialize(qq)
                };

                _Main.Instance.Client.Send(JsonSerializer.Serialize(command));

                if(_item!=null) _item.Name = namesubject;


               
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
        public void Overlay(string message = "", bool visibleLoad = false, bool isenable = true)
        {
            loadProgressBar.Visibility = visibleLoad == true ? Visibility.Visible : Visibility.Collapsed;
            errorMessage.Visibility = message.Trim().Length == 0 ? Visibility.Collapsed : Visibility.Visible;
            errorMessage.Text = message;

            errorMessage.IsEnabled = isenable;
            insertSubject.IsEnabled = isenable;
            cancelSubject.IsEnabled = isenable;


            if (isenable==false)
            {
                if(message.Trim()!= string.Empty) Logger.Message($"[Message] - {message}");
            }

            if (isenable)
            {
                if (message.Trim() != string.Empty) Logger.Error($"[Error] - {message}");
            }

        }

         

        public void Update(int index, string value)
        {
            _item.Index= index;
            _item.Name = value;         
        }

        private void cancelSubject_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
