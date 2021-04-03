using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace TWatchSKDesigner.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        protected void OnPropertyChanged(string name)
        {
            this.RaisePropertyChanged(name);
        }
    }
}
