using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace AdaptiveTestingSystem.DLL.Converts
{
    public class StringToPercentString : IValueConverter
    {
    
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if(value!=null)     
                return $"{Math.Round((double)value).ToString().Trim()}%";
                return "";
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return DependencyProperty.UnsetValue;
            }
        
    }
}
