using System;
using System.Numerics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class MainPage : Page
    {
        Sun sun = new Sun();

        CanvasGeometry shipGeometry;
        ICanvasImage shipImage;

        Vector2 shipCenter = new Vector2(200, 200);
        float shipAngle = 0;
        Matrix3x2 ShipTransform => Matrix3x2.CreateRotation(shipAngle) * Matrix3x2.CreateTranslation(shipCenter);

        Vector2 canvasSize = new Vector2(1, 1);
        Matrix3x2 viewTransform = Matrix3x2.Identity;

        public MainPage()
        {
            this.InitializeComponent();

            this.Unloaded += MainPage_Unloaded;

            Canvas.ClearColor = Windows.UI.Colors.Black;

            Canvas.SizeChanged += Canvas_SizeChanged;

            Canvas.CreateResources += Canvas_CreateResources;
            Canvas.Update += Canvas_Update;
            Canvas.Draw += Canvas_Draw;
        }

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            canvasSize = new Vector2((float)e.NewSize.Width, (float)e.NewSize.Height);
            viewTransform = Matrix3x2.CreateTranslation(canvasSize * 0.5f);
        }

        private void Canvas_Draw(Microsoft.Graphics.Canvas.UI.Xaml.ICanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedDrawEventArgs args)
        {
            args.DrawingSession.Transform = viewTransform;
            sun.Draw(args.DrawingSession);

            args.DrawingSession.Transform = ShipTransform * viewTransform;
            args.DrawingSession.DrawImage(shipImage);
            
        }

        private void Canvas_Update(Microsoft.Graphics.Canvas.UI.Xaml.ICanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedUpdateEventArgs args)
        {
            // TODO
        }

        private void Canvas_CreateResources(Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            sun.CreateResources(sender);

            var shipPoints = new Vector2[]
            {
            new Vector2(-15, -10),
            new Vector2(20, 0),
            new Vector2(-15,10)
            };
            shipGeometry = CanvasGeometry.CreatePolygon(sender, shipPoints);

            // Create the ship image.n
            var shipCommandList = new CanvasCommandList(sender);
            using (var drawingSession = shipCommandList.CreateDrawingSession())
            {
                drawingSession.FillGeometry(shipGeometry, Colors.DarkSlateGray);
                drawingSession.DrawGeometry(shipGeometry, Colors.SteelBlue, 2);
            }
            shipImage = shipCommandList;
        }

        private void MainPage_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Canvas.RemoveFromVisualTree();
        }
    }
}
