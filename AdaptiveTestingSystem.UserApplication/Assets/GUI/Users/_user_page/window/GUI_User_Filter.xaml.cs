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

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Users._user_page.window
{
    /// <summary>
    /// Логика взаимодействия для GUI_User_Filter.xaml
    /// </summary>
    public partial class GUI_User_Filter : Window
    {

        private bool isDataEditEnable;

        public bool IsDataEditEnable
        {
            get { return isDataEditEnable; }
            set { isDataEditEnable = value; _Main.Instance.customPropertyChanged.OnPropertyChanged("IsDataEditEnable"); }
        }


        ViewUserModel viewUserModel;
        Code IsRole;
        public GUI_User_Filter(ViewUserModel userModel, Code isRole)
        {
            InitializeComponent();
            this.viewUserModel = userModel;
            this.IsRole = isRole;
            //Установка значений фильтра
            SetVarable();
        }

        private void SetVarable()
        {
            switch (viewUserModel.FilterUser.GenderUser)
            {
                case StructfilterUser.GenderUserSelected.all:
                    gender_All.IsChecked = true;
                    break;
                case StructfilterUser.GenderUserSelected.male:
                    gender_Male.IsChecked = true;
                    break;
                case StructfilterUser.GenderUserSelected.female:
                    gender_Fmale.IsChecked = true;
                    break;
            }

            if(viewUserModel.FilterUser.DateBirch_To!=DateTime.MinValue) filter_DatebirchUser_To.SelectedDate = viewUserModel.FilterUser.DateBirch_To;
            if(viewUserModel.FilterUser.DateBirch_From!=DateTime.MinValue) filter_DatebirchUser_From.SelectedDate = viewUserModel.FilterUser.DateBirch_From;

            filter_Date_noInclude.IsChecked = viewUserModel.FilterUser.IsDateFilter;
        }

        private void HeaderControl_CloseClick()
        {
            Close();
        }

        private void filter_Date_noInclude_Checked(object sender, RoutedEventArgs e)
        {
            filter_Date_noInclude.ToolTip = "Не включать в фильтр дату рождения";
            IsDataEditEnable = true;
        }

        private void filter_Date_noInclude_Unchecked(object sender, RoutedEventArgs e)
        {
            filter_Date_noInclude.ToolTip = "Включать в фильтр дату рождения";
            IsDataEditEnable = false;
        }

        private async void filterButton_Click(object sender, RoutedEventArgs e)
        {
            var filter = new Date_FilterUser()
            {
                Gender = SelectGender(),
            };

            if (IsDataEditEnable)
            {

                if (filter_DatebirchUser_From.Text == string.Empty || filter_DatebirchUser_To.Text == string.Empty)
                {
                    MessageShow.Show("Поля дат не могут быть пустыми.", "Возникла ошибка", MessageShow.Type.Error);
                    return;
                }

       
                filter.From = DateTime.Parse(filter_DatebirchUser_From.Text);
                filter.To = DateTime.Parse(filter_DatebirchUser_To.Text);

    
                string error = CheckedDate(filter);

                if (error.Trim().Length > 0)
                {
                    MessageShow.Show(error, "Возникла ошибка", MessageShow.Type.Error);
                    return;
                }

                filter.IsFilterData = IsDataEditEnable;
            }


            _Main.Instance.OverlayShow(true, TypeOverlay.loading, title: "Пользователи", subtitle: "загрузка фильтра...");
            await Task.Delay(250);


            var viewer = new Data_UserPacket()
            {
                IsRoleUser = IsRole,
                IsFilter = true,
                FilterUser = filter,
                IsCode = Code.ThreadStart
            };

            var send = new Data_FirstCommand()
            {
                Command = "Command_GetFilterUserList",
                Json = JsonSerializer.Serialize(viewer)
            };
            _Main.Instance.Client.Send(JsonSerializer.Serialize(send));

            Close();
        }

        private string CheckedDate(Date_FilterUser filter)
        {
            string error = "";
            if (filter.From > DateTime.Now)
            {
                error += "Дата в поле 'ОТ' не может быть больше сегодняшней\n";
            }

            if (filter.To > DateTime.Now)
            {
                error += "Дата в поле 'До' не может быть больше сегодняшней\n";
            }

            if (filter.From > filter.To)
            {
                error += $"Дата в поле 'От':{filter.From.ToShortDateString()} не может быть больше 'До':{filter.To.ToShortDateString()}\n";
            }

            return error;
        }

        private int SelectGender()
        {
            if (gender_Male.IsChecked == true) return 1;
            if (gender_Fmale.IsChecked == true) return 2;
            return 3;
        }
    }
}
