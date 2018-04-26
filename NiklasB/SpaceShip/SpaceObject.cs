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

        // NEW - Determine whether this object intersects with another object.
        public bool IntersectsWith(SpaceObject other)
        {
            // Compute transform from world space to this object's model space.
            Matrix3x2 modelTransform;
            Matrix3x2.Invert(WorldTransform, out modelTransform);

            // Compute transform from the other object's model space to this
            // object's model space.
            Matrix3x2 transform = other.WorldTransform * modelTransform;

            // Compare the two geometries, applying the transform to the other
            // geometry so they're both in the same coordinate space.
            var comparison = Geometry.CompareWith(other.Geometry, transform, 2.0f);

            return comparison != CanvasGeometryRelation.Disjoint;
        }
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
