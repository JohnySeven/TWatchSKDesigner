using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TWatchSKDesigner.Models;

namespace TWatchSKDesigner.ViewModels
{
    public class BindingEditorViewModel : ViewModelBase
    {
        public Binding Binding { get; set; }
        public BindingEditorModifier Modifier { get; set; }
    }
}
