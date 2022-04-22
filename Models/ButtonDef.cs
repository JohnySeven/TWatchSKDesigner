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
    public class ButtonDef : ComponentDef
    {
        public ButtonDef()
        {
            Icon = MaterialDesign.Avalonia.PackIcon.PackIconKind.Button;
        }

        private string _Text;

        [JsonProperty("text")]
        [ComponentProperty(typeof(TextBox))]
        public string Text
        {
            get { return _Text; }
            set
            {
                _Text = value;
                this.RaisePropertyChanged();
                PreviewText = value;
            }
        }

        private string _Font;

        [JsonProperty("font")]
        [ComponentProperty(typeof(EnumComboBox<ComponentFont>))]
        public string Font
        {
            get { return _Font; }
            set { _Font = value; this.RaisePropertyChanged(nameof(Font)); }
        }

        private PutRequest _Put;
        [JsonProperty("put")]
        [ComponentProperty(typeof(PutEditor))]
        public PutRequest Put
        {
            get { return _Put; }
            set { _Put = value; OnPropertyChanged(nameof(Put)); }
        }

    }
}
