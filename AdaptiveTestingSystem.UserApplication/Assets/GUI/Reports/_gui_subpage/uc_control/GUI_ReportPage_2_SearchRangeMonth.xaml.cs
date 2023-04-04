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
    /// Логика взаимодействия для GUI_ReportPage_2_SearchRangeMonth.xaml
    /// </summary>
    public partial class GUI_ReportPage_2_SearchRangeMonth : UserControl
    {
        public GUI_ReportPage_2_SearchRangeMonth()
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
                YearBox.Items.Add(new PopupItemControl() { Index = i, Caption = $"{i}" });
                await Task.Delay(10);
            }

        }

        private async Task SetMonth()
        {
            MonthBox_start.ClearItems();
            MonthBox_end.ClearItems();
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
                MonthBox_start.Items.Add(new PopupItemControl() { Index = i + 1, Caption = strLabel[i], HiddenField = $"{i + 1}" });
                MonthBox_end.Items.Add(new PopupItemControl() { Index = i + 1, Caption = strLabel[i], HiddenField = $"{i + 1}" });
                await Task.Delay(10);
            }
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

        private void MonthBox_start_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
     
        }

        private void MonthBox_start_TextChanged(object sender, TextChangedEventArgs e)
        {
            var obj = sender as ComboTextBox;
            if (obj != null)
            {
                var item = obj.Items.Find(o => o.Caption.ToLower().Trim() == (obj.Text.ToLower().Trim()) || o.HiddenField.ToLower().Trim() == (obj.Text.ToLower().Trim()));
                if (item == null)
                {
                    obj.Error("Некорректный месяц");
                    obj.BorderBrush = Brushes.Red;
                    obj.BorderThickness = new Thickness(1);
                    return;
                }
                else
                {
                    obj.BorderBrush = Brushes.Transparent;
                    obj.BorderThickness = new Thickness(0);
                    obj.CloseError();

                    if (obj.Name == "MonthBox_end")
                    {

                        try
                        {
                            var selectItem = MonthBox_start.ReturnSelectItem();
                            var endItem = MonthBox_end.ReturnSelectItem();
                            if (endItem == null) return;

                            if (int.Parse(selectItem.HiddenField) > int.Parse(endItem.HiddenField))
                            {
                                obj.Error("Конечный месяц не может быть меньше начального");
                                obj.BorderBrush = Brushes.Red;
                                obj.BorderThickness = new Thickness(1);
                     
                            }
                            else
                            {
                                obj.BorderBrush = Brushes.Transparent;
                                obj.BorderThickness = new Thickness(0);
                                obj.CloseError();
                            }
                        } catch(Exception ex) 
                        {
                            Logger.Error(ex.Message);
                        }
                    }

                }

            }
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

        private void DayBox_start_GotFocus(object sender, RoutedEventArgs e)
        {
            if (MonthBox_end.Text.Trim() == string.Empty)
            {
                DayBox_start.Clear();
                MonthBox_end.Error("Заполните это поле");
                MonthBox_end.Focus();        

            }
        }
    }
}
