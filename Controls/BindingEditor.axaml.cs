using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using System;
using TWatchSKDesigner.Modals;
using TWatchSKDesigner.Models;
using TWatchSKDesigner.ViewModels;
using TWatchSKDesigner.Views;

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

        private ComponentProperty? Property => (ComponentProperty?)DataContext;

        private async void OnButtonClick(object sender, RoutedEventArgs e)
        {
            Binding? binding = (Property?.Value as Binding)?.Copy() ?? new Binding() { Multiply = 1.0f, Period = 1000 };

            var modal = new BindingEditorModal()
            {
                DataContext = binding
            };

            if (await modal.ShowDialog<bool>(MainWindow.Instance) && Property != null)
            {
                Property.Value = binding;
                this.Find<TextBlock>("SKPath").Text = binding.Path;
            }
        }
    }
}
