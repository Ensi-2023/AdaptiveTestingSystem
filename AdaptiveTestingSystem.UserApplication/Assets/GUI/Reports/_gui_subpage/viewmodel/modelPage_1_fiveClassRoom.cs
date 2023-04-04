using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.Generic;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using LiveChartsCore.SkiaSharpView.VisualElements;

using System;

using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Windows.Markup;
using LiveChartsCore.Kernel.Sketches;
using System.Windows.Forms.VisualStyles;


namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports._gui_subpage.viewmodel
{
    public class modelPage_1_fiveClassRoom : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChange([CallerMemberName] string propd = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propd));
        }

        SolidColorPaint LabelColor { get; set; }

        public modelPage_1_fiveClassRoom(SolidColorPaint line, SolidColorPaint label) 
        {
            Series = new ObservableCollection<ISeries>();

            LabelColor = label;

            // Series = data.AsLiveChartsPieSeries(); this could be enough in some cases 
            // but you can customize the series properties using the following overload: 
         
            Title = new LabelVisual
            {
                Text = "5 лучших классов",
                TextSize = 25,
                Padding = new LiveChartsCore.Drawing.Padding(15),
                Paint = LabelColor
            };

        }

        private ObservableCollection<ISeries> series;

        public ObservableCollection<ISeries> Series
        {
            get { return series; }
            set { series = value; OnPropertyChange("Series"); }
        }


        public void SetData(List<Data_5ClassRoomForAverageScore> allScore)
        {
                 
            foreach (var item in allScore)
            {
                var it = (new PieSeries<double> { Values = new double[] { item.AverageScore }, Name = item.ClassRoomName });
                it.DataLabelsPaint = LabelColor;
                it.DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle;
                it.DataLabelsFormatter = p => $"{item.ClassRoomName} ({Math.Round (item.AverageScore,2)})";
                Series.Add(it);
            }

        }

        public LabelVisual Title { get; set; }
          
    }
}
