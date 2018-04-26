using System;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.Brushes;
using System.Numerics;
using Windows.UI;

namespace SpaceShip
{
    sealed class AsteroidShape : ISpaceResource
    {
        Vector2[] m_vertices;
        CanvasGeometry m_geometry;
        ICanvasImage m_image;

        public AsteroidShape(Vector2[] vertices)
        {
            m_vertices = vertices;
        }

        public static Vector2[] SquareVertices = new Vector2[]
        {
            new Vector2(-8, -8),
            new Vector2(8, -8),
            new Vector2(8, 8),
            new Vector2(-8, 8)
        };

        public static Vector2[] SmallVertices = new Vector2[]
        {
            new Vector2(-9, -9),
            new Vector2(-7, -11),
            new Vector2(0, -7),
            new Vector2(3, -9),
            new Vector2(9, -7),
            new Vector2(11, 0),
            new Vector2(8, 7),
            new Vector2(0, 11),
            new Vector2(-3, 7),
            new Vector2(-7, 6),
            new Vector2(-11, -1)
        };

        public void CreateResources(ICanvasResourceCreator resourceCreator)
        {
            m_geometry = CanvasGeometry.CreatePolygon(resourceCreator, m_vertices);

            var commandList = new CanvasCommandList(resourceCreator);
            using (var drawingSession = commandList.CreateDrawingSession())
            {
                drawingSession.FillGeometry(m_geometry, Color.FromArgb(255, 64, 64, 64));
                drawingSession.DrawGeometry(m_geometry, Color.FromArgb(255, 128, 128, 128), 0.5f);
            }

            m_image = commandList;
        }

        public void Dispose()
        {
            m_geometry?.Dispose();
            m_geometry = null;

            m_image?.Dispose();
            m_image = null;
        }

        public CanvasGeometry Geometry => m_geometry;
        public ICanvasImage Image => m_image;
    }

    class Asteroid : MovingSpaceObject
    {
        AsteroidShape Shape { get; }

        public Asteroid(AsteroidShape shape)
        {
            Shape = shape;
        }

        public override CanvasGeometry Geometry => Shape.Geometry;

        public override void Draw(CanvasDrawingSession drawingSession)
        {
            drawingSession.DrawImage(Shape.Image);
        }
    }
}
