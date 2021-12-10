using Avalonia.Controls;
using MaterialDesign.Avalonia.PackIcon;
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
using TWatchSKDesigner.ViewModels;

namespace TWatchSKDesigner.Models
{
    public class ComponentDef : ViewModelBase
    {
        private int[] _Location;

        [JsonProperty("location")]
        [ComponentProperty(typeof(XYEditor), ViewLayout.off, true)]
        public int[] Location
        {
            get { return _Location; }
            set { _Location = value; OnPropertyChanged(nameof(Location)); }
        }

        private string _PreviewText;

        [JsonIgnore]
        public string PreviewText
        {
            get { return _PreviewText; }
            set { _PreviewText = value; OnPropertyChanged(nameof(PreviewText)); }
        }

        [JsonIgnore]
        public bool IsRemovable { get; protected set; } = true;

        [JsonProperty("type")]
        [ComponentProperty(typeof(EnumComboBox<ComponentType>))]
        public string Type { get; set; }

        private bool _IsSelected;

        public bool IsSelected
        {
            get { return _IsSelected; }
            set { _IsSelected = value; OnPropertyChanged(nameof(IsSelected)); }
        }

        [JsonIgnore]
        public PackIconKind Icon { get; protected set; }


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
