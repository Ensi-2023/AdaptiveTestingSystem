using AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports._gui_subpage.viewmodel;
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

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports._gui_subpage.chartUC
{
    /// <summary>
    /// Логика взаимодействия для GUI_FiveClassRoomForAvgScore.xaml
    /// </summary>
    public partial class GUI_FiveClassRoomForAvgScore : UserControl
    {
        modelPage_1_fiveClassRoom modelPage_1_FiveClass;

        private SolidColorBrush TextForeground;
        private SolidColorBrush PanelBackground;
        private SolidColorBrush LineForeground;

        public GUI_FiveClassRoomForAvgScore()
        {
            InitializeComponent();



        }

        public void SetData(List<Data_5ClassRoomForAverageScore> averageScores5ClassRoom)
        {
            modelPage_1_FiveClass.SetData(averageScores5ClassRoom);

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            TextForeground = (SolidColorBrush)this.TryFindResource("DefaultTextForegroud");
            LineForeground = (SolidColorBrush)this.TryFindResource("DefaultTextForegroud");
            PanelBackground = (SolidColorBrush)this.TryFindResource("DefaultPopupPanelBackground");

            SolidColorPaint paintText = new SolidColorPaint(new SKColor(TextForeground.Color.R, TextForeground.Color.G, TextForeground.Color.B));
            SolidColorPaint lineText = new SolidColorPaint(new SKColor(LineForeground.Color.R, LineForeground.Color.G, LineForeground.Color.B));


            Chart.LegendTextPaint = paintText;

            modelPage_1_FiveClass = new modelPage_1_fiveClassRoom(lineText, paintText);
            DataContext = modelPage_1_FiveClass;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UIHelper.SaveUIToPNG(charVisual);
        }
    }
}
