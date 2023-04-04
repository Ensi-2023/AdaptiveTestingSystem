using AdaptiveTestingSystem.UserApplication.Assets.GUI.Roly._roly_subpage;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Roly._roly_subpage.window;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Subject._subject_subpage;
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

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Roly
{
    /// <summary>
    /// Логика взаимодействия для GUI_roly.xaml
    /// </summary>
    public partial class GUI_Roly : UserControl
    {
        ViewRolyUserModel _userModel;

        public GUI_Roly()
        {
            InitializeComponent();
            DataContext = _userModel = _Main.Instance.MVVM_Manager.RolyModel;
        }

        private void countView_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var obj = (ComboTextBox)sender;
            if (obj != null && _userModel != null)
            {
                _userModel.SetCountView(ParserVariables.GetInt(obj.Text));
            }
        }
        public void Update()
        {
            _userModel.OnUpdate(true);
        }
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var text = ((TextBox)sender).Text;
                _userModel.Search(text);
            }
        }


        private void Row_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Ensure row was clicked and not empty space

            var row = DataGridHelper.GetRow(sender, e);
            if (row == null) return;
            _userModel.ViewInformation.Execute(row.Item);
        }



        private void DataGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Ensure row was clicked and not empty space
            var row = DataGridHelper.GetRow(sender, e);
            if (row == null) return;

            if (row.IsSelected) row.IsSelected = false;
        }


        private void root_Loaded(object sender, RoutedEventArgs e)
        {
            _userModel.Update += _userModel_Update;
            _userModel.OverlayShowing += _userModel_OverlayShowing; ;
            _userModel.OverlayChangeInformation += _userModel_OverlayChangeInformation; ;
            _userModel.DeleteObjects += _userModel_DeleteObjects; ;
            _userModel.ViewerInformationRolyUser += _userModel_ViewerInformationRolyUser; 
            _userModel.IsView = true;
            _userModel.OnUpdate();
        }

        private void _userModel_ViewerInformationRolyUser(RolyUser rolyUser)
        {
            _Main.Instance.Manager.Next(new GUI_RolyInfViewer(rolyUser.Index, rolyUser.Name));
        }

        private void _userModel_DeleteObjects(System.Collections.IList deleteList)
        {
            if (deleteList != null)
            {

                if (deleteList.Count == 0) return;

               
            }
        }

        private void _userModel_OverlayChangeInformation(string firstValue, string lastValue)
        {
            _Main.Instance.OverlayShow(true, TypeOverlay.loading, title: "Список ролей", subtitle: $"Обработано: {firstValue} из {lastValue}");
        }

        private void _userModel_OverlayShowing(bool show)
        {
            _Main.Instance.OverlayShow(show);
        }

        private async void _userModel_Update(bool skipCheck)
        {
            _Main.Instance.OverlayShow(!skipCheck, TypeOverlay.loading, title: "Список ролей", subtitle: "загрузка...");

            await Task.Delay(250);

            Data_Roly data = new Data_Roly()
            {
                IsCode = Code.ThreadStart
            };


            ThreadManager.Send("Command_GetRolyList", data);



        }

        private void root_Unloaded(object sender, RoutedEventArgs e)
        {
            _userModel.Update -= _userModel_Update;
            _userModel.OverlayShowing -= _userModel_OverlayShowing; ;
            _userModel.OverlayChangeInformation -= _userModel_OverlayChangeInformation; ;
            _userModel.DeleteObjects -= _userModel_DeleteObjects; ;
            _userModel.ViewerInformationRolyUser -= _userModel_ViewerInformationRolyUser;
            _userModel.IsView = false;

            ThreadManager.CloseActiveThread();
        }

        private void addNewRoly_Click(object sender, RoutedEventArgs e)
        {
            var wind = new GUI_AddNewRolyUser();
            wind.ShowDialog();
            _userModel.OnUpdate(false);
        }

        private void updateDB_Click(object sender, RoutedEventArgs e)
        {
            _userModel.OnUpdate();
        }
    }
}
