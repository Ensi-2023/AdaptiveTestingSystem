using AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports._gui_subpage.viewmodel;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using Microsoft.Win32;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
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


namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports._gui_subpage.chartUC
{
    /// <summary>
    /// Логика взаимодействия для GUI_ClassRoomAllScore.xaml
    /// </summary>
    public partial class GUI_ClassRoomAllScore : UserControl
    {
        private SolidColorBrush TextForeground;
        private SolidColorBrush PanelBackground;
        private SolidColorBrush LineForeground;
        private modelPage_1 ModelPage_1;
      

        public GUI_ClassRoomAllScore()
        {
            InitializeComponent();
        }

        public void SetData(List<Data_ClassroomScore_general> allScore)
        {
            SolidColorPaint paintText = new SolidColorPaint(new SKColor(TextForeground.Color.R, TextForeground.Color.G, TextForeground.Color.B));

            ModelPage_1.SetData(allScore, paintText);

        }
        public LabelVisual Title { get; set; }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            TextForeground = (SolidColorBrush)this.TryFindResource("DefaultTextForegroud");
            LineForeground = (SolidColorBrush)this.TryFindResource("DefaultTextForegroud");
            PanelBackground = (SolidColorBrush)this.TryFindResource("DefaultPopupPanelBackground");

            SolidColorPaint paintText = new SolidColorPaint(new SKColor(TextForeground.Color.R, TextForeground.Color.G, TextForeground.Color.B));
            SolidColorPaint lineText = new SolidColorPaint(new SKColor(LineForeground.Color.R, LineForeground.Color.G, LineForeground.Color.B));
            SolidColorPaint panelColor = new SolidColorPaint(new SKColor(PanelBackground.Color.R, PanelBackground.Color.G, PanelBackground.Color.B));


            ModelPage_1 = new modelPage_1(lineText, paintText, "Оценки по всем классам");
            DataContext = ModelPage_1;


            Chart.LegendTextPaint = paintText;
            Chart.Foreground = Brushes.Red;
            Chart.ZoomMode = LiveChartsCore.Measure.ZoomAndPanMode.None;
            
            Chart.UpdateLayout();



        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UIHelper.SaveUIToPNG(charVisual);
        }
    }
}
