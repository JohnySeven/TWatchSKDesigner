using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Linq;
using TWatchSKDesigner.Models;

namespace TWatchSKDesigner.Controls
{
    public class EnumComboBox : UserControl
    {
        public EnumComboBox()
        {
            InitializeComponent();
        }

        protected ComboBox Combo => this.Find<ComboBox>("Combo");

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            Initialize();
        }

        protected virtual void Initialize() { }
    }

    public class EnumComboBox<T> : EnumComboBox
    {
        protected override void Initialize()
        {
            Combo.Items = Enum.GetNames(typeof(T)).Select(v => v.ToLower()).ToArray();
            Combo.Bind(ComboBox.SelectedItemProperty, new Avalonia.Data.Binding("Value", Avalonia.Data.BindingMode.TwoWay));
        }

        protected override void OnDataContextChanged(EventArgs e)
        {
            base.OnDataContextChanged(e);
        }
    }
}