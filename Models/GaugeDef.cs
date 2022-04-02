using Avalonia.Controls;
using Newtonsoft.Json;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TWatchSKDesigner.Controls;

namespace TWatchSKDesigner.Models
{
    public class GaugeDef : ComponentDef
    {
        public GaugeDef()
        {
            Icon = MaterialDesign.Avalonia.PackIcon.PackIconKind.Gauge;
        }

        private string _Color;

        [ComponentProperty(typeof(ColorPickerEditor), propertyDescription: "Changes guage outer color.")]
        [JsonProperty("color")]
        public string Color
        {
            get { return _Color; }
            set { _Color = value; this.RaisePropertyChanged(nameof(Color)); }
        }

        private string _TextColor;
        [ComponentProperty(typeof(ColorPickerEditor), propertyDescription: "Changes text value color.")]
        [JsonProperty("text-color")]
        public string TextColor
        {
            get { return _TextColor; }
            set { _TextColor = value; this.RaisePropertyChanged(nameof(TextColor)); }
        }

        private Binding _Binding;

        [JsonProperty("binding")]
        [ComponentProperty(typeof(BindingEditor), propertyDescription: "Links gauge with Signal K path, defines transformations, output format.")]
        public Binding Binding
        {
            get { return _Binding; }
            set
            {
                _Binding = value;
                this.RaisePropertyChanged(nameof(Binding));

                if (value != null)
                {
                    PreviewText = value?.Path;
                }
                else
                {
                    PreviewText = "";
                }
            }
        }

        private float? _Minimum;
        private float? _Maximum;

        [JsonProperty("minimum")]
        [ComponentProperty(typeof(FloatEditor), propertyDescription: "Defines gauge minimum value.")]
        public float? Minimum
        {
            get { return _Minimum; }
            set
            {
                _Minimum = value;
                this.RaisePropertyChanged();
            }
        }

        [JsonProperty("maximum")]
        [ComponentProperty(typeof(FloatEditor), propertyDescription: "Defines gauge maximum value.")]
        public float? Maximum
        {
            get { return _Maximum; }
            set
            {
                _Maximum = value;
                this.RaisePropertyChanged();
            }
        }
    }
}