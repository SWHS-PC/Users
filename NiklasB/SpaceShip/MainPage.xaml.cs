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
        // Resources created by Canvas_CreateResources
        CanvasGeometry m_sunGeometry;
        ICanvasImage m_sunImage;

        CanvasGeometry m_shipGeometry;
        ICanvasImage m_shipImage;

        // Current position of the ship.
        Vector2 m_shipCenter = new Vector2(200, 200);
        float m_shipAngle = 0;
        Matrix3x2 ShipTransform => Matrix3x2.CreateRotation(m_shipAngle) * Matrix3x2.CreateTranslation(m_shipCenter);

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
            // Create the sun geometry.
            m_sunGeometry = CanvasGeometry.CreateCircle(sender, new Vector2(), 80);

            // Create the sun image.
            var sunCommandList = new CanvasCommandList(sender);
            using (var drawingSession = sunCommandList.CreateDrawingSession())
            {
                drawingSession.FillGeometry(m_sunGeometry, Colors.Yellow);
            }
            m_sunImage = sunCommandList;

            // Create the ship geometry.
            var shipPoints = new Vector2[]
            {
                new Vector2(-15, -10),
                new Vector2(18, 0),
                new Vector2(-15, 10)
            };
            m_shipGeometry = CanvasGeometry.CreatePolygon(sender, shipPoints);

            // Create the ship image.
            var shipCommandList = new CanvasCommandList(sender);
            using (var drawingSession = shipCommandList.CreateDrawingSession())
            {
                drawingSession.FillGeometry(m_shipGeometry, Colors.DarkSlateGray);
                drawingSession.DrawGeometry(m_shipGeometry, Colors.SteelBlue, 2);
            }
            m_shipImage = shipCommandList;
        }

        private void Canvas_Update(Microsoft.Graphics.Canvas.UI.Xaml.ICanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedUpdateEventArgs args)
        {
            // TODO - here we will update values that change from one frame to the next,
            //        such as the ship position.
        }

        private void Canvas_Draw(Microsoft.Graphics.Canvas.UI.Xaml.ICanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedDrawEventArgs args)
        {
            // Draw the sun using the view transform.
            // The sun is already at the origin of the world coordinate space.
            args.DrawingSession.Transform = m_viewTransform;
            args.DrawingSession.DrawImage(m_sunImage);

            // Draw the ship using the product of the ship transform and view transform.
            // The ship transform goes from the ship's model space to world space.
            // The view transform goes from the world coordinate space to canvas coordinates.
            args.DrawingSession.Transform = ShipTransform * m_viewTransform;
            args.DrawingSession.DrawImage(m_shipImage);
        }
    }
}
