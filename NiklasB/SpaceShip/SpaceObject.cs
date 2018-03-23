using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System.Numerics;
using Windows.UI;

namespace SpaceShip
{
    abstract class SpaceObject
    {
        public abstract CanvasGeometry Geometry { get; }
        public abstract Matrix3x2 WorldTransform { get; }
        public abstract void Draw(CanvasDrawingSession drawingSession);
    }

    abstract class MovingSpaceObject : SpaceObject
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }

        public override Matrix3x2 WorldTransform => Matrix3x2.CreateTranslation(Position);

        public void AddGravity(float seconds, float g, Vector2 center)
        {
            Velocity += Helpers.ComputeGravity(seconds, g, Position, center);
        }

        public void Move(float seconds)
        {
            Position += Velocity * seconds;
        }
    }

    interface ISpaceResource : IDisposable
    {
        void CreateResources(ICanvasResourceCreator resourceCreator);
    }
}
