using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TWatchSKDesigner.Enums;
using TWatchSKDesigner.ViewModels;

namespace TWatchSKDesigner.Models
{
    public class ComponentProperty : ViewModelBase
    {
        public string? Name { get; set; }

        private object? _Value;

        public object? Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                OnPropertyChanged(nameof(Value));
                Property?.SetValue(Parent, value);
                if(OnChanged != null && OnChanged.TryGetTarget(out Action<ComponentProperty> callback))
                {
                    callback(this);
                }
            }
        }

        public PropertyInfo? Property { get; set; }
        public object? Parent { get; set; }
        public Type? EditorType { get; set; }
        public WeakReference<Action<ComponentProperty>>? OnChanged { get; private set; }
        public ViewLayout VisibleOnLayout { get; private set; }

        private static bool IsVisibleInLayout(WatchView view, ComponentProperty componentPropertyAttribute)
        {
            if(componentPropertyAttribute.VisibleOnLayout == ViewLayout.none)
            {
                return true;
            }
            else
            {
                if(Enum.TryParse(view.Layout, out ViewLayout layout))
                {
                    return componentPropertyAttribute.VisibleOnLayout == layout;
                }
                else
                {
                    return false;
                }
            }
        }

        public static ComponentProperty[] GetProperties(WatchView view, object instance, Action<ComponentProperty> onChanged)
        {
            var properties = instance.GetType().
                GetProperties()
                .Where(p => p.GetCustomAttributes(typeof(ComponentPropertyAttribute), true).Any())
                .Select(p => new ComponentProperty()
                {
                    Name = p.Name,
                    Property = p,
                    Parent = instance,
                    Value = p.GetValue(instance),
                    EditorType = p.GetCustomAttributes(true).OfType<ComponentPropertyAttribute>().First().EditorType,
                    VisibleOnLayout = p.GetCustomAttributes(true).OfType<ComponentPropertyAttribute>().First().VisibleOnLayout,
                    OnChanged = new WeakReference<Action<ComponentProperty>>(onChanged)
                })
                .Where(p => IsVisibleInLayout(view, p))
                .ToArray();

            return properties;
        }
    }
}
