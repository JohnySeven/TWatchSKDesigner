using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using Splat;
using System;
using System.Diagnostics;
using System.Reflection;
using TWatchSKDesigner.Controls;
using TWatchSKDesigner.Converters;
using TWatchSKDesigner.Enums;
using TWatchSKDesigner.Intefaces;
using TWatchSKDesigner.Modals;
using TWatchSKDesigner.ViewModels;

namespace TWatchSKDesigner.Views
{
    public class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }

        public MainWindow()
        {
            Instance = this;
            InitializeComponent();
            var version = Assembly.GetEntryAssembly().GetName().Version;

            Title += $" v{version.Major}.{version.Minor}";
#if DEBUG
            this.AttachDevTools();
#endif
            InitializeEditorTemplates();
        }

        private static void InitializeEditorTemplates()
        {
            PropertyEditorConverter.InitializeTemplates();
        }

        public MainWindowViewModel Model => DataContext as MainWindowViewModel ?? throw new InvalidOperationException();

        private void InitializeComponent()
        {
            DataContext = new MainWindowViewModel();
            AvaloniaXamlLoader.Load(this);
        }

        private void Exit_Clicked(object sender, RoutedEventArgs eventArgs)
        {
            Close();
        }

        private async void Save_Clicked(object sender, RoutedEventArgs eventArgs)
        {
            await ProgressWindow.ShowProgress("Saving view...", async () =>
            {
                var result = await Model.SaveView();

                if(result.IsSuccess == true)
                {
                    await MessageBox.Show("Saved!");
                }
                else
                {
                    await MessageBox.Show(result.ErrorMessage);
                }
            });
        }



        private async void Console_Clicked(object sender, RoutedEventArgs eventArgs)
        {
            var consoleWindow = new ConsoleWindow();

            await consoleWindow.ShowDialog(this);
        }


        private async void UploadFirmware_Clicked(object sender, RoutedEventArgs eventArgs)
        {
            var firmwareDialog = new FlashFirmwareModal();

            await firmwareDialog.ShowDialog(this);
        }

        private void NewView_Clicked(object sender, RoutedEventArgs eventArgs)
        {
            Model?.CreateNewView();
        }

        private void NewLabel_Clicked(object sender, RoutedEventArgs eventArgs)
        {
            Model?.AddNewLabel();
        }

        private void NewGauge_Clicked(object sender, RoutedEventArgs eventArgs)
        {
            Model?.AddNewGauge();
        }

        private void NewSwitch_Clicked(object sender, RoutedEventArgs eventArgs)
        {
            Model?.AddNewSwitch();
        }
        private void NewButton_Clicked(object sender, RoutedEventArgs eventArgs)
        {
            Model?.AddNewButton();
        }

        private async void OpenUrl_Clicked(object sender, RoutedEventArgs eventArgs)
        {
            if (sender is MenuItem menu && menu.Tag is string url)
            {
                try
                {
                    var platform = Locator.Current.GetService<IPlatformSupport>();

                    await platform.LaunchBrowser(new Uri(url));
                }
                catch (Exception ex)
                {
                    await MessageBox.Show("Unable to launch browser, error: " + ex.Message);
                }
            }
        }
    }
}
