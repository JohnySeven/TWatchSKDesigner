using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using System;
using TWatchSKDesigner.ViewModels;

namespace TWatchSKDesigner.Views
{
    public class ComponentProperties : UserControl
    {
        private ComponentPropertiesViewModel? Model => DataContext as ComponentPropertiesViewModel;

        public ComponentProperties()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
