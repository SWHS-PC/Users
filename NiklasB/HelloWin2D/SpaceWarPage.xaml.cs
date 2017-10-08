using System;
using System.Numerics;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.System;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.Effects;

namespace HelloWin2D
{
    public sealed partial class SpaceWarPage : Page
    {
        class SpaceObject
        {
            public CanvasGeometry Geometry;
            public ICanvasImage Image;
            public Vector2 Position;
            public Vector2 Velocity;
            public float Heading;
            public float Radius;
        }

        class SpaceShip : SpaceObject
        {
            public bool IsThrusting;
            public bool IsRotatingLeft;
            public bool IsRotatingRight;
            public bool IsFiring;
            public bool IsExploding;
            public float ExplosionAge;
        }

        class Missile : SpaceObject
        {
            public float Age;
        }

        Vector2 m_canvasSize;
        SpaceShip m_ship1 = new SpaceShip { Radius = m_shipRadius };
        SpaceShip m_ship2 = new SpaceShip { Radius = m_shipRadius };
        List<Missile> m_missiles = new List<Missile>();
        public CanvasGeometry m_flameGeometry;
        public ICanvasImage m_flameImage;
        public CanvasGeometry m_missileGeometry;
        public ICanvasImage m_missileImage;
        bool m_isInitialized = false;
        bool m_isGameOver = false;

        const float m_rotationRate = 3.14f; // radians per second
        const float m_thrust = 500.0f;      // pixels per second per second
        const float m_shipRadius = 20;
        const float m_missileRadius = 7;
        const float m_missileMaxAge = 2;
        const float m_missileSpeed = 400;
        const float m_explosionDuration = 1;

        public SpaceWarPage()
        {
            this.InitializeComponent();

            m_canvas.KeyDown += Canvas_KeyDown;
            m_canvas.KeyUp += Canvas_KeyUp;

            m_canvas.SizeChanged += Canvas_SizeChanged;
            m_canvas.CreateResources += Canvas_CreateResources;

            m_canvas.Update += Canvas_Update;
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
            if (!e.KeyStatus.WasKeyDown)
            {
                if (OnKeyChange(e.Key, true) || OnKeyPress(e.Key, e.KeyStatus.ScanCode))
                {
                    e.Handled = true;
                }
            }
        }

        private void Canvas_KeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (OnKeyChange(e.Key, false))
            {
                e.Handled = true;
            }
        }

        bool OnKeyChange(VirtualKey key, bool isDown)
        {
            switch (key)
            {
                case VirtualKey.A:
                    m_ship1.IsRotatingLeft = isDown;
                    return true;
                case VirtualKey.S:
                    m_ship1.IsThrusting = isDown;
                    return true;
                case VirtualKey.D:
                    m_ship1.IsRotatingRight = isDown;
                    return true;

                case VirtualKey.J:
                    m_ship2.IsRotatingLeft = isDown;
                    return true;
                case VirtualKey.K:
                    m_ship2.IsThrusting = isDown;
                    return true;
                case VirtualKey.L:
                    m_ship2.IsRotatingRight = isDown;
                    return true;
            }
            return false;
        }

        bool OnKeyPress(VirtualKey key, uint scanCode)
        {
            if (key == VirtualKey.F)
            {
                m_ship1.IsFiring = true;
                return true;
            }

            switch (scanCode)
            {
                case 39:
                    m_ship2.IsFiring = true;
                    return true;
            }

            return false;
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
                m_ship1.Position = m_canvasSize * new Vector2(0.1f, 0.5f);
                m_ship1.Heading = (float)(Math.PI * 0.5);

                m_ship2.Position = m_canvasSize * new Vector2(0.9f, 0.5f);
                m_ship2.Heading = (float)(Math.PI * -0.5);

                m_isInitialized = true;
            }
        }

        Matrix3x2 ComputeObjectTransform(SpaceObject obj)
        {
            return Matrix3x2.CreateRotation(obj.Heading) * Matrix3x2.CreateTranslation(obj.Position);
        }

        private void Canvas_CreateResources(Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            var geometry = MakeShipGeometry();

            m_ship1.Geometry = geometry;
            m_ship1.Image = MakeBlurImage(geometry, Colors.PaleGreen, 1.0f);

            m_ship2.Geometry = geometry;
            m_ship2.Image = MakeBlurImage(geometry, Colors.Yellow, 1.0f);

            m_flameGeometry = MakeFlameGeometry();
            m_flameImage = MakeBlurImage(m_flameGeometry, Colors.White, 2.0f);

            m_missileGeometry = MakeMissileGeometry();
            m_missileImage = MakeBlurImage(m_missileGeometry, Colors.White, 2.0f);

            foreach (var missile in m_missiles)
            {
                missile.Geometry = m_missileGeometry;
                missile.Image = m_missileImage;
            }
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

        CanvasGeometry MakeMissileGeometry()
        {
            var points = new Vector2[]
            {
                new Vector2(2, -6),
                new Vector2(2, 6),
                new Vector2(-2, 6),
                new Vector2(-2, -6)
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
            var drawingSession = args.DrawingSession;

            DrawShip(drawingSession, m_ship1);
            DrawShip(drawingSession, m_ship2);

            foreach (var missile in m_missiles)
            {
                drawingSession.Transform = ComputeObjectTransform(missile);
                drawingSession.DrawImage(missile.Image);
            }
        }

        void DrawShip(CanvasDrawingSession drawingSession, SpaceShip ship)
        {
            drawingSession.Transform = ComputeObjectTransform(ship);

            if (ship.IsExploding)
            {
                if (ship.ExplosionAge < m_explosionDuration)
                {
                    var image = new GaussianBlurEffect
                    {
                        Source = ship.Image,
                        BlurAmount = ship.ExplosionAge * 10
                    };

                    drawingSession.DrawImage(image);
                }
            }
            else
            {
                drawingSession.DrawImage(ship.Image);

                if (ship.IsThrusting && !m_isGameOver)
                {
                    drawingSession.DrawImage(m_flameImage);
                }
            }
        }

        private void Canvas_Update(Microsoft.Graphics.Canvas.UI.Xaml.ICanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedUpdateEventArgs args)
        {
            float seconds = (float)args.Timing.ElapsedTime.TotalSeconds;

            UpdateShip(m_ship1, seconds);
            UpdateShip(m_ship2, seconds);

            for (int i = m_missiles.Count - 1; i >= 0; --i)
            {
                var missile = m_missiles[i];

                bool isExpired = false;

                if ((missile.Age += seconds) > m_missileMaxAge)
                {
                    isExpired = true;
                }
                else
                {
                    MoveObject(missile, seconds);

                    if (!m_isGameOver)
                    {
                        if (Intersects(missile, m_ship1))
                        {
                            m_ship1.IsExploding = true;
                            isExpired = true;
                        }

                        if (Intersects(missile, m_ship2))
                        {
                            m_ship2.IsExploding = true;
                            isExpired = true;
                        }
                    }
                }

                if (isExpired)
                {
                    m_missiles.RemoveAt(i);
                }
            }

            if (!m_isGameOver && Intersects(m_ship1, m_ship2))
            {
                m_ship1.IsExploding = true;
                m_ship2.IsExploding = true;
            }
        }

        void UpdateShip(SpaceShip ship, float seconds)
        {
            if (ship.IsExploding || m_isGameOver)
            {
                MoveObject(ship, seconds);

                if (ship.IsExploding)
                {
                    ship.ExplosionAge += seconds;

                    if (!m_isGameOver && ship.ExplosionAge >= m_explosionDuration)
                    {
                        m_isGameOver = true;

                        var ignored = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            m_gameOverTextBlock.Visibility = Visibility.Visible;
                        });
                    }
                }
            }
            else
            {
                if (ship.IsRotatingLeft)
                {
                    ship.Heading -= m_rotationRate * seconds;
                }

                if (ship.IsRotatingRight)
                {
                    ship.Heading += m_rotationRate * seconds;
                }

                if (ship.IsThrusting)
                {
                    ship.Velocity += Vector2.Transform(
                        new Vector2(0, -m_thrust * seconds),
                        Matrix3x2.CreateRotation(ship.Heading)
                        );
                }

                MoveObject(ship, seconds);

                if (ship.IsFiring)
                {
                    var headingMatrix = Matrix3x2.CreateRotation(ship.Heading);
                    var positionDelta = Vector2.Transform(new Vector2(0, -(m_shipRadius + m_missileRadius)), headingMatrix);
                    var velocityDelta = Vector2.Transform(new Vector2(0, -m_missileSpeed), headingMatrix);

                    var missile = new Missile
                    {
                        Geometry = m_missileGeometry,
                        Image = m_missileImage,
                        Position = ship.Position + positionDelta,
                        Velocity = ship.Velocity + velocityDelta,
                        Heading = ship.Heading,
                        Radius = m_missileRadius
                    };

                    m_missiles.Add(missile);

                    ship.IsFiring = false;
                }
            }
        }

        void MoveObject(SpaceObject obj, float seconds)
        {
            obj.Position += obj.Velocity * seconds;

            WrapPosition(ref obj.Position.X, obj.Velocity.X, m_canvasSize.X);
            WrapPosition(ref obj.Position.Y, obj.Velocity.Y, m_canvasSize.Y);
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

        bool Intersects(SpaceObject obj1, SpaceObject obj2)
        {
            float r = obj1.Radius + obj2.Radius;
            Vector2 v = obj2.Position - obj1.Position;
            if ((v.X * v.X) + (v.Y * v.Y) >= (r * r))
                return false;

            var matrix = Matrix3x2.CreateRotation(obj2.Heading) *
                Matrix3x2.CreateTranslation(v) *
                Matrix3x2.CreateRotation(-obj1.Heading);

            var relation = obj1.Geometry.CompareWith(obj2.Geometry, matrix, 2.0f);

            return relation != CanvasGeometryRelation.Disjoint;
        }
    }
}
