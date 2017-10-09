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
using Microsoft.Graphics.Canvas.Brushes;

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
        SpaceObject m_sun = new SpaceObject { Radius = m_sunRadius };
        SpaceShip m_ship1 = new SpaceShip { Radius = m_shipRadius };
        SpaceShip m_ship2 = new SpaceShip { Radius = m_shipRadius };
        List<Missile> m_missiles = new List<Missile>();
        public DirectionalBlurEffect m_flameImage;
        public CanvasGeometry m_missileGeometry;
        public ICanvasImage m_missileImage;
        bool m_isInitialized = false;
        bool m_isGameOver = false;
        bool m_showSun = true;

        const float m_rotationRate = 3.14f; // radians per second
        const float m_thrust = 150.0f;      // pixels per second per second
        const float m_shipRadius = 20;
        const float m_missileRadius = 7;
        const float m_missileMaxAge = 3;
        const float m_missileSpeed = 100;
        const float m_explosionDuration = 1;
        const float m_sunRadius = 50;
        const float m_gravity = 2000000;

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

        private void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            lock (this)
            {
                InitializeGame();

                m_isGameOver = false;

                m_gameOverBox.Visibility = Visibility.Collapsed;
            }
        }

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            lock (this)
            {
                // Save the canvas size.
                var newSize = new Vector2(
                    (float)m_canvas.ActualWidth,
                    (float)m_canvas.ActualHeight
                    );

                var oldSize = m_canvasSize;
                m_canvasSize = newSize;

                // Center the sun.
                m_sun.Position = newSize * new Vector2(0.5f, 0.5f);

                if (m_isInitialized)
                {
                    var scale = newSize / oldSize;

                    m_ship1.Position *= scale;
                    m_ship2.Position *= scale;

                    foreach (var missile in m_missiles)
                    {
                        missile.Position *= scale;
                    }
                }
                else
                {
                    // Set all the object positions to their initial values.
                    InitializeGame();
                    m_isInitialized = true;
                }
            }
        }

        void InitializeGame()
        {
            const float shipDistance = m_sunRadius * 5;
            float shipSpeed = m_showSun ? 100.0f : 0.0f;

            m_ship1.Position = m_sun.Position + new Vector2(-shipDistance, 0);
            m_ship1.Velocity = new Vector2(0, -shipSpeed);
            m_ship1.Heading = 0;
            m_ship1.IsExploding = false;
            m_ship1.ExplosionAge = 0;

            m_ship2.Position = m_sun.Position + new Vector2(shipDistance, 0);
            m_ship2.Velocity = new Vector2(0, shipSpeed);
            m_ship2.Heading = (float)Math.PI;
            m_ship2.IsExploding = false;
            m_ship2.ExplosionAge = 0;

            m_missiles.Clear();
        }

        Matrix3x2 ComputeObjectTransform(SpaceObject obj)
        {
            return Matrix3x2.CreateRotation(obj.Heading) * Matrix3x2.CreateTranslation(obj.Position);
        }

        private void Canvas_CreateResources(Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            m_sun.Geometry = MakeSunGeometry();
            m_sun.Image = MakeSunImage(m_sun.Geometry);

            var shipGeometry = MakeShipGeometry();

            m_ship1.Geometry = shipGeometry;
            m_ship1.Image = MakeBlurImage(shipGeometry, Colors.PaleGreen, 1.0f);

            m_ship2.Geometry = shipGeometry;
            m_ship2.Image = MakeBlurImage(shipGeometry, Colors.Yellow, 1.0f);

            m_flameImage = new DirectionalBlurEffect
            {
                Source = MakeBlurImage(MakeFlameGeometry(), Color.FromArgb(255, 140, 180, 255), 1.0f),
                Angle = -0.5f * 3.14159f
            };

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
                new Vector2(4.5f, 13),
                new Vector2(0, 28),
                new Vector2(-4.5f, 13)
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

        CanvasGeometry MakeSunGeometry()
        {
            return CanvasGeometry.CreateCircle(m_canvas, new Vector2(), m_sunRadius);
        }

        ICanvasImage MakeSunImage(CanvasGeometry geometry)
        {
            var stops = new CanvasGradientStop[]
            {
                new CanvasGradientStop { Color = Color.FromArgb(255, 255, 192, 121), Position = 0 },
                new CanvasGradientStop { Color = Color.FromArgb(255, 255, 173, 87), Position = 0.3f },
                new CanvasGradientStop { Color = Color.FromArgb(255, 244, 94, 0), Position = 0.9f },
                new CanvasGradientStop { Color = Color.FromArgb(255, 187, 13, 4), Position = 1.0f }
            };

            using (var brush = new CanvasRadialGradientBrush(m_canvas, stops))
            {
                brush.RadiusX = m_sunRadius;
                brush.RadiusY = m_sunRadius;

                var commandList = new CanvasCommandList(m_canvas);
                using (var g = commandList.CreateDrawingSession())
                {
                    g.FillGeometry(geometry, brush);
                }

                return commandList;
            }
        }

        CanvasCommandList CommandListFromGeometry(CanvasGeometry geometry, Color color)
        {
            var commandList = new CanvasCommandList(m_canvas);
            using (var g = commandList.CreateDrawingSession())
            {
                g.FillGeometry(geometry, color);
            }
            return commandList;
        }

        GaussianBlurEffect MakeBlurImage(CanvasGeometry geometry, Color color, float blur)
        {
            return new GaussianBlurEffect
            {
                Source = CommandListFromGeometry(geometry, color),
                BlurAmount = blur
            };
        }

        private void Canvas_Draw(Microsoft.Graphics.Canvas.UI.Xaml.ICanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedDrawEventArgs args)
        {
            lock (this)
            {
                var drawingSession = args.DrawingSession;

                if (m_showSun)
                {
                    drawingSession.Transform = ComputeObjectTransform(m_sun);
                    drawingSession.DrawImage(m_sun.Image);
                }

                m_flameImage.BlurAmount = 4.0f +
                    3.5f * (float)Math.Sin(args.Timing.TotalTime.TotalSeconds * 20);

                DrawShip(drawingSession, m_ship1);
                DrawShip(drawingSession, m_ship2);

                foreach (var missile in m_missiles)
                {
                    drawingSession.Transform = ComputeObjectTransform(missile);
                    drawingSession.DrawImage(missile.Image);
                }
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
            lock (this)
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

                        if (m_showSun && Intersects(missile, m_sun))
                        {
                            isExpired = true;
                        }
                        else if (!m_isGameOver)
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

                if (!m_isGameOver)
                {
                    if (Intersects(m_ship1, m_ship2))
                    {
                        m_ship1.IsExploding = true;
                        m_ship2.IsExploding = true;
                    }

                    if (m_showSun)
                    {
                        if (Intersects(m_ship1, m_sun))
                        {
                            m_ship1.IsExploding = true;
                        }

                        if (Intersects(m_ship2, m_sun))
                        {
                            m_ship2.IsExploding = true;
                        }
                    }
                }
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
                            m_gameOverBox.Visibility = Visibility.Visible;
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

        void AddGravity(SpaceObject obj, float seconds)
        {
            Vector2 vecToSun = m_sun.Position - obj.Position;
            float d2 = vecToSun.LengthSquared();
            float d = (float)Math.Sqrt(d2);
            if (d > m_sunRadius)
            {
                Vector2 deltaV = vecToSun * (seconds * (m_gravity / d2) / d);
                obj.Velocity += deltaV;
            }
            else
            {
                obj.Velocity = new Vector2();
            }
        }

        void MoveObject(SpaceObject obj, float seconds)
        {
            if (m_showSun)
            {
                AddGravity(obj, seconds);
            }

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
            if (v.LengthSquared() >= r * r)
                return false;

            var matrix = Matrix3x2.CreateRotation(obj2.Heading) *
                Matrix3x2.CreateTranslation(v) *
                Matrix3x2.CreateRotation(-obj1.Heading);

            var relation = obj1.Geometry.CompareWith(obj2.Geometry, matrix, 2.0f);

            return relation != CanvasGeometryRelation.Disjoint;
        }
    }
}
