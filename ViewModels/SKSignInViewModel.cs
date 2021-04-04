using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TWatchSKDesigner.ViewModels
{
    public class SKSignInViewModel : ViewModelBase
    {
        public SKSignInViewModel()
        {
#if DEBUG
            _Address = "pi.boat:3000";
            _User = "signalk";
            _Password = "signalk";
            _IsValid = true;
#endif
        }

        private string? _Address;

        public string? Address
        {
            get { return _Address; }
            set { _Address = value; OnPropertyChanged(nameof(Address)); UpdateValid(); }
        }

        private string? _User;

        public string? User
        {
            get { return _User; }
            set { _User = value; OnPropertyChanged(nameof(User)); UpdateValid(); }
        }

        private string? _Password;

        public string? Password
        {
            get { return _Password; }
            set { _Password = value; OnPropertyChanged(nameof(Password)); UpdateValid(); }
        }

        private bool _IsWorking;

        public bool IsWorking
        {
            get { return _IsWorking; }
            set { _IsWorking = value; OnPropertyChanged(nameof(IsWorking)); }
        }

        public Action<string> OnMessage { get; set; }

        private bool _IsValid;

        public bool IsValid
        {
            get { return _IsValid; }
            set { _IsValid = value; OnPropertyChanged(nameof(IsValid)); }
        }

        public SignalKManager SKManager { get; internal set; }

        private void UpdateValid()
        {
            IsValid = !string.IsNullOrEmpty(Address) && !string.IsNullOrEmpty(User) && !string.IsNullOrEmpty(Password);
        }

        private string _ErrorMessage;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; OnPropertyChanged(nameof(ErrorMessage)); }
        }



        public async Task<bool> PerformLogin()
        {
            var result = await SKManager.Authorize(Address, User, Password);

            if(result.IsSuccess)
            {
                return true;
            }
            else
            {
                ErrorMessage = result.ErrorMessage;

                return false;
            }
        }
    }
}
