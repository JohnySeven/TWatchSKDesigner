using Avalonia.Collections;
using Newtonsoft.Json;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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

        public MainWindowViewModel()
        {
            Views.Add(new WatchView()
            {
                Name = "Sample 1"
            });
            Views.Add(new WatchView()
            {
                Name = "Sample 2"
            });
            Views.Add(new WatchView()
            {
                Name = "Sample 3"
            });

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
        }

        public async Task<bool> LoadView(string path)
        {
            var ret = false;
            try
            {
                var json = await File.ReadAllTextAsync(path);

                UI = await Task.Run(() => JsonConvert.DeserializeObject<WatchDynamicUI>(json));
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

            }

            return ret;
        }
    }
}
