using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using TWatchSKDesigner.Models;
using Avalonia.Controls;
using TWatchSKDesigner.Controls;
using TWatchSKDesigner.Enums;
using System.Collections.ObjectModel;
using TWatchSKDesigner.Helpers;

namespace TWatchSKDesigner.ViewModels
{
    public class WatchView : ComponentDef
    {
        private string? _Name;

        [JsonProperty("name")]
        [ComponentProperty(typeof(TextBox))]
        public string? Name
        {
            get { return _Name; }
            set
            {
                _Name = value; 
                this.RaisePropertyChanged(nameof(Name));
                PreviewText = value;
            }
        }

        private string? _Layout;

        [JsonProperty("layout")]
        [ComponentProperty(typeof(EnumComboBox<ViewLayout>))]
        public string? Layout
        {
            get { return _Layout; }
            set { _Layout = value; this.RaisePropertyChanged(nameof(Layout)); }
        }

        public void OnLayoutChanged()
        {
            this.RaisePropertyChanged(nameof(Layout));
        }

        [JsonProperty("components")]
        public JArray? Components { get; set; }

        private bool _loadedComponents = false;
        [JsonIgnore]
        public ObservableCollection<ComponentDef> LoadedComponents { get; private set; } = new ObservableCollection<ComponentDef>();

        [ComponentProperty(typeof(EnumComboBox<ViewType>))]
        [JsonProperty("type")]
        public string? ViewType { get; set; }

        [JsonIgnore]
        public new string Type { get; private set; } = "View";

        private string? _Background;
        [JsonProperty("background")]
        [ComponentProperty(typeof(ColorPickerEditor))]
        public string? Background
        {
            get { return _Background; }
            set { _Background = value; this.RaisePropertyChanged(nameof(Background)); }
        }


        public void LoadAllComponents()
        {
            IsRemovable = false;
            if (Components != null && !_loadedComponents)
            {
                foreach (var component in Components)
                {
                    switch (component["type"]?.ToString())
                    {
                        case "label":
                            LoadedComponents.Add(LabelHelpers.Load(component));
                            break;
                    }
                }
            }
        }

        internal void SynchronizeJson()
        {
            var json = JsonConvert.SerializeObject(LoadedComponents);
            Components = JArray.Parse(json);
        }

        public WatchView()
        {
            Icon = MaterialDesign.Avalonia.PackIcon.PackIconKind.Watch;
        }
    }
}
