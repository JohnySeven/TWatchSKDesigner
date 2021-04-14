using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Threading.Tasks;
using TWatchSKDesigner.Views;

namespace TWatchSKDesigner.Modals
{
    public class ConfirmBox : Window
    {
        public ConfirmBox()
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
            Close(true);
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            Close(false);
        }        

        public static Task<bool?> Show(string message)
        {
            var confirmBox = new ConfirmBox()
            {
                Title = MainWindow.Instance?.Title,
                Tag = message
            };

            return confirmBox.ShowDialog<bool?>(MainWindow.Instance);
        }
    }
}
