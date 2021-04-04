using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Threading.Tasks;
using TWatchSKDesigner.Views;

namespace TWatchSKDesigner.Modals
{
    public class ProgressWindow : Window
    {
        public ProgressWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public static async Task ShowProgress(string title, Func<Task> task)
        {
            var progress = new ProgressWindow()
            {
                Tag = title
            };

            progress.Show(MainWindow.Instance);

            await task();

            progress.Close();
        }
    }
}
