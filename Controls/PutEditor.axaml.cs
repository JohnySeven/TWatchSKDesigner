using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using TWatchSKDesigner.Modals;
using TWatchSKDesigner.Models;
using TWatchSKDesigner.Views;

namespace TWatchSKDesigner.Controls
{
    public partial class PutEditor : UserControl
    {
        public PutEditor()
        {
            InitializeComponent();
        }

        private ComponentProperty Property => (ComponentProperty)DataContext;

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async void OnDeleteClick(object sender, RoutedEventArgs e)
        {
            if (await ConfirmBox.Show("Doing this you will delete Signal K put request, are you sure?"))
            {
                Property.Value = null;
            }
        }

        private async void OnButtonClick(object sender, RoutedEventArgs e)
        {
            var push = (Property?.Value as PutRequest)?.Copy() ?? new PutRequest() ;

            var modal = new PutEditorModal(push);

            if (await modal.ShowDialog<bool>(MainWindow.Instance) && Property != null)
            {
                Property.Value = push;
                this.Find<TextBlock>("SKPath").Text = push.Path;
            }
        }
    }
}
