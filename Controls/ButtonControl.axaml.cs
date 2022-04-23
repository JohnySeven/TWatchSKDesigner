using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace TWatchSKDesigner.Controls
{
    public partial class ButtonControl : UserControl
    {
        private Border _Outer;

        public static readonly StyledProperty<string> TextProperty =
            AvaloniaProperty.Register<ButtonControl, string>(nameof(Text), "Sample text");

        public string Text
        {
            get => GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public ButtonControl()
        {
            InitializeComponent();
            _Outer = this.Find<Border>("Outer");
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var ret = base.MeasureOverride(availableSize);
            _Outer.CornerRadius = new CornerRadius(availableSize.Height / 2.0);

            return ret;
        }
    }
}
