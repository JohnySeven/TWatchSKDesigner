using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TWatchSKDesigner.Intefaces;
using TWatchSKDesigner.Modals;
using TWatchSKDesigner.Models;

namespace TWatchSKDesigner.ViewModels
{
    public class FirmwareUploadViewModel : ViewModelBase
    {
        private string? _SelectedPort;

        public string? SelectedPort
        {
            get { return _SelectedPort; }
            set { _SelectedPort = value; OnPropertyChanged(nameof(SelectedPort)); }
        }

        private string[]? _AvailablePorts;

        public string[]? AvailablePorts
        {
            get { return _AvailablePorts; }
            set { _AvailablePorts = value; OnPropertyChanged(nameof(AvailablePorts)); }
        }

        private bool _ClearFlash;

        public bool ClearFlash
        {
            get { return _ClearFlash; }
            set { _ClearFlash = value; OnPropertyChanged(nameof(ClearFlash)); }
        }

        private bool _FlashButtonEnabled;

        public bool FlashButtonEnabled
        {
            get { return _FlashButtonEnabled; }
            set { _FlashButtonEnabled = value; OnPropertyChanged(nameof(FlashButtonEnabled)); }
        }


        public ICommand RefreshPortsCommand { get; }
        public ICommand UploadFirmwareCommand { get; }

        public ICommand? CancelCommand { get; set; }

        private IEsp32ToolService? _esp32svc = null;

        public FirmwareUploadViewModel()
        {
            RefreshPortsCommand = ReactiveCommand.CreateFromTask(RefreshPorts);
            UploadFirmwareCommand = ReactiveCommand.CreateFromTask(UploadFirmware);
        }

        public async Task Load()
        {
            _esp32svc = Locator.Current.GetService<IEsp32ToolService>();
            await RefreshPorts();
        }

        private IEsp32ToolService Service => _esp32svc ?? throw new InvalidOperationException();

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

            FlashButtonEnabled = ports.Count > 0;
        }

        private async Task UploadFirmware()
        {
            if (SelectedPort != null)
            {
                var monitor = new ProgressWindowTaskMonitor();
                await monitor.Run(async () =>
                {
                    var initResult = await Service.Initialize(monitor);

                    if (initResult.IsSuccess)
                    {
                        var downloadResult = await Service.DownloadLatestFirmware(monitor);

                        if (downloadResult.IsSuccess)
                        {
                            var flashResult = await Service.FlashFirmware(SelectedPort,  downloadResult.Data.FullName, _ClearFlash, monitor);

                            if (flashResult.IsSuccess)
                            {
                                await MessageBox.Show($"TWatch 2020 firmware upload succesful!");
                            }
                        }
                    }
                });
            }
        }
    }
}
