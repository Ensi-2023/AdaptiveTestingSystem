using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports._gui_subpage.uc_control.GUI_ReportPage_2_RangeDay;

namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports._gui_subpage.uc_control.CScript
{
    public class CreateReportPage_2_Range
    {
        public struct ReportRange
        {
            public string MonthStart = "", MonthEnd = "";
            public string DayStart = "", DayEnd = "";
            public string YearStart = "", YearEnd = "";
            public ViewData ViewData = ViewData.day;
            public ReportRange() { }

        }

        ReportRange _reportRange;

        public CreateReportPage_2_Range()
        {
            _reportRange = new ReportRange();
        }

        public void Add(string daystart, string monthstart, string yearstart, ViewData view , string dayend = "", string monthend = "", string yearend = "")
        {
            _reportRange.DayEnd = dayend;
            _reportRange.DayStart = daystart;
            _reportRange.MonthStart = monthstart;
            _reportRange.MonthEnd = monthend;
            _reportRange.YearStart = yearstart;
            _reportRange.YearEnd = yearend;
            _reportRange.ViewData = view;
        }

        public ReportRange RrturnReport()
        {           
            return _reportRange;
        }
    }
}
