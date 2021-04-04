using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using TWatchSKDesigner.Models;

namespace TWatchSKDesigner.Modals
{
    public class BindingEditorModal : Window
    {
        public BindingEditorModal()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        public Binding? Model => (Binding?)DataContext;

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            Close(true);
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            Close(false);
        }

        private async void PickSKPath(object sender, RoutedEventArgs e)
        {
            var skPathPick = new SelectSKPath();

            if(await skPathPick.ShowDialog<bool>(this))
            {
                //this is hack, we need to create real model!
                var skPath = this.Find<TextBox>("SKPath");
                skPath.Text = skPathPick.SelectedPath?.Path;
            }
        }
    }
}
