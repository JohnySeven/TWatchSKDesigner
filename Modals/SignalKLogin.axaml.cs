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
            InitializeComponent();

            DataContext = new SKSignInViewModel();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        public SKSignInViewModel? Model => (SKSignInViewModel?)DataContext;

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async void LoginClicked(object sender, RoutedEventArgs e)
        {
            if (await Model.PerformLogin())
            {
                Close(true);
            }
        }

        private void CancelClicked(object sender, RoutedEventArgs e)
        {
            Close(false);
        }


        public static async Task<OperationResult<SKSignInViewModel>> ShowLogin(SignalKManager signalKManager)
        {
            var dialog = new SignalKLogin();
            dialog.Model.SKManager = signalKManager;

            if(await dialog.ShowDialog<bool>(MainWindow.Instance))
            {
                return new OperationResult<SKSignInViewModel>(dialog.Model);
            }
            else
            {
                return new OperationResult<SKSignInViewModel>("Canceled by user");
            }
        }
    }
}
