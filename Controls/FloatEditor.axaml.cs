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
    public class FloatEditor : UserControl
    {
        public FloatEditor()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
