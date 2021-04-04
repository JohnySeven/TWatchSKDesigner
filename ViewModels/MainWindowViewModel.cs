using Avalonia.Collections;
using Avalonia.Controls;
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
using TWatchSKDesigner.Models;
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

        private WatchView? _SelectedView;

        public WatchView? SelectedView
        {
            get { return _SelectedView; }
            set { _SelectedView = value; OnPropertyChanged(nameof(SelectedView)); }
        }


        private bool _ShowOpenHelpText = true;

        public bool ShowOpenHelpText
        {
            get { return _ShowOpenHelpText; }
            set { _ShowOpenHelpText = value; OnPropertyChanged(nameof(ShowOpenHelpText)); }
        }

        private bool _ViewLoaded;

        public bool ViewLoaded
        {
            get { return _ViewLoaded; }
            set { _ViewLoaded = value; OnPropertyChanged(nameof(ViewLoaded)); }
        }



        public MainWindowViewModel()
        {
            StartEditingView = ReactiveCommand.Create<WatchView>(v =>
            {
                ShowProperties = true;
                _propertiesViewModel.LoadViewObjects(v);
                v.IsSelected = true;
                SelectedView = v;

                foreach (var view in Views)
                {
                    if (view != v)
                    {
                        view.IsSelected = false;
                    }
                }
            });

            SignalKManager = new SignalKManager();
            Json = "";
            JsonError = "";
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
                    OperationResult loadSKPathsResult = null;

                    await ProgressWindow.ShowProgress("Loading Signal K paths...", async () =>
                    {
                       loadSKPathsResult = await SignalKManager.LoadSKPaths();
                    });

                    if (loadSKPathsResult?.IsSuccess == true)
                    {
                        var loadResult = await SignalKManager.DownloadView();

                        if (loadResult.IsSuccess)
                        {
                            ShowOpenHelpText = false;
                            ViewLoaded = await LoadView(loadResult.Data);
                        }
                        else
                        {
                            await MessageBox.Show(loadResult.ErrorMessage);
                        }
                    }
                    else
                    {
                        await MessageBox.Show(loadSKPathsResult?.ErrorMessage ?? "Error!");
                    }
                }
            }
            catch (Exception ex)
            {
                await MessageBox.Show(ex.Message);
            }
        }

        internal Task<OperationResult> SaveView()
        {
            if (SelectedTabIndex == 0)
            {
                UpdateJson();
            }

            var viewJson = JObject.Parse(Json);

            return SignalKManager.StoreView(viewJson);
        }

        internal void AddNewLabel()
        {
            SelectedView.LoadedComponents.Add(new LabelDef() { Type = "label", Font = "roboto40" });
        }

        internal void CreateNewView()
        {
            var newView = new WatchView()
            {
                Name = "New view",
                Components = new JArray(),
                Layout = "column_mid",
                ViewType = "normal",
                Background = "white"
            };

            UI.Views.Add(newView);

            newView.LoadAllComponents();

            Views.Add(newView);
        }

        private string _Json;

        public string Json
        {
            get { return _Json; }
            set
            {
                _Json = value; OnPropertyChanged(nameof(Json));
                TryParseJson();
            }
        }

        private async void TryParseJson()
        {
            var error = "";

            await Task.Run(() =>
            {
                try
                {
                    JObject.Parse(Json);
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }
            });

            JsonError = error;
        }

        private string _JsonError;

        public string JsonError
        {
            get { return _JsonError; }
            set { _JsonError = value; OnPropertyChanged(nameof(JsonError)); }
        }


        private int _SelectedTabIndex;

        public int SelectedTabIndex
        {
            get { return _SelectedTabIndex; }
            set
            {
                _SelectedTabIndex = value;
                OnPropertyChanged(nameof(SelectedTabIndex));
                if(value == 0) //switching to preview
                {
                    SwitchedToPreview();
                }
                else if(value == 1) //switching to JSON
                {
                    UpdateJson();
                }
            }
        }
        public void UpdateJson()
        {
            //Update JSON from edits of designer
            if (UI?.Views != null)
            {
                foreach(var view in UI.Views)
                {
                    view.SynchronizeJson();
                }

                Json = JsonConvert.SerializeObject(UI, Formatting.Indented, new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            }
            else
            {
                Json = "{ }";
            }
        }

        private async void SwitchedToPreview()
        {
            //Update Preview from switch from Json
            try
            {
                var obj = JObject.Parse(Json);
                await LoadView(obj);
            }
            catch (Exception ex)
            {
                await MessageBox.Show("Json error: " + ex.Message);
            }
        }

        public async Task<bool> LoadView(JObject? viewJson)
        {
            var ret = false;
            try
            {
                if (viewJson != null)
                {
                    Json = viewJson.ToString();
                    UI = await Task.Run(() => JsonConvert.DeserializeObject<WatchDynamicUI>(viewJson.ToString()));
                    Views.Clear();
                    if (UI.Views == null)
                    {
                        UI.Views = new List<WatchView>();
                    }

                    UI.Views?.ForEach(v =>
                    {
                        v.LoadAllComponents();
                        Views.Add(v);
                    });

                    ret = true;
                }
            }
            catch (Exception ex)
            {
               await MessageBox.Show(ex.Message);
            }

            return ret;
        }
    }
}
