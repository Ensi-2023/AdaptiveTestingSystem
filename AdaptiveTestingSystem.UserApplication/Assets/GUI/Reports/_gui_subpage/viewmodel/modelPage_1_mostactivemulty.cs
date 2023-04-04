using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.Painting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports._gui_subpage.viewmodel
{
    public class modelPage_1_mostactivemulty : modelPage_1_mostactiveone
    {
        public modelPage_1_mostactivemulty(SolidColorPaint paint) : base(paint)
        {
        }
        public void SetData(List<Data_MostActiveUser> oneMostActiveUser)
        {

             if (oneMostActiveUser == null) return;

            Title.Text = $"Данные за {DateTime.Now.Year}";
            Series = new ObservableCollection<ISeries>();


            foreach (var user in oneMostActiveUser)
            {

                double[] values = new double[12];

                for (int i = 0; i < 12; i++)
                {
                    var item = user.UserDataInMonth[i];
                    values[i] = Math.Round(item.AVG, 2);

                }

                var it = (new LineSeries<double>
                {
                    Values = values,
                    DataLabelsPaint = _colorLabel,
                    Name = user.Name
                });

                Series.Add(it);
            }

        }
    }
}
