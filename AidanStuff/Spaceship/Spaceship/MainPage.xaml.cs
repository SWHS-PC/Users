using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System.Numerics;
using Windows.UI;

namespace Spaceship
{
    public sealed partial class MainPage : Page
    {
        Sun sun = new Sun();
        Ship ship = new Ship();
        AsteroidShape basicAsteroidShape = new AsteroidShape(AsteroidShape.SmallVertices);
        List<Asteroids> asteroidsList = new List<Asteroids>();
        Random random = new Random();

        IList<ISpaceResource> SpaceResources => new ISpaceResource[]
        {
            sun,
            ship,
            basicAsteroidShape
        };

        Vector2 canvasSize = new Vector2(1, 1);
        Matrix3x2 viewTransform = Matrix3x2.Identity;

        public MainPage()
        {
            this.InitializeComponent();


            this.Unloaded += (object sender, RoutedEventArgs e) =>
            {
                Canvas.RemoveFromVisualTree();
            };
            
            Canvas.ClearColor = Colors.Black;
            Canvas.SizeChanged += Canvas_SizeChanged;
            Canvas.CreateResources += Canvas_CreateResources;
            Canvas.Update += Canvas_Update;
            Canvas.Draw += Canvas_Draw;

            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;

            for(int i = 0; i < 99; i++)
            {
                AddAsteroid(300, 400, basicAsteroidShape);
            }

            SetInitialShipPosition();
        }

        void AddAsteroid(float minOrbit, float maxOrbit, AsteroidShape shape)
        {
            float radius = (float)( minOrbit + random.NextDouble() * (maxOrbit - minOrbit));
            float angle = (float)(random.NextDouble() * (Math.PI * 2));

            float angularVelocity = Computationals.ComputeOrbitalRadiansPerSecond(sun.Gravity, radius);
            var asteroid = new Asteroids(shape)
            {
                Position = Computationals.ComputeOrbitalPosition(angle, radius),
                Velocity = Computationals.ComputeOrbitalVelocity(angle, radius, angularVelocity)
            };

            asteroidsList.Add(asteroid);
        }

        private void CoreWindow_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case Windows.System.VirtualKey.Left:
                    ship.IsRotatingLeft = true;
                    args.Handled = true;
                    break;

                case Windows.System.VirtualKey.Right:
                    ship.IsRotatingRight = true;
                    args.Handled = true;
                    break;

                case Windows.System.VirtualKey.Up:
                    ship.IsThrusting = true;
                    args.Handled = true;
                    break;
            }
        }

        private void CoreWindow_KeyUp(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case Windows.System.VirtualKey.Left:
                    ship.IsRotatingLeft = false;
                    args.Handled = true;
                    break;

                case Windows.System.VirtualKey.Right:
                    ship.IsRotatingRight = false;
                    args.Handled = true;
                    break;

                case Windows.System.VirtualKey.Up:
                    ship.IsThrusting = false;
                    args.Handled = true;
                    break;
            }
        }

        void SetInitialShipPosition()
        {
            float radius = 200;
            float angularVelocity = Computationals.ComputeOrbitalRadiansPerSecond(sun.Gravity, radius);

            ship.Position = new Vector2(radius, 0);
            ship.Velocity = Computationals.ComputeOrbitalVelocity(0, radius, angularVelocity);
        }

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            canvasSize = new Vector2((float)e.NewSize.Width, (float)e.NewSize.Height);
            viewTransform = Matrix3x2.CreateTranslation(canvasSize * 0.5f);
        }

        private void Canvas_CreateResources(Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            foreach (var item in SpaceResources)
            {
                item.Dispose();
                item.CreateResources(sender);
            }
        }

        private void Canvas_Update(Microsoft.Graphics.Canvas.UI.Xaml.ICanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedUpdateEventArgs args)
        {
            float seconds = (float)args.Timing.ElapsedTime.TotalSeconds;

            ship.AddGravity(seconds, sun.Gravity, sun.Center);
            ship.Move(seconds);
            foreach(var asteroid in asteroidsList)
            {
                asteroid.AddGravity(seconds, sun.Gravity, sun.Center);
                asteroid.Move(seconds);
            }

            if(Collisions())
            {
                SetInitialShipPosition();
            }

        }

        bool Collisions()
        {
            if(ship.Intersect(sun))
            {
                return true;
            }

            foreach(var asteroid in asteroidsList)
            {
                if(ship.Intersect(asteroid))
                {
                    return true;
                }
            }

            return false;
        }
        private void Canvas_Draw(Microsoft.Graphics.Canvas.UI.Xaml.ICanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedDrawEventArgs args)
        {
            foreach (var asteroid in asteroidsList)
            {
                args.DrawingSession.Transform = asteroid.WorldTransform * viewTransform;
                asteroid.Draw(args.DrawingSession);
            }

            args.DrawingSession.Transform = viewTransform;
            sun.Draw(args.DrawingSession);
            args.DrawingSession.Transform = ship.WorldTransform * viewTransform;
            ship.Draw(args.DrawingSession);

            

        }
    }
}
