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
    /// Логика взаимодействия для GUI_Top3Subject.xaml
    /// </summary>
    public partial class GUI_Top3Subject : UserControl
    {
        private SolidColorBrush TextForeground;
        private SolidColorBrush PanelBackground;
        private SolidColorBrush LineForeground;


        modelPage_1_3Subject modelPage_1_3;

        public GUI_Top3Subject()
        {
            InitializeComponent();
        }

        public void SetData(List<Data_3MostTestedSubject> allCount)
        {

            SolidColorPaint paintText = new SolidColorPaint(new SKColor(TextForeground.Color.R, TextForeground.Color.G, TextForeground.Color.B));
            modelPage_1_3.SetData(allCount, paintText);

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            TextForeground = (SolidColorBrush)this.TryFindResource("DefaultTextForegroud");
            LineForeground = (SolidColorBrush)this.TryFindResource("DefaultTextForegroud");
            PanelBackground = (SolidColorBrush)this.TryFindResource("DefaultPopupPanelBackground");

            SolidColorPaint paintText = new SolidColorPaint(new SKColor(TextForeground.Color.R, TextForeground.Color.G, TextForeground.Color.B));
            SolidColorPaint lineText = new SolidColorPaint(new SKColor(LineForeground.Color.R, LineForeground.Color.G, LineForeground.Color.B));
            SolidColorPaint panelColor = new SolidColorPaint(new SKColor(PanelBackground.Color.R, PanelBackground.Color.G, PanelBackground.Color.B));


            modelPage_1_3 = new modelPage_1_3Subject(lineText, paintText);
            DataContext = modelPage_1_3;


            Chart.LegendTextPaint = paintText;
            Chart.Foreground = Brushes.Red;
           



            Chart.UpdateLayout();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UIHelper.SaveUIToPNG(charVisual);
        }
    }
}
