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

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.ClassRoom._classRoom_page._classRoom_subPage
{
    /// <summary>
    /// Логика взаимодействия для GUI_AddOrUpdateEmployeeToClassRoom.xaml
    /// </summary>
    public partial class GUI_AddOrUpdateEmployeeToClassRoom : Window
    {

        List<UserCard> _cards;

        UserCard _userCard;
        bool _isUpdate;
        int _classRoomIndex= 0;
        public GUI_AddOrUpdateEmployeeToClassRoom(UserCard user,int classRoomIndex,bool isupdate=false)
        {
            InitializeComponent();
            _userCard = user;
            _isUpdate= isupdate;
            _classRoomIndex= classRoomIndex;
        }

        private void Header_CloseClick()
        {
            Close();    
        }

        private void root_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }

        private async void UpdateData()
        {
            Overlay.Visibility= Visibility.Visible;

            await Task.Delay(250);

            var data = new Data_UserList()
            {
                IsCode = Code.ThreadStart
            };

            if (_isUpdate)
            {
                data.Id = _userCard.Id;
                data.IsUpdateOrInsert = true;
            }
            else
            {
                data.IsUpdateOrInsert = false;
            }

            ThreadManager.Send("Command_GetEmployee", data);
        }

        public async Task SetData(List<Data_UserList> list)
        {
            wPanel.Items.Clear();
            _cards = new List<UserCard>();
            foreach (var item in list)
            {
            
                if (_userCard.Id == item.Id) continue;                            
                
                UserCard card = new UserCard();
                card.Id= item.Id;
                card.NameUser= item.Name;
                card.DateBirch= item.DateBirch;
                card.Gender= item.Gender;

                _cards.Add(card);
                wPanel.Items.Add(card);

                await Task.Delay(40);
            }

            Overlay.Visibility = Visibility.Collapsed;
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

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            { 
                var obj = sender as TextBox;

                if (obj.Text == string.Empty)
                {
                    ViewAll();
                }
                else
                {
                    Search(obj.Text);
                }
            }
        }

        private async void Search(string text)
        {
            Overlay.Visibility = Visibility.Visible;
            wPanel.Items.Clear();
            foreach (UserCard item in _cards)
            {
                if (item.NameUser.Trim().ToLower().Contains(text.ToLower().Trim()) || item.Gender.Trim().ToLower().Contains(text.ToLower().Trim())) wPanel.Items.Add(item);
                await Task.Delay(1);
            }
            Overlay.Visibility = Visibility.Collapsed;
        }

        private async void ViewAll()
        {
            Overlay.Visibility = Visibility.Visible;
            wPanel.Items.Clear();
            foreach (var item in _cards)
            {
                wPanel.Items.Add(item);
                await Task.Delay(1);
            }
            Overlay.Visibility = Visibility.Collapsed;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var obj = SelectUserCard();
            if (obj != null)
            {
        
                var updateData = new Data_Klass_UpdateEmployee()
                {
                    Index_Class = _classRoomIndex,
                    Index_Employee = obj.Id,
                    IsCode = _isUpdate == true ? Code.Update : Code.Insert,
                };


                _userCard.NameUser = obj.NameUser;
                _userCard.Id = obj.Id;
                _userCard.Gender = obj.Gender;
                _userCard.DateBirch = obj.DateBirch;


                var packet = new Data_FirstCommand()
                {
                    Command = "Command_ClassRoom_UpdateEmployee",
                    Json = JsonSerializer.Serialize(updateData)
                };

                _Main.Instance.Client.Send(JsonSerializer.Serialize(packet));
                Close();
            }
        }

        private UserCard SelectUserCard()
        {
            return wPanel.SelectedItem as UserCard;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            wPanel.UnselectAll();
        }

        private void updateDB_Click(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }

        private void root_Unloaded(object sender, RoutedEventArgs e)
        {
            ThreadManager.CloseActiveThread();
        }
    }
}
