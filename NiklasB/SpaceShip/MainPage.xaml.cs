using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System.Numerics;
using Windows.UI;

namespace SpaceShip
{
    public sealed partial class MainPage : Page
    {
        Sun m_sun = new Sun();
        Ship m_ship = new Ship();

        // Canvas size and view transform.
        Vector2 m_canvasSize = new Vector2(1, 1);
        Matrix3x2 m_viewTransform = Matrix3x2.Identity;

        public MainPage()
        {
            this.InitializeComponent();

            // Handle the Page.Unloaded event as described here:
            // http://microsoft.github.io/Win2D/html/QuickStart.htm
            this.Unloaded += (object sender, RoutedEventArgs e) =>
            {
                Canvas.RemoveFromVisualTree();
            };

            // Set the clear color to the black of outer space.
            Canvas.ClearColor = Colors.Black;

            // Add Canvas event handlers.
            Canvas.SizeChanged += Canvas_SizeChanged;
            Canvas.CreateResources += Canvas_CreateResources;
            Canvas.Update += Canvas_Update;
            Canvas.Draw += Canvas_Draw;

            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;

            SetInitialShipPosition();
        }

        private void CoreWindow_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case Windows.System.VirtualKey.Left:
                    m_ship.IsRotatingLeft = true;
                    args.Handled = true;
                    break;

                case Windows.System.VirtualKey.Right:
                    m_ship.IsRotatingRight = true;
                    args.Handled = true;
                    break;

                case Windows.System.VirtualKey.Up:
                    m_ship.IsThrusting = true;
                    args.Handled = true;
                    break;
            }
        }

        private void CoreWindow_KeyUp(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case Windows.System.VirtualKey.Left:
                    m_ship.IsRotatingLeft = false;
                    args.Handled = true;
                    break;

                case Windows.System.VirtualKey.Right:
                    m_ship.IsRotatingRight = false;
                    args.Handled = true;
                    break;

                case Windows.System.VirtualKey.Up:
                    m_ship.IsThrusting = false;
                    args.Handled = true;
                    break;
            }
        }

        void SetInitialShipPosition()
        {
            float radius = 200;
            float angularVelocity = Helpers.ComputeOrbitalRadiansPerSecond(m_sun.Gravity, radius);

            m_ship.Position = new Vector2(radius, 0);
            m_ship.Velocity = Helpers.ComputeOrbitalVelocity(0, radius, angularVelocity);
        }

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Save the new canvas size.
            m_canvasSize = new Vector2((float)e.NewSize.Width, (float)e.NewSize.Height);

            // Create a view transform such that the origin of the world coordinate space
            // is in the center of the canvas.
            m_viewTransform = Matrix3x2.CreateTranslation(m_canvasSize * 0.5f);
        }

        private void Canvas_CreateResources(Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            m_sun.CreateResources(sender);
            m_ship.CreateResources(sender);
        }

        private void Canvas_Update(Microsoft.Graphics.Canvas.UI.Xaml.ICanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedUpdateEventArgs args)
        {
            float seconds = (float)args.Timing.ElapsedTime.TotalSeconds;

            m_ship.AddGravity(seconds, m_sun.Gravity, m_sun.Center);
            m_ship.Move(seconds);
        }

        private void Canvas_Draw(Microsoft.Graphics.Canvas.UI.Xaml.ICanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedDrawEventArgs args)
        {
            // Draw the sun using the view transform.
            // The sun is already at the origin of the world coordinate space.
            args.DrawingSession.Transform = m_viewTransform;
            m_sun.Draw(args.DrawingSession);

            // Draw the ship using the product of the ship transform and view transform.
            // The ship transform goes from the ship's model space to world space.
            // The view transform goes from the world coordinate space to canvas coordinates.
            args.DrawingSession.Transform = m_ship.WorldTransform * m_viewTransform;
            m_ship.Draw(args.DrawingSession);
        }
    }
}
