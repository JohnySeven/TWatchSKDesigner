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
    public class BindingEditor<T> : BindingEditor where T:BindingEditorModifier, new()
    {
        public BindingEditor() : base(new T())
        {

        }
    }

    public class BindingEditor : UserControl
    {
        public BindingEditor() : this(new BindingEditorModifier())
        {

        }

        public BindingEditor(BindingEditorModifier modifier)
        {
            InitializeComponent();
            Modifier = modifier;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private ComponentProperty Property => (ComponentProperty)DataContext;

        protected BindingEditorModifier Modifier { get; set; } = new BindingEditorModifier();

        private async void OnDeleteClick(object sender, RoutedEventArgs e)
        {
            if(await ConfirmBox.Show("Doing this you will remove binding, are you sure?"))
            {
                Property.Value = null;
            }
        }

        private async void OnButtonClick(object sender, RoutedEventArgs e)
        {
            Binding binding = (Property?.Value as Binding)?.Copy() ?? new Binding() { Multiply = 1.0f, Period = 1000 };

            var modal = new BindingEditorModal(binding, Modifier);

            if (await modal.ShowDialog<bool>(MainWindow.Instance) && Property != null)
            {
                Property.Value = binding;
                this.Find<TextBlock>("SKPath").Text = binding.Path;                
            }
        }
    }
}
