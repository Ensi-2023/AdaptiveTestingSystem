using AdaptiveTestingSystem.UserApplication.Assets.GUI.Subject._subject_subpage;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage;
using AdaptiveTestingSystem.UserApplication.Assets.MVVM.Model;
using System;
using System.Collections;
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
    /// Логика взаимодействия для GUI_AllTestViewer.xaml
    /// </summary>
    public partial class GUI_AllTestViewer : UserControl
    {

        ViewTestModel viewTesting;

        public GUI_AllTestViewer()
        {
            InitializeComponent();

            DataContext = viewTesting = _Main.Instance.MVVM_Manager.TestingModel;
        }

        public void Update()
        {
            viewTesting.OnUpdate(true);
        }

        private void countView_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var obj = (ComboTextBox)sender;
            if (obj != null && viewTesting != null)
            {
                viewTesting.SetCountView(ParserVariables.GetInt(obj.Text));
            }
        }

        private void searchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var text = ((TextBox)sender).Text;
                viewTesting.Search(text);
            }

        }

        private void root_Loaded(object sender, RoutedEventArgs e)
        {
            viewTesting.Update += ViewTesting_Update; ;
            viewTesting.OverlayShowing += ViewTesting_OverlayShowing; ;
            viewTesting.OverlayChangeInformation += ViewTesting_OverlayChangeInformation; ;
            viewTesting.DeleteObjects += ViewTesting_DeleteObjects; ;
            viewTesting.ViewerInformationTestng += ViewTesting_ViewerInformationTestng; ;
            viewTesting.IsView = true;
            viewTesting.OnUpdate();
        }

        private void ViewTesting_ViewerInformationTestng(MVVM.Model.Testing testing)
        {
            _Main.Instance.Manager.Next(new GUI_TestViewer(testing.Index, testing.NameTest, testing.NamePredmet));
        }


        private void SetDeleteClassRoomList(IList deleteList, out List<MVVM.Model.Testing> list, out List<Data_Testing> commandDeleteList)
        {
            list = deleteList.Cast<MVVM.Model.Testing>().ToList();
            commandDeleteList = new List<Data_Testing>();
            foreach (var item in list)
            {
                commandDeleteList.Add(new Data_Testing()
                {
                    Index = item.Index,            
                });
            }
        }
        private void ViewTesting_DeleteObjects(System.Collections.IList deleteList)
        {
            if (deleteList != null)
            {

                if (deleteList.Count == 0) return;


                List<MVVM.Model.Testing> list;
                List<Data_Testing> commandDeleteList;
                SetDeleteClassRoomList(deleteList, out list, out commandDeleteList);

                if (MessageShow.Show("Удалить тест этот ?\nВосстановить будет невозможно", "", MessageShow.Type.Question) == true)
                {
             
                    var obj = new Data_FirstCommand()
                    {
                        Command = "Command_DeleteTest",
                        Json = JsonSerializer.Serialize(commandDeleteList)
                    };

                    _Main.Instance.Client.Send(JsonSerializer.Serialize(obj));


                    viewTesting.DeleteData(deleteList);
                    viewTesting.OnUpdate();
                }


            }
        }

        private void ViewTesting_OverlayChangeInformation(string firstValue, string lastValue)
        {
            _Main.Instance.OverlayShow(true, TypeOverlay.loading, title: "Список тестов", subtitle: $"Обработано: {firstValue} из {lastValue}");

        }

        private void ViewTesting_OverlayShowing(bool show)
        {
            _Main.Instance.OverlayShow(show);

        }

        private async void ViewTesting_Update(bool skipCheck)
        {
            _Main.Instance.OverlayShow(!skipCheck, TypeOverlay.loading, title: "Список тестов", subtitle: "загрузка...");

            await Task.Delay(250);

            var send = new Data_FirstCommand()
            {
                Command = "Command_GetAllTest",
            };
            _Main.Instance.Client.Send(JsonSerializer.Serialize(send));
        }

        private void root_Unloaded(object sender, RoutedEventArgs e)
        {
            viewTesting.Update -= ViewTesting_Update; ;
            viewTesting.OverlayShowing -= ViewTesting_OverlayShowing; ;
            viewTesting.OverlayChangeInformation -= ViewTesting_OverlayChangeInformation; ;
            viewTesting.DeleteObjects -= ViewTesting_DeleteObjects; ;
            viewTesting.ViewerInformationTestng -= ViewTesting_ViewerInformationTestng; ;
            viewTesting.IsView = false;
        }


        private void RolyGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Ensure row was clicked and not empty space
            var row = DataGridHelper.GetRow(sender, e);
            if (row == null) return;

            if (row.IsSelected) row.IsSelected = false;
        }

        private void RolyGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Ensure row was clicked and not empty space

            var row = DataGridHelper.GetRow(sender, e);
            if (row == null) return;
            viewTesting.ViewInformation.Execute(row.Item);
        }


        private void addNewTest_Click(object sender, RoutedEventArgs e)
        {
            _Main.Instance.Manager.Next(new GUI_GeneratorTest());
        }

     
        private void updateDB_Click(object sender, RoutedEventArgs e)
        {
            viewTesting.OnUpdate();
        }

        private void editAnsw_Click(object sender, RoutedEventArgs e)
        {
            if (RolyGrid.SelectedItem != null)
            {
                _Main.Instance.Manager.Next(new GUI_GeneratorTest(true, (RolyGrid.SelectedItem as MVVM.Model.Testing).Index));
            }



        }
    }
}
