using AdaptiveTestingSystem.Data;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage._testing_mini_mvvm;
using System;
using System.Collections.Generic;
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

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing
{
    /// <summary>
    /// Логика взаимодействия для GUI_MultyTestingServerCreate.xaml
    /// </summary>
    public partial class GUI_MultyTestingServerCreate : UserControl
    {
        VM_AllTestingViewer mvvm_AllTestingViewer;

        public GUI_MultyTestingServerCreate()
        {
            InitializeComponent();

            mvvm_AllTestingViewer = new VM_AllTestingViewer();
            DataContext = mvvm_AllTestingViewer;
        }

        private void updateDB_Click(object sender, RoutedEventArgs e)
        {

        }

        private void searchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var text = ((TextBox)sender).Text;
                mvvm_AllTestingViewer.Search(text);
            }
        }

        private void Row_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Ensure row was clicked and not empty space

            var row = DataGridHelper.GetRow(sender, e);
            if (row == null) return;
            mvvm_AllTestingViewer.ViewInformation(row.Item);
        }



        private void DataGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Ensure row was clicked and not empty space
            var row = DataGridHelper.GetRow(sender, e);
            if (row == null) return;

            if (row.IsSelected)
            {
              
            }

        }

        private void root_Loaded(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void Update()
        {
            _Main.Instance.OverlayShow(true, TypeOverlay.loading, title: "Загрузка данных..");

            var data = new Data_AllTestForSB()
            {
                IsCode = Code.ThreadStart
            };


            ThreadManager.Send("Command_GetAllTestForCreateServer", data);
                
        }

        public async void SetDataPacket(List<Data_AllTestForSB> obj)
        {
            if (mvvm_AllTestingViewer != null) await mvvm_AllTestingViewer.SetData(obj);
        }

        private void createServer_Click(object sender, RoutedEventArgs e)
        {

            string error = "";

            if (countQuest.Value == 0)
            {
                error += "Количество вопросов не должно быть равно 0\n";
            }

            if (mvvm_AllTestingViewer.SelectedTest == null)
            {
                error += "Выберите тест\n";
            }


            if (error.Trim()!=string.Empty)
            {
                _Main.Instance._Notification.Add("", error, TypeNotification.Error);
                return;
            }

            Data_MultyServer data = new Data_MultyServer()
            {
                IndexCreator = _Main.Instance.MyAccount.ID,
                IsAdaptive = _rbAdaptiveYes.IsChecked == true ? true : false,
                IndexTest = mvvm_AllTestingViewer.SelectedTest.Index,
                NameTest= mvvm_AllTestingViewer.SelectedTest.NameTest,
                Password = ServerPAssword.Password.Trim()==string.Empty?string.Empty:(ServerPAssword.Password),
                CountQuestForTesting = (int)countQuest.Value,
                IsCode = Code.ThreadStart
            };


            if (MessageShow.Show("Создать тест?","Создание",MessageShow.Type.Question) == true)
            {
                ThreadManager.Send("Command_AddMultyServerTesting", data);
            }

        }

        private void TestingGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = TestingGrid.SelectedItem as MV_AllTesting;
            if (item == null) return;
            int count = int.Parse(item.CountQuest);
            if (count >= 20 && count < 40)
            {
                countQuest.Minimum = 15;
                countQuest.Maximum = count;
                countQuest.Value = 15;
            }
            else

                if (count > 40)
            {
                countQuest.Minimum = 20;
                countQuest.Maximum = count;
                countQuest.Value = 20;
            }
            else
            {
                countQuest.Minimum = 5;
                countQuest.Maximum = count;
                countQuest.Value = 5;
            }

        }

        private void root_Unloaded(object sender, RoutedEventArgs e)
        {
          //  ThreadManager.CloseActiveThread();
        }
    }
}
