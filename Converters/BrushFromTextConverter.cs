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
    public class BrushFromTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = value.ToString();
            
            if (string.IsNullOrEmpty(color))
            {
                return Brushes.White;
            }
            else if (color.StartsWith("#"))
            {
                return new SolidColorBrush(Colors.Red);
            }
            else if (Color.TryParse(color, out Color parsedColor))
            {
                return new SolidColorBrush(parsedColor);
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
