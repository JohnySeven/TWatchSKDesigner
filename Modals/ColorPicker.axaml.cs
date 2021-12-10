using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using System.Collections;
using TWatchSKDesigner.Models.SK;
using TWatchSKDesigner.ViewModels;
using TWatchSKDesigner.Views;

namespace TWatchSKDesigner.Modals
{
    public class ColorPicker : Window
    {
        public Color[] SampleColors { get; set; } = new[] { Colors.White, Colors.Black, Colors.Blue, Colors.Red, Colors.Green, Colors.Gray, Colors.Yellow, Colors.Orange, Colors.Coral };
        //public string[] Colors { get; set; } = new[] { "white", "black", "blue", "red", "green", "gray" };
        public ColorPicker()
        {
            InitializeComponent();

            DataContext = new ColorPickerModel();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        public ColorPickerModel Model => DataContext as ColorPickerModel;


        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnColorButtonClick(object sender, RoutedEventArgs e)
        {
            if(sender is Button btn && btn.DataContext is Color color)
            {
                Model?.UpdateColor(color);
            }
        }

        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            Close(true);
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            Close(false);
        }
    }
}
