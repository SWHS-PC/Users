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
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.UI.Xaml;

namespace Spaceship
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

        public void Move(float seconds)
        {
            Position += Velocity * seconds;
        }
    }

    interface SpaceResource : IDisposable
    {
        void CreateResources(ICanvasResourceCreator resourceCreator);
    }
}
