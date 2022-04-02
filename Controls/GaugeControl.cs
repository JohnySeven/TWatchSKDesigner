using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using Avalonia.Threading;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TWatchSKDesigner.Helpers;

namespace TWatchSKDesigner.Controls
{
    public class Gauge : Control
    {

        private class ArcGaugeCustomOperation : ICustomDrawOperation
        {
            public Rect Bounds { get; }

            private Color _foreground;
            private Color _background;
            private Color _textColor;
            private float _value;
            private string _text;

            public ArcGaugeCustomOperation(Rect rect, Color foreground, Color textColor, Color background, float value, string text)
            {
                Bounds = rect;
                _foreground = foreground;
                _background = background;
                _textColor = textColor;
                _value = value;
                _text = text;
            }


            public void Dispose()
            {

            }

            public bool Equals(ICustomDrawOperation other)
            {
                return Equals(this, other);
            }

            public bool HitTest(Point p)
            {
                return false;
            }

            public void Render(IDrawingContextImpl context)
            {
                var canvas = (context as ISkiaDrawingContextImpl)?.SkCanvas;

                if(canvas != null)
                {
                    try
                    {
                        var width = (int)Bounds.Width;
                        var height = (int)Bounds.Height;

                        //  Empty Gauge Styling
                        SKPaint paint1 = new()
                        {
                            Style = SKPaintStyle.Stroke,
                            Color = new SKColor(_background.R, _background.G, _background.B),// Colour of Radial Gauge
                            StrokeWidth = 10.0f,// getFactoredWidth(radialGaugeWidth), // Width of Radial Gauge
                            StrokeCap = SKStrokeCap.Round                                   // Round Corners for Radial Gauge
                        };

                        // Filled Gauge Styling
                        SKPaint paint2 = new()
                        {
                            Style = SKPaintStyle.Stroke,
                            Color = new SKColor(_foreground.R, _foreground.G, _foreground.B),// Overlay Colour of Radial Gauge
                            StrokeWidth = 10.0f, // Overlay Width of Radial Gauge
                            StrokeCap = SKStrokeCap.Round                                   // Round Corners for Radial Gauge
                        };

                        // Defining boundaries for Gauge
                        SKRect rect = new(10.0f, 10.0f,  Math.Max((float)Bounds.Width - 10.0f, 0),  Math.Max((float)Bounds.Height - 10.0f, 0));

                        // Rendering Empty Gauge
                        SKPath path1 = new();
                        path1.AddArc(rect, -240.0f, 300.0f);
                        canvas.DrawPath(path1, paint1);

                        // Rendering Filled Gauge
                        SKPath path2 = new();
                        path2.AddArc(rect, -240.0f, 60.0f);
                        canvas.DrawPath(path2, paint2);
                        var font = new SKFont()
                        {
                            Size = 16.0f
                        };
                        var textPaint = new SKPaint(font)
                        {
                            Color = new SKColor(_textColor.R, _textColor.G, _textColor.B)
                        };

                        var textSize = SKRect.Empty;
                        var textWidth = textPaint.MeasureText(_text.AsSpan(), ref textSize);

                        canvas.DrawText(_text, (width / 2.0f) - (textSize.Width / 2.0f), (height / 2.0f), textPaint);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.StackTrace);
                    }
                }
            }
        }

        static Gauge()
        {
            AffectsRender<Gauge>(ValueProperty);
            AffectsRender<Gauge>(MinimumProperty);
            AffectsRender<Gauge>(MaximumProperty);
            AffectsRender<Gauge>(ColorProperty);
            AffectsRender<Gauge>(ValueFormatProperty);
            AffectsRender<Gauge>(BackgroundProperty);
        }

        public Gauge()
        {

        }

        public static readonly StyledProperty<double> ValueProperty =
            AvaloniaProperty.Register<Gauge, double>(nameof(Value));

        public static readonly StyledProperty<double> MinimumProperty =
            AvaloniaProperty.Register<Gauge, double>(nameof(Minimum));

        public static readonly StyledProperty<double> MaximumProperty =
    AvaloniaProperty.Register<Gauge, double>(nameof(Maximum));

        public static readonly StyledProperty<Color> ColorProperty =
            AvaloniaProperty.Register<Gauge, Color>(nameof(Color), Colors.Red);

        public static readonly StyledProperty<Color> TextColorProperty =
            AvaloniaProperty.Register<Gauge, Color>(nameof(TextColor), Colors.Red);


        public static readonly StyledProperty<IBrush> BackgroundProperty =
            AvaloniaProperty.Register<Gauge, IBrush>(nameof(Color));

        public static readonly StyledProperty<string> ValueFormatProperty =
            AvaloniaProperty.Register<Gauge, string>(nameof(ValueFormat), "$$");

        public IBrush Background
        {
            get => GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }

        public string ValueFormat
        {
            get => GetValue(ValueFormatProperty);
            set => SetValue(ValueProperty, value);
        }

        public double Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public double Maximum
        {
            get => GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        public double Minimum
        {
            get => GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        public Color Color
        {
            get => GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public Color TextColor
        {
            get => GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public override void Render(DrawingContext drawingContext)
        {
            drawingContext.FillRectangle(Background, new Rect(0.0, 0.0, Width, Height));
            drawingContext.Custom(new ArcGaugeCustomOperation(new Rect(0, 0, Width, Height), Color, TextColor, Colors.Gray, (float)Value, ValueFormat?.Replace("$$", Value.ToString())));
        }
    }
}
