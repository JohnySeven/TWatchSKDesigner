using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TWatchSKDesigner.Converters
{
    public class SelectionBrushConverter : IValueConverter
    {
        public ISolidColorBrush SelectedColor { get; set; } = Brushes.CornflowerBlue;
        public ISolidColorBrush UnselectedColor { get; set; } = Brushes.LightGray;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is bool boolValue)
            {
                return boolValue ? SelectedColor : UnselectedColor;
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
