using AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports._gui_subpage.uc_control.CScript;
using AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports._gui_subpage.uc_control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaptiveTestingSystem.UserApplication.Assets.CScript;

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports._gui_subpage.CScript
{

    /// <summary>
    /// МОжет быть ошибки прикручу.
    /// Пока что будет так, если все поля пустые, поиск будет
    /// по текущей дате
    /// </summary>
    public class AddDataInReport
    {
        public static CreateReportPage_2_Range.ReportRange Create(CreateReportPage_2_Range.ReportRange obj)
        {
           var report = obj;

            switch (obj.ViewData)
            {
                case ViewData.day: report = CreateData(obj); break;
                case ViewData.month: report = CreateData(obj); break;
                case ViewData.year: report = CreateData(obj); break;
            }

            return report;
        }

        private static CreateReportPage_2_Range.ReportRange CreateData(CreateReportPage_2_Range.ReportRange obj)
        {
            var report = obj;


            if (report.DayStart.Trim() == string.Empty) report.DayStart = ToDay();
            if (report.MonthStart.Trim() == string.Empty) report.MonthStart = ToMonth();
            if (report.MonthEnd.Trim() == string.Empty) report.MonthEnd = report.MonthStart;
            if (report.YearStart.Trim() == string.Empty) report.YearStart = ToYear();
            if (report.YearEnd.Trim() == string.Empty) report.YearEnd = report.YearStart;
            if (report.DayEnd.Trim() == string.Empty)
            {
                if (report.MonthEnd.Equals(report.YearStart) == false) report.DayEnd = "31";
            }

            return report;
        }

        //private static CreateReportPage_2_Range.ReportRange CheckError_Month(CreateReportPage_2_Range.ReportRange obj)
        //{
        //    var report = obj;

        //    if (report.DayStart.Trim() == string.Empty) report.DayStart = ToDay();
        //    if (report.MonthStart.Trim() == string.Empty) report.MonthStart = ToMonth();
        //    if (report.MonthEnd.Trim() == string.Empty) report.MonthEnd = report.MonthStart;
        //    if (report.YearStart.Trim() == string.Empty) report.YearStart = ToYear();
        //    if (report.DayEnd.Trim() == string.Empty)
        //    {
        //        if (report.MonthEnd.Equals(report.YearStart) == false) report.DayEnd = "31";
        //    }


        //    return report;

        //}
        //private static CreateReportPage_2_Range.ReportRange CheckError_Day(CreateReportPage_2_Range.ReportRange obj)
        //{
        //    var report = obj;

        //    if (report.DayStart.Trim() == string.Empty) report.DayStart = ToDay();
        //    if (report.MonthStart.Trim() == string.Empty) report.MonthStart = ToMonth();
        //    if (report.YearStart.Trim() == string.Empty) report.YearStart = ToYear();
        //    if (report.DayEnd.Trim() == string.Empty) report.DayEnd = DaysInMonth(report.YearStart, report.MonthStart);
             
        //    return report;
        //}




        private static string ToDay()
        {
           return DateTime.Now.Day.ToString();
        }

        private static string ToMonth()
        { 
            return DateTime.Now.Month.ToString();
        }

        private static string ToYear() 
        {
            return DateTime.UtcNow.Year.ToString();
        }
        private static string DaysInMonth(string year, string month)
        {
            return DateTime.DaysInMonth(int.Parse(year), int.Parse(month)).ToString();
        }
    }
}
