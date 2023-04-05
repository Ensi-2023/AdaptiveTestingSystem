
namespace AdaptiveTestingSystem.UserApplication.Assets.GUI.Reports._gui_subpage.CScript
{
    public class StatisticCreate
    {
        public static bool IsUpload { get; set; }


        System.Windows.Controls.Grid Body;
        Data_StatisticGeneral Data;
        Data_StatisticCustom Custom;

        public void Create(Data_StatisticGeneral obj, Data_StatisticCustom custom, System.Windows.Controls.Grid body)
        {
            Data = obj;
            Custom = custom;

            if (body.Children[0] as GUI_Report_Page1 != null)
            {
                (body.Children[0] as GUI_Report_Page1).SetData(Data);
            }

            if (body.Children[0] as GUI_Report_Page2 != null)
            {
                (body.Children[0] as GUI_Report_Page2).SetData(Custom);
            }

        }

        public void SetInfo(double packetSize, double maxSize, System.Windows.Controls.Grid body)
        {
            if (body.Children[0] as GUI_Report_Page1 != null)
            {
                (body.Children[0] as GUI_Report_Page1).SendInfo(packetSize, maxSize);
            }


            if (body.Children[0] as GUI_Report_Page2 != null)
            {
                (body.Children[0] as GUI_Report_Page2).SendInfo(packetSize, maxSize);
            }
        }

        public void SetError(System.Windows.Controls.Grid body)
        {
            if (body.Children[0] as GUI_Report_Page1 != null)
            {
                (body.Children[0] as GUI_Report_Page1).SetError();
            }

            if (body.Children[0] as GUI_Report_Page2 != null)
            {
                (body.Children[0] as GUI_Report_Page2).SetError();
            }
        }

        public bool IsCanceling(System.Windows.Controls.Grid body)
        {
            bool c = false;

            if (body.Children[0] as GUI_Report_Page1 != null)
            {
                return (body.Children[0] as GUI_Report_Page1).IsCancelUpload;
            }


            if (body.Children[0] as GUI_Report_Page1 != null)
            {
                return (body.Children[0] as GUI_Report_Page2).IsCancelUpload;
            }


            return c;
        }
    }
}
