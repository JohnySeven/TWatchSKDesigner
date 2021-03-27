using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace TWatchSKDesigner.Controls
{
    public class BindingEditor : UserControl
    {
        public BindingEditor()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
