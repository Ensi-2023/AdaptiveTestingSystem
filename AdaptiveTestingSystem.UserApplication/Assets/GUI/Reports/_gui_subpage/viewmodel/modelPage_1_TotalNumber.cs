using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports._gui_subpage.viewmodel
{
    public class modelPage1TotalNumber : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChange([CallerMemberName] string propd = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propd));
    }

    public void SetData(List<Data_AllScoreTest> allScoreTests, SolidColorPaint textForeground)
    {


        foreach (var item in allScoreTests)
        {
            var series = (new ColumnSeries<double>
            {
                Name = item.TestName,
                Values = new double[] { item.Count_Score2_general, item.Count_Score3_general, item.Count_Score4_general, item.Count_Score5_general },
                DataLabelsPaint = textForeground

            });

            Series.Add(series);
        }
    }


    public void SetData(List<Data_ClassroomScore_general> allScore)
    {


        foreach (var item in allScore)
        {
            var series = (new ColumnSeries<double>
            {
                Name = item.ClassRoomName,
                Values = new double[] { item.Count_Score2, item.Count_Score3, item.Count_Score4, item.Count_Score5 },
            });

            Series.Add(series);
        }
    }


    private ObservableCollection<ISeries> series;

    public ObservableCollection<ISeries> Series
    {
        get { return series; }
        set { series = value; OnPropertyChange("Series"); }
    }

    public Axis[] XAxes { get; set; } =
    {

    };


    public Axis[] YAxes { get; set; } =
    {

    };

    public LabelVisual Title { get; set; }

    public modelPage1TotalNumber(SolidColorPaint line, SolidColorPaint label, string title)
    {
        Series = new ObservableCollection<ISeries>();

        XAxes = new Axis[]
        {
                new Axis
                {
                     Labels = new string[] { "Два", "Три", "Четыре","Пять" },
                     LabelsRotation = 0,
                     TicksAtCenter = true,
                     SeparatorsAtCenter = false,
                     SeparatorsPaint= line,
                     LabelsPaint = label,
                     CrosshairLabelsPaint= line,


                }
        };

        YAxes = new Axis[]
        {
                new Axis{ LabelsPaint = label}
        };

        Title = new LabelVisual
        {
            Text = title,
            TextSize = 25,
            Padding = new LiveChartsCore.Drawing.Padding(15),
            Paint = label
        };
    }
}
}
