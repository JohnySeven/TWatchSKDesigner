using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Newtonsoft.Json;
using System;
using System.Linq;
using TWatchSKDesigner.Converters;
using TWatchSKDesigner.Helpers;
using TWatchSKDesigner.Models;
using TWatchSKDesigner.ViewModels;

namespace TWatchSKDesigner.Views
{
    public class ViewPreview : UserControl
    {
        private WatchView _attached;

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
            }
        }

        private void View_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(WatchView.Layout))
            {
                LoadLayout(_attached.Layout);
                LoadComponents(_attached);
            }
        }

        private void LoadViewPreview(WatchView view)
        {
            _attached = view;
            view.PropertyChanged += View_PropertyChanged;
            Root.Bind(Border.BackgroundProperty, new Avalonia.Data.Binding("Background")
            {
                Path = "Background",
                Source = view,
                Converter = new BrushFromTextConverter()
            });

            LoadLayout(view.Layout);
            LoadComponents(view);
            
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

                if (renderedComponent != null)
                {
                    SetLayout(component, renderedComponent);
                    ((Panel)Root.Child).Children.Add(renderedComponent);
                }
            }
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
                }
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

            if(componentDef.Size != null && componentDef.Size.Length == 2)
            {
                control.SetValue(WidthProperty, (double)componentDef.Size[0]);
                control.SetValue(HeightProperty, (double)componentDef.Size[1]);
            }
        }

        private void LoadLayout(string? layout)
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
