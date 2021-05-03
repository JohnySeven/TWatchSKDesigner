using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using TWatchSKDesigner.Modals;
using TWatchSKDesigner.Models;
using TWatchSKDesigner.Views;

namespace TWatchSKDesigner.Controls
{
    public class ColorPickerEditor : UserControl
    {
        public ColorPickerEditor()
        {
            InitializeComponent();
        }

        private ComponentProperty? Property => (ComponentProperty?)DataContext;


        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async void OnButtonClick(object sender, RoutedEventArgs e)
        {
            if (Property != null)
            {
                var modal = new ColorPicker();
                modal.Model?.FromText(Property?.Value?.ToString());
                if (await modal.ShowDialog<bool>(MainWindow.Instance))
                {
                    Property.Value = modal.Model?.ToHex().ToLower();
                }
            }
        }
    }
}
