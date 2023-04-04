using AdaptiveTestingSystem.UserApplication.Assets.GUI.ClassRoom._classRoom_page._classRoom_mvvm;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.ClassRoom._classRoom_page._classRoom_subPage;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Subject._subject_mvvm;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Subject._subject_subpage.window;
using AdaptiveTestingSystem.UserApplication.Assets.MVVM.Model;
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

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Subject._subject_subpage
{
    /// <summary>
    /// Логика взаимодействия для GUI_Subject_Viewer.xaml
    /// </summary>
    public partial class GUI_Subject_Viewer : UserControl
    {
        MVVM.Model.Subject _subjectViewer;
        ViewSubjectModel _viewSubject;
        public object MVVM
        {
            get { return DataContext; }
        }



        public GUI_Subject_Viewer(MVVM.Model.Subject subject, ViewSubjectModel viewSubject)
        {
            InitializeComponent();
            _subjectViewer = subject;
            _viewSubject = viewSubject;
            DataContext = new ViewModal_SV_User(subject.Users, subject.Index);

            SetData();
        }

        private void SetData()
        {
            predmetName.Text = _subjectViewer.Name;
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var mvvm = MVVM as ViewModal_SV_User;

                if ((sender as TextBox).Text == string.Empty) { mvvm.SearchClear(); return; }

                mvvm.Search((sender as TextBox).Text);


            }
        }

        private void UserCard_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var obj = (sender as UserCard);
            if (obj != null)
            {
                _Main.Instance.Manager.Next(new Users._user_page.GUI_User_Viewer(obj.Id, (MVVM as ViewModal_CRV_User)));
            }

        }

        private void addNewUserToSubject_Click(object sender, RoutedEventArgs e)
        {
            var obj = new GUI_AddNewUserToSubject(_subjectViewer.Index, DataContext);
            obj.ShowDialog();
        }

        private void red_Click(object sender, RoutedEventArgs e)
        {
            var wind = new GUI_Subject_Changer(_subjectViewer);
            wind.ShowDialog();
            SetData();
        }

        private async void delete_Click(object sender, RoutedEventArgs e)
        {

            if (MessageShow.Show($"Вы действительно желаете удалить предмет: {_subjectViewer.Name} ? ", "Удаление", MessageShow.Type.Question) == true)
            {
                _Main.Instance.OverlayShow(true);

                var json = new Data_Subject_Delete()
                {
                    IsCode = Code.Delete,
                    Subject = new List<Data_Subject>() { new Data_Subject() {Id_data = _subjectViewer.Index } },
                    Description = $"Запрос на удаление #1 записей из таблицы Предметы"
                };

                var firstCommand = new Data_FirstCommand()
                {
                    Command = "Command_DeleteSubject",
                    Json = JsonSerializer.Serialize(json)
                };

                _Main.Instance.Client.Send(JsonSerializer.Serialize(firstCommand));



                List<MVVM.Model.Subject> subjects = new List<MVVM.Model.Subject>();

                foreach (var obj in json.Subject)
                {
                    subjects.Add(new Assets.MVVM.Model.Subject() {Index = obj.Id_data,Name = obj.Name_data });
                }
            
                _viewSubject.DeleteData(subjects,true);
                subjects = null;
                await Task.Delay(250);
                _Main.Instance.OverlayShow(false);
                _Main.Instance.Manager.Back();
            }


         
            
        }

        private void updateDB_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
