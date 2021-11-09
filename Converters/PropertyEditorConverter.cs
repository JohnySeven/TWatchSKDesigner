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
using TWatchSKDesigner.Controls;
using TWatchSKDesigner.Enums;

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

        internal static void InitializeTemplates()
        {

            if (EditorTemplates.Count == 0)
            {
                EditorTemplates.Add(typeof(TextBox), new FuncDataTemplate<object>((v, s) =>
                {
                    return new TextBox()
                    {
                        [!TextBox.TextProperty] = new Binding("Value", BindingMode.TwoWay),
                        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch
                    };
                }));

                EditorTemplates.Add(typeof(EnumComboBox<ComponentType>), new FuncDataTemplate<object>((v, s) =>
                {
                    return new EnumComboBox<ComponentType>()
                    {
                        DataContext = v,
                        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch
                    };
                }));

                EditorTemplates.Add(typeof(EnumComboBox<ComponentFont>), new FuncDataTemplate<object>((v, s) =>
                {
                    return new EnumComboBox<ComponentFont>()
                    {
                        DataContext = v,
                        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch
                    };
                }));

                EditorTemplates.Add(typeof(EnumComboBox<ViewType>), new FuncDataTemplate<object>((v, s) =>
                {
                    return new EnumComboBox<ViewType>()
                    {
                        DataContext = v,
                        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch
                    };
                }));

                EditorTemplates.Add(typeof(EnumComboBox<ViewLayout>), new FuncDataTemplate<object>((v, s) =>
                {
                    return new EnumComboBox<ViewLayout>()
                    {
                        DataContext = v
                    };
                }));

                EditorTemplates.Add(typeof(BindingEditor), new FuncDataTemplate<object>((v, s) =>
                {
                    return new BindingEditor()
                    {
                        DataContext = v
                    };
                }));

                EditorTemplates.Add(typeof(ColorPickerEditor), new FuncDataTemplate<object>((v, s) =>
                {
                    return new ColorPickerEditor()
                    {
                        DataContext = v
                    };
                }));

                EditorTemplates.Add(typeof(XYEditor), new FuncDataTemplate<object>((v, s) =>
                {
                    return new XYEditor()
                    {
                        DataContext = v
                    };
                }));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
