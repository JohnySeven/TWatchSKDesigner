using Avalonia.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TWatchSKDesigner.Modals;
using TWatchSKDesigner.Views;

namespace TWatchSKDesigner.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public AvaloniaList<WatchView> Views { get; private set; } = new AvaloniaList<WatchView>();
        private WatchDynamicUI? _UI;
        public WatchDynamicUI? UI
        {
            get { return _UI; }
            set { _UI = value; this.RaiseAndSetIfChanged(ref _UI, value); }
        }

        private ComponentPropertiesViewModel _propertiesViewModel = new ComponentPropertiesViewModel();

        public ComponentPropertiesViewModel PropertiesViewModel
        {
            get { return _propertiesViewModel; }
            set { _propertiesViewModel = value; }
        }


        private bool _showProperties;

        public bool ShowProperties
        {
            get { return _showProperties; }
            set { this.RaiseAndSetIfChanged(ref _showProperties, value); }
        }

        public ICommand? StartEditingView { get; private set; }
        public ICommand? ExitCommand { get; set; }

        public SignalKManager SignalKManager { get; private set; }

        private bool _ShowOpenHelpText = true;

        public bool ShowOpenHelpText
        {
            get { return _ShowOpenHelpText; }
            set { _ShowOpenHelpText = value; OnPropertyChanged(nameof(ShowOpenHelpText)); }
        }


        public MainWindowViewModel()
        {
            StartEditingView = ReactiveCommand.Create<WatchView>(v =>
            {
                ShowProperties = true;
                _propertiesViewModel.LoadViewObjects(v);
                v.IsSelected = true;

                foreach(var view in Views)
                {
                    if(view != v)
                    {
                        view.IsSelected = false;
                    }
                }
            });

            SignalKManager = new SignalKManager();
        }

        public async void OpenUIFromSK()
        {
            try
            {
                bool canLoadView = false;

                if (!SignalKManager.TokenIsPresent)
                {
                    var result = await SignalKLogin.ShowLogin(SignalKManager);
                    canLoadView = result.IsSuccess;
                }
                else
                {
                    canLoadView = true;
                }

                if (canLoadView)
                {
                    var loadSKPathsResult = await SignalKManager.LoadSKPaths();
                    if (loadSKPathsResult.IsSuccess)
                    {
                        var loadResult = await SignalKManager.DownloadView();

                        if (loadResult.IsSuccess)
                        {
                            ShowOpenHelpText = false;
                            await LoadView(loadResult.Data);
                        }
                        else
                        {
                            await MessageBox.Show(loadResult.ErrorMessage);
                        }
                    }
                    else
                    {
                        await MessageBox.Show(loadSKPathsResult.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                await MessageBox.Show(ex.Message);
            }
        }

        public async Task<bool> LoadView(JObject viewJson)
        {
            var ret = false;
            try
            {
                UI = await Task.Run(() => JsonConvert.DeserializeObject<WatchDynamicUI>(viewJson.ToString()));
                Views.Clear();
                UI.Views?.ForEach(v =>
                {
                    v.LoadAllComponents();
                    Views.Add(v);
                });

                ret = true;
            }
            catch (Exception ex)
            {
               await MessageBox.Show(ex.Message);
            }

            return ret;
        }
    }
}
