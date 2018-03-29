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
    class Ship : MovingSpaceObject, ISpaceResource
    {
        CanvasGeometry shipGeometry;
        ICanvasImage shipImage;
        ICanvasImage thrustImage;
        
        const float PI = (float)Math.PI;
        const float RotationSpeed = 0.75f * PI * 2;
        const float Acceleration = 50;
        
        float shipAngle = 0;
        
        public bool IsRotatingLeft { get; set; }
        public bool IsRotatingRight { get; set; }
        public bool IsThrusting { get; set; }

        public override CanvasGeometry Geometry => shipGeometry;

        public override Matrix3x2 WorldTransform => Matrix3x2.CreateRotation(shipAngle) * base.WorldTransform;

        public void CreateResources(ICanvasResourceCreator resourceCreator)
        {
  
            var shipPoints = new Vector2[]
            {
                new Vector2(-15, -10),
                new Vector2(18, 0),
                new Vector2(-15, 10)
            };
            shipGeometry = CanvasGeometry.CreatePolygon(resourceCreator, shipPoints);

        
            var shipCommandList = new CanvasCommandList(resourceCreator);
            using (var drawingSession = shipCommandList.CreateDrawingSession())
            {
                drawingSession.FillGeometry(shipGeometry, Colors.DarkSlateGray);
                drawingSession.DrawGeometry(shipGeometry, Colors.SteelBlue, 2);
            }
            shipImage = shipCommandList;

            var thrustPoints = new Vector2[]
            {
                new Vector2(-18, 4),
                new Vector2(-32, 0),
                new Vector2(-18, -4)
            };

            var thrustCommandList = new CanvasCommandList(resourceCreator);
            using (var drawingSession = thrustCommandList.CreateDrawingSession())
            {
                using (var thrustGeometry = CanvasGeometry.CreatePolygon(resourceCreator, thrustPoints))
                {
                    drawingSession.FillGeometry(thrustGeometry, Colors.LightBlue);
                }
            }
            thrustImage = thrustCommandList;
        }

        public void Dispose()
        {
            shipGeometry?.Dispose();
            shipGeometry = null;

            shipImage?.Dispose();
            shipImage = null;
        
            thrustImage?.Dispose();
            thrustImage = null;
        }

        public override void Draw(CanvasDrawingSession drawingSession)
        {
            drawingSession.DrawImage(shipImage);

        
            if (IsThrusting)
            {
                drawingSession.DrawImage(thrustImage);
            }
        }

        public new void Move(float seconds)
        {
            if (IsRotatingLeft)
            {
                shipAngle -= RotationSpeed * seconds;
            }

            if (IsRotatingRight)
            {
                shipAngle += RotationSpeed * seconds;
            }

            if (IsThrusting)
            {
                Velocity += Vector2.Transform(
                    new Vector2(Acceleration * seconds, 0),
                    Matrix3x2.CreateRotation(shipAngle)
                    );
            }

            Position += Velocity * seconds;
        }
    }
}
