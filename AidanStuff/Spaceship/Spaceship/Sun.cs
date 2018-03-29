using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.UI.Xaml;

namespace Spaceship
{
    class Sun : SpaceObject, SpaceResource
    {
        CanvasGeometry sGeometry;
        ICanvasImage sImage;

        public override CanvasGeometry Geometry => sGeometry;
        public override Matrix3x2 WorldTransform => Matrix3x2.Identity;

        public float Radius => 80;

        public void CreateResources(ICanvasResourceCreator resourceCreator)
        {
            sGeometry = CanvasGeometry.CreateCircle(resourceCreator, new Vector2(), Radius);

            var sunCommandList = new CanvasCommandList(resourceCreator);
            using (var drawingSession = sunCommandList.CreateDrawingSession())
            {

                var gradientStops = new CanvasGradientStop[]
                {
                    new CanvasGradientStop {Color = Color.FromArgb(255,255,192,121), Position = 0},
                    new CanvasGradientStop {Color = Color.FromArgb(255,255,173,87), Position = 0.3f},
                    new CanvasGradientStop {Color = Color.FromArgb(255,244,94,0), Position = 0.9f},
                    new CanvasGradientStop {Color = Color.FromArgb(255,187,13,4), Position = 1.0f}

                };
                using (var brush = new CanvasRadialGradientBrush(resourceCreator, gradientStops))
                {
                    brush.RadiusX = Radius;
                    brush.RadiusY = Radius;
                    drawingSession.FillGeometry(sGeometry, brush);
                }
                drawingSession.DrawGeometry(sGeometry, Color.FromArgb(255, 187, 13, 4));
            }
            sImage = sunCommandList;
        }

        public void Dispose()
        {
            sGeometry?.Dispose();
            sGeometry = null;
            sImage?.Dispose();
            sImage = null;
        }

        public override void Draw(CanvasDrawingSession drawingSession)
        {
            drawingSession.DrawImage(sImage);
        }
    }
}
