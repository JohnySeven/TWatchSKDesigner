using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TWatchSKDesigner.Models;

namespace TWatchSKDesigner.Helpers
{
    public static class ComponentHelpers
    {
        public static readonly Dictionary<string, Type> ComponentFactory = new()
        {
            { "label", typeof(LabelDef) },
            { "gauge", typeof(GaugeDef) },
            { "switch", typeof(SwitchDef) }
        };

        internal static ComponentDef LoadComponent(JToken component)
        {
            var typeOf = component["type"]?.ToString();

            if (ComponentFactory.TryGetValue(typeOf, out Type componentType))
            {
                return component.ToObject(componentType) as ComponentDef;
            }
            else
            {
                return JsonConvert.DeserializeObject<ComponentDef>(component.ToString());
            }
        }
    }
}
