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
    public class DoubleToIntToString : IValueConverter
    {
    
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
            var obj = value.ToString();
            if (obj == null) return "";

            try
            { 
                return int.Parse(obj);
            }
            catch(Exception ex) 
            {
              return "";
            }
                   
               
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return DependencyProperty.UnsetValue;
            }
        


    }
}
