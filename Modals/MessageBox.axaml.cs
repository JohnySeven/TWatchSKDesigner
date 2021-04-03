using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Threading.Tasks;
using TWatchSKDesigner.Views;

namespace TWatchSKDesigner.Modals
{
    public class MessageBox : Window
    {
        public MessageBox()
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

        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public static Task Show(string message)
        {
            var msgBox = new MessageBox()
            {
                Title = MainWindow.Instance?.Title,
                Tag = message
            };

            return msgBox.ShowDialog(MainWindow.Instance);
        }
    }
}
