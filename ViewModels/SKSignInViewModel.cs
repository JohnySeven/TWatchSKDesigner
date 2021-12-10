using Newtonsoft.Json.Linq;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TWatchSKDesigner.ViewModels
{
    public class SKSignInViewModel : ViewModelBase
    {
        public SKSignInViewModel(SignalKManager signalKManager)
        {
            SKManager = signalKManager;
            _Port = 3000;

            LoadSettings();
        }

        private string _Address;

        public string Address
        {
            get { return _Address; }
            set { _Address = value; OnPropertyChanged(nameof(Address)); UpdateValid(); }
        }

        private string _User;

        public string User
        {
            get { return _User; }
            set { _User = value; OnPropertyChanged(nameof(User)); UpdateValid(); }
        }

        private string _Password;

        public string Password
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

        private int _Port;

        public int Port
        {
            get { return _Port; }
            set { _Port = value; OnPropertyChanged(nameof(Port)); UpdateValid(); }
        }

        private bool _IsValid;

        public bool IsValid
        {
            get { return _IsValid; }
            set { _IsValid = value; OnPropertyChanged(nameof(IsValid)); }
        }

        public SignalKManager SKManager { get; internal set; }

        private void UpdateValid()
        {
            IsValid = !string.IsNullOrEmpty(Address) && !string.IsNullOrEmpty(User) && !string.IsNullOrEmpty(Password) && Port > 0 && Port < ushort.MaxValue;
        }

        private string _ErrorMessage = "";

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; OnPropertyChanged(nameof(ErrorMessage)); }
        }



        public async Task<bool> PerformLogin()
        {
            if (Address != null && User != null && Password != null)
            {
                var result = await SKManager.Authorize(Address, Port, User, Password);

                if (result.IsSuccess)
                {
                    await SaveSettings();
                    return true;
                }
                else
                {
                    ErrorMessage = result.ErrorMessage;

                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private static string LoginSettingsPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "TWatchSK.Designer.json");

        private async Task SaveSettings()
        {
            var json = new JObject
            {
                [nameof(User)] = User,
                [nameof(Address)] = Address,
                [nameof(Port)] = Port
            };

            try
            {
                await File.WriteAllTextAsync(LoginSettingsPath, json.ToString());
            }
            catch (Exception)
            {

            }
        }

        private async void LoadSettings()
        {
            if(File.Exists(LoginSettingsPath))
            {
                try
                {

                    var json = await File.ReadAllTextAsync(LoginSettingsPath);

                    var loginInfo = JObject.Parse(json);

                    User = loginInfo[nameof(User)]?.ToString();
                    if(int.TryParse(loginInfo[nameof(Port)]?.ToString(), out int parsedPort))
                    {
                        Port = parsedPort;
                    }

                    Address = loginInfo[nameof(Address)]?.ToString();
                }
                catch (Exception)
                {

                }
            }
        }
    }
}
