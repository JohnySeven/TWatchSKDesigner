using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using TWatchSKDesigner.Models;
using TWatchSKDesigner.Models.SK;
using TWatchSKDesigner.ViewModels;
using TWatchSKDesigner.Views;

namespace TWatchSKDesigner.Modals
{
    public class SelectUnitConversion : Window
    {
        public SelectUnitConversion() : this(null)
        {

        }


        public SelectUnitConversion(Func<Conversion, bool>? filter)
        {
            Conversions = LoadConversions(filter);
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private Conversion[] LoadConversions(Func<Conversion, bool>? filter)
        {
            var ret = new List<Conversion>();
            var conversions = JObject.Parse(Properties.Resources.Conversions);

            foreach (var unit in conversions.Properties())
            {
                if (conversions[unit.Name] is JObject unitObj) {

                    foreach (var toConversion in unitObj.Properties())
                    {
                        if (unitObj[toConversion.Name] is JObject mathInfo)
                        {
                            var conv = new Conversion()
                            {
                                From = unit.Name,
                                To = toConversion.Name,
                                Multiply = ((float?)mathInfo["multiply"]) ?? 1.0f,
                                OffSet = ((float?)mathInfo["offset"]) ?? 0.0f
                            };

                            if(filter == null || filter(conv))
                            {
                                ret.Add(conv);
                            }                            
                        }
                    }
                }
            }
            return ret.ToArray();
        }

        public Conversion[] Conversions { get; set; }

        public Conversion? SelectedConversion { get; set; }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            if (SelectedConversion != null)
            {
                Close(true);
            }
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            Close(false);
        }
    }
}
