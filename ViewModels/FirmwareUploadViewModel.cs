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
        private string _SelectedPort;

        public string SelectedPort
        {
            get { return _SelectedPort; }
            set
            {
                _SelectedPort = value; 
                OnPropertyChanged(nameof(SelectedPort)); 
                UpdateFlashButtonEnabled();
            }
        }

        private string[] _AvailablePorts;

        public string[] AvailablePorts
        {
            get { return _AvailablePorts; }
            set { _AvailablePorts = value; OnPropertyChanged(nameof(AvailablePorts)); }
        }

        private FirmwareLink[] _FirmwareList;

        public FirmwareLink[] FirmwareList
        {
            get { return _FirmwareList; }
            set { _FirmwareList = value; OnPropertyChanged(nameof(FirmwareList)); }
        }

        private FirmwareLink _SelectedFirmware;

        public FirmwareLink SelectedFirmware
        {
            get { return _SelectedFirmware; }
            set
            {
                _SelectedFirmware = value; 
                OnPropertyChanged(nameof(SelectedFirmware));
                UpdateFlashButtonEnabled();
            }
        }

        private string _FirmwareVersion = "Loading...";

        public string FirmwareVersion
        {
            get { return _FirmwareVersion; }
            set { _FirmwareVersion = value; OnPropertyChanged(nameof(FirmwareVersion)); }
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

        private void UpdateFlashButtonEnabled()
        {
            FlashButtonEnabled = AvailablePorts.Length > 0 && FirmwareList?.Length > 0;
        }

        private bool _ShowConsoleAfterFlashing = true;

        public bool ShowConsoleAfterFlashing
        {
            get { return _ShowConsoleAfterFlashing; }
            set { _ShowConsoleAfterFlashing = value; OnPropertyChanged(nameof(ShowConsoleAfterFlashing)); }
        }

        public ICommand RefreshPortsCommand { get; }
        public ICommand UploadFirmwareCommand { get; }
        public ICommand CancelCommand { get; set; }

        private IEsp32ToolService _esp32svc = null;

        public FirmwareUploadViewModel()
        {
            RefreshPortsCommand = ReactiveCommand.CreateFromTask(RefreshPorts);
            UploadFirmwareCommand = ReactiveCommand.CreateFromTask(UploadFirmware);
        }

        public async Task Load()
        {
            _esp32svc = Locator.Current.GetService<IEsp32ToolService>();
            await RefreshPorts();
            await LoadFirmwareList();
        }

        private async Task LoadFirmwareList()
        {
            var result = await Service.DownloadFirmwareList();

            if (result.IsSuccess)
            {
                FirmwareList = result.Data.Links;
                FirmwareVersion = result.Data.Version;
            }
            else
            {
                FirmwareList = Array.Empty<FirmwareLink>();
                await MessageBox.Show("Firmware list download failed: " + result.ErrorMessage);
            }
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
        }

        private async Task UploadFirmware()
        {
            if (SelectedPort != null && SelectedFirmware != null)
            {
                var monitor = new ProgressWindowTaskMonitor();
                await monitor.Run(async () =>
                {
                    var initResult = await Service.Initialize(monitor);

                    if (initResult.IsSuccess)
                    {
                        var downloadResult = await Service.DownloadLatestFirmware(_SelectedFirmware, monitor);

                        if (downloadResult.IsSuccess)
                        {
                            var firmwareInfo = await Service.LoadFirmwareInfo(downloadResult.Data.FullName);

                            if (firmwareInfo.IsSuccess)
                            {
                                var flashResult = await Service.FlashFirmware(SelectedPort, downloadResult.Data.FullName, _ClearFlash, monitor);

                                if (flashResult.IsSuccess)
                                {
                                    if (ShowConsoleAfterFlashing)
                                    {
                                        var consoleWindow = new ConsoleWindow(SelectedPort);

                                        await consoleWindow.ShowDialog(Views.MainWindow.Instance);
                                    }
                                    else
                                    {
                                        await MessageBox.Show($"TWatch 2020 firmware {SelectedFirmware.Hardware} {firmwareInfo.Data.Version} upload succesful!");
                                    }
                                }
                                else
                                {
                                    await MessageBox.Show($"Firmware update failed with error: {flashResult.ErrorMessage}");
                                }
                            }
                            else
                            {
                                await MessageBox.Show($"Unable to load firmware info file at {downloadResult.Data.FullName}!");
                            }
                        }
                    }
                });
            }
        }
    }
}
