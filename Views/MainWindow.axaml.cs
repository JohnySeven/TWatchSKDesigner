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
        public static MainWindow? Instance { get; private set; }

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
            if (PropertyEditorConverter.EditorTemplates.Count == 0)
            {
                PropertyEditorConverter.EditorTemplates.Add(typeof(TextBox), new FuncDataTemplate<object>((v, s) =>
                {
                    return new TextBox()
                    {
                        [!TextBox.TextProperty] = new Binding("Value", BindingMode.TwoWay),
                        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch
                    };
                }));

                PropertyEditorConverter.EditorTemplates.Add(typeof(EnumComboBox<ComponentType>), new FuncDataTemplate<object>((v, s) =>
                {
                    return new EnumComboBox<ComponentType>()
                    {
                        DataContext = v,
                        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch
                    };
                }));

                PropertyEditorConverter.EditorTemplates.Add(typeof(EnumComboBox<ComponentFont>), new FuncDataTemplate<object>((v, s) =>
                {
                    return new EnumComboBox<ComponentFont>()
                    {
                        DataContext = v,
                        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch
                    };
                }));

                PropertyEditorConverter.EditorTemplates.Add(typeof(EnumComboBox<ViewType>), new FuncDataTemplate<object>((v, s) =>
                {
                    return new EnumComboBox<ViewType>()
                    {
                        DataContext = v,
                        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch
                    };
                }));

                PropertyEditorConverter.EditorTemplates.Add(typeof(EnumComboBox<ViewLayout>), new FuncDataTemplate<object>((v, s) =>
                {
                    return new EnumComboBox<ViewLayout>()
                    {
                        DataContext = v
                    };
                }));

                PropertyEditorConverter.EditorTemplates.Add(typeof(BindingEditor), new FuncDataTemplate<object>((v, s) =>
                {
                    return new BindingEditor()
                    {
                        DataContext = v
                    };
                }));

                PropertyEditorConverter.EditorTemplates.Add(typeof(ColorPickerEditor), new FuncDataTemplate<object>((v, s) =>
                {
                    return new ColorPickerEditor()
                    {
                        DataContext = v
                    };
                }));

                PropertyEditorConverter.EditorTemplates.Add(typeof(XYEditor), new FuncDataTemplate<object>((v, s) =>
                {
                    return new XYEditor()
                    {
                        DataContext = v
                    };
                }));
            }
        }

        public MainWindowViewModel? Model => DataContext as MainWindowViewModel;

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
                var result = await Model?.SaveView();

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

        private async void UploadFirmware_Cliked(object sender, RoutedEventArgs eventArgs)
        {
            var model = Model ?? throw new InvalidOperationException("Model isn't set!");
            var result = await model.FlashTWatch();

            if(result.IsSuccess)
            {
                await MessageBox.Show("TWatch SK firmware upload has been succesful!");
            }
            else
            {
                await MessageBox.Show($"{result.Code}: {result.ErrorMessage}");
            }
        }

        private void NewView_Clicked(object sender, RoutedEventArgs eventArgs)
        {
            Model?.CreateNewView();
        }

        private void NewLabel_Clicked(object sender, RoutedEventArgs eventArgs)
        {
            Model?.AddNewLabel();
        }
    }
}
