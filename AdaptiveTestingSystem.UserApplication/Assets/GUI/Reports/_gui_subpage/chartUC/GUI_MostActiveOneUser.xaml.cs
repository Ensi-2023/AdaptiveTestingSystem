using AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports._gui_subpage.viewmodel;
using LiveChartsCore.Drawing;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView.Drawing;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
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
using LiveChartsCore.SkiaSharpView.WPF;

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports._gui_subpage.chartUC
{
    /// <summary>
    /// Логика взаимодействия для GUI_MostActiveOneUser.xaml
    /// </summary>
    public partial class GUI_MostActiveOneUser : UserControl
    {

        bool IsScroll { get; set; } = false;
        public GUI_MostActiveOneUser()
        {
            InitializeComponent();

 

         //   Chart.AddHandler(System.Windows.UIElement.PreviewMouseWheelEvent, new MouseWheelEventHandler(Chart_PreviewMouseWheel), true);
        }

 
        private SolidColorBrush TextForeground;
        modelPage_1_mostactiveone modelPage_1_4;
        private ZoomDirection  zooms= ZoomDirection.ZoomOut;

        public void SetData(Data_MostActiveUser oneMostActiveUser)
        {
            modelPage_1_4.SetData(oneMostActiveUser);

            titleChart.Text = $"Данные {oneMostActiveUser.Name} за {DateTime.Now.Year}";
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            TextForeground = (SolidColorBrush)this.TryFindResource("DefaultTextForegroud");
            SolidColorPaint paintText = new SolidColorPaint(new SKColor(TextForeground.Color.R, TextForeground.Color.G, TextForeground.Color.B));
            Chart.LegendTextPaint = paintText;
            modelPage_1_4 = new modelPage_1_mostactiveone(paintText);
            DataContext = modelPage_1_4;
        }

        private void Chart_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
                   
                //CartesianChart<SkiaSharpDrawingContext> obj = (CartesianChart<SkiaSharpDrawingContext>)Chart.CoreChart;
                //Point position = e.GetPosition(this);
                //obj.Zoom(new LvcPoint((float)position.X, (float)position.Y), (e.Delta <= 0) ? ZoomDirection.ZoomOut : ZoomDirection.ZoomIn);
            
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            button.Visibility= Visibility.Collapsed;
            UIHelper.SaveUIToPNG(charVisual);
            button.Visibility = Visibility.Visible;
        }

       

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
          //  Chart.RemoveHandler(System.Windows.UIElement.PreviewMouseWheelEvent, new MouseWheelEventHandler(Chart_PreviewMouseWheel));
        }

    }
}
