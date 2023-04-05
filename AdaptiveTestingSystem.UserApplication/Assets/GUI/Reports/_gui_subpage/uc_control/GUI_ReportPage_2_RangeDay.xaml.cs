using AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports._gui_subpage.uc_control.CScript;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
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
using static AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports._gui_subpage.uc_control.CScript.CreateReportPage_2_Range;

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports._gui_subpage.uc_control
{
    /// <summary>
    /// Логика взаимодействия для GUI_ReportPage_2_RangeDay.xaml
    /// </summary>
    public partial class GUI_ReportPage_2_RangeDay : UserControl
    {
        CreateReportPage_2_Range createReport;

        private ViewData _viewData = ViewData.day;

        public GUI_ReportPage_2_RangeDay(CreateReportPage_2_Range createReportPage)
        {
            InitializeComponent();
          
            createReport = createReportPage;
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
            YearBox_start.ClearItems();
            YearBox_end.ClearItems();
            for (int i = DateTime.Now.Year; i >= 1990; i--)
            {
                YearBox_start.Items.Add(new PopupItemControl() { Index = i, Caption = $"{i}" });
                YearBox_end.Items.Add(new PopupItemControl() { Index = i, Caption = $"{i}" });
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


        public void SetView(ViewData view)
        {
            _viewData = view;

            switch (_viewData)
            {
                case ViewData.day:
                    MonthBox_end.Visibility= Visibility.Collapsed;
                    YearBox_end.Visibility= Visibility.Collapsed;
                    break;
                case ViewData.month:
                    MonthBox_end.Visibility = Visibility.Visible;
                    MonthBox_end.SetErrorNoMessage();
                    YearBox_end.Visibility = Visibility.Collapsed;
                    break;
                case ViewData.year:
                    MonthBox_end.Visibility = Visibility.Visible;
                    YearBox_end.Visibility = Visibility.Visible;
                    MonthBox_end.SetErrorNoMessage();
                    YearBox_end.SetErrorNoMessage();
                    break;
            }
        }

        public ReportRange CreateReport()
        {
            createReport.Add(DayBox_start.IsError?"": DayBox_start.Text,
                MonthBox_start.IsError?"": MonthBox_start.Text, 
                YearBox_start.IsError?"": YearBox_start.Text,
                _viewData,
                dayend: DayBox_end.IsError?"": DayBox_end.Text,
                monthend: MonthBox_end.IsError ? "" : MonthBox_end.Text,
                yearend:YearBox_end.IsError ? "" : YearBox_end.Text);
            return createReport.RrturnReport();
        }


        private void DayBox_start_LostFocus(object sender, RoutedEventArgs e)
        {
            try
                {
                    var obj = sender as ComboTextBox;
                    if (obj != null)
                    {
                    var day = int.Parse(obj.Text);
                    var daycount = 31;
                    if (_viewData == ViewData.day)
                    {
                        if (obj.Text == string.Empty) return;

                        var numberMonth = DateTime.Now.Month;

                        if (MonthBox_start.IsError == false)
                        {
                            if (MonthBox_start.Text.Trim() != string.Empty)
                            {
                                numberMonth = DateTime.ParseExact(MonthBox_start.Text, "MMMM", CultureInfo.CurrentCulture).Month;
                            }
                        }


                        if (YearBox_start.IsError == false)
                        {
                            if (YearBox_start.Text.Trim() != string.Empty)
                            {
                                daycount = DateTime.DaysInMonth(int.Parse(YearBox_start.Text), numberMonth);
                            }
                        }
                    }

                    if (day >= daycount)
                    {
                        _Main.Instance._Notification.Add("", "Стартовый день не может быть больше либо равен конечному", TypeNotification.Error);
                        obj.Text = $"{daycount - 1}";
                        return;
                    }


                    if (day <= 0)
                        {
                            _Main.Instance._Notification.Add("", "Введен некорректный день", TypeNotification.Error);
                            obj.Text = $"1";
                            return;
                        }

                }
            }
                catch { }   
        }

    

        private void DayBox_end_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var obj = sender as ComboTextBox;
                if (obj != null)
                {
                    if (obj.Text == string.Empty) return;

                    var numberMonth = DateTime.Now.Month;

                    if (MonthBox_start.Text.Trim() == string.Empty)
                    {
                        numberMonth = DateTime.ParseExact(MonthBox_start.Text, "MMMM", CultureInfo.CurrentCulture).Month;
                    }
                    
                
                    var daycount = 31;

                    var day = int.Parse(obj.Text);
                    var startDay = int.Parse(DayBox_start.Text);
                    if (_viewData == ViewData.day)
                    {
                        daycount = DateTime.DaysInMonth(int.Parse(YearBox_start.Text.Trim()==string.Empty?DateTime.Now.Year.ToString():YearBox_start.Text), numberMonth);
                    }


                    if (day > daycount)
                    {
                        _Main.Instance._Notification.Add("", $"Конечный день день не может быть больше максимального числа: {daycount}", TypeNotification.Error);
                        obj.Text = $"{daycount}";
                        return;
                    }

                    if (day < startDay)
                    {
                        obj.Error("Конечный день не может быть меньше стартового");
                        return;
                    }
                    else
                    {
                        obj.CloseError();
                    }

                    return;
                }
            }
            catch { }
        }

        private void YearBox_end_TextChanged(object sender, TextChangedEventArgs e)
        {
            try 
            {
                YearBox_end.IsValid = false;
                var startYear = int.Parse(YearBox_start.Text);
                var endYear = int.Parse(YearBox_end.Text);

                if (startYear > endYear)
                {
                    YearBox_end.Error("Конечный год не может быть меньше стартового");
                }
                else
                {
                    YearBox_end.CloseError();
                    YearBox_end.IsValid = true;
                }

              
            }
            catch { }
        }

        private void MountBox_end_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var obj = sender as ComboTextBox;
                obj.IsValid= false;
                var selectItem = MonthBox_start.ReturnSelectItem();
                var endItem = MonthBox_end.ReturnSelectItem();
                if (endItem == null) return;

                if (int.Parse(selectItem.HiddenField) > int.Parse(endItem.HiddenField))
                {
                    obj.Error("Конечный месяц не может быть меньше начального");
            

                }
                else
                {
                    obj.CloseError();
                    obj.IsValid = true;
                }
            }
            catch (Exception ex)
            {
                MonthBox_end.IsValid = true;
                Logger.Error(ex.Message);
            }
        }

        private void MonthBox_start_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(MonthBox_start.Text == string.Empty) MonthBox_end.Text = string.Empty;
        }

        private void YearBox_start_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (YearBox_start.Text == string.Empty) YearBox_end.Text = string.Empty;
        }

        private void DayBox_start_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DayBox_start.Text == string.Empty) DayBox_end.Text = string.Empty;
        }
    }
}
