using AdaptiveTestingSystem.Control.CustomControl;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.ClassRoom._classRoom_page;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Users._user_page.window;
using AdaptiveTestingSystem.UserApplication.Assets.MVVM.Model;
using System;
using System.Collections;
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

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.ClassRoom
{
    /// <summary>
    /// Логика взаимодействия для GUI_ClassRoom.xaml
    /// </summary>
    public partial class GUI_ClassRoom : UserControl
    {
        ViewClassRoomModel viewClass;

        public GUI_ClassRoom()
        {
            InitializeComponent();
            DataContext=  viewClass = _Main.Instance.MVVM_Manager.ClassRoomModel;



            AccessUser();
        }

        private void AccessUser()
        {
            if (_Main.Instance.MyAccount.ReadClass)
            {
                AppendClassRoomUser.Visibility = Visibility.Visible;
                deleteClass.Visibility = Visibility.Visible;
            }
            else
            {
                AppendClassRoomUser.Visibility = Visibility.Collapsed;
                deleteClass.Visibility = Visibility.Collapsed;
            }
        }

        public void Update()
        {
            viewClass.OnUpdate(true);
        }
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var text = ((TextBox)sender).Text;
                viewClass.Search(text);
            }
        }


        private void Row_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Ensure row was clicked and not empty space

            var row = DataGridHelper.GetRow(sender, e);
            if (row == null) return;
            viewClass.ViewInformation.Execute(row.Item);
        }



        private void DataGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Ensure row was clicked and not empty space
            var row = DataGridHelper.GetRow(sender, e);
            if (row == null) return;

            if (row.IsSelected) row.IsSelected = false;
        }


        private void countView_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var obj = (ComboTextBox)sender;
            if (obj != null && viewClass != null)
            {
                viewClass.SetCountView(ParserVariables.GetInt(obj.Text));
            }
        }

        private void root_Loaded(object sender, RoutedEventArgs e)
        {
            viewClass.Update += ViewClass_Update; ;
            viewClass.OverlayShowing += ViewClass_OverlayShowing; ;
            viewClass.OverlayChangeInformation += ViewClass_OverlayChangeInformation; ;
            viewClass.DeleteObjects += ViewClass_DeleteObjects; ;
            viewClass.ViewerInformationRoom += ViewClass_ViewerInformationRoom; ;
            viewClass.IsView = true;
            viewClass.OnUpdate();
        }

        private void ViewClass_ViewerInformationRoom(CRoom room)
        {
            _Main.Instance.Manager.Next(new GUI_ClassRoom_Viewer(room, viewClass));
        }

        private void ViewClass_DeleteObjects(System.Collections.IList deleteList)
        {

            if (deleteList != null)
            {

                if (deleteList.Count == 0) return;

                List<CRoom> list;
                List<Data_Klass> commandDeleteList;
                SetDeleteClassRoomList(deleteList, out list, out commandDeleteList);

                if (list.Count > 0)
                {
                    if (MessageShow.Show($"Вы действительно желаете удалить #{list.Count} записей?", "Удаление", MessageShow.Type.Question) == true)
                    {
                        var json = new Data_Klass_Delete()
                        {
                            IsCode = Code.Delete,
                            Klasses = commandDeleteList,
                            Description = $"Запрос на удаление #{list.Count} записей из таблицы Klass"
                        };

                        var firstCommand = new Data_FirstCommand()
                        {
                            Command = "Command_DeleteClassRoom",
                            Json = JsonSerializer.Serialize(json)
                        };

                        _Main.Instance.Client.Send(JsonSerializer.Serialize(firstCommand));
                      
                    }
                }
            }


        }

        private  void SetDeleteClassRoomList(IList deleteList, out List<CRoom> list, out List<Data_Klass> commandDeleteList)
        {
            list = deleteList.Cast<CRoom>().ToList();
            commandDeleteList = new List<Data_Klass>();
            foreach (var item in list)
            {
                commandDeleteList.Add(new Data_Klass()
                {
                    Id = item.Index,
                    Name = item.ClassName

                });

              
            }
        }

        private void ViewClass_OverlayChangeInformation(string firstValue, string lastValue)
        {
            _Main.Instance.OverlayShow(true, TypeOverlay.loading, title: "Список классов", subtitle: $"Обработано: {firstValue} из {lastValue}");
        }

        private void ViewClass_OverlayShowing(bool show)
        {
            _Main.Instance.OverlayShow(show);
        }

        private async void ViewClass_Update(bool skipCheck)
        {
            _Main.Instance.OverlayShow(!skipCheck, TypeOverlay.loading, title: "Список классов", subtitle: "загрузка...");

            await Task.Delay(250);

            var data = new Data_Klass()
            {
                IsCode = Code.ThreadStart
            };
   
            ThreadManager.Send("Command_GetCLassRoomList", data);
        }

        private void root_Unloaded(object sender, RoutedEventArgs e)
        {
            viewClass.IsView = false;
            viewClass.Update -= ViewClass_Update; ;
            viewClass.OverlayShowing -= ViewClass_OverlayShowing; ;
            viewClass.OverlayChangeInformation -= ViewClass_OverlayChangeInformation; ;
            viewClass.DeleteObjects -= ViewClass_DeleteObjects; ;
            viewClass.ViewerInformationRoom -= ViewClass_ViewerInformationRoom; ;

            ThreadManager.CloseActiveThread();
        }

        private void AppendClassRoomUser_Click(object sender, RoutedEventArgs e)
        {
            GUI_User_Insert_ClassRoomInsert guic = new GUI_User_Insert_ClassRoomInsert();
            guic.ShowDialog();
            viewClass.OnUpdate();
        }

        private void updateDB_Click(object sender, RoutedEventArgs e)
        {
            viewClass.OnUpdate();
        }
    }
}
