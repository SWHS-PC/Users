using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Numerics;

namespace HelloWin2D
{
    public sealed partial class GraphPage : Page
    {
        Vector2 m_canvasSize;
        Vector2 m_normalizedOrigin = new Vector2(0.5f, 0.5f);
        Vector2 m_pixelOrigin;
        Matrix3x2 m_matrix;
        Matrix3x2 m_inverseMatrix;
        bool m_isMatrixValid = false;
        float m_scale = 50;

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

            // Compute where we want the origin of the coordinate space to be on the canvas, in pixels.
            // To do so, multiple the "normalized origin" by the canvas size. In normalized coordinates,
            // (0,0) and (1,1) are the top-left and bottom-right cordners of the canvas.
            m_pixelOrigin = m_normalizedOrigin * m_canvasSize;

            // Compute a matrix to translate from the world coordinate space (i.e., the abstract
            // coordinate space of the graph) to pixel coordinates on the canvas. The matrix is
            // the product of two transformations:
            //   1. Scale by the current scale factor and flip the Y axis so positive Y is up.
            //   2. Translate so the origin is at desired position on the canvas.
            m_matrix = 
                Matrix3x2.CreateScale(m_scale, -m_scale) *
                Matrix3x2.CreateTranslation(m_pixelOrigin);

            // Compute the inverse of the above matrix. This can be used to convert from pixel
            // coordinates on the canvas back to world coordinates.
            m_isMatrixValid = Matrix3x2.Invert(m_matrix, out m_inverseMatrix);
        }

        private string EquationText
        {
            get { return "y = 0.5 * (x + 4) * x + 1"; }
        }

        private double YFromX(double x)
        {
            return 0.5 * (x + 4) * x + 1;
        }

        private Vector2 PointFromX(float x)
        {
            return new Vector2(x, (float)YFromX(x));
        }

        private void Canvas_Draw(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs args)
        {
            var drawingSession = args.DrawingSession;

            drawingSession.DrawText(EquationText, new Vector2(10, 60), Colors.DarkBlue);

            if (m_isMatrixValid)
            {
                DrawAxes(drawingSession);
                DrawCurve(drawingSession);
            }
        }

        private void DrawAxes(CanvasDrawingSession drawingSession)
        {
            // Draw the X axis.
            drawingSession.DrawLine(
                new Vector2(0, m_pixelOrigin.Y),
                new Vector2(m_canvasSize.X, m_pixelOrigin.Y),
                Colors.Black
                );

            // Draw the Y axis.
            drawingSession.DrawLine(
                new Vector2(m_pixelOrigin.X, 0),
                new Vector2(m_pixelOrigin.X, m_canvasSize.Y),
                Colors.Black
                );

            // Draw tick marks on the Y axis.
            for (float y = m_pixelOrigin.Y - m_scale; y > 0; y -= m_scale)
            {
                drawingSession.DrawLine(
                    new Vector2(m_pixelOrigin.X - 5, y),
                    new Vector2(m_pixelOrigin.X + 5, y),
                    Colors.Black
                    );
            }

            for (float y = m_pixelOrigin.Y + m_scale; y < m_canvasSize.Y; y += m_scale)
            {
                drawingSession.DrawLine(
                    new Vector2(m_pixelOrigin.X - 5, y),
                    new Vector2(m_pixelOrigin.X + 5, y),
                    Colors.Black
                    );
            }

            // Draw tick marks on the X axis.
            for (float x = m_pixelOrigin.X - m_scale; x > 0; x -= m_scale)
            {
                drawingSession.DrawLine(
                    new Vector2(x, m_pixelOrigin.Y - 5),
                    new Vector2(x, m_pixelOrigin.Y + 5),
                    Colors.Black
                    );
            }

            for (float x = m_pixelOrigin.X + m_scale; x < m_canvasSize.X; x += m_scale)
            {
                drawingSession.DrawLine(
                    new Vector2(x, m_pixelOrigin.Y - 5),
                    new Vector2(x, m_pixelOrigin.Y + 5),
                    Colors.Black
                    );
            }
        }

        private void DrawCurve(CanvasDrawingSession drawingSession)
        {
            // Create a path builder object, which we'll use to create a geometry object.
            using (var pathBuilder = new CanvasPathBuilder(m_canvas))
            {
                // To create the geometry, we will compute the value of Y for many different
                // values of X, and add a series of line segments connecting those points.
                // Start with the left-most point in world space that is visible on the canvas.
                // End with the right-most point in world space taht is visible on the canvas.
                // Add points at increments of one canvas pixel.
                float minX = Vector2.Transform(new Vector2(), m_inverseMatrix).X;
                float maxX = Vector2.Transform(m_canvasSize, m_inverseMatrix).X;
                float deltaX = (maxX - minX) / m_canvasSize.X;

                pathBuilder.BeginFigure(PointFromX(minX));

                for (float x = minX + deltaX; x < maxX; x += deltaX)
                {
                    pathBuilder.AddLine(PointFromX(x));
                }

                pathBuilder.EndFigure(CanvasFigureLoop.Open);

                // Create the geometry object.
                using (var geometry = CanvasGeometry.CreatePath(pathBuilder))
                {
                    // Set the transform, draw the geometry, and restore the transform.
                    drawingSession.Transform = m_matrix;

                    drawingSession.DrawGeometry(
                        geometry,
                        Colors.DarkRed,
                        2.0f / m_scale // stroke width
                        );

                    drawingSession.Transform = Matrix3x2.Identity;
                }
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            m_canvas.RemoveFromVisualTree();
            m_canvas = null;
        }
    }
}
