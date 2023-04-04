using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AdaptiveTestingSystem.DLL.Converts
{
    public class DateTimeToDateConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parametr, CultureInfo culture)
        {
            var obj =  value;
            return DateTime.Parse(obj.ToString()).ToShortDateString();     
        }

        public object ConvertBack(object value, Type targetType, object parametr, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
