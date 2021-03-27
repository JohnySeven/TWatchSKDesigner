using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TWatchSKDesigner.Converters
{
    public class DoubleFromIntArray : IValueConverter
    {
        public int Index { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double ret = 0;

            if(value != null && value is int[] array)
            {
                if(array.Length >= Index + 1)
                {
                    ret = (double)array[Index];
                }
            }

            return ret;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
