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
        private string _Name;

        [JsonProperty("name")]
        [ComponentProperty(typeof(TextBox))]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; this.RaisePropertyChanged(nameof(Name)); }
        }

        [JsonProperty("layout")]
        public string? Layout { get; set; }
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

        [JsonIgnore]
        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set { this.RaiseAndSetIfChanged(ref _isSelected, value); }
        }

        public void LoadAllComponents()
        {
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
    }
}
