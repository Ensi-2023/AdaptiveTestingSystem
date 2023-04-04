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
using AdaptiveTestingSystem.DLL.wpf;
using AdaptiveTestingSystem.Data.JsonData;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.ClassRoom._classRoom_page._classRoom_mvvm;

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Users._user_page
{
    /// <summary>
    /// Логика взаимодействия для GUI_User_Viewer.xaml
    /// </summary>
    public partial class GUI_User_Viewer : UserControl, INotifyPropertyChanged
    {

        #region DefaultData
        private string _user_fio;
        private string _user_date;
        private string _user_gender;
        #endregion

        private bool _edit_fio;
        private bool _edit_date;
        private bool _edit_gender;
        private bool _edit_password;
        private int _indexUser = 0;
        private bool _me = false;
        private UserCard _userCard = null;
        private ViewModal_CRV_User _viewModal_CRV_User = null;


        public event PropertyChangedEventHandler? PropertyChanged;

#nullable disable
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }


        private string _userName="";
        public string UserName
        {
            get { return _userName.Replace("NULL",""); }
            set {

                _userName = value;
                OnPropertyChanged("UserName");
            }
        }

        private string _userRole;
        public string UserRole
        {
            get { return _userRole; }
            set { _userRole = value;
              OnPropertyChanged("UserRole");
            }
        }


        private string _dateBirch = DateTime.Now.ToString();

        public string DateBirch
        {
            get { return _dateBirch; }
            set { _dateBirch = value; OnPropertyChanged("DateBirch"); }
        }

        private string _gender = "";

        public string Gender
        {
            get { return _gender; }
            set { _gender = value; OnPropertyChanged("Gender"); }
        }


        private string _login = "";

        public string Login
        {
            get { return _login; }
            set { _login = value; OnPropertyChanged("Login"); }
        }


        private bool _editAll;

        public bool IsEditAll
        {
            get { return _editAll; }
            set
            { 
                _editAll = value;
                ViewComponent(value);
            }
        }

        private void ViewComponent(bool value)
        {
            if (value)
            {
                content_EditButton.IsEnabled = false;
                editing_ButtonViewer.Visibility = Visibility.Visible;
                IsEditFIO = true;
                IsEditDate = true;
                IsEditGender = true;
                IsEditPassword = true;
            }
            else 
            {
                IsEditFIO = false;
                IsEditDate = false;
                IsEditGender = false;
                IsEditPassword = false;

                content_EditButton.IsEnabled = true;

                editing_ButtonViewer.Visibility = Visibility.Collapsed;


                edit_OldPasswordBox.Clear();
                edit_PasswordBox.Clear();
                edit_PasswordBox_Verty.Clear();

            }
        }



  

        public bool IsEditFIO
        {
            get { return _edit_fio; }

            set 
            {
                _edit_fio = value;

                if (value)
                {
                    _user_fio = UserName;
                    edit_FIO_box.Visibility = Visibility.Visible;
                    title_FIO.Visibility = Visibility.Collapsed;

                    changer_FIO_Apply.Visibility = Visibility.Visible;
                    changer_FIO_Cancel.Visibility = Visibility.Visible;
                    changer_FIO_Edit.Visibility = Visibility.Collapsed;
                }
                else
                {
                    edit_FIO_box.Visibility = Visibility.Collapsed;
                    title_FIO.Visibility = Visibility.Visible;

                    changer_FIO_Apply.Visibility = Visibility.Collapsed;
                    changer_FIO_Cancel.Visibility = Visibility.Collapsed;
                    changer_FIO_Edit.Visibility = Visibility.Visible;

                    UserName = _user_fio;
                }
            }
        }
        public bool IsEditDate
        {

            get { return _edit_date; }
            set
            {
                _edit_date = value;

                if (value)
                {
                    _user_date = DateBirch;
                    edit_DatebirchUser.Visibility = Visibility.Visible;
                    title_DateBirch.Visibility = Visibility.Collapsed;

                    changer_Date_Apply.Visibility = Visibility.Visible;
                    changer_Date_Cancel.Visibility = Visibility.Visible;
                    changer_Date_Edit.Visibility = Visibility.Collapsed;
                }
                else
                {
                     DateBirch = _user_date;
                    edit_DatebirchUser.Visibility = Visibility.Collapsed;
                    title_DateBirch.Visibility = Visibility.Visible;

                    changer_Date_Apply.Visibility = Visibility.Collapsed;
                    changer_Date_Cancel.Visibility = Visibility.Collapsed;
                    changer_Date_Edit.Visibility = Visibility.Visible;
                }
            }         
        }
        public bool IsEditGender
        {

            get { return _edit_gender; }
            set
            {
                _edit_gender = value;

                if (value)
                {
                    _user_gender = Gender;
                    edit_GenderUser.Visibility = Visibility.Visible;
                    title_Gender.Visibility=Visibility.Collapsed;

                    changer_Gender_Apply.Visibility = Visibility.Visible;
                    changer_Gender_Cancel.Visibility = Visibility.Visible;
                    changer_Gender_Edit.Visibility = Visibility.Collapsed; 
                }
                else
                {
                    Gender = _user_gender;


                    edit_GenderUser.Visibility = Visibility.Collapsed;
                    title_Gender.Visibility = Visibility.Visible;

                    changer_Gender_Apply.Visibility = Visibility.Collapsed;
                    changer_Gender_Cancel.Visibility = Visibility.Collapsed;
                    changer_Gender_Edit.Visibility = Visibility.Visible;
                }
            }
        }
        public bool IsEditPassword
        {
            get { return _edit_password; }
            set
            {
                _edit_password = value;

                if (value)
                {
                    edit_PasswordPanel.Visibility = Visibility.Visible;
                    changer_Password_Apply.Visibility = Visibility.Visible;
                    changer_Password_Cancel.Visibility = Visibility.Visible;
                    changer_Password_Edit.Visibility = Visibility.Collapsed;
                    title_Password.Visibility = Visibility.Collapsed;
                }
                else 
                {
                    edit_PasswordPanel.Visibility = Visibility.Collapsed;
                    RandomPasswordChar();

                    changer_Password_Apply.Visibility = Visibility.Collapsed;
                    changer_Password_Cancel.Visibility = Visibility.Collapsed;
                    changer_Password_Edit.Visibility = Visibility.Visible;
                    title_Password.Visibility = Visibility.Visible;

                }
            }
        }

        public GUI_User_Viewer(User _user)
        {
            InitializeComponent();
            SetDateUser(_user);
        }


        public GUI_User_Viewer(int index, ViewModal_CRV_User viewModal_CRV_User)
        {
            InitializeComponent();
            _viewModal_CRV_User = viewModal_CRV_User;
            SendPacket(index);
        }

        public GUI_User_Viewer(UserCard card)
        {
            InitializeComponent();
            _userCard = card;
            SendPacket(_userCard.Id);
        }

        public GUI_User_Viewer(int index)
        {
            InitializeComponent();         
            SendPacket(index);
        }

        private void SendPacket(int index)
        {
            var user = new Data_UserList()
            {
                Id = index,
            };

            var packet = new Data_FirstCommand
            {
                Command = "Command_GetUserInformationByIndex",
                Json=JsonSerializer.Serialize(user)
            };


            _Main.Instance.Client.Send(JsonSerializer.Serialize(packet));
            _Main.Instance.OverlayShow(true, TypeOverlay.loading, "Подождите...", "Получаю данные с сервера");
        }

        public void SetDateUser(User _user)
        {
          
            UserRole = _user.Role;
            UserName = _user.FIO;
            _indexUser = _user.Index;


            Gender = _user.Gender;
            DateBirch = _user.Datebirch;
            Login = _user.Login;


            WindowsAssist.SetUCTitle(this, $"Просмотр данных пользователя: {UserName}");

            RandomPasswordChar();


            if (_Main.Instance.MyAccount.AllRoly)
                admin_deleteUser.Visibility = Visibility.Visible;

        }

        private void RandomPasswordChar()
        {
            Random r = new Random();
            title_Password.Text = string.Empty;
            for (int i = 0; i < r.Next(10,30); i++)
                title_Password.Text += "*";
        }

        public GUI_User_Viewer()
        {
            InitializeComponent();

            if (_Main.Instance.MyAccount.AllRoly)
                admin_deleteUser.Visibility = Visibility.Visible;

            _indexUser = _Main.Instance.MyAccount.ID;
            _me = true;
            RandomPasswordChar();
            Uid = "32_1";
            WindowsAssist.SetUCTitle(this, $"Просмотр данных пользователя: {_Main.Instance.MyAccount.GetName()}");
            UserRole = _Main.Instance.MyAccount.GetRole();
            UserName = _Main.Instance.MyAccount.GetName();
            Gender = _Main.Instance.MyAccount.Gender;
            DateBirch = _Main.Instance.MyAccount.Datebirch;
            Login = _Main.Instance.MyAccount.Login;

        }

        private void changer_FIO_Edit_Click(object sender, RoutedEventArgs e)
        {
            IsEditFIO = true;
        }

        private void changer_FIO_Cancel_Click(object sender, RoutedEventArgs e)
        {
            IsEditFIO = false;
        }

        private void changer_Date_Edit_Click(object sender, RoutedEventArgs e)
        {
            IsEditDate = true;
        }

        private void changer_Date_Cancel_Click(object sender, RoutedEventArgs e)
        {
            IsEditDate = false;
        }

        private void changer_Gender_Edit_Click(object sender, RoutedEventArgs e)
        {
            IsEditGender = true;
        }

        private void changer_Gender_Cancel_Click(object sender, RoutedEventArgs e)
        {
            IsEditGender = false;
        }

        private void changer_Password_Edit_Click(object sender, RoutedEventArgs e)
        {
            IsEditPassword = true;
        }

        private void changer_Password_Cancel_Click(object sender, RoutedEventArgs e)
        {
            IsEditPassword = false;
        }

        private void edit_SaveAll_Click(object sender, RoutedEventArgs e)
        {
            if (MessageShow.Show("Изменить данные?", "Редактирование", MessageShow.Type.Question) == true)
            {
                var fio = edit_FIO_box.Text;
                var date = edit_DatebirchUser.Text;
                var gender = edit_GenderUser.Text;

                string error = "";

                if (fio == string.Empty) error += "Заполните поле 'ФИО'\n";
                if (date == string.Empty) error += "Заполните поле 'Дата рождения'\n";
                if (gender == string.Empty) error += "Заполните поле 'Пол'\n";

          


                var packet = new Data_UserEdit()
                {
                    FIO = fio,
                    DateBirch=date,
                    Gender=gender,
                    IsEditDateBirch=true, 
                    IsEditFIO = true,
                    IsEditGender=true,
                };


                if (edit_OldPasswordBox.Password != string.Empty)
                {
                    var newPass = edit_PasswordBox.Password;
                    var vertyPass = edit_PasswordBox_Verty.Password;

                    if (newPass == string.Empty) error += "Поле 'новый пароль' должно быть заполнено\n";
                    if (vertyPass == string.Empty) error += "Поле 'проверки пароля' должно быть заполнено\n";

                    if (newPass != vertyPass)
                    {
                        error += "Пароли не совпадают. Проверьте правильность ввода\n";
                    }
                    else
                    {
                        packet.OldPassword = edit_OldPasswordBox.Password;
                        packet.NewPassword = edit_PasswordBox.Password;
                        packet.IsEditPassword = true;
                    }

                }
                else
                {
                    packet.IsEditPassword = false;
                    packet.OldPassword = string.Empty;
                    packet.NewPassword = string.Empty;
                }

                if (error != string.Empty)
                {
                    MessageShow.Show($"Возникла ошибка:\n{error}", "Ошибка", MessageShow.Type.Error);
                    return;
                }



                if (_viewModal_CRV_User != null)
                {
                    _viewModal_CRV_User.SaveData(_indexUser, edit_FIO_box.Text, edit_GenderUser.Text, edit_DatebirchUser.Text);
                }


                if (_userCard != null)
                {
                    _userCard.NameUser = edit_FIO_box.Text;
                    _userCard.DateBirch = edit_DatebirchUser.Text;
                    _userCard.Gender = edit_GenderUser.Text;
                }


                Send(packet);


            }
        }

        private void edit_Cancel_Click(object sender, RoutedEventArgs e)
        {
            IsEditAll = false;
        }

        private void content_EditButton_Click(object sender, RoutedEventArgs e)
        {
            IsEditAll = true;
        }

        private void changer_FIO_Apply_Click(object sender, RoutedEventArgs e)
        {
            if (MessageShow.Show("Изменить данные?", "Редактирвоание", MessageShow.Type.Question) == true)
            {
                var fio = edit_FIO_box.Text;

                if (fio != string.Empty)
                {

                    var packet = new Data_UserEdit()
                    {
                        FIO = fio,
                        IsEditFIO = true,
                    };

                    Send(packet);


                    if (_viewModal_CRV_User != null)
                    {
                        _viewModal_CRV_User.SaveData(_indexUser, edit_FIO_box.Text, edit_GenderUser.Text, edit_DatebirchUser.Text);
                    }


                    if (_userCard != null)
                    {
                        _userCard.NameUser = edit_FIO_box.Text;
                        _userCard.DateBirch = edit_DatebirchUser.Text;
                        _userCard.Gender = edit_GenderUser.Text;
                    }

                }
                else
                    _Main.Instance._Notification.Add("", "Поле не может быть пустым", TypeNotification.Error);
            }
        }

        private void Send(Data_UserEdit packet)
        {
            _Main.Instance.OverlayShow(true, TypeOverlay.loading, "Сохранение..");

            packet.IndexUser = _indexUser;

            var command = new Data_FirstCommand()
            {
                Command = "Command_UserEdit",
                Json = JsonSerializer.Serialize(packet),
            };

            _Main.Instance.Client.Send(JsonSerializer.Serialize(command));
        }

        private void changer_Date_Apply_Click(object sender, RoutedEventArgs e)
        {
            if (MessageShow.Show("Изменить данные?", "Редактирвоание", MessageShow.Type.Question) == true)
            {
                var date = edit_DatebirchUser.Text;

                if (date != string.Empty)
                {
                    var packet = new Data_UserEdit()
                    {
                        DateBirch = date,
                        IsEditDateBirch = true,
                    };

                    Send(packet);


                    if (_viewModal_CRV_User != null)
                    {
                        _viewModal_CRV_User.SaveData(_indexUser, edit_FIO_box.Text, edit_GenderUser.Text, edit_DatebirchUser.Text);
                    }


                    if (_userCard != null)
                    {
                        _userCard.NameUser = edit_FIO_box.Text;
                        _userCard.DateBirch = edit_DatebirchUser.Text;
                        _userCard.Gender = edit_GenderUser.Text;
                    }


                }
                else
                    _Main.Instance._Notification.Add("", "Поле не может быть пустым", TypeNotification.Error);
            }
        }

        private void changer_Gender_Apply_Click(object sender, RoutedEventArgs e)
        {
            if (MessageShow.Show("Изменить данные?", "Редактирвоание", MessageShow.Type.Question) == true)
            {
                var gender = edit_GenderUser.Text;

                if (gender != string.Empty)
                {
                    var packet = new Data_UserEdit()
                    {
                        Gender = gender,
                        IsEditGender = true,
                    };

                    Send(packet);


                    if (_viewModal_CRV_User != null)
                    {
                        _viewModal_CRV_User.SaveData(_indexUser, edit_FIO_box.Text, edit_GenderUser.Text, edit_DatebirchUser.Text);
                    }


                    if (_userCard != null)
                    {
                        _userCard.NameUser = edit_FIO_box.Text;
                        _userCard.DateBirch = edit_DatebirchUser.Text;
                        _userCard.Gender = edit_GenderUser.Text;
                    }
                }
                else
                    _Main.Instance._Notification.Add("", "Поле не может быть пустым", TypeNotification.Error);
            }
        }

        private void changer_Password_Apply_Click(object sender, RoutedEventArgs e)
        {
            if (MessageShow.Show("Изменить данные?", "Редактирвоание", MessageShow.Type.Question) == true)
            {


                if (edit_OldPasswordBox.Password != string.Empty)
                {
                    var newPass = edit_PasswordBox.Password;
                    var vertyPass = edit_PasswordBox_Verty.Password;

                    string error = string.Empty;

                    if (newPass == string.Empty) error += "Поле 'новый пароль' должно быть заполнено\n";
                    if (vertyPass == string.Empty) error += "Поле 'проверки пароля' должно быть заполнено\n";

                    if (newPass != vertyPass)
                    {
                        error += "Пароли не совпадают. Проверьте правильность ввода\n";
                    }


                    if (error != string.Empty)
                    {
                        MessageShow.Show($"Возникла ошибка:\n{error}", "Ошибка", MessageShow.Type.Error);
                        return;
                    }

                }
                else 
                {
                    _Main.Instance._Notification.Add("", "Поле старый пароль не может быть пустым", TypeNotification.Error);
                    return;
                }


                var packet = new Data_UserEdit()
                {
                    OldPassword = edit_OldPasswordBox.Password,
                    NewPassword = edit_PasswordBox.Password,
                    IsEditPassword = true
                };

                Send(packet);
            }
        }

        internal void SaveData()
        {
            if (IsEditDate)
            {
                _user_date = edit_DatebirchUser.Text;
                IsEditDate = false;
            }

            if (IsEditFIO)
            {
                _user_fio = edit_FIO_box.Text;
                IsEditFIO = false;
            }

            if (IsEditGender)
            {
              _user_gender = edit_GenderUser.Text;
               IsEditGender = false;
            }

      


            if (_me)
            {
                _Main.Instance.MyAccount.Datebirch = _user_date;
                _Main.Instance.MyAccount.Gender = _user_gender;

                var mas = _user_fio.Split(' ');

                if (mas.Length > 2)
                {
                    _Main.Instance.MyAccount.Surname = mas[0];
                    _Main.Instance.MyAccount.Firstname = mas[1];
                    _Main.Instance.MyAccount.Middlemane = mas[2];
                }
                else
                {
                    _Main.Instance.MyAccount.Surname = mas[0];
                    _Main.Instance.MyAccount.Firstname = mas[1];
                    _Main.Instance.MyAccount.Middlemane = "";
                }


                WindowsAssist.SetUCTitle(this, $"Просмотр данных пользователя: {_Main.Instance.MyAccount.GetName()}");         
     

                var meData = (_Main.Instance.MainBody.Children[0] as View_BodyApplication).titleAccount.Children[0] as AccountInformationCard;
                if (meData != null)
                {
                    meData.AccountName = UserName;
                }

            }
            else
            {
                WindowsAssist.SetUCTitle(this, $"Просмотр данных пользователя: {UserName}");

            }


            _Main.Instance.Manager.SetTitle(this);
            IsEditPassword = false;

            content_EditButton.IsEnabled = true;
     
            editing_ButtonViewer.Visibility = Visibility.Collapsed;
        }

        private void admin_deleteUser_Click(object sender, RoutedEventArgs e)
        {

            if(_Main.Instance.MyAccount.ID == _indexUser)
            {
                MessageShow.Show("Себя нельзя удалять.", "Удаление", MessageShow.Type.Error);
                return;
            }


            if(MessageShow.Show("Удалить пользователя?","Удаление",MessageShow.Type.Question) == true) 
            {
              

               _Main.Instance.OverlayShow(true, TypeOverlay.loading, "Удаление..");

                var delete = new Data_UserEdit_Delete()
                {
                    IndexUser = _Main.Instance.MyAccount.ID,
                    IndexDeletedUser = _indexUser
                };

                var command = new Data_FirstCommand()
                {
                    Command = "Command_UserEdit_Delete",
                    Json = JsonSerializer.Serialize(delete),
                };

                _Main.Instance.Client.Send(JsonSerializer.Serialize(command));

            }
        }

        private void root_Loaded(object sender, RoutedEventArgs e)
        {
            if (_Main.Instance.MyAccount.AllRoly) { return; } else accountData.Visibility= Visibility.Collapsed;


            if (_indexUser != _Main.Instance.MyAccount.ID) 
            {
                if (_Main.Instance.MyAccount.ReadUser)
                {
                    content_EditingPanel.Visibility = Visibility.Visible;
                    changer_FIO_Edit.Visibility = Visibility.Visible;
                    changer_Date_Edit.Visibility = Visibility.Visible;
                    changer_Gender_Edit.Visibility = Visibility.Visible;
                    changer_Password_Edit.Visibility = Visibility.Visible;
                }
                else
                {
                    content_EditingPanel.Visibility = Visibility.Collapsed;
                    changer_FIO_Edit.Visibility = Visibility.Collapsed;
                    changer_Date_Edit.Visibility = Visibility.Collapsed;
                    changer_Gender_Edit.Visibility = Visibility.Collapsed;
                    changer_Password_Edit.Visibility = Visibility.Collapsed;
                }
            }
            else accountData.Visibility = Visibility.Visible;
        }
    }
}
