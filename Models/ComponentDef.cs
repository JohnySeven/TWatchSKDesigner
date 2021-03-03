using Avalonia.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TWatchSKDesigner.Controls;
using TWatchSKDesigner.Enums;
using TWatchSKDesigner.Helpers;

namespace TWatchSKDesigner.Models
{
    public class ComponentDef : ReactiveObject
    {
        [JsonProperty("location")]
        public int[]? Location { get; set; }

        [JsonProperty("size")]
        public int[]? Size { get; set; }

        [JsonProperty("type")]
        [ComponentProperty(typeof(EnumComboBox<ComponentType>))]
        public string? Type { get; set; }

        public static ComponentDef GetComponent(JObject component)
        {
            switch (component["type"]?.ToString())
            {
                case "label":
                    return LabelHelpers.Load(component);
                default:
                    return JsonConvert.DeserializeObject<ComponentDef>(component.ToString());
            }
        }
    }
}
