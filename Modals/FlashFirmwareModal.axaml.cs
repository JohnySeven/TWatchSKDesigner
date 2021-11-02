using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using System;
using TWatchSKDesigner.ViewModels;

namespace TWatchSKDesigner.Modals
{
    public partial class FlashFirmwareModal : Window
    {
        public FlashFirmwareModal()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            DataContext = new FirmwareUploadViewModel()
            {
                CancelCommand = ReactiveCommand.Create(Close)
            };

            Opened += FlashFirmwareModal_Opened;        }

        private async void FlashFirmwareModal_Opened(object? sender, EventArgs e)
        {
            if (!Design.IsDesignMode)
            {
                await ViewModel.Load();
            }
        }

        public FirmwareUploadViewModel ViewModel => DataContext as FirmwareUploadViewModel ?? throw new InvalidOperationException();

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
