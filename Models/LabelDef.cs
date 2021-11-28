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
        public LabelDef()
        {
            Icon = MaterialDesign.Avalonia.PackIcon.PackIconKind.Label;
        }

        private string? _Text;

        [JsonProperty("text")]
        [ComponentProperty(typeof(TextBox))]
        public string? Text
        {
            get { return _Text; }
            set
            {
                _Text = value;
                this.RaisePropertyChanged();
                PreviewText = value;
            }
        }


        private string? _Font;

        [JsonProperty("font")]
        [ComponentProperty(typeof(EnumComboBox<ComponentFont>))]
        public string? Font
        {
            get { return _Font; }
            set { _Font = value; this.RaisePropertyChanged(nameof(Font)); }
        }


        private string? _Color;

        [ComponentProperty(typeof(ColorPickerEditor))]
        [JsonProperty("color")]
        public string? Color
        {
            get { return _Color; }
            set { _Color = value; this.RaisePropertyChanged(nameof(Color)); }
        }

        private Binding? _Binding;

        [JsonProperty("binding")]
        [ComponentProperty(typeof(BindingEditor))]
        public Binding? Binding
        {
            get { return _Binding; }
            set
            {
                _Binding = value; 
                this.RaisePropertyChanged(nameof(Binding));

                if(value != null)
                {
                    Text = value?.Format?.Replace("$$", "--") ?? "--";
                    PreviewText = value?.Path;
                }
            }
        }

    }
}
