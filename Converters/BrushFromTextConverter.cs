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
            var color = value?.ToString();
            
            if (string.IsNullOrEmpty(color))
            {
                return Brushes.Transparent;
            }
            else if (color.StartsWith("#"))
            {
                if (color.Length == 7)
                {
                    var r = byte.Parse(color.Substring(1, 2), NumberStyles.HexNumber);
                    var g = byte.Parse(color.Substring(3, 2), NumberStyles.HexNumber);
                    var b = byte.Parse(color.Substring(5, 2), NumberStyles.HexNumber);
                    return new SolidColorBrush(Color.FromRgb(r, g, b));
                }
                else
                {
                    return Brushes.Black;
                }
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
