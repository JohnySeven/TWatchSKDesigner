using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TWatchSKDesigner.Models
{
    public class BindingEditorModifier
    {
        public bool ShowMultiplyAndOffset { get; protected set; } = true;
        public bool ShowDecimals { get; protected set; } = true;
        public bool ShowFormat { get; protected set; } = true;
        public Func<SK.SKPath, bool> SignalKPathFilter { get; protected set; } = p => true;
    }
}
