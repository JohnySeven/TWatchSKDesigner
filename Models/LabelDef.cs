using Avalonia.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using TWatchSKDesigner.Controls;
using TWatchSKDesigner.Enums;

namespace TWatchSKDesigner.Models
{
    public class LabelDef : ComponentDef
    {
        private string? _Text;

        [JsonProperty("text")]
        [ComponentProperty(typeof(TextBox))]
        public string? Text
        {
            get { return _Text; }
            set { _Text = value; this.RaisePropertyChanged(); }
        }


        [JsonProperty("font")]
        [ComponentProperty(typeof(EnumComboBox<ComponentFont>))]
        public string? Font { get; set; }
        [JsonProperty("color")]
        
        public string? Color { get; set; }

        [JsonProperty("binding")]
        public Binding? Binding { get; set; }
    }
}
