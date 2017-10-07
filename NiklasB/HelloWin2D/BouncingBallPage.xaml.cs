using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Graphics.Canvas;

namespace HelloWin2D
{
    public sealed partial class BouncingBallPage : Page
    {
        class Ball
        {
            public Vector2 position;
            public Vector2 velocity;
            public float radius;
            public Color color;
        }

        const float m_gravity = 500.0f;
        const float m_newBallInterval = 1;

        float m_newBallTimer = 0;
        List<Ball> m_balls = new List<Ball>();
        Vector2 m_canvasSize;
        Matrix3x2 m_matrix = Matrix3x2.Identity;
        Random m_random = new Random();

        public BouncingBallPage()
        {
            this.InitializeComponent();

            this.Unloaded += Page_Unloaded;

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

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Save the canvas size.
            m_canvasSize = new Vector2(
                (float)m_canvas.ActualWidth,
                (float)m_canvas.ActualHeight
                );

            // Compute a transformation matrix such that positive Y is up.
            m_matrix = Matrix3x2.CreateScale(1, -1);

            // Add a displacement so the bottom of the canvas is at Y = 0.
            m_matrix.M32 = m_canvasSize.Y;
        }

        static void Bounce(ref float position, ref float velocity, float minPosition, float maxPosition)
        {
            if (minPosition >= maxPosition)
            {
                position = minPosition;
                velocity = 0;
            }
            else if (position <= minPosition)
            {
                position = minPosition;
                if (velocity < 0)
                    velocity = -velocity;
            }
            else if (position >= maxPosition)
            {
                position = maxPosition;
                if (velocity > 0)
                    velocity = -velocity;
            }
        }

        private void Canvas_CreateResources(Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
        }

        private void Canvas_Draw(Microsoft.Graphics.Canvas.UI.Xaml.ICanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedDrawEventArgs args)
        {
            Update((float)args.Timing.ElapsedTime.TotalSeconds);

            var drawingSession = args.DrawingSession;
            drawingSession.Transform = m_matrix;

            foreach (var ball in m_balls)
            {
                drawingSession.FillCircle(ball.position, ball.radius, ball.color);
            }

            drawingSession.Transform = Matrix3x2.Identity;
        }

        void Update(float seconds)
        {
            foreach (var ball in m_balls)
            {
                MoveBall(ball, seconds);
            }

            if (m_newBallTimer > seconds)
            {
                m_newBallTimer -= seconds;
            }
            else
            {
                AddBall();
                m_newBallTimer = m_newBallInterval;
            }
        }

        void MoveBall(Ball ball, float seconds)
        {
            // Add velocity * time to the current position.
            var position = ball.position + (ball.velocity * seconds);

            // Add gravity (i.e., vertical acceleration) to the velocity.
            var velocity = ball.velocity;
            velocity.Y -= (m_gravity * seconds);

            // Multiply the horizontal or vertical velocity by this number
            // when the ball bounces off a surface. This reverses the direction
            // and reduces the speed by 5%.
            const float bounceMultiplier = -0.95f;

            // Detect if the ball is bouncing off the floor.
            float minY = ball.radius;
            if (position.Y <= minY && velocity.Y < 0)
            {
                velocity.Y = velocity.Y * bounceMultiplier;
            }

            // Detect if the ball is bouncing off the left or right wall.
            float minX = ball.radius;
            float maxX = Math.Max(m_canvasSize.X - ball.radius, minX);

            if (position.X <= minX)
            {
                if (velocity.X < 0)
                {
                    velocity.X *= bounceMultiplier;
                }
            }
            else if (position.X >= maxX)
            {
                if (velocity.X > 0)
                {
                    velocity.X *= bounceMultiplier;
                }
            }

            ball.position = position;
            ball.velocity = velocity;
        }

        void AddBall()
        {
            var radius = (float)m_random.Next(8, 32);
            float altitude = (float)m_random.Next(400, 600);
            float velocityX = (float)m_random.Next(100, 300);

            var ball = new Ball
            {
                position = new Vector2(-radius, altitude),
                velocity = new Vector2(velocityX, 0),
                radius = radius,
                color = MakeRandomColor()
            };

            m_balls.Add(ball);
        }

        Color MakeRandomColor()
        {
            return Color.FromArgb(
                byte.MaxValue,
                (byte)m_random.Next(128),
                (byte)m_random.Next(128),
                (byte)m_random.Next(128)
                );
        }
    }
}
