using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml.Templates;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TWatchSKDesigner.Converters
{
    public class PropertyEditorConverter : IValueConverter
    {
        public static Dictionary<Type, FuncDataTemplate<object>> EditorTemplates { get; private set; } = new Dictionary<Type, FuncDataTemplate<object>>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is Type editorType)
            {
                return EditorTemplates[editorType];
            }
            else
            {
                return new FuncDataTemplate<object>((v, s) => new TextBlock()
                {
                    [!TextBlock.TextProperty] = new Binding(".")
                });
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
