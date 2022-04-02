using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using Splat;
using System;
using TWatchSKDesigner.Controls;
using TWatchSKDesigner.Converters;
using TWatchSKDesigner.Enums;
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
    }
}
