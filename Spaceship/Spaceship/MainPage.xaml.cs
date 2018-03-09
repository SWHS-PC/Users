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
using Windows.UI;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Spaceship
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // Resources created by Canvas_CreateResources
        CanvasGeometry m_shipGeometry;
        ICanvasImage m_shipImage;

        CanvasGeometry m_sunGeometry;
        ICanvasImage m_sunImage;

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

            this.Unloaded += MainPage_Unloaded;

            Canvas.ClearColor = Windows.UI.Colors.Black;

            Canvas.SizeChanged += Canvas_SizeChanged;

            Canvas.CreateResources += Canvas_CreateResources;
            Canvas.Update += Canvas_Update;
            Canvas.Draw += Canvas_Draw;
        }

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            m_canvasSize = new Vector2((float)e.NewSize.Width, (float)e.NewSize.Height);
            m_viewTransform = Matrix3x2.CreateTranslation(m_canvasSize * 0.5f);
        }

        private void Canvas_Draw(Microsoft.Graphics.Canvas.UI.Xaml.ICanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedDrawEventArgs args)
        {
            args.DrawingSession.Transform = m_viewTransform;
            args.DrawingSession.DrawImage(m_sunImage);


            args.DrawingSession.Transform = ShipTransform * m_viewTransform;
            args.DrawingSession.DrawImage(m_shipImage);
            
        }

        private void Canvas_Update(Microsoft.Graphics.Canvas.UI.Xaml.ICanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedUpdateEventArgs args)
        {
            // TODO
        }

        private void Canvas_CreateResources(Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            //Create the sun geometry
            m_sunGeometry = CanvasGeometry.CreateCircle(sender, new Vector2(), 80);

            //Create the sun image
            var sunCommandList = new CanvasCommandList(sender);
            using (var drawingSession = sunCommandList.CreateDrawingSession())
            {
                drawingSession.FillGeometry(m_sunGeometry, Colors.Yellow);
                drawingSession.DrawGeometry(m_sunGeometry, Colors.Orange);
            }
           
            // Create the ship geometry
            var shipPoints = new Vector2[]
            {
            new Vector2(-15, -10),
            new Vector2(20, 0),
            new Vector2(-15,10)
            };
            m_shipGeometry = CanvasGeometry.CreatePolygon(sender, shipPoints);

            // Create the ship image.n
            var shipCommandList = new CanvasCommandList(sender);
            using (var drawingSession = shipCommandList.CreateDrawingSession())
            {
                drawingSession.FillGeometry(m_shipGeometry, Colors.DarkSlateGray);
                drawingSession.DrawGeometry(m_shipGeometry, Colors.SteelBlue, 2);
            }
            m_shipImage = shipCommandList;
            m_sunImage = sunCommandList;
        }

        private void MainPage_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Canvas.RemoveFromVisualTree();
        }
    }
}
