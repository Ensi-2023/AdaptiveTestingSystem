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

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Users
{
    /// <summary>
    /// Логика взаимодействия для GUI_Users.xaml
    /// </summary>
    public partial class GUI_Users : UserControl
    {
        ViewUserModel user_View;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isNoUser">выбрать всех с ролью не пользователь</param>
        /// 

        Code IsRole { get; set; }
        string nameOverlay = "";

        public GUI_Users(Code role)
        {
            InitializeComponent();
            IsRole = role;

            switch (role)
            {
                case Code.GUI_UserAll: nameOverlay = "Пользователи"; break;
                case Code.GUI_User: nameOverlay = "Пользователи"; DataContext = user_View = _Main.Instance.MVVM_Manager.UserModel; break;
                case Code.GUI_Staff: nameOverlay = "Учителя"; DataContext = user_View = _Main.Instance.MVVM_Manager.StaffModel; break;
                case Code.GUI_UserModify: nameOverlay = "Индивидуальные пользователи"; DataContext = user_View = _Main.Instance.MVVM_Manager.ModifyUserModel; break;
            }
        }

        private void AccessUser()
        {
            if (_Main.Instance.MyAccount.ReadUser)
            {
                AppendNewUser.Visibility = Visibility.Visible;
                deleteUser.Visibility = Visibility.Visible;

            }
            else
            {
                AppendNewUser.Visibility = Visibility.Collapsed;
                deleteUser.Visibility = Visibility.Collapsed;
            }
        }


        private void GUI_Users_Unloaded(object sender, RoutedEventArgs e)
        {
            user_View.IsView = false;
            user_View.Update -= User_View_Update;
            user_View.OverlayShowing -= User_View_OverlayShowing;
            user_View.OverlayChangeInformation -= User_View_OverlayChangeInformation;
            user_View.DeleteObjects -= User_View_DeleteObjects;
            user_View.ViewerInformationUser -= User_View_ViewerInformationUser;

            ThreadManager.CloseActiveThread();
        }

        private void GUI_Users_Loaded(object sender, RoutedEventArgs e)
        {

            AccessUser();

            user_View.Update += User_View_Update;
            user_View.OverlayShowing += User_View_OverlayShowing;
            user_View.OverlayChangeInformation += User_View_OverlayChangeInformation;
            user_View.DeleteObjects += User_View_DeleteObjects;
            user_View.ViewerInformationUser += User_View_ViewerInformationUser;
            user_View.IsView = true;
            user_View.OnUpdate();
        }

        private void User_View_ViewerInformationUser(User _user)
        {
            _Main.Instance.Manager.Next(new _user_page.GUI_User_Viewer(_user));
        }

        private void User_View_DeleteObjects(System.Collections.IList deleteList)
        {
            if (deleteList.Count > 0)
            {
                if (MessageShow.Show($"Вы действительно хотите удалить #{deleteList.Count} записей ?\nВосстановление будет не возможным.","Удаление",MessageShow.Type.Question) == true)
                {
                    var list = deleteList.Cast<User>().ToList();
                    var commandDeleteList = new List<Data_DeleteUser>();

                    foreach (var item in list)
                    {
                        commandDeleteList.Add(new Data_DeleteUser()
                        {
                            Id = item.Index,
                            IsCode = Code.Delete
                        });
                    }
              
                        Data_FirstCommand data = new Data_FirstCommand()
                        {
                            Command = "Command_DeleteUser",
                            Json = JsonSerializer.Serialize(commandDeleteList)
                        };

                        _Main.Instance.Client.Send(JsonSerializer.Serialize(data));
                        _Main.Instance._Notification.Add("Запрос на удаление отправлен.");

                        user_View.DeleteData(deleteList);
                    
                  
                }
            }
        }

        private void User_View_OverlayChangeInformation(string firstValue, string lastValue)
        {
            _Main.Instance.OverlayShow(true,TypeOverlay.loading, title: $"{nameOverlay}", subtitle: $"Обработано: {firstValue} из {lastValue}");
        }

        private void User_View_OverlayShowing(bool show)
        {
            _Main.Instance.OverlayShow(show);
        }

        private async void User_View_Update(bool skipCheck)
        {          
            _Main.Instance.OverlayShow(!skipCheck, TypeOverlay.loading, title: $"{nameOverlay}", subtitle:"загрузка...",visibleButton:Visibility.Visible);

            await Task.Delay(250);

            var viewer = new Data_UserPacket()
            {
                IsRoleUser = IsRole,
                IsCode = Code.ThreadStart
            };

            ThreadManager.Send("Command_GetUserList", viewer);

        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scrollviewer = sender as ScrollViewer;
            if (e.Delta > 0)
                scrollviewer.LineUp();
            else
                scrollviewer.LineDown();
        }

      
        private void Row_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Ensure row was clicked and not empty space

            var row = DataGridHelper.GetRow(sender, e);
            if (row == null) return;
            user_View.ViewInformation.Execute(row.Item);
        }



        private void DataGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Ensure row was clicked and not empty space
            var row = DataGridHelper.GetRow(sender, e);
            if (row == null) return;

            if (row.IsSelected) row.IsSelected = false;
        }

        private void PTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var obj = (ComboTextBox)sender;
            if (obj != null && user_View != null)
            {
                user_View.SetCountView(ParserVariables.GetInt(obj.Text));
            }

        }


        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var text = ((TextBox)sender).Text;
                user_View.Search(text);
            }
        }

       
        private void AppendNewUser_Click(object sender, RoutedEventArgs e)
        {
            _Main.Instance.Manager.Next(new _user_page.GUI_Users_Insert());
        }

        private void filterUser_Click(object sender, RoutedEventArgs e)
        {
            var user_Filter = new _user_page.window.GUI_User_Filter(user_View,IsRole);
            user_Filter.ShowDialog();
        }

        private void closefilterUser_Click(object sender, RoutedEventArgs e)
        {
            _Main.Instance.OverlayShow(true, TypeOverlay.loading, title: "Пользователи", subtitle: "загрузка...",visibleButton:Visibility.Visible);

       
            var viewer = new Data_UserPacket()
            {
                IsRoleUser = IsRole,
            };

            ThreadManager.Send("Command_GetUserList", viewer);
        }

        private void updateDB_Click(object sender, RoutedEventArgs e)
        {
            user_View.OnUpdate();
        }
    }
}
