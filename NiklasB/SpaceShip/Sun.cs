using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.Brushes;
using System.Numerics;
using Windows.UI;

namespace SpaceShip
{
    class Sun : SpaceObject, ISpaceResource
    {
        CanvasGeometry m_geometry;
        ICanvasImage m_image;

        public float Gravity => 50 * 100 * 100;

        public Vector2 Center => new Vector2();

        public override CanvasGeometry Geometry => m_geometry;

        public override Matrix3x2 WorldTransform => Matrix3x2.Identity;

        public float Radius => 80;

        public void CreateResources(ICanvasResourceCreator resourceCreator)
        {
            // Create the sun geometry.
            m_geometry = CanvasGeometry.CreateCircle(resourceCreator, new Vector2(), Radius);

            // Create the sun image.
            var sunCommandList = new CanvasCommandList(resourceCreator);
            using (var drawingSession = sunCommandList.CreateDrawingSession())
            {
                var gradientStops = new CanvasGradientStop[]
                {
                    new CanvasGradientStop { Color = Color.FromArgb(255, 255, 192, 121), Position = 0 },
                    new CanvasGradientStop { Color = Color.FromArgb(255, 255, 173, 87), Position = 0.3f },
                    new CanvasGradientStop { Color = Color.FromArgb(255, 244, 94, 0), Position = 0.9f },
                    new CanvasGradientStop { Color = Color.FromArgb(255, 187, 13, 4), Position = 1.0f }
                };

                using (var brush = new CanvasRadialGradientBrush(resourceCreator, gradientStops))
                {
                    brush.RadiusX = Radius;
                    brush.RadiusY = Radius;

                    drawingSession.FillGeometry(m_geometry, brush);
                }
            }
            m_image = sunCommandList;
        }

        public void Dispose()
        {
            m_geometry?.Dispose();
            m_geometry = null;

            m_image?.Dispose();
            m_image = null;
        }

        public override void Draw(CanvasDrawingSession drawingSession)
        {
            drawingSession.DrawImage(m_image);
        }
    }
}
