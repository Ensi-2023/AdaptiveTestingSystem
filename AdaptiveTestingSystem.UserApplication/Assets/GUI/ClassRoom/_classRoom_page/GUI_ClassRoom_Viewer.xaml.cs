using AdaptiveTestingSystem.Control.CustomControl;
using AdaptiveTestingSystem.Control.Windows;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.ClassRoom._classRoom_page._classRoom_mvvm;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.ClassRoom._classRoom_page._classRoom_subPage;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Users._user_page.window;
using AdaptiveTestingSystem.UserApplication.Assets.MVVM.Model;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.ClassRoom._classRoom_page
{
    /// <summary>
    /// Логика взаимодействия для GUI_ClassRoom_Viewer.xaml
    /// </summary>
    public partial class GUI_ClassRoom_Viewer : UserControl
    {
        public object MVVM
        {
            get { return DataContext; } 
        }
        ViewClassRoomModel _viewClass;
        CRoom cRoom;

        public GUI_ClassRoom_Viewer(CRoom room, ViewClassRoomModel viewClass)
        {
            InitializeComponent();

            cRoom = room;
            _viewClass = viewClass;
            DataContext = new ViewModal_CRV_User(room.UserList, cRoom.Index);

            SetData();
            //collection.ItemsSource=CollectionUser;
        }

        private void SetData()
        {
            _classRoomTeacher.NameUser = cRoom.FIO;
            _classRoomTeacher.DateBirch = cRoom.DatebirchEmployee;
            _classRoomTeacher.Gender = cRoom.GenderEmployee;
            _classRoomTeacher.Id = cRoom.EmployeeID;


            if (cRoom.EmployeeID != 0)
            {
                updateEmp.Visibility = Visibility.Visible;
                deleteEmp.Visibility = Visibility.Visible;
                viewDataEmp.Visibility = Visibility.Visible;
            }
            else 
            {
                addNewEmp.Visibility = Visibility.Visible;
              
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(collection.Items.Count.ToString());
        }

        private void collection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void addNewUserToClassRoom_Click(object sender, RoutedEventArgs e)
        {
            var obj = new GUI_AddNewUserToClassRoom(cRoom.Index,DataContext);
            obj.ShowDialog();
        }

        private void red_Click(object sender, RoutedEventArgs e)
        {
            var window  = new GUI_User_Insert_ClassRoomInsert(true,cRoom.Index,cRoom.ClassName);
            window.ShowDialog();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var mvvm = MVVM as ViewModal_CRV_User;

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

        private void addNewEmp_Click(object sender, RoutedEventArgs e)
        {
            var obj = new GUI_AddOrUpdateEmployeeToClassRoom(_classRoomTeacher, cRoom.Index);
            obj.ShowDialog();

            CheckData();
        }

        private void CheckData()
        {

            if (_classRoomTeacher.Id != 0)
            {
                updateEmp.Visibility = Visibility.Visible;
                deleteEmp.Visibility = Visibility.Visible;
                viewDataEmp.Visibility = Visibility.Visible;
                addNewEmp.Visibility = Visibility.Collapsed;
            }
            else
            {
                addNewEmp.Visibility = Visibility.Visible;
                updateEmp.Visibility = Visibility.Collapsed;
                deleteEmp.Visibility = Visibility.Collapsed;
                viewDataEmp.Visibility = Visibility.Collapsed;
            }
        }

        private void updateEmp_Click(object sender, RoutedEventArgs e)
        {
            var obj = new GUI_AddOrUpdateEmployeeToClassRoom(_classRoomTeacher, cRoom.Index,true);
            obj.ShowDialog();

            CheckData();
        }

        private void deleteEmp_Click(object sender, RoutedEventArgs e)
        {
           
            if(MessageShow.Show("Очистить данные ?","Удаление",MessageShow.Type.Question)==true)
            DeleteCard();
        }

        private void DeleteCard()
        {
            var updateData = new Data_Klass_UpdateEmployee()
            {
                Index_Class = cRoom.Index,
                Index_Employee = 0,
                IsCode = Code.Update,
            };

            _classRoomTeacher.NameUser = string.Empty;
            _classRoomTeacher.Id = 0;
            _classRoomTeacher.Gender = string.Empty;
            _classRoomTeacher.DateBirch = string.Empty;


            var packet = new Data_FirstCommand()
            {
                Command = "Command_ClassRoom_UpdateEmployee",
                Json = JsonSerializer.Serialize(updateData)
            };

            _Main.Instance.Client.Send(JsonSerializer.Serialize(packet));
            CheckData();
        }

        private void viewDataEmp_Click(object sender, RoutedEventArgs e)
        {
            if (_classRoomTeacher.Id != 0)
            {
                _Main.Instance.Manager.Next(new Users._user_page.GUI_User_Viewer(_classRoomTeacher));
            }
        }

        private async void delete_Click(object sender, RoutedEventArgs e)
        {
            if (collection.Items.Count > 0)
            {
                MessageShow.Show("Класс не был удален. Очистите список учащихся", "", MessageShow.Type.Error);
                return;
            }

            if (MessageShow.Show($"Вы действительно желаете удалить класс '{cRoom.ClassName}' ?", "Удаление", MessageShow.Type.Question) == true)
            {
                List<Data_Klass> commandDeleteList = new List<Data_Klass>()
                {
                    new Data_Klass()
                    {
                       Id = cRoom.Index
                    }
                };

                _Main.Instance.OverlayShow(true);
                var json = new Data_Klass_Delete()
                {
                   IsCode = Code.Delete,
                   Klasses = commandDeleteList,
                   Description = $"Запрос на удаление класса '{cRoom.ClassName}' из таблицы Klass"
                };

                var firstCommand = new Data_FirstCommand()
                {
                   Command = "Command_DeleteClassRoom",
                   Json = JsonSerializer.Serialize(json)
                };

                _Main.Instance.Client.Send(JsonSerializer.Serialize(firstCommand));
                await Task.Delay(250);
                _Main.Instance.OverlayShow(false);
                _Main.Instance.Manager.Back();
            }
            
        }
    }
}
