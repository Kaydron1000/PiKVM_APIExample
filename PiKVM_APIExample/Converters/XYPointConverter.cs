using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PiKVM_APIExample.Converters
{
    internal class XYPointConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is string paramStr)
            {
                if (paramStr == "X")
                {
                    return ((System.Windows.Point)value).X;
                }
                else if (paramStr == "Y")
                {
                    return ((System.Windows.Point)value).Y;
                }
                else
                {
                    throw new ArgumentException("Parameter must be 'X' or 'Y'.");
                }
            }
            return new System.Windows.Point(0, 0); // Default return value if parameter is not recognized
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
