using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.Brushes;
using System.Numerics;
using Windows.UI;

namespace Spaceship
{
    sealed class AsteroidShape : ISpaceResource
    {
        Vector2[] vertices;
        CanvasGeometry asteroidGeometry;
        ICanvasImage asteroidImage;

        public CanvasGeometry Geometry => asteroidGeometry;
        public ICanvasImage Image => asteroidImage;

        public AsteroidShape(Vector2[] mvertices)
        {
            vertices = mvertices;
        }

        public static Vector2[] SmallVertices = new Vector2[] 
        {
            new Vector2(-9, -9),
            new Vector2(-7, -11),
            new Vector2(0, -7),
            new Vector2(3, -9),
            new Vector2(9, -7),
            new Vector2(11, 0),
            new Vector2(8, 7),
            new Vector2(2, 9),
            new Vector2(-3, 7),
            new Vector2(-9, 6),
            new Vector2(-3, -10)

        };

        public void CreateResources(ICanvasResourceCreator resourceCreator)
        {
            asteroidGeometry = CanvasGeometry.CreatePolygon(resourceCreator, vertices);

            var commandList = new CanvasCommandList(resourceCreator);
            using (var drawingSession = commandList.CreateDrawingSession())
            {
                drawingSession.FillGeometry(asteroidGeometry, Color.FromArgb(255,65,64,64));
                drawingSession.DrawGeometry(asteroidGeometry, Color.FromArgb(255,128,128,128));
            }

            asteroidImage = commandList;
        }

        public void Dispose()
        {
            asteroidGeometry?.Dispose();
            asteroidGeometry = null;

            asteroidImage?.Dispose();
            asteroidImage = null;
        }
    }

    class Asteroids : MovingSpaceObject 
    {
        AsteroidShape Shape { get; }

        public Asteroids(AsteroidShape shape)
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
