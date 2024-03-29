using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.Linq;
using TWatchSKDesigner.Models;
using TWatchSKDesigner.ViewModels;
using TWatchSKDesigner.Views;

namespace TWatchSKDesigner.Modals
{
    public class BindingEditorModal : Window
    {
        /// <summary>
        /// This ctor is just for designer preview support
        /// </summary>
        public BindingEditorModal() : this(new Binding() {  Path = "sample.binding.preview", Period = 1000, Decimals = 1, Format = "$$ unit", Multiply = 100.0f, OffSet = 200.0f }
                                                            , new BindingEditorModifier())
        {

        }

        public BindingEditorModal(Binding binding, BindingEditorModifier modifier)
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            DataContext = new BindingEditorViewModel()
            {
                Binding = binding,
                Modifier = modifier
            };
        }

        public BindingEditorViewModel Model => (BindingEditorViewModel)DataContext;

        private bool Validate(out string errorMessage)
        {
            var ret = true;

            var binding = Model.Binding;

            if(binding.OffSet == null)
            {
                binding.OffSet = 0.0f;
            }

            if(binding.Multiply == null)
            {
                binding.Multiply = 1.0f;
            }

            if (!string.IsNullOrEmpty(binding?.Format) && binding?.Format.Contains("$$") == false)
            {
                ret = false;
                errorMessage = "Binding format must contain $$ symbols to make replacement working!";
            }
            else if(binding?.Period < 1000)
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

            var skPath = MainWindow.Instance?.Model?.SignalKManager.GetSignalKPaths(Model.Modifier.SignalKPathFilter).Where(p => p.Path == path).FirstOrDefault();

            Func<Conversion, bool> filter = null;
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
            var skPathPick = new SelectSKPath(Model.Modifier.SignalKPathFilter);

            if(await skPathPick.ShowDialog<bool>(this))
            {
                //this is hack, we need to create real model!
                var skPath = this.Find<TextBox>("SKPath");
                skPath.Text = skPathPick.Model?.SelectedPath?.Path;
            }
        }
    }
}
