using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Collections;
using TWatchSKDesigner.Models.SK;
using TWatchSKDesigner.ViewModels;
using TWatchSKDesigner.Views;

namespace TWatchSKDesigner.Modals
{
    public class SelectSKPath : Window
    {
        
        public SelectSKPath()
        {
            InitializeComponent();
            DataContext = new SelectSKPathModel();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        public SelectSKPathModel Model => DataContext as SelectSKPathModel;

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            if (Model?.SelectedPath != null)
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
