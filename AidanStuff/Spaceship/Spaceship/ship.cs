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
    class Ship : MovingSpaceObject, SpaceResource
    {
        CanvasGeometry sGeometry;
        ICanvasImage sImage;
        ICanvasImage thrustImage;

        const float PI = (float)Math.PI;
        const float rSpeed = 0.75f * PI * 2;
        const float Accel = 50;

        float shipAngle = 0;

        public bool isRotatingLeft { get; set; }
        public bool isRotatingRight { get; set; }
        public bool isThrusting { get; set; }

        public override CanvasGeometry Geometry => sGeometry;
        public override Matrix3x2 WorldTransform => Matrix3x2.CreateRotation(shipAngle) * base.WorldTransform;



        public void CreateResources(ICanvasResourceCreator resourceCreator)
        {
            
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
