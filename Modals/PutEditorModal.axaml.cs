using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using TWatchSKDesigner.Models;
using TWatchSKDesigner.ViewModels;
using TWatchSKDesigner.Views;

namespace TWatchSKDesigner.Modals
{
    public class PutEditorModal : Window
    {
        /// <summary>
        /// This ctor is just for designer preview support
        /// </summary>
        public PutEditorModal() : this(new PutRequest() { Path = "sample.binding.preview", Value = JToken.Parse("false") })
        {

        }

        public PutEditorModal(PutRequest put)
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            DataContext = new PutEditorModalViewModel()
            {
                Put = put
            };

            Model.Load();
        }

        public PutEditorModalViewModel Model => (PutEditorModalViewModel)DataContext;

        private bool Validate(out string errorMessage)
        {
            var ret = true;

            var push = Model.Put;

            if (string.IsNullOrEmpty(push.Path))
            {
                ret = false;
                errorMessage = "Signal K put request must contain path!";
            }
            else if(push.Value == null)
            {
                errorMessage = "Signal K put request must contain value!";
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
