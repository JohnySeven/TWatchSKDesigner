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
using System.Linq;
using TWatchSKDesigner.Intefaces;
using Splat;

namespace TWatchSKDesigner.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public AvaloniaList<WatchView> Views { get; private set; } = new AvaloniaList<WatchView>();

        private WatchDynamicUI _UI;
        public WatchDynamicUI UI
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

        private bool _ShowMovingPlacers;

        public bool ShowMovingPlacers
        {
            get { return _ShowMovingPlacers; }
            set { this.RaiseAndSetIfChanged(ref _ShowMovingPlacers, value); }
        }

        private bool _ShowUpdateBanner;

        public bool ShowUpdateBanner
        {
            get { return _ShowUpdateBanner; }
            set { _ShowUpdateBanner = value; OnPropertyChanged(nameof(ShowUpdateBanner)); }
        }

        private string _UpdateBannerText;

        public string UpdateBannerText
        {
            get { return _UpdateBannerText; }
            set { _UpdateBannerText = value; OnPropertyChanged(nameof(UpdateBannerText)); }
        }

        private UpdateInfo _updateInfo;

        private WatchView _movingView;

        public ICommand StartEditingView { get; private set; }
        public ICommand ExitCommand { get; set; }

        public ICommand CloseComponentsPanel { get; set; }

        public ICommand MoveCommand { get; set; }

        public ICommand MoveViewAtPlaceCommand { get; set; }

        public ICommand OpenUpdateDownloadCommand { get; private set; }

        public ICommand DismissUpdateCommand { get; private set; }

        public SignalKManager SignalKManager { get; private set; }

        private WatchView _SelectedView;

        public WatchView SelectedView
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


        private bool _NoViews;

        internal void AddNewSwitch()
        {
            if (SelectedView != null)
            {
                SelectedView.LoadedComponents.Add(new SwitchDef() { Type = "switch", Size = new[] { 60, 40 } });
            }
        }

        internal void AddNewButton()
        {
            if (SelectedView != null)
            {
                SelectedView.LoadedComponents.Add(new ButtonDef() { Type = "button", Size = new[] { 100, 40 } });
            }
        }

        public bool NoViews
        {
            get { return _NoViews; }
            set { _NoViews = value; OnPropertyChanged(nameof(NoViews)); }
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

            MoveCommand = ReactiveCommand.Create<WatchView>(v =>
            {
                ShowMovingPlacers = true;
                _movingView = v;
                StartEditingView.Execute(v);
            });

            MoveViewAtPlaceCommand = ReactiveCommand.Create<WatchView>(v =>
            {
                if(_movingView != null && _movingView != v)
                {
                    var placeIndex = Views.IndexOf(v);
                    Views.Remove(_movingView);
                    
                    Views.Insert(placeIndex, _movingView);
                    //Update view order in JSON
                    UI.Views = Views.ToList();
                    _movingView = null;
                }

                ShowMovingPlacers = false;
            });

            CloseComponentsPanel = ReactiveCommand.Create(() =>
            {
                ShowProperties = false;
            });

            OpenUpdateDownloadCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var platform = Locator.Current.GetService<IPlatformSupport>();

                try
                {
                    await platform.LaunchBrowser(_updateInfo.ShowUri);
                    ShowUpdateBanner = false;
                }
                catch (Exception ex)
                {
                    await MessageBox.Show(ex.Message);
                }
            });

            DismissUpdateCommand = ReactiveCommand.Create(() =>
            {
                ShowUpdateBanner = false;
            });

            SignalKManager = new SignalKManager();
            _Json = "";
            _JsonError = "";

            CheckNewVersion();
        }



        private async void CheckNewVersion()
        {
            try
            {
                var updateService = Locator.Current.GetService<IUpdateService>();

                if (updateService != null)
                {
                    var result = await updateService.CheckNewVersion();

                    if (result.IsSuccess)
                    {
                        _updateInfo = result.Data;
                        UpdateBannerText = $"New version {result.Data.Version} is available.";
                        ShowUpdateBanner = true;
                    }
                    else
                    {
                        ShowUpdateBanner = false;
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private async void DeleteView(WatchView view)
        {
            if (await ConfirmBox.Show($"Are you sure you want to delete {view.Name}?") == true)
            {
                if (Views.Remove(view))
                {
                    UI.Views?.Remove(view);
                }

                NoViews = Views.Count == 0;
                if(!NoViews)
                {
                    StartEditingView.Execute(Views.Last());
                }
            }
        }

        public async void OpenUIFromSK()
        {
            try
            {
                bool canLoadView = false;

                if (!SignalKManager.TokenIsPresent)
                {
                    ShowOpenHelpText = false;
                    var result = await SignalKLogin.ShowLogin(SignalKManager);
                    canLoadView = result.IsSuccess;
                    ShowOpenHelpText = !canLoadView;
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
                        else if (loadResult.StatusCode == 404)
                        {
                            ShowOpenHelpText = false;
                            ViewLoaded = await LoadView(JObject.Parse("{ }"));
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
            if (SelectedView != null)
            {
                SelectedView.LoadedComponents.Add(new LabelDef() { Type = "label", Font = "roboto40" });
            }
        }

        internal void AddNewGauge()
        {
            if (SelectedView != null)
            {
                SelectedView.LoadedComponents.Add(new GaugeDef() { Type = "gauge", Size = new[] { 200, 200 } });
            }
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

            NoViews = Views.Count == 0;

            StartEditingView.Execute(newView);
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

                var json = JObject.FromObject(UI, new JsonSerializer()
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

                RemoveNulls(json);

                Json = json.ToString(Formatting.Indented);
            }
            else
            {
                Json = "{ }";
            }
        }

        private void RemoveNulls(JObject json)
        {
            var properties = json.Properties().ToArray();

            foreach(var property in properties)
            {
                if(property.Value == null || property.Value.Type == JTokenType.Null)
                {
                    property.Remove();
                }
                else if(property.Value.Type == JTokenType.Object)
                {
                    RemoveNulls((JObject)property.Value);
                }
                else if(property.Value.Type == JTokenType.Array)
                {
                    foreach(var item in ((JArray)property.Value))
                    {
                        if(item?.Type == JTokenType.Object)
                        {
                            RemoveNulls((JObject)item);
                        }
                    }
                }
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

        public async Task<bool> LoadView(JObject viewJson)
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

                    NoViews = Views.Count == 0;
                    SelectedView = null;
                    ShowProperties = false;

                    ret = true;
                }
                else
                {
                    NoViews = true;
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
