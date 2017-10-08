using System;
using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.System;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.Effects;

namespace HelloWin2D
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SpaceWarPage : Page
    {
        class Ship
        {
            public CanvasGeometry geometry;
            public ICanvasImage image;
            public Vector2 position;
            public Vector2 velocity = new Vector2();
            public float heading;

            public bool isThrusting;
            public bool isRotatingLeft;
            public bool isRotatingRight;
        }

        Vector2 m_canvasSize;
        Ship m_ship1 = new Ship();
        Ship m_ship2 = new Ship();
        public CanvasGeometry m_flameGeometry;
        public ICanvasImage m_flameImage;
        bool m_isInitialized = false;

        const float m_rotationRate = 3.14f; // radians per second
        const float m_thrust = 500.0f;      // pixels per second per second
        const float m_shipRadius = 20;

        public SpaceWarPage()
        {
            this.InitializeComponent();

            m_canvas.KeyDown += Canvas_KeyDown;
            m_canvas.KeyUp += Canvas_KeyUp;

            m_canvas.SizeChanged += Canvas_SizeChanged;
            m_canvas.CreateResources += Canvas_CreateResources;
            m_canvas.Draw += Canvas_Draw;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            m_canvas.RemoveFromVisualTree();
            m_canvas = null;
        }

        private void Canvas_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            OnKeyChange(e.Key, true);
        }

        private void Canvas_KeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            OnKeyChange(e.Key, false);
        }

        void OnKeyChange(VirtualKey key, bool isDown)
        {
            switch (key)
            {
                case VirtualKey.A:
                    m_ship1.isRotatingLeft = isDown;
                    break;
                case VirtualKey.S:
                    m_ship1.isThrusting = isDown;
                    break;
                case VirtualKey.D:
                    m_ship1.isRotatingRight = isDown;
                    break;

                case VirtualKey.J:
                    m_ship2.isRotatingLeft = isDown;
                    break;
                case VirtualKey.K:
                    m_ship2.isThrusting = isDown;
                    break;
                case VirtualKey.L:
                    m_ship2.isRotatingRight = isDown;
                    break;
            }
        }

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Save the canvas size.
            m_canvasSize = new Vector2(
                (float)m_canvas.ActualWidth,
                (float)m_canvas.ActualHeight
                );

            // Initialize the ship positions and headings on the first call.
            if (!m_isInitialized)
            {
                m_ship1.position = m_canvasSize * new Vector2(0.1f, 0.5f);
                m_ship1.heading = (float)(Math.PI * 0.5);

                m_ship2.position = m_canvasSize * new Vector2(0.9f, 0.5f);
                m_ship2.heading = (float)(Math.PI * -0.5);

                m_isInitialized = true;
            }
        }

        Matrix3x2 ComputeShipTransform(Ship ship)
        {
            return Matrix3x2.CreateRotation(ship.heading) * Matrix3x2.CreateTranslation(ship.position);
        }

        private void Canvas_CreateResources(Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            var geometry = MakeShipGeometry();

            m_ship1.geometry = geometry;
            m_ship1.image = MakeBlurImage(geometry, Colors.PaleGreen, 1.0f);

            m_ship2.geometry = geometry;
            m_ship2.image = MakeBlurImage(geometry, Colors.Yellow, 1.0f);

            m_flameGeometry = MakeFlameGeometry();
            m_flameImage = MakeBlurImage(m_flameGeometry, Colors.White, 2.0f);
        }

        CanvasGeometry MakeShipGeometry()
        {
            // Create a path builder object, which we'll use to create a geometry object.
            using (var pathBuilder = new CanvasPathBuilder(m_canvas))
            {
                pathBuilder.BeginFigure(new Vector2(0, -15));
                pathBuilder.AddLine(new Vector2(10, 14));
                pathBuilder.AddLine(new Vector2(6, 14));
                pathBuilder.AddLine(new Vector2(6, 11));
                pathBuilder.AddLine(new Vector2(-6, 11));
                pathBuilder.AddLine(new Vector2(-6, 14));
                pathBuilder.AddLine(new Vector2(-10, 14));
                pathBuilder.EndFigure(CanvasFigureLoop.Closed);

                pathBuilder.BeginFigure(new Vector2(0, -8));
                pathBuilder.AddLine(new Vector2(5, 9));
                pathBuilder.AddLine(new Vector2(-5, 9));
                pathBuilder.EndFigure(CanvasFigureLoop.Closed);

                return CanvasGeometry.CreatePath(pathBuilder);
            }
        }

        CanvasGeometry MakeFlameGeometry()
        {
            var points = new Vector2[]
            {
                new Vector2(4, 13),
                new Vector2(0, 28),
                new Vector2(-4, 13)
            };

            return CanvasGeometry.CreatePolygon(m_canvas, points);
        }

        ICanvasImage MakeBlurImage(CanvasGeometry geometry, Color color, float blur)
        {
            var commandList = new CanvasCommandList(m_canvas);
            using (var g = commandList.CreateDrawingSession())
            {
                g.FillGeometry(geometry, color);
            }

            return new GaussianBlurEffect
            {
                Source = commandList,
                BlurAmount = blur
            };
        }

        private void Canvas_Draw(Microsoft.Graphics.Canvas.UI.Xaml.ICanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedDrawEventArgs args)
        {
            Update((float)args.Timing.ElapsedTime.TotalSeconds);

            var drawingSession = args.DrawingSession;

            drawingSession.Transform = ComputeShipTransform(m_ship1);
            drawingSession.DrawImage(m_ship1.image);

            if (m_ship1.isThrusting)
            {
                drawingSession.DrawImage(m_flameImage);
            }

            drawingSession.Transform = ComputeShipTransform(m_ship2);
            drawingSession.DrawImage(m_ship2.image);

            if (m_ship2.isThrusting)
            {
                drawingSession.DrawImage(m_flameImage);
            }
        }

        void Update(float seconds)
        {
            UpdateShip(m_ship1, seconds);
            UpdateShip(m_ship2, seconds);
        }

        void UpdateShip(Ship ship, float seconds)
        {
            if (ship.isRotatingLeft)
            {
                ship.heading -= m_rotationRate * seconds;
            }

            if (ship.isRotatingRight)
            {
                ship.heading += m_rotationRate * seconds;
            }

            if (ship.isThrusting)
            {
                ship.velocity += Vector2.Transform(
                    new Vector2(0, -m_thrust * seconds),
                    Matrix3x2.CreateRotation(ship.heading)
                    );
            }

            ship.position += ship.velocity * seconds;

            WrapPosition(ref ship.position.X, ship.velocity.X, m_canvasSize.X);
            WrapPosition(ref ship.position.Y, ship.velocity.Y, m_canvasSize.Y);
        }

        void WrapPosition(ref float position, float velocity, float canvasSize)
        {
            if (velocity < 0)
            {
                if (position <= -m_shipRadius)
                {
                    position = canvasSize + m_shipRadius;
                }
            }
            else
            {
                if (position >= canvasSize + m_shipRadius)
                {
                    position = -m_shipRadius;
                }
            }
        }

    }
}
