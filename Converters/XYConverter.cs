using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TWatchSKDesigner.Converters
{
    public class XYConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is int[] intArray)
            {
                return string.Join(";", intArray);
            }
            else
            {
                return "0;0";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string strValue)
            {
                try
                {
                    return strValue.Split(new[] { ';' }, 2).Select(v => int.Parse(v)).ToArray();
                }
                catch (Exception)
                {
                    return new int[] { 0, 0 };
                }
            }
            else
            {
                return new int[] { 0, 0 };
            }
        }
    }
}
