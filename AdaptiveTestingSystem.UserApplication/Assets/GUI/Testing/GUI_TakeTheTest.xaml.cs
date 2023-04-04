using AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage;
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

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing
{
    /// <summary>
    /// Логика взаимодействия для GUI_TakeTheTest.xaml
    /// </summary>
    public partial class GUI_TakeTheTest : UserControl
    {

        ViewTestModel viewTesting;

        public GUI_TakeTheTest()
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
                searchBox.Focus();
            }

        }

        private void root_Loaded(object sender, RoutedEventArgs e)
        {
            Predmet.Items.Add(new PopupItemControl() {Caption = "Все"});
            Predmet.SelectedText = "Все";
            Predmet.SelectionChanged += Predmet_SelectionChanged;


            viewTesting.Update += ViewTesting_Update; 
            viewTesting.OverlayShowing += ViewTesting_OverlayShowing; 
            viewTesting.OverlayChangeInformation += ViewTesting_OverlayChangeInformation; 
            viewTesting.ViewerInformationTestng += ViewTesting_ViewerInformationTestng;
            viewTesting.UpdatePredmetViewer += ViewTesting_UpdatePredmetViewer;
            viewTesting.IsView = true;
            viewTesting.OnUpdate();
     
        }

        private void Predmet_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var selectItem = Predmet.Items[Predmet.SelectedIndex];
            if (selectItem != null)
                viewTesting.SetPredmet(selectItem.Caption);
       
        }

        private void ViewTesting_UpdatePredmetViewer(string predmet)
        {
            var item = Predmet.Items.Find(o=> o.Caption.Contains(predmet));
            if (item == null)
            {
                Predmet.Items.Add(new PopupItemControl() { Caption = predmet });
            }
        }

        private void ViewTesting_ViewerInformationTestng(MVVM.Model.Testing testing)
        {
            _Main.Instance.UI_TestReady = new GUI_TestReady();
            _Main.Instance.UI_TestReady.Uid = (Guid.NewGuid().ToString());
            _Main.Instance.UI_TestReady.SetData(testing.NameTest, testing.Index, testing.CountQuest);
            _Main.Instance.Hide();
            _Main.Instance.UI_TestReady.ShowDialog();
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

            if (_Main.Instance.IsVisible)
            {
                _Main.Instance.OverlayShow(!skipCheck, TypeOverlay.loading, title: "Список тестов", subtitle: "загрузка...");
                await Task.Delay(250);
                var send = new Data_FirstCommand()
                {
                    Command = "Command_GetAllTest",
                };
                _Main.Instance.Client.Send(JsonSerializer.Serialize(send));
            }
        }

        private void root_Unloaded(object sender, RoutedEventArgs e)
        {
            viewTesting.Update -= ViewTesting_Update; ;
            viewTesting.OverlayShowing -= ViewTesting_OverlayShowing; 
            viewTesting.OverlayChangeInformation -= ViewTesting_OverlayChangeInformation;        
            viewTesting.ViewerInformationTestng -= ViewTesting_ViewerInformationTestng;
            viewTesting.UpdatePredmetViewer -= ViewTesting_UpdatePredmetViewer;
            Predmet.SelectionChanged -= Predmet_SelectionChanged;
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

    
    }
}
