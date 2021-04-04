using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Collections;
using TWatchSKDesigner.Models.SK;
using TWatchSKDesigner.Views;

namespace TWatchSKDesigner.Modals
{
    public class SelectSKPath : Window
    {
        public SelectSKPath()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        public SKPath[]? Paths => MainWindow.Instance?.Model?.SignalKManager.GetSignalKPaths();

        public SKPath? SelectedPath { get; set; }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            if (SelectedPath != null)
            {
                Close(true);
            }
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            Close(false);
        }
    }
}
