using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Graphics.Canvas.Geometry;
using System.Numerics;

namespace HelloWin2D
{
    public sealed partial class GraphPage : Page
    {
        Vector2 m_canvasSize = new Vector2(500.0f, 500.0f);
        Vector2 m_normalizedOrigin = new Vector2(0.5f, 0.5f);
        float m_scale = 100;

        public GraphPage()
        {
            this.InitializeComponent();

            this.Unloaded += Page_Unloaded;

            m_canvas.SizeChanged += Canvas_SizeChanged;
            m_canvas.Draw += Canvas_Draw;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            m_canvasSize = new Vector2(
                (float)m_canvas.ActualWidth,
                (float)m_canvas.ActualHeight
                );
        }

        Matrix3x2 ComputeMatrix()
        {
            return Matrix3x2.CreateScale(m_scale, -m_scale) *
                Matrix3x2.CreateTranslation(m_canvasSize * m_normalizedOrigin);
        }

        private string FormulaText
        {
            get { return "sin(x) + 1 + x/4"; }
        }

        private double YFromX(double x)
        {
            return System.Math.Sin(x) + 1 + (0.25 * x);
        }

        private Vector2 PointFromX(float x)
        {
            return new Vector2(x, (float)YFromX(x));
        }

        private void Canvas_Draw(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs args)
        {
            var matrix = ComputeMatrix();


            Matrix3x2 inverseMatrix;
            if (!Matrix3x2.Invert(matrix, out inverseMatrix))
                return;

            var origin = Vector2.Transform(new Vector2(), matrix);

            args.DrawingSession.DrawLine(
                new Vector2(origin.X, 0),
                new Vector2(origin.X, m_canvasSize.Y),
                Colors.Black
                );

            args.DrawingSession.DrawLine(
                new Vector2(0, origin.Y),
                new Vector2(m_canvasSize.X, origin.Y),
                Colors.Black
                );

            float minX = Vector2.Transform(new Vector2(), inverseMatrix).X;
            float maxX = Vector2.Transform(m_canvasSize, inverseMatrix).X;
            float deltaX = (maxX - minX) / m_canvasSize.X;

            using (var pathBuilder = new CanvasPathBuilder(sender))
            {
                pathBuilder.BeginFigure(PointFromX(minX));

                for (float x = minX + deltaX; x < maxX; x += deltaX)
                {
                    pathBuilder.AddLine(PointFromX(x));
                }

                pathBuilder.EndFigure(CanvasFigureLoop.Open);

                using (var geometry = CanvasGeometry.CreatePath(pathBuilder))
                {
                    args.DrawingSession.Transform = matrix;

                    args.DrawingSession.DrawGeometry(
                        geometry,
                        Colors.DarkRed,
                        2.0f / m_scale // stroke width
                        );
                }
            }

            args.DrawingSession.Transform = Matrix3x2.Identity;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            m_canvas.RemoveFromVisualTree();
            m_canvas = null;
        }
    }
}
