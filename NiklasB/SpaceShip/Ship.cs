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
    class Ship : MovingSpaceObject, ISpaceResource
    {
        // Resources created by CreateResources.
        CanvasGeometry m_shipGeometry;
        ICanvasImage m_shipImage;
        ICanvasImage m_thrustImage;

        // Constants.
        const float PI = (float)Math.PI;
        const float RotationSpeed = 0.75f * PI * 2;
        const float Acceleration = 50;

        // A ship has an angle in addition to the position and velociy
        // inherited from MovingSpaceObject.
        float m_shipAngle = 0;

        // Ship control states.
        public bool IsRotatingLeft { get; set; }
        public bool IsRotatingRight { get; set; }
        public bool IsThrusting { get; set; }

        public override CanvasGeometry Geometry => m_shipGeometry;

        public override Matrix3x2 WorldTransform => Matrix3x2.CreateRotation(m_shipAngle) * base.WorldTransform;

        public void CreateResources(ICanvasResourceCreator resourceCreator)
        {
            // Create the ship geometry.
            var shipPoints = new Vector2[]
            {
                new Vector2(-15, -10),
                new Vector2(18, 0),
                new Vector2(-15, 10)
            };
            m_shipGeometry = CanvasGeometry.CreatePolygon(resourceCreator, shipPoints);

            // Create the ship image.
            var shipCommandList = new CanvasCommandList(resourceCreator);
            using (var drawingSession = shipCommandList.CreateDrawingSession())
            {
                drawingSession.FillGeometry(m_shipGeometry, Colors.DarkSlateGray);
                drawingSession.DrawGeometry(m_shipGeometry, Colors.SteelBlue, 2);
            }
            m_shipImage = shipCommandList;
        }

        public void Dispose()
        {
            m_shipGeometry?.Dispose();
            m_shipGeometry = null;

            m_shipImage?.Dispose();
            m_shipImage = null;

            m_thrustImage?.Dispose();
            m_thrustImage = null;
        }

        public override void Draw(CanvasDrawingSession drawingSession)
        {
            drawingSession.DrawImage(m_shipImage);
        }

        public new void Move(float seconds)
        {
            if (IsRotatingLeft)
            {
                m_shipAngle -= RotationSpeed * seconds;
            }

            if (IsRotatingRight)
            {
                m_shipAngle += RotationSpeed * seconds;
            }

            if (IsThrusting)
            {
                Velocity += Vector2.Transform(
                    new Vector2(Acceleration * seconds, 0),
                    Matrix3x2.CreateRotation(m_shipAngle)
                    );
            }

            Position += Velocity * seconds;
        }
    }
}
