using AdaptiveTestingSystem.UserApplication.Assets.GUI.ClassRoom._classRoom_page._classRoom_mvvm;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Subject._subject_mvvm;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Subject._subject_subpage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Users._user_page
{
    /// <summary>
    /// Логика взаимодействия для GUI_Insert.xaml
    /// </summary>
    public partial class GUI_Users_Insert : UserControl
    {

        private bool _isVisibleChangeClassRoom = false;
        private bool _subject = false;
        private int _indexClassRoom = 0;
        private int _indexSubject = 0;
        public List<UIElement> KlassesList { get; set; } = new List<UIElement>();

        public GUI_Users_Insert()
        {
            InitializeComponent();
        }


        public GUI_Users_Insert(bool isVisibleChangeClassRoom,int indexClassRoom)
        {
            InitializeComponent();

            if (isVisibleChangeClassRoom)
            {
                _isVisibleChangeClassRoom = isVisibleChangeClassRoom;
                _indexClassRoom = indexClassRoom;
                panelClassRoom.Visibility = Visibility.Collapsed;
                klassesBody.Visibility = Visibility.Collapsed;
                mainClassRoomPanel.Visibility = Visibility.Collapsed;
                klasses_GUI.Visibility = Visibility.Collapsed;
                classLine.Visibility = Visibility.Collapsed;
            }

        }

        public GUI_Users_Insert(bool isVisibleChangeClassRoom, int indexSubject,bool flag)
        {
            InitializeComponent();

            if (isVisibleChangeClassRoom)
            {
                _isVisibleChangeClassRoom = isVisibleChangeClassRoom;

                _indexSubject = indexSubject;
                panelClassRoom.Visibility = Visibility.Collapsed;
                klassesBody.Visibility = Visibility.Collapsed;
                mainClassRoomPanel.Visibility = Visibility.Collapsed;
                klasses_GUI.Visibility = Visibility.Collapsed;
                classLine.Visibility = Visibility.Collapsed;
                _subject = flag;
            }

        }


        public double MenyWidtch
        {
            get { return (double)GetValue(MenyWidtchProperty); }
            set { SetValue(MenyWidtchProperty, value); }
        }

        private bool _open;
        public bool IsOpen
        {
            get { return _open; }
            private set
            {

                _open = value;

                if (value)
                {

                    Animation.AnimatedWidth(klasses_GUI, klasses_GUI.ActualWidth, 380, TimeSpan.FromMilliseconds(260));
                    DLL.wpf.ButtonAssist.SetIcon(klassInfo, MahApps.Metro.IconPacks.PackIconMaterialKind.ArrowLeft);
                    Animation.AnimatedOpacity(klassesBody, klassesBody.Opacity, 1, TimeSpan.FromMilliseconds(260));

                }
                else
                {
                    Animation.AnimatedWidth(klasses_GUI, klasses_GUI.ActualWidth, 35, TimeSpan.FromMilliseconds(260));
                    DLL.wpf.ButtonAssist.SetIcon(klassInfo, MahApps.Metro.IconPacks.PackIconMaterialKind.ArrowRight);
                    Animation.AnimatedOpacity(klassesBody, klassesBody.Opacity, 0, TimeSpan.FromMilliseconds(260));
                }
            }
        }

        public void DeleteSelected(Data_Klass_Delete obj)
        {

            foreach (ToggleButton toggle in deleteTogless(obj))
            {
                studentKlasses.Children.Remove(toggle);
            }          


            List<ToggleButton> deleteTogless(Data_Klass_Delete obj)
            {
                var list = new List<ToggleButton>();

                foreach(var item in studentKlasses.Children)
                {
                    if ((item as ToggleButton).IsChecked == true)
                    {

                        var chbx = (item as CustomCheckBox);
                        var rdbx = (item as CustomRadioButton);

                        if (chbx != null)
                        {
                            var klasses = obj.Klasses.FirstOrDefault(o => (o.Id == chbx.ID));
                            if (klasses != null) continue;
                        }

                        if (rdbx != null)
                        {
                            var klasses = obj.Klasses.FirstOrDefault(o => (o.Id == rdbx.ID));
                            if (klasses != null) continue;
                        }

                        list.Add(item as ToggleButton);
                    }
                }
                return list;
            };
        }

        // Using a DependencyProperty as the backing store for MenyWidtch.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MenyWidtchProperty =
            DependencyProperty.Register("MenyWidtch", typeof(double), typeof(GUI_Users_Insert), new PropertyMetadata(380.0));




        public void Clear(ClassRoom._classRoom_page.GUI_ClassRoom_Viewer userviewer=null,int index = 0)
        {
            if (userviewer != null)
            {               
                (userviewer.MVVM as ViewModal_CRV_User).Add(index, FIOUser.Text, DatebirchUser.Text, GenderUser.Text);
            }

            ClearVariables();
        }


        public void Clearv2(GUI_Subject_Viewer userviewer = null, int index = 0)
        {
            if (userviewer != null)
            {
                (userviewer.MVVM as ViewModal_SV_User).Add(index, FIOUser.Text, DatebirchUser.Text, GenderUser.Text);
            }

            ClearVariables();
        }



        private void ClearVariables()
        {
            FIOUser.Text = string.Empty;
            DatebirchUser.Text = string.Empty;
            GenderUser.Text = string.Empty;
            UserLogin.Text = string.Empty;
            UserPassword.Password = string.Empty;
            UserVertyPassword.Password = string.Empty;
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scrollviewer = sender as ScrollViewer;
            if (e.Delta > 0)
                scrollviewer.LineUp();
            else
                scrollviewer.LineDown();
        }

        private async void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {

            var fio = FIOUser.Text.Split(' ');
            var sur = "";
            var last = "";
            var midd = "";
            var dateB = DatebirchUser.Text;
            var gender = GenderUser.Text;

            var login = UserLogin.Text;
            var pass = UserPassword.Password;
            var passVerty = UserVertyPassword.Password;


            var error = "";

            if (fio.Length == 0)
            {
                error += "Не заполнено поле 'Фамилия'\n";
            }
            else sur = fio[0];

            if (fio.Length < 2)
            {
                error += "Не заполнено поле 'Имя'\n";                      
            }
            else last = fio[1];

            if(fio.Length>2) midd = fio[2];

            if (ParserVariables.CountSymbol(dateB) == 0)
            {
                error += "Не заполнено поле 'Дата рождения'\n";
            }
            else
            {
                if (ParserVariables.GetDate(dateB) > DateTime.Now)
                {
                    error += "Дата не может быть больше сегодняшней\n";
                }
            }

            if (ParserVariables.CountSymbol(gender) == 0)
            {
                error += "Не заполнено поле 'Пол'\n";
            }

            if (ParserVariables.CountSymbol(login) == 0)
            {
                error += "Не заполнено поле 'Логин'\n";
            }

            if (ParserVariables.CountSymbol(pass) == 0 || ParserVariables.CountSymbol(passVerty) == 0)
            {
                error += "Не заполнено поле 'Пароль' или 'Повторите пароль'\n";
            }
            else
            {
                if (pass != passVerty)
                {
                    error += "Пароли не совпадают!\n";
                }
            }

            if (ParserVariables.CountSymbol(error) > 0)
            {
                MessageShow.Show(error);
                return;
            }


            ///Нужно сделать код для добавления нового
            ///пользователя из другой формы
            ///форма не будет закрываться и не будет сильно взаимодействовать с основной формой
            ///будет только добавлять нового ученика в случае успешного добавления
            ///на данный момент происходит добавление и закрытие формы


            Code code;

            if (_isVisibleChangeClassRoom == false && student.IsChecked == true)
            {
                code = Code.NewUserInsert;
            }
            else if (_isVisibleChangeClassRoom == false && student.IsChecked == false)
            {
                code = Code.NewStaffInsert;
            }
            else
            {
                //Код добавления для новой формы.
                if (_subject) { code = Code.Subject_Insert_User; } else  code = Code.NewUserClassRoomInsert;         
            }


           
            var obj = new Data_NewUserInsert()
            {
                Login = login,
                Password = pass,
                DatebirchUser = dateB,
                GenderUser = gender,
                LastnameUser = last,
                MiddlenameUser = midd,
                SurnameUser = sur,
                IsCode = code,
                Klasses = GetSelectedKlasses(),
                Subject = _indexSubject
            };

            var send = new Data_FirstCommand()
            {
                Command = "Command_NewUserInsert",
                Json = JsonSerializer.Serialize(obj)
            };
            _Main.Instance.OverlayShow(true, TypeOverlay.loading, "Подождите...", "Идет добавление пользователя");
            await Start(JsonSerializer.Serialize(send));




            List<int> GetSelectedKlasses()
            {
                var list = new List<int>();
                if(_isVisibleChangeClassRoom)
                {
                    list.Add(_indexClassRoom);
                    return list;
                }


                foreach (var item in studentKlasses.Children)
                {
                    if ((item as ToggleButton).IsChecked == true)
                    {

                        var chbx = (item as CustomCheckBox);
                        var rdbx = (item as CustomRadioButton);

                        if (chbx != null)
                        {
                            list.Add(chbx.ID);
                            continue;
                        }

                        if (rdbx != null)
                        {
                            list.Add(rdbx.ID);
                        }
                       
                    }
                }
                return list;
            }

        }

        public async Task Start(string message)
        {
            await Task.Delay(150);
            _Main.Instance.Client.Send(message);
        }

        private void searchKlasses_TextChanged(object sender, TextChangedEventArgs e)
        {
            var component = sender as TextBox;
            if (component == null) return;

            if (component.Text.Trim().Length > 0)
            {
                Search(component.Text.Trim());
            }
            else
            {
                CancelSearch();
            }

        }

        private async void CancelSearch()
        {

            searchKlasses.Clear();
            studentKlasses.Children.Clear();
            foreach (var item in KlassesList.ToList())
            {
                studentKlasses.Children.Add(item);
                await Task.Delay(5);
            }


        }

        private void Search(string value)
        {
            studentKlasses.Children.Clear();
            foreach (ToggleButton item in KlassesList)
            {
                if (item.Content.ToString().Trim().ToLower().Contains(value.Trim().ToLower()))
                {
                    studentKlasses.Children.Add(item);
                }
            }
        }



        public void Overlay(bool value)
        {
            if (OverlayKlass == null) return;

            switch (value)
            {
                case true: OverlayKlass.Visibility = Visibility.Visible; studentKlasses.IsEnabled = false; break;
                case false: OverlayKlass.Visibility = Visibility.Collapsed; studentKlasses.IsEnabled = true; break;
            }
        }

        private async void student_Checked(object sender, RoutedEventArgs e)
        {



            titleKlass.Content = "Учится в: ";

            var klass = new Data_Klass()
            {
                IsCheking = false
            };

            var send = new Data_FirstCommand()
            {
                Command = "Command_GetKlassInfo",
                Json = JsonSerializer.Serialize(klass)
            };


            await Start(JsonSerializer.Serialize(send));
        }

        private async void student_Unchecked(object sender, RoutedEventArgs e)
        {

            titleKlass.Content = "Классный руководитель в: ";

            var klass = new Data_Klass()
            {
                IsCheking = true
            };

            var send = new Data_FirstCommand()
            {
                Command = "Command_GetKlassInfo",
                Json = JsonSerializer.Serialize(klass)
            };


            await Start(JsonSerializer.Serialize(send));
        }

        private void root_Loaded(object sender, RoutedEventArgs e)
        {
            student.IsChecked = true;
        }

        private void klassInfo_Click(object sender, RoutedEventArgs e)
        {
            IsOpen = !IsOpen;
        }

        private void InsertKlassData_Click(object sender, RoutedEventArgs e)
        {
            var insert = new window.GUI_User_Insert_ClassRoomInsert();
            insert.ShowDialog();
        }

        private async void DeleteKlassData_Click(object sender, RoutedEventArgs e)
        {

            var list = await GetSelectedKlassRoom(student.IsChecked);

            if (list.Count > 0)
            {
                if (MessageShow.Show($"Вы действительно желаете удалить #{list.Count} записей?", "Удаление", MessageShow.Type.Question) == true)
                {
                    var json = new Data_Klass_Delete()
                    {
                        IsCode = Code.GUI_User,
                        Klasses = list,
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

        private async Task<List<Data_Klass>> GetSelectedKlassRoom(bool? isChecked)
        {
            var list = new List<Data_Klass>();
            IsEnabled = false;
            foreach (var item in studentKlasses.Children)
            {
                if (isChecked == true)
                {
                    var obj = item as CustomRadioButton;
                    if (obj == null) continue;
                    if (obj.IsChecked == true)
                    {
                        list.Add(new Data_Klass() { Id = obj.ID,Name = obj.Content.ToString()});
                    }
                }
                else
                {
                    var obj = item as CustomCheckBox;
                    if (obj == null) continue;
                    if (obj.IsChecked == true)
                    {
                        list.Add(new Data_Klass() { Id = obj.ID, Name = obj.Content.ToString() });
                    }
                }
                await Task.Delay(1);
            }
            IsEnabled = true;
            return list;
        }

        private void clearData_Click(object sender, RoutedEventArgs e)
        {
            IsOpen= false;
            Clear();
        }
    }
}
