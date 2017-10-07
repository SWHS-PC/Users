using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Numerics;

namespace HelloWin2D
{
    public sealed partial class BouncingBallPage : Page
    {
        Vector2 m_canvasSize = new Vector2(500.0f, 500.0f);
        Vector2 m_ballCenter = new Vector2(50, 50);
        Vector2 m_ballVelocity = new Vector2(80.0f, 0);
        float m_ballRadius = 10;
        float m_gravity = 500.0f;

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
            m_canvasSize = new Vector2(
                (float)m_canvas.ActualWidth,
                (float)m_canvas.ActualHeight
                );
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
            float seconds = (float)args.Timing.ElapsedTime.TotalSeconds;
            m_ballVelocity.Y += seconds * m_gravity;
            m_ballCenter += m_ballVelocity * seconds;

            Bounce(ref m_ballCenter.X, ref m_ballVelocity.X, m_ballRadius, m_canvasSize.X - m_ballRadius);
            Bounce(ref m_ballCenter.Y, ref m_ballVelocity.Y, m_ballRadius, m_canvasSize.Y - m_ballRadius);

            args.DrawingSession.FillCircle(m_ballCenter, m_ballRadius, Colors.DarkRed);
        }
    }
}
