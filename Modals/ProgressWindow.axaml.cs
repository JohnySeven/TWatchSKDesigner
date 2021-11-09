using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
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

        public void Update(string text)
        {
            System.Diagnostics.Debug.WriteLine("PROGRESS: " + text);

            if(!Dispatcher.UIThread.CheckAccess())
            {
                Dispatcher.UIThread.Post(() =>
                {
                    Tag = text;
                });
            }
            else
            {
                Tag = text;
            }
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

        public static async Task ShowProgress(Func<ProgressWindow, Task> task)
        {
            var progress = new ProgressWindow()
            {
                Tag = "..."
            };

            progress.Show(MainWindow.Instance);

            await task(progress);

            progress.Close();
        }
    }
}
