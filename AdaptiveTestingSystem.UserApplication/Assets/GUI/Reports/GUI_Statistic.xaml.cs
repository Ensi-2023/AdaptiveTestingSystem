using AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports._gui_subpage;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports._gui_subpage.CScript;
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


namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports
{
    /// <summary>
    /// Логика взаимодействия для GUI_Statistic.xaml
    /// </summary>
    public partial class GUI_Statistic : UserControl
    {
        StatisticCreate Statistic;
        bool IsUpload { get; set; } = false;
        public bool IsNoMouseScroll { get; private set; }

        public GUI_Statistic()
        {
            InitializeComponent();
        }

        public void SetChild(UIElement element)
        {
            Body.Children.Clear();
            Body.Children.Add(element);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Statistic = new StatisticCreate();
        }

        private async void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var list = (ListBox)sender;
            _Main.Instance.IsEnabled = false;
            switch (list.SelectedIndex)
            {
                case 0: SetChild(new _gui_subpage.GUI_Report_Page1()); break;
                case 1: SetChild(new _gui_subpage.GUI_Report_Page2 ()); break;
            }

        }

        public void SetData(Data_StatisticGeneral obj,Data_StatisticCustom custom)
        {
            _Main.Instance.OverlayShow(false);
            Statistic.Create(obj, custom, Body);
        }

        public void SetDataOverlay()
        {
            StatisticCreate.IsUpload = true;
        }

        public void CloseUpload()
        {
            StatisticCreate.IsUpload = false;
        }

        public void SetInfo(double packetSize, double maxSize)
        {
            Statistic.SetInfo(packetSize, maxSize, Body);
        }

        public void SetError()
        {
            Statistic.SetError(Body);
        }

        public bool IsCancelUpload()
        {
            return Statistic.IsCanceling(Body);
        }

        private void ScrollViewer_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Body.Children.Count > 0)
            {
                var child = Body.Children[0] as GUI_Report_Page1;
                if (child != null)
                {
                    if (child.IsNoMouseScroll)
                    {
                        var obj = sender as ScrollViewer;
                        if (obj != null) 
                        {
                            obj.CanContentScroll= true;
                        }
                    }
                }
            }
        }

        private void ScrollViewer_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {
            Logger.Debug($"scroll");
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
          if(_Main.Instance.KeyPress == Key.LeftShift) e.Handled= true;          
        }


    }
}
