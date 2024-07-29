using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using TWatchSKDesigner.Intefaces;
using TWatchSKDesigner.Modals;
using TWatchSKDesigner.Models;

namespace TWatchSKDesigner.ViewModels
{
    public class ConsoleWindowModel : ViewModelBase
    {
        private string _SelectedPort;

        public string SelectedPort
        {
            get { return _SelectedPort; }
            set { _SelectedPort = value; OnPropertyChanged(nameof(SelectedPort)); }
        }

        private string[] _AvailablePorts;

        public string[] AvailablePorts
        {
            get { return _AvailablePorts; }
            set { _AvailablePorts = value; OnPropertyChanged(nameof(AvailablePorts)); }
        }

        private bool _ConnectButtonEnabled;

        public bool ConnectButtonEnabled
        {
            get { return _ConnectButtonEnabled; }
            set { _ConnectButtonEnabled = value; OnPropertyChanged(nameof(ConnectButtonEnabled)); }
        }

        private bool _CloseButtonEnabled;

        public bool CloseButtonEnabled
        {
            get { return _CloseButtonEnabled; }
            set { _CloseButtonEnabled = value; OnPropertyChanged(nameof(CloseButtonEnabled)); }
        }

        private bool _PortPickerEnabled = true;

        public bool PortPickerEnabled
        {
            get { return _PortPickerEnabled; }
            set { _PortPickerEnabled = value; OnPropertyChanged(nameof(PortPickerEnabled)); }
        }


        public ICommand RefreshPortsCommand { get; }
        public ICommand CloseCommand { get; set; }
        public ICommand StoreToFileCommand { get; set; }

        private IEsp32ToolService _esp32svc = null;
        private CancellationTokenSource _cancelationToken;

        public ConsoleWindowModel()
        {
            RefreshPortsCommand = ReactiveCommand.CreateFromTask(RefreshPorts);
            CloseCommand = ReactiveCommand.Create(Close);
        }

        public async Task Load(string _openComPort)
        {
            _esp32svc = Locator.Current.GetService<IEsp32ToolService>();
            await RefreshPorts();

            SelectedPort = _openComPort;
        }

        private IEsp32ToolService Service => _esp32svc ?? throw new InvalidOperationException();

        public ICommand ConnectCommand { get; internal set; }

        private async Task RefreshPorts()
        {
            var ports = await Service.GetAvailableSerialPorts();

            AvailablePorts = ports.ToArray();

            if (SelectedPort != null && !AvailablePorts.Contains(SelectedPort))
            {
                SelectedPort = null;
            }
            else
            {
                SelectedPort = AvailablePorts.FirstOrDefault();
            }

            ConnectButtonEnabled = ports.Count > 0;
        }

        public async Task Connect(ITextView textView)
        {
            if (SelectedPort != null)
            {
                _cancelationToken = new CancellationTokenSource();
                ConnectButtonEnabled = false;
                CloseButtonEnabled = true;
                PortPickerEnabled = false;
                try
                {
                    await Service.ConnectToConsole(SelectedPort, textView, _cancelationToken.Token);
                }
                catch (TaskCanceledException)
                {
                    textView.AppendLine("Port closed.");
                }
                catch(ObjectDisposedException)
                {
                    textView.AppendLine("Port closed.");
                }
                catch (Exception ex)
                {
                    textView.AppendLine($"Error: {ex}");
                }
                finally
                {
                    ConnectButtonEnabled = true;
                    CloseButtonEnabled = false;
                    PortPickerEnabled = true;
                }
            }
        }

        public void Close()
        {
            _cancelationToken?.Cancel();
        }
    }
}
