using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TWatchSKDesigner.Models
{
    public class ComponentProperty : ReactiveObject
    {
        public string? Name { get; set; }

        private object? _Value;

        public object? Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                this.RaiseAndSetIfChanged(ref _Value, value);
                Property?.SetValue(Parent, value);
            }
        }

        public PropertyInfo? Property { get; set; }
        public object? Parent { get; set; }
        public Type? EditorType { get; set; }

        public static ComponentProperty[] GetProperties(object instance)
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
                            EditorType = p.GetCustomAttributes(true).OfType<ComponentPropertyAttribute>().First().EditorType
                        })
                .ToArray();

            return properties;
        }
    }
}
