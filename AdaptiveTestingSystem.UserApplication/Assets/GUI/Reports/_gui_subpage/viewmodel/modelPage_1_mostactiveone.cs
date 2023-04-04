using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using LiveChartsCore.SkiaSharpView.VisualElements;
using LiveChartsCore.SkiaSharpView.Painting;
using System.Windows.Shapes;
using LiveChartsCore.Kernel;

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports._gui_subpage.viewmodel
{
    public class modelPage_1_mostactiveone : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChange([CallerMemberName] string propd = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propd));
        }

        public LabelVisual Title { get; set; }

        public SolidColorPaint _colorLabel;



        public Axis[] XAxes { get; set; } =
{       

         };


        public Axis[] YAxes { get; set; } =
      {

         };


        public void SetData(Data_MostActiveUser oneMostActiveUser)
        {

             if (oneMostActiveUser == null) return;

            Title.Text = $"Данные {oneMostActiveUser.Name}";
        
  
            double[] values = new double[12];

            Series = new ObservableCollection<ISeries>();
            for(int i=0;i<12;i++)
            {
                var item = oneMostActiveUser.UserDataInMonth[i];
                values[i] = Math.Round(item.AVG,2);
               
            }
        
            var it = (new LineSeries<double>
            {
                Values = values,     
                DataLabelsPaint = _colorLabel,
                Name = oneMostActiveUser.Name
            });

          

            Series.Add(it);

        }

        public modelPage_1_mostactiveone(SolidColorPaint paint)
        {
            _colorLabel = paint;
      

            string[] strLabel = new string[12] { "Январь",
                                                 "Февраль",
                                                 "Март",
                                                 "Апрель",
                                                 "Май",
                                                 "Июнь",
                                                 "Июль",
                                                 "Август",
                                                 "Сентябрь",
                                                 "Октябрь",
                                                 "Ноябрь",
                                                 "Декабрь" };


            XAxes = new Axis[]
            {
                new Axis()
                {
                    Labels = strLabel,
                    LabelsRotation = 0,
                    TicksAtCenter = true,
                    SeparatorsAtCenter = false,
                    LabelsPaint= _colorLabel,
                }
            };

            YAxes = new Axis[]
           {
               new Axis
               {
                  LabelsPaint = _colorLabel,
               }
           };


            Title = new LabelVisual
            {
               
                TextSize = 20,
                Padding = new LiveChartsCore.Drawing.Padding(30),
                Paint= _colorLabel,
            };


            OnPropertyChange("XAxes");
            OnPropertyChange("YAxes");
            OnPropertyChange("Title");
        }


        private ObservableCollection<ISeries> series;

        public ObservableCollection<ISeries> Series
        {
            get { return series; }
            set { series = value; OnPropertyChange("Series"); }
        }
    }
}
