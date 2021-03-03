using Avalonia.Controls;
using Newtonsoft.Json.Linq;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TWatchSKDesigner.Converters;
using TWatchSKDesigner.Models;

namespace TWatchSKDesigner.ViewModels
{
    public class ComponentPropertiesViewModel : ReactiveObject
    {
        public ComponentPropertiesViewModel()
        {
            
        }

        private WatchView? _view;

        public WatchView? View
        {
            get { return _view; }
            set { this.RaiseAndSetIfChanged(ref _view, value); }
        }

        public ObservableCollection<ComponentDef> Components { get; } = new ObservableCollection<ComponentDef>();

        public ObservableCollection<ComponentProperty> ComponentProperties { get; private set; } = new ObservableCollection<ComponentProperty>();

        internal void LoadViewObjects(WatchView? view)
        {
            View = view;
            if(view?.Components != null)
            {
                var list = view.LoadedComponents;

                Components.Clear();

                Components.Add(view);

                foreach(var item in list)
                {
                    Components.Add(item);
                }

                SelectedComponentIndex = 0;
            }
        }

        private int _selectedComponentIndex;

        public int SelectedComponentIndex
        {
            get { return _selectedComponentIndex; }
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedComponentIndex, value);
                SelectComponent(_selectedComponentIndex != -1 ? Components[_selectedComponentIndex] : null);
            }
        }

        private void SelectComponent(ComponentDef? component)
        {
            SelectedComponent = component;

            if (component != null)
            {

                var propertyList = ComponentProperty.GetProperties(component);

                ComponentProperties.Clear();

                foreach (var property in propertyList)
                {
                    ComponentProperties.Add(property);
                }
            }
            else
            {
                ComponentProperties.Clear();
            }
        }

        private ComponentDef? _selectedComponent;

        public ComponentDef? SelectedComponent
        {
            get { return _selectedComponent; }
            set { this.RaiseAndSetIfChanged(ref _selectedComponent, value); }
        }


    }
}
