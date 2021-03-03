using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using System;
using TWatchSKDesigner.Controls;
using TWatchSKDesigner.Converters;
using TWatchSKDesigner.Enums;
using TWatchSKDesigner.ViewModels;

namespace TWatchSKDesigner.Views
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

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
            }
        }

        public MainWindowViewModel? Model => DataContext as MainWindowViewModel;

        private void InitializeComponent()
        {
            DataContext = new MainWindowViewModel();
            AvaloniaXamlLoader.Load(this);
            LoadData();
        }

        private async void LoadData()
        {
            await Model?.LoadView("sk_view.json");
        }

    }
}
