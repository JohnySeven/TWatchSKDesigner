using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TWatchSKDesigner.Models;

namespace TWatchSKDesigner.Converters
{
    public class BindingToValueFormat : IValueConverter
    {
        public object Value { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is Binding binding)
            {
                if(binding.Format != null)
                {
                    return binding.Format?.Replace("$$", Value?.ToString());
                }
                else
                {
                    return Value?.ToString();
                }
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
