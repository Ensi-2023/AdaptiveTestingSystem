using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using SkiaSharp;

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports._gui_subpage.viewmodel
{
    public class modelPage_1_3Subject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChange([CallerMemberName] string propd = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propd));
        }

        private ObservableCollection<ISeries> series;

        public ObservableCollection<ISeries> Series
        {
            get { return series; }
            set { series = value; }
        }
        SolidColorPaint LabelColor { get; set; }

        public LabelVisual Title { get; set; }
        public Axis[] XAxes { get; set; } =
      {

         };


        public Axis[] YAxes { get; set; } =
      {

         };

        public modelPage_1_3Subject(SolidColorPaint line, SolidColorPaint label)
        {
            Series = new ObservableCollection<ISeries>();

            XAxes = new Axis[]
 {
                new Axis
                {
                     Labels = new string[] { "Количество тестов" },
                     LabelsRotation = 0,
                     TicksAtCenter = true,
                     SeparatorsAtCenter = false,
                     SeparatorsPaint= line,
                     LabelsPaint = label,
                     CrosshairLabelsPaint= line,
                     
                     

                }
 };

            Title = new LabelVisual
            {
                Text = "Топ 3 любимых предмета",
                TextSize = 25,
                Padding = new LiveChartsCore.Drawing.Padding(15),
                Paint = label
            };


            YAxes = new Axis[]
            {
                new Axis{ LabelsPaint = label}
            };
        }



        public void SetData(List<Data_3MostTestedSubject> data_3Mosts, SolidColorPaint textForeground)
        {
            foreach (var item in data_3Mosts)
            {
                var series = (new ColumnSeries<double>
                {
                    Name = item.SubjectName,
                    Values = new double[] { item.Count },
                    DataLabelsPaint = textForeground
                });

                Series.Add(series);
            }
        }
    }
}

