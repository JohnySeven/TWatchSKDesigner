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
    public class SwitchBindingModifier : BindingEditorModifier
    {
        public SwitchBindingModifier()
        {
            ShowDecimals = false;
            ShowFormat = false;
            ShowMultiplyAndOffset = false;
            SignalKPathFilter = s =>
            {
                return true;
            };
        }
    }

    public class SwitchDef : ComponentDef
    {
        public SwitchDef()
        {
            Icon = MaterialDesign.Avalonia.PackIcon.PackIconKind.ToggleSwitch;
        }

        private Binding _Binding;

        [JsonProperty("binding")]
        [ComponentProperty(typeof(BindingEditor<SwitchBindingModifier>))]
        public Binding Binding
        {
            get { return _Binding; }
            set
            {
                _Binding = value;
                this.RaisePropertyChanged(nameof(Binding));
            }
        }
    }
}
