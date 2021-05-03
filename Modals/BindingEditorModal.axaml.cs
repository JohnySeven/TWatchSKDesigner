using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.Linq;
using TWatchSKDesigner.Models;
using TWatchSKDesigner.Views;

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

        private bool Validate(out string errorMessage)
        {
            var ret = true;

            if (!string.IsNullOrEmpty(Model?.Format) && Model?.Format.Contains("$$") == false)
            {
                ret = false;
                errorMessage = "Binding format must contain $$ symbols to make replacement working!";
            }
            else if(Model?.Period < 1000)
            {
                errorMessage = "Period must be at least 1000 ms!";
                ret = false;
            }
            else
            {
                errorMessage = "";
            }

            return ret;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async void OnOkClick(object sender, RoutedEventArgs e)
        {
            if (!Validate(out string errorMessage))
            {
                await MessageBox.Show(errorMessage);
            }
            else
            {
                Close(true);
            }
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            Close(false);
        }

        private async void SelectUnits(object sender, RoutedEventArgs e)
        {
            var skPathTextBox = this.Find<TextBox>("SKPath");
            var path = skPathTextBox.Text;

            var skPath = MainWindow.Instance?.Model?.SignalKManager.GetSignalKPaths().Where(p => p.Path == path).FirstOrDefault();

            Func<Conversion, bool>? filter = null;
            if(skPath != null && skPath.Units != null)
            {
                filter = c => c.From == skPath.Units;
            }

            var unitConversion = new SelectUnitConversion(filter);

            if (await unitConversion.ShowDialog<bool>(this))
            {
                var offset = this.Find<NumericUpDown>("OffSet");
                var multiply = this.Find<NumericUpDown>("Multiply");

                offset.Text = unitConversion.SelectedConversion?.OffSet.ToString();
                multiply.Text = unitConversion.SelectedConversion?.Multiply.ToString();

            }
        }

        private async void PickSKPath(object sender, RoutedEventArgs e)
        {
            var skPathPick = new SelectSKPath();

            if(await skPathPick.ShowDialog<bool>(this))
            {
                //this is hack, we need to create real model!
                var skPath = this.Find<TextBox>("SKPath");
                skPath.Text = skPathPick.Model?.SelectedPath?.Path;
            }
        }
    }
}
