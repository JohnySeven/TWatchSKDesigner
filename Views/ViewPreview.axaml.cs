using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using TWatchSKDesigner.Controls;
using TWatchSKDesigner.Converters;
using TWatchSKDesigner.Helpers;
using TWatchSKDesigner.Models;
using TWatchSKDesigner.ViewModels;

namespace TWatchSKDesigner.Views
{
    public class ViewPreview : UserControl
    {
        private WatchView _attached;

        private Dictionary<ComponentDef, Control> _controlToComponent = new Dictionary<ComponentDef, Control>();

        public Border Root { get; }

        public ViewPreview()
        {
            InitializeComponent();
            Root = this.Find<Border>("Root");
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override void OnDataContextChanged(EventArgs e)
        {
            if(DataContext is WatchView view)
            {
                Detach();
                LoadViewPreview(view);
            }
            else
            {
                Detach();
            }
            base.OnDataContextChanged(e);
        }

        private void Detach()
        {
            if(_attached != null)
            {
                _attached.PropertyChanged -= View_PropertyChanged;
                _attached.LoadedComponents.CollectionChanged -= LoadedComponents_CollectionChanged;

                foreach(var component in _attached.LoadedComponents)
                {
                    component.PropertyChanged -= Component_PropertyChanged;
                }

                _controlToComponent.Clear();
            }
        }

        private void Component_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(Root.Child is Canvas && sender is ComponentDef component)
            {
                SetLayout(component, _controlToComponent[component]);
            }
        }

        private void View_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(WatchView.Layout) && _attached != null)
            {
                LoadLayout(_attached.Layout);
                LoadComponents(_attached);
            }
        }

        private void LoadViewPreview(WatchView view)
        {
            _attached = view;
            _controlToComponent.Clear();
            view.PropertyChanged += View_PropertyChanged;

            Root.Bind(Border.BackgroundProperty, new Avalonia.Data.Binding("Background")
            {
                Path = "Background",
                Source = view,
                Converter = new BrushFromTextConverter()
            });

            LoadLayout(view.Layout);
            LoadComponents(view);

            foreach (var component in _attached.LoadedComponents)
            {
                component.PropertyChanged += Component_PropertyChanged;
            }

            view.LoadedComponents.CollectionChanged += LoadedComponents_CollectionChanged;
        }

        private void LoadedComponents_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            _attached?.SynchronizeJson();
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems.OfType<ComponentDef>())
                {
                    item.PropertyChanged += Component_PropertyChanged;
                }
            }

            if(e.OldItems != null)
            {
                foreach (var item in e.OldItems.OfType<ComponentDef>())
                {
                    item.PropertyChanged -= Component_PropertyChanged;
                    _controlToComponent.Remove(item);
                }
            }
        }

        private void LoadComponents(WatchView view)
        {
            foreach (var component in view.LoadedComponents)
            {
                Control renderedComponent = null;

                if (component is LabelDef label)
                {
                    renderedComponent = LoadLabel(label);
                }
                else if (component is GaugeDef gauge)
                {
                    renderedComponent = LoadGauge(gauge);
                }
                else if (component is SwitchDef swtch)
                {
                    renderedComponent = LoadSwitch(swtch);
                }

                if (renderedComponent != null)
                {
                    if (!_controlToComponent.ContainsKey(component))
                    {
                        _controlToComponent.Add(component, renderedComponent);
                    }

                    SetLayout(component, renderedComponent);
                    ((Panel)Root.Child).Children.Add(renderedComponent);
                }
            }
        }

        private Gauge LoadGauge(GaugeDef gaugeDef)
        {
            return new Gauge()
            {
                DataContext = gaugeDef,
                [!Gauge.WidthProperty] = new Avalonia.Data.Binding("Size", Avalonia.Data.BindingMode.OneWay)
                {
                    Converter = new DoubleFromIntArray() { Index = 0 }
                },
                [!Gauge.HeightProperty] = new Avalonia.Data.Binding("Size", Avalonia.Data.BindingMode.OneWay)
                {
                    Converter = new DoubleFromIntArray() { Index = 1 }
                },
                [!Gauge.ColorProperty] = new Avalonia.Data.Binding("Color", Avalonia.Data.BindingMode.OneWay)
                {
                    Converter = new ColorFromTextConverter()
                },
                [!Gauge.ValueFormatProperty] = new Avalonia.Data.Binding("Binding", Avalonia.Data.BindingMode.OneWay)
                {
                    Converter = new BindingToValueFormat() { Value = 0.0f }
                },
                [!Gauge.BackgroundProperty] = new Avalonia.Data.Binding("IsSelected", Avalonia.Data.BindingMode.OneWay)
                {
                    Converter = new SelectionBrushConverter()
                    {
                        SelectedColor = new SolidColorBrush(Colors.LightBlue),
                        UnselectedColor = Brushes.Transparent
                    }
                },
                [!Gauge.TextColorProperty] = new Avalonia.Data.Binding("Color", Avalonia.Data.BindingMode.OneWay)
                {
                    Converter = new ColorFromTextConverter()
                }
            };
        }

        private Label LoadLabel(LabelDef labelDef)
        {
            var ret = new Label()
            {
                DataContext = labelDef,
                [!Label.ContentProperty] = new Avalonia.Data.Binding("Text", Avalonia.Data.BindingMode.TwoWay),
                [!Label.ForegroundProperty] = new Avalonia.Data.Binding("Color", Avalonia.Data.BindingMode.OneWay)
                {
                    Converter = new BrushFromTextConverter()
                },
                [!Label.FontSizeProperty] = new Avalonia.Data.Binding("Font", Avalonia.Data.BindingMode.OneWay)
                {
                    Converter = new FontSizeConverter()
                },
                [!Label.BackgroundProperty] = new Avalonia.Data.Binding("IsSelected", Avalonia.Data.BindingMode.OneWay)
                {
                    Converter = new SelectionBrushConverter()
                    {
                        SelectedColor = new SolidColorBrush(Colors.LightBlue),
                        UnselectedColor = Brushes.Transparent
                    }
                }
            };

            return ret;
        }

        private SwitchControl LoadSwitch(SwitchDef switchDef)
        {
            var ret = new SwitchControl()
            {
                DataContext = switchDef,
                [!WidthProperty] = new Avalonia.Data.Binding("Size", Avalonia.Data.BindingMode.OneWay)
                {
                    Converter = new DoubleFromIntArray() { Index = 0 }
                },
                [!HeightProperty] = new Avalonia.Data.Binding("Size", Avalonia.Data.BindingMode.OneWay)
                {
                    Converter = new DoubleFromIntArray() { Index = 1 }
                },
            };

            return ret;
        }

        private void SetLayout(ComponentDef componentDef, Control control)
        {
            if(Root.Child is Canvas canvas)
            {
                if(componentDef.Location != null && componentDef.Location.Length == 2)
                {
                    control.Bind(Canvas.LeftProperty, new Avalonia.Data.Binding("Location")
                    {
                        Converter = new DoubleFromIntArray() { Index = 0 }
                    });
                    control.Bind(Canvas.TopProperty, new Avalonia.Data.Binding("Location")
                    {
                        Converter = new DoubleFromIntArray() { Index = 1 }
                    });
                }
            }
        }

        private void LoadLayout(string layout)
        {
            if (!string.IsNullOrEmpty(layout))
            {
                if (layout == "off")
                {
                    Root.Child = new Canvas()
                    {
                        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch
                    };
                }
                else if (layout.StartsWith("pretty") || layout == "grid")
                {
                    Root.Child = new WrapPanel()
                    {
                        Orientation = Avalonia.Layout.Orientation.Horizontal,
                        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch
                    };
                }
                else
                {
                    var stack = new StackPanel();

                    if (layout == "center")
                    {
                        stack.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center;
                        stack.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center;
                    }
                    else if (layout == "column_left")
                    {
                        stack.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
                        stack.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
                    }
                    else if (layout == "column_mid")
                    {
                        stack.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center;
                        stack.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
                    }
                    else if (layout == "column_right")
                    {
                        stack.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right;
                        stack.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
                    }
                    else if (layout == "row_top")
                    {
                        stack.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch;
                        stack.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
                        stack.Orientation = Avalonia.Layout.Orientation.Horizontal;
                    }
                    else if (layout == "row_mid")
                    {
                        stack.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch;
                        stack.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
                        stack.Orientation = Avalonia.Layout.Orientation.Horizontal;

                    }
                    else if (layout == "row_bottom")
                    {
                        stack.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch;
                        stack.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
                        stack.Orientation = Avalonia.Layout.Orientation.Horizontal;
                    }

                    Root.Child = stack;
                }
            }
            else
            {
                Root.Child = new Canvas()
                {
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch
                };
            }
        }
    }
}
