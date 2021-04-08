using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Threading.Tasks;
using TWatchSKDesigner.ViewModels;
using TWatchSKDesigner.Views;

namespace TWatchSKDesigner.Modals
{
    public class SignalKLogin : Window
    {
        public SignalKLogin()
        {
            //To support designer
        }

        public SignalKLogin(SignalKManager signalKManager)
        {
            InitializeComponent();

            DataContext = new SKSignInViewModel(signalKManager);
#if DEBUG
            this.AttachDevTools();
#endif
        }

        public SKSignInViewModel Model => (SKSignInViewModel)DataContext;

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async void LoginClicked(object sender, RoutedEventArgs e)
        {
            await ProgressWindow.ShowProgress("Authenticating with SignalK...", async () =>
            {
                if (await Model.PerformLogin())
                {
                    Close(true);
                }
            });
        }

        private void CancelClicked(object sender, RoutedEventArgs e)
        {
            Close(false);
        }


        public static async Task<OperationResult<SKSignInViewModel>> ShowLogin(SignalKManager signalKManager)
        {
            var dialog = new SignalKLogin(signalKManager);

            if(await dialog.ShowDialog<bool>(MainWindow.Instance))
            {
                return new OperationResult<SKSignInViewModel>(dialog.Model);
            }
            else
            {
                return new OperationResult<SKSignInViewModel>("Canceled by user", -1);
            }
        }
    }
}
