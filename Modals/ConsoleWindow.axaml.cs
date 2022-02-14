using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using ReactiveUI;
using System;
using System.ComponentModel;
using System.IO;
using TWatchSKDesigner.Intefaces;
using TWatchSKDesigner.ViewModels;

namespace TWatchSKDesigner.Modals
{
    public partial class ConsoleWindow : Window, ITextView
    {
        private readonly TextBox _console;
        private string _openComPort = null;
        public ConsoleWindowModel Model => DataContext as ConsoleWindowModel;

        public bool IsOpen { get; set; }

        public ConsoleWindow() : this(null)
        {

        }

        public ConsoleWindow(string openComPort)
        {
            _openComPort = openComPort;
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            _console = this.FindControl<TextBox>("ConsoleLog");

            DataContext = new ConsoleWindowModel()
            {
                ConnectCommand = ReactiveCommand.Create(Connect),
                StoreToFileCommand = ReactiveCommand.Create(StoreToFile)
            };

            OnLoaded();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            IsOpen = false;
            Model.Close();
        }

        private async void StoreToFile()
        {
            var dialog = new SaveFileDialog()
            {
                Title = "Save console text file",
                InitialFileName = $"TWatchSK-{DateTime.Now:s}.log"
            };

            var path = await dialog.ShowAsync(this);

            if(path != null)
            {
                var text = _console.Text;

                await File.WriteAllTextAsync(path, text);
            }
        }

        private async void OnLoaded()
        {
            await Model.Load(_openComPort);

            if(_openComPort != null)
            {
                Connect();
            }
        }

        private async void Connect()
        {
            IsOpen = true;
            await Model.Connect(this);
        }
 
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void AppendLine(string line)
        {
            Dispatcher.UIThread.Post(() =>
            {
                _console.Text += line;
                _console.CaretIndex = _console.Text.Length;
            });
        }
    }
}
