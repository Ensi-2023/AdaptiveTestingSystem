using AdaptiveTestingSystem.UserApplication.Assets.CScript;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Логика взаимодействия для GUI_ReportPage_2_SearchRangeData.xaml
    /// </summary>
    public partial class GUI_ReportPage_2_SearchRangeData : UserControl
    {
        public enum TypeSearch
        { 
            Day,
            Month,
            Year
        }
        public struct SearchStr
        {
            public SearchStr()
            {
            }

            public TypeSearch Type { get; set; } = TypeSearch.Day;
        }

        public bool IsRun { get; set; } = false;

        public GUI_ReportPage_2_SearchRangeData()
        {
            InitializeComponent();
            DataUpload();
      
        }

        private async void DataUpload()
        {
            overlay.Visibility = Visibility.Visible;

            await SetMonth();
            await SetYear();

            overlay.Visibility = Visibility.Collapsed;

        }

        private async Task SetYear()
        {
            YearBox.ClearItems();
            for (int i = DateTime.Now.Year; i >= 1990; i--)
            {
                YearBox.Items.Add(new PopupItemControl() { Index = i,Caption=$"{i}"});
                await Task.Delay(10);
            }

        }

        private async Task SetMonth()
        {
            MountBox.ClearItems();
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

            for (int i = 0; i < strLabel.Length; i++)
            {
                MountBox.Items.Add(new PopupItemControl() { Index = i+1, Caption = strLabel[i], HiddenField =$"{i + 1}" });
                await Task.Delay(10);
            }
        }

        private void DayBox_start_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void DayBox_end_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var obj = sender as ComboTextBox;
                if (obj != null)
                {
                    if (obj.Text == string.Empty) return;

                    var countday = DateTime.ParseExact(MountBox.Text, "MMMM", CultureInfo.CurrentCulture).Month;
                    var daycount = DateTime.DaysInMonth(int.Parse(YearBox.Text), countday);

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
    
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            IsRun = false;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Factory.StartNew(async () =>
            {
                IsRun = true;

                while (IsRun)
                {
                
                        if (Application.Current == null) break;

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            try
                            {

                                var item = DateTime.ParseExact(MountBox.Text, "MMMM", CultureInfo.CurrentCulture).Month;
                                if (YearBox.Text == string.Empty || YearBox.Text.Length < 4) throw new Exception();
                                var dym = DateTime.Parse($"1.{item}.{YearBox.Text}");

                                if (DayBox_start.IsEnabled == false)
                                    DayBox_start.IsEnabled = true;
             
                            }
                            catch
                            {
                                DayBox_start.IsEnabled = false;
                                DayBox_start.Text = string.Empty;
                                DayBox_end.Text = string.Empty;
                                DayBox_end.BorderThickness = new Thickness(0);
                                DayBox_end.CloseError();

                            }
                        });
                    await Task.Delay(50);
                } 
    
                
            });
        }

        private void DayBox_start_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                var obj = sender as ComboTextBox;
                if (obj != null)
                {
                    if (obj.Text == string.Empty) return;
                    var countday = DateTime.ParseExact(MountBox.Text, "MMMM", CultureInfo.CurrentCulture).Month;
                    var daycount = DateTime.DaysInMonth(int.Parse(YearBox.Text), countday);

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

        private void DayBox_start_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }
    }
}
