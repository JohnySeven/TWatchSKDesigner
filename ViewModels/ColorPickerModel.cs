using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TWatchSKDesigner.Converters;

namespace TWatchSKDesigner.ViewModels
{
    public class ColorPickerModel : ViewModelBase
    {
        private int _R;

        public int R
        {
            get { return _R; }
            set { _R = value; OnPropertyChanged(nameof(R)); UpdateColor(); }
        }

        private int _G;

        public int G
        {
            get { return _G; }
            set { _G = value; OnPropertyChanged(nameof(G)); UpdateColor(); }
        }

        private int _B;

        public int B
        {
            get { return _B; }
            set { _B = value; OnPropertyChanged(nameof(B)); UpdateColor(); }
        }

        private void UpdateColor()
        {
            Color = Color.FromRgb((byte)_R, (byte)_G, (byte)_B);
        }

        public void FromText(string? text)
        {
            var conv = new BrushFromTextConverter();

            var brush = conv.Convert(text, typeof(Brush), null, CultureInfo.CurrentCulture) as SolidColorBrush;
            if (brush != null)
            {
                R = brush.Color.R;
                G = brush.Color.G;
                B = brush.Color.B;
                Color = brush.Color;
            }
        }

        public Color SelectedColor
        {
            get { return _Color; }
            set { UpdateColor(value); }
        }


        public Color[] SampleColors { get; set; } = typeof(Colors).GetProperties().Select(p => (Color)p.GetValue(null)).ToArray();

        internal void UpdateColor(Color color)
        {
            R = color.R;
            G = color.G;
            B = color.B;
        }

        private Color _Color;

        public Color Color
        {
            get { return _Color; }
            set { _Color = value; OnPropertyChanged(nameof(Color)); }
        }

        private string GetHex(int value)
        {
            var ret = value.ToString("x");
            if(ret.Length == 1)
            {
                return "0" + ret;
            }
            else
            {
                return ret;
            }
        }

        public string ToHex()
        {
            return "#" + GetHex(_R) + GetHex(_G) + GetHex(_B);
        }
    }
}
