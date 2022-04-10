using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace TWatchSKDesigner.Controls
{
    public partial class SwitchControl : UserControl
    {
        private Border _Outer;
        private Border _OnOffToggle;

        public SwitchControl()
        {
            InitializeComponent();
            _Outer = this.Find<Border>("Outer");
            _OnOffToggle = this.Find<Border>("OnOffToggle");
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var ret = base.MeasureOverride(availableSize);
            _Outer.CornerRadius = new CornerRadius(availableSize.Height / 2.0);
            _OnOffToggle.CornerRadius = new CornerRadius(availableSize.Height / 2.0);
            _OnOffToggle.Width = availableSize.Height - 20.0;
            _OnOffToggle.Height = availableSize.Height - 20.0f;

            return ret;
        }
    }
}
