using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports._gui_subpage.uc_control
{
    /// <summary>
    /// Логика взаимодействия для GUI_ReportPage_2_SearchRangeYear.xaml
    /// </summary>
    public partial class GUI_ReportPage_2_SearchRangeYear : UserControl
    {
        public GUI_ReportPage_2_SearchRangeYear()
        {
            InitializeComponent();
        }

        private void YearBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        private void DayBox_end_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var obj = sender as ComboTextBox;
                if (obj != null)
                {
                    if (obj.Text == string.Empty) return;

                    var countday = DateTime.ParseExact(MonthBox_start.Text, "MMMM", CultureInfo.CurrentCulture).Month;
                    var daycount = 31;

                    var day = int.Parse(obj.Text);
                    var startDay = int.Parse(DayBox_start.Text);

                    if (day > daycount)
                    {
                        _Main.Instance._Notification.Add("", $"Конечный день день не может быть больше максимального числа: {daycount}", TypeNotification.Error);
                        obj.Text = $"{daycount}";
                        return;
                    }


                    if (day <= startDay)
                    {
                        obj.Error("Конечный день не может быть меньше или равен стартовому");
                        obj.BorderBrush = Brushes.Red;
                        obj.BorderThickness = new Thickness(1);
                        return;
                    }
                    else
                    {
                        obj.BorderBrush = Brushes.Transparent;
                        obj.BorderThickness = new Thickness(0);
                        obj.CloseError();
                    }

                    return;
                }
            }
            catch { }
        }


        private void DayBox_start_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                var obj = sender as ComboTextBox;
                if (obj != null)
                {
                    if (obj.Text == string.Empty) return;
                    var countday = DateTime.ParseExact(MonthBox_start.Text, "MMMM", CultureInfo.CurrentCulture).Month;
                    var daycount = 31;

                    var day = int.Parse(obj.Text);

                    if (day >= daycount)
                    {
                        _Main.Instance._Notification.Add("", "Стартовый день не может быть больше либо равен конечному", TypeNotification.Error);
                        obj.Text = $"{daycount - 1}";
                        return;
                    }

                }
            }
            catch { }
        }



    }
}
