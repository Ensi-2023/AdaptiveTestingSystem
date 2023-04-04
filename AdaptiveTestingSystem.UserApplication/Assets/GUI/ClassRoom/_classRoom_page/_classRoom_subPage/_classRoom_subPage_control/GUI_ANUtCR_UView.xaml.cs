﻿using AdaptiveTestingSystem.UserApplication.Assets.GUI.ClassRoom._classRoom_page._classRoom_mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
    /// Логика взаимодействия для GUI_ANUtCR_UView.xaml
    /// </summary>
    public partial class GUI_ANUtCR_UView : UserControl
    {

        object MainView = null;
        
        
        ViewModal_CRV_User viewModal = null;

        private int _indexClassRoom;
        public GUI_ANUtCR_UView(int index, object mainDataContext)
        {
            InitializeComponent();
            this._indexClassRoom= index;
            DataContext = viewModal = new ViewModal_CRV_User(index);

            MainView = mainDataContext;
        }

        public void SetData(List<Data_UserList> userLists)
        {

            if (userLists.Count > 0)
            {
                viewModal.SetUserList(userLists);   
            }
            else
            {
                _Main.Instance._Notification.Add("Данные не найдены");
                Overlay.Visibility = Visibility.Collapsed;
            }


        }

        private void root_Loaded(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private async void Update()
        {
            Overlay.Visibility = Visibility.Visible;
            Overlay.Title = "Загрузка данных...";
            viewModal.Loaded += ViewModal_Loaded;
            await Task.Delay(250);

            var packet = new Data_UserViewer()
            {
                IsCode = Code.ThreadStart
            };

            ThreadManager.Send("Command_GetUserNoClassRoom", packet);

           
        }

        private void ViewModal_Loaded()
        {
            Overlay.Visibility = Visibility.Collapsed;
        }

        private void root_Unloaded(object sender, RoutedEventArgs e)
        {
           viewModal.Loaded -= ViewModal_Loaded;

            ThreadManager.CloseActiveThread();

        }

        private async void collection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var obj = sender as ListView;

            if (obj == null) return;

            if (obj.SelectedItems.Count > 0)
            {
                commandButtonPanel.Visibility = Visibility.Visible;
                Animation.AnimatedOpacity(commandButtonPanel, commandButtonPanel.Opacity, 1, TimeSpan.FromMilliseconds(150));
            }
            else
            {
                Animation.AnimatedOpacity(commandButtonPanel, commandButtonPanel.Opacity, 0, TimeSpan.FromMilliseconds(150));
                await Task.Delay(170);
                commandButtonPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            collection.UnselectAll();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var list = collection.SelectedItems;
            var packetList = new List<Data_UserToClass>();
            commandButtonPanel.Visibility = Visibility.Visible;

            foreach (var obj in collection.SelectedItems)
            {
                var item = obj as Modal_CRV_User;
                if (item != null)
                {
                    (MainView as ViewModal_CRV_User).Add(item.Index, item.Name, item.DayBirch, item.Gender);
                    packetList.Add(new Data_UserToClass { IndexClass = _indexClassRoom, IndexUser = item.Index, IsCode = Code.Null });
                    await Task.Delay(100);
                }
            }

            var packet = new Data_FirstCommand
            {
                Command = "Command_UserToClassRoom",
                Json = JsonSerializer.Serialize(packetList)
            };

            _Main.Instance.Client.Send(JsonSerializer.Serialize(packet));
            viewModal.DeleteItemView(list);
            commandButtonPanel.Visibility = Visibility.Collapsed;
            collection.UnselectAll();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Overlay.Visibility = Visibility.Visible;
                if ((sender as TextBox).Text == string.Empty) { viewModal.SearchClear(false); return; }
                viewModal.Search((sender as TextBox).Text,false);

                Overlay.Visibility = Visibility.Collapsed;

                (sender as TextBox).Focus();
            }
        }

        private void UserCard_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
        
        }

        private void updateDB_Click(object sender, RoutedEventArgs e)
        {
            Update();
        }
    }
}
