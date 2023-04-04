using AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing._testing_subpage.window;
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

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Testing
{
    /// <summary>
    /// Логика взаимодействия для GUI_TestingServerBrowser.xaml
    /// </summary>
    public partial class GUI_TestingServerBrowser : UserControl
    {
        ViewServerBrowserModel viewServer;

        public GUI_TestingServerBrowser()
        {
            InitializeComponent();
            DataContext = viewServer = _Main.Instance.MVVM_Manager.ServerBrowserModel;
        }

        public void Update()
        {
            viewServer.OnUpdate(true);
        }

        private void countView_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var obj = (ComboTextBox)sender;
            if (obj != null && viewServer != null)
            {
                viewServer.SetCountView(ParserVariables.GetInt(obj.Text));
            }
        }

        private void searchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var text = ((TextBox)sender).Text;
                viewServer.Search(text);
                searchBox.Focus();
            }

        }
        private void addNewTest_Click(object sender, RoutedEventArgs e)
        {
            _Main.Instance.Manager.Next(new GUI.Testing.GUI_MultyTestingServerCreate());
        }

        private void updateDB_Click(object sender, RoutedEventArgs e)
        {
            viewServer.OnUpdate();
        }


        private void ViewTesting_Update(bool skipCheck)
        {
            if (_Main.Instance.IsVisible)
            {
                _Main.Instance.OverlayShow(!skipCheck, TypeOverlay.loading, title: "Список тестов", subtitle: "загрузка...");
                var packet = new Data_ListPacketServer()
                { 
                    IsCode = Code.ThreadStart
                };
                ThreadManager.Send("Command_GetMultyServerTesting", packet);
            }
        }

        private void Predmet_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var selectItem = Predmet.Items[Predmet.SelectedIndex];
            if (selectItem != null)
                viewServer.SetPredmet(selectItem.Caption);

        }

        private void ViewTesting_OverlayChangeInformation(string firstValue, string lastValue)
        {
            _Main.Instance.OverlayShow(true, TypeOverlay.loading, title: "Список тестов", subtitle: $"Обработано: {firstValue} из {lastValue}");

        }

        private void ViewTesting_OverlayShowing(bool show)
        {
            _Main.Instance.OverlayShow(show);

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
            viewServer.ViewInformation.Execute(row.Item);
        }



        private void ViewTesting_ViewerInformationTestng(MVVM.Model.Testing testing)
        {

            if (testing.IsPassword == "Да")
            {
                var passCheck = new GUI_Password(testing.Password);
                if (passCheck.ShowDialog() == true)
                {
                    ConnectToServer(testing);
                }

            }
            else
            {
                ConnectToServer(testing);
            }
  
        }

        private static void ConnectToServer(MVVM.Model.Testing testing)
        {
            _Main.Instance.OverlayShow(true, TypeOverlay.loading, "Подключаюсь", "Ожидаю подтверждение подключения", visibleButton: Visibility.Visible);

            var connectTestingServer = new Data_ConnectTestingServer()
            {
                IndexServer = testing.IndexServer,
                Hash = testing.Password,
            };

            Data_FirstCommand data = new Data_FirstCommand()
            {
                Command = "Command_ConnectToServerThisIndexServer",
                Json = JsonSerializer.Serialize(connectTestingServer),
            };

            _Main.Instance.Client.Send(JsonSerializer.Serialize(data));
        }

        private void ViewTesting_UpdatePredmetViewer(string predmet)
        {
            var item = Predmet.Items.Find(o => o.Caption.Contains(predmet));
            if (item == null)
            {
                Predmet.Items.Add(new PopupItemControl() { Caption = predmet });
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        
            Predmet.Items.Add(new PopupItemControl() { Caption = "Все" });
            Predmet.SelectedText = "Все";
            Predmet.SelectionChanged += Predmet_SelectionChanged;


            viewServer.Update += ViewTesting_Update;
            viewServer.OverlayShowing += ViewTesting_OverlayShowing;
            viewServer.OverlayChangeInformation += ViewTesting_OverlayChangeInformation;
            viewServer.ViewerInformationTestng += ViewTesting_ViewerInformationTestng;
            viewServer.UpdatePredmetViewer += ViewTesting_UpdatePredmetViewer;
            viewServer.IsView = true;
            viewServer.OnUpdate();

        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

            viewServer.Update -= ViewTesting_Update;
            viewServer.OverlayShowing -= ViewTesting_OverlayShowing;
            viewServer.OverlayChangeInformation -= ViewTesting_OverlayChangeInformation;
            viewServer.ViewerInformationTestng -= ViewTesting_ViewerInformationTestng;
            viewServer.UpdatePredmetViewer -= ViewTesting_UpdatePredmetViewer;
            viewServer.IsView = false;

            ThreadManager.CloseActiveThread();
     
        }
    }
}
