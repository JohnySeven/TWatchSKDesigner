﻿using Avalonia.Controls;
using Newtonsoft.Json.Linq;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using TWatchSKDesigner.Converters;
using TWatchSKDesigner.Modals;
using TWatchSKDesigner.Models;

namespace TWatchSKDesigner.ViewModels
{
    public class ComponentPropertiesViewModel : ReactiveObject
    {
        public ComponentPropertiesViewModel()
        {
            RemoveComponent = ReactiveCommand.Create<ComponentDef>(OnRemoveComponent);
        }

        private WatchView _view;

        public WatchView View
        {
            get { return _view; }
            set { this.RaiseAndSetIfChanged(ref _view, value); }
        }

        public ObservableCollection<ComponentDef> Components { get; } = new ObservableCollection<ComponentDef>();

        public ObservableCollection<ComponentProperty> ComponentProperties { get; private set; } = new ObservableCollection<ComponentProperty>();

        public IReactiveCommand RemoveComponent { get; set; }

        internal void LoadViewObjects(WatchView view)
        {
            if (View != null)
            {
                View.LoadedComponents.CollectionChanged -= LoadedComponents_CollectionChanged;
            }

            View = view;

            View.LoadedComponents.CollectionChanged += LoadedComponents_CollectionChanged;

            RefreshComponents(view);
        }

        private void RefreshComponents(WatchView view)
        {
            if (view?.Components != null)
            {
                var list = view.LoadedComponents;

                Components.Clear();

                Components.Add(view);

                foreach (var item in list)
                {
                    Components.Add(item);
                }

                SelectedComponent = Components.FirstOrDefault();
            }
        }

        private void LoadedComponents_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RefreshComponents(View);
            if(e.NewItems?.Count > 0)
            {
                var addedComponent = e.NewItems.OfType<ComponentDef>().First();
                SelectedComponent = addedComponent;
            }
        }

        private void SelectComponent(ComponentDef component)
        {
            if (component != null)
            {
                var propertyList = ComponentProperty.GetProperties(_view ?? throw new InvalidOperationException(), component, OnPropertyChanged);
                ClearProperties();

                component.IsSelected = true;

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

        private void OnPropertyChanged(ComponentProperty property)
        {
            System.Diagnostics.Debug.WriteLine($"View {View?.Name} object {SelectedComponent?.Type} property {property.Name} changed to {property.Value}");
            View?.SynchronizeJson();

            if(property.UpdateViewLayoutOnChange)
            {
                View?.OnLayoutChanged();
            }
        }

        private void ClearProperties()
        {
            foreach(var property in ComponentProperties)
            {
                property.OnChanged = null;
            }

            ComponentProperties.Clear();
        }

        private ComponentDef _selectedComponent;

        public ComponentDef SelectedComponent
        {
            get { return _selectedComponent; }
            set
            {
                if(_selectedComponent != null)
                {
                    _selectedComponent.IsSelected = false;
                }
                this.RaiseAndSetIfChanged(ref _selectedComponent, value);
                SelectComponent(value); 
            }
        }

        private async void OnRemoveComponent(ComponentDef component)
        {
            if(await ConfirmBox.Show($"This will remove component {component.PreviewText} from view, are you sure?"))
            {
                View.LoadedComponents.Remove(component);
                View.SynchronizeJson();
            }
        }
    }
}
