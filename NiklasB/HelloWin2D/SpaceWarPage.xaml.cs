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
using Windows.UI.Xaml.Navigation;

namespace HelloWin2D
{
    public sealed partial class SpaceWarPage : Page
    {
        // Base class for objects on the canvas, such as ships and missiles.
        class SpaceObject
        {
            public CanvasGeometry Geometry { get; set; }
            public ICanvasImage Image { get; set; }
            public Vector2 Position { get; set; }
            public Vector2 Velocity { get; set; }
            public float Heading { get; set; }
            public float Radius { get; set; }

            public Matrix3x2 Transform
            {
                get { return Matrix3x2.CreateRotation(Heading) * Matrix3x2.CreateTranslation(Position); }
            }
        }

        class SpaceShip : SpaceObject
        {
            public bool IsThrusting { get; set; }
            public bool IsRotatingLeft { get; set; }
            public bool IsRotatingRight { get; set; }
            public bool IsFiring { get; set; }
            public bool IsExploding { get; set; }
            public float ExplosionAge { get; set; }
            public float Fuel { get; set; }

            public void Reset()
            {
                Position = new Vector2();
                Velocity = new Vector2();
                Heading = 0;
                IsFiring = false;
                IsExploding = false;
                ExplosionAge = 0;
                Fuel = m_maxFuel;
            }
        }

        class Missile : SpaceObject
        {
            public float Age { get; set; }
        }

        // Size of the canvas, initialized by the Canvas_SizeChanged event handler.
        Vector2 m_canvasSize;

        // Objects on the canvas.
        SpaceObject m_sun = new SpaceObject { Radius = m_sunRadius };
        SpaceShip m_ship1 = new SpaceShip { Radius = m_shipRadius };
        SpaceShip m_ship2 = new SpaceShip { Radius = m_shipRadius };
        List<Missile> m_missiles = new List<Missile>();

        // Graphics resources not associatd with specific objects.
        public DirectionalBlurEffect m_flameImage;
        public CanvasGeometry m_missileGeometry;
        public ICanvasImage m_missileImage;

        // Game state and options.
        bool m_isInitialized = false;
        bool m_isGameOver = false;
        bool m_showSun = false;
        bool m_longRangeMissiles = false;
        float MaxMissileAge => m_longRangeMissiles ? 20.0f : 2.0f;
        bool m_infiniteFuel = false;

        // Constants.
        const float m_guageWidth = 120.0f;
        const float m_guageHeight = 4.0f;
        const float m_guageMargin = 20.0f;
        const float m_rotationRate = 3.14f; // radians per second
        const float m_thrust = 150.0f;      // pixels per second per second
        const float m_maxFuel = 5.0f;       // seconds of thrust
        const float m_shipRadius = 20;
        const float m_missileRadius = 7;
        const float m_missileSpeed = 100;
        const float m_explosionDuration = 1;
        const float m_sunRadius = 50;
        const float m_gravity = 2000000;

        public SpaceWarPage()
        {
            this.InitializeComponent();

            m_canvas.SizeChanged += Canvas_SizeChanged;
            m_canvas.CreateResources += Canvas_CreateResources;

            m_canvas.Update += Canvas_Update;
            m_canvas.Draw += Canvas_Draw;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            Window.Current.CoreWindow.KeyDown -= CoreWindow_KeyDown;
            Window.Current.CoreWindow.KeyUp -= CoreWindow_KeyUp;

            base.OnNavigatedFrom(e);
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

        private void CoreWindow_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            if (!args.KeyStatus.WasKeyDown)
            {
                if (OnKeyChange(args.VirtualKey, true) || OnKeyPress(args.VirtualKey, args.KeyStatus.ScanCode))
                {
                    args.Handled = true;
                }
            }
        }

        private void CoreWindow_KeyUp(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            if (OnKeyChange(args.VirtualKey, false))
            {
                args.Handled = true;
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
            switch (key)
            {
                case VirtualKey.F:
                case VirtualKey.Q:
                    m_ship1.IsFiring = true;
                    return true;

                case VirtualKey.Space:
                    m_ship2.IsFiring = true;
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

        void ShowStatus()
        {
            if (m_isGameOver)
            {
                m_statusText.Text = "Game Over!";
            }

            m_gameStatusBox.Visibility = Visibility.Visible;
            m_canvas.Paused = true;
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            lock (this)
            {
                m_isGameOver = false;
                m_showSun = (bool)m_showSunCheckBox.IsChecked;
                m_longRangeMissiles = (bool)m_longRangMissileCheckBox.IsChecked;
                m_infiniteFuel = (bool)m_infiniteFuelCheckBox.IsChecked;

                InitializeGame();

                m_gameStatusBox.Visibility = Visibility.Collapsed;

                m_canvas.Paused = false;
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
                    // The canvas is being resized after the game is already initialized.
                    // Compute the proportional change in size (i.e., the scale).
                    Vector2 scale = newSize / oldSize;

                    // Multiply each ship's position by the scale, so it stays in the same
                    // relative position on the canvas.
                    m_ship1.Position *= scale;
                    m_ship2.Position *= scale;

                    // Do likewise for any missiles.
                    foreach (var missile in m_missiles)
                    {
                        missile.Position *= scale;
                    }
                }
                else
                {
                    // This is the first size event; initialize object positions.
                    InitializeGame();
                    m_isInitialized = true;
                }
            }
        }

        void InitializeGame()
        {
            const float shipDistance = m_sunRadius * 5;
            float shipSpeed = m_showSun ? 100.0f : 0.0f;

            // Position ship1 to the left of the Sun facing up.
            m_ship1.Reset();
            m_ship1.Position = m_sun.Position + new Vector2(-shipDistance, 0);
            m_ship1.Velocity = new Vector2(0, -shipSpeed);
            m_ship1.Heading = 0;

            // Position ship2 to the right of teh sun facing down.
            m_ship2.Reset();
            m_ship2.Position = m_sun.Position + new Vector2(shipDistance, 0);
            m_ship2.Velocity = new Vector2(0, shipSpeed);
            m_ship2.Heading = (float)Math.PI;

            // Clear any missiles.
            m_missiles.Clear();
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
                    drawingSession.Transform = m_sun.Transform;
                    drawingSession.DrawImage(m_sun.Image);
                }

                if (!m_infiniteFuel)
                {
                    DrawFuelGuages(drawingSession);
                }

                m_flameImage.BlurAmount = 4.0f +
                    3.5f * (float)Math.Sin(args.Timing.TotalTime.TotalSeconds * 20);

                DrawShip(drawingSession, m_ship1);
                DrawShip(drawingSession, m_ship2);

                foreach (var missile in m_missiles)
                {
                    drawingSession.Transform = missile.Transform;
                    drawingSession.DrawImage(missile.Image);
                }
            }
        }

        void DrawFuelGuages(CanvasDrawingSession drawingSession)
        {
            float y = m_canvasSize.Y - (m_guageMargin + m_guageHeight);

            drawingSession.Transform = Matrix3x2.CreateTranslation(
                m_guageMargin,
                y
                );

            DrawGuage(drawingSession, m_ship1.Fuel / m_maxFuel, Colors.Green);

            drawingSession.Transform = Matrix3x2.CreateTranslation(
                m_canvasSize.X - (m_guageMargin + m_guageWidth),
                y
                );

            DrawGuage(drawingSession, m_ship2.Fuel / m_maxFuel, Colors.Gold);
        }

        void DrawGuage(CanvasDrawingSession drawingSession, float level, Color color)
        {
            float fillWidth = m_guageWidth * level;

            if (fillWidth < m_guageWidth)
            {
                drawingSession.FillRectangle(
                    fillWidth, 0,
                    m_guageWidth - fillWidth, m_guageHeight,
                    Color.FromArgb(64, color.R, color.G, color.B)
                    );
            }

            if (fillWidth > 0)
            {
                drawingSession.FillRectangle(
                    0, 0,
                    fillWidth, m_guageHeight,
                    color
                    );
            }
        }

        void DrawShip(CanvasDrawingSession drawingSession, SpaceShip ship)
        {
            drawingSession.Transform = ship.Transform;

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

                if (ship.IsThrusting && ship.Fuel > 0 && !m_isGameOver)
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

                float maxMissileAge = MaxMissileAge;

                for (int i = m_missiles.Count - 1; i >= 0; --i)
                {
                    var missile = m_missiles[i];

                    bool isExpired = false;

                    if ((missile.Age += seconds) > maxMissileAge)
                    {
                        isExpired = true;
                    }
                    else
                    {
                        if (!MoveObject(missile, seconds))
                        {
                            isExpired = true;
                        }
                        else if (m_showSun && Intersects(missile, m_sun))
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
                            ShowStatus();
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
                    float thrustSeconds = seconds;
                    if (!m_infiniteFuel)
                    {
                        thrustSeconds = Math.Min(seconds, ship.Fuel);
                        ship.Fuel -= thrustSeconds;
                    }

                    if (thrustSeconds != 0)
                    {
                        ship.Velocity += Vector2.Transform(
                            new Vector2(0, -m_thrust * thrustSeconds),
                            Matrix3x2.CreateRotation(ship.Heading)
                            );
                    }
                }

                if (!MoveObject(ship, seconds))
                {
                    ship.IsExploding = true;
                }

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

        bool MoveObject(SpaceObject obj, float seconds)
        {
            // Adjust the velocity based on the Sun's gravity, if enabled.
            if (m_showSun)
            {
                AddGravity(obj, seconds);
            }

            // Add the velocity to the object's position.
            Vector2 newPosition = obj.Position + (obj.Velocity * seconds);
            obj.Position = newPosition;

            // Return true if and only if the object is in bounds.
            float r = obj.Radius;
            return newPosition.X >= r && newPosition.X <= m_canvasSize.X - r &&
                newPosition.Y >= r && newPosition.Y <= m_canvasSize.Y - r;
        }

        /// <summary>
        /// Determine whether two space objects intersect, for collision detection.
        /// </summary>
        bool Intersects(SpaceObject obj1, SpaceObject obj2)
        {
            // As an optimization, we can quickly return false if the distance squared
            // between the objects exceeds the sum of their radii squared.
            float r = obj1.Radius + obj2.Radius;
            Vector2 v = obj2.Position - obj1.Position;
            if (v.LengthSquared() >= r * r)
                return false;

            // The objects potentially intersect. Compute a matrix that transforms the 
            // second object to the model space of the first object.
            var matrix = Matrix3x2.CreateRotation(obj2.Heading) *
                Matrix3x2.CreateTranslation(v) *
                Matrix3x2.CreateRotation(-obj1.Heading);

            // Determine the relation between the two geometries, after applyin the
            // transformation to the second object's geometry.
            var relation = obj1.Geometry.CompareWith(obj2.Geometry, matrix, 2.0f);

            // Return true if the geometries are not disjoint.
            return relation != CanvasGeometryRelation.Disjoint;
        }
    }
}
