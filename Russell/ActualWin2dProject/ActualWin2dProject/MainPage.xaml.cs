using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Numerics;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.Graphics.Canvas.Effects;
using Windows.UI;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ActualWin2dProject
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        ICanvasImage m_wordbubble;

        public MainPage()
        {
            this.InitializeComponent();
        }
        private void canvas_CreateResources(CanvasControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {

            var commandList = new CanvasCommandList(sender);
            using (var drawingSession = commandList.CreateDrawingSession())
            {

                using (var roundedRect = CanvasGeometry.CreateRoundedRectangle(sender, new Rect(50, 50, 300, 200), 10, 10))
                {
                    drawingSession.FillGeometry(roundedRect, Colors.SkyBlue);
                    drawingSession.DrawGeometry(roundedRect, Colors.Blue, 2);
                }
                drawingSession.DrawText("Hello World!", 100, 100, Colors.Black);

            }

            var blur = new GaussianBlurEffect
            {
                Source = commandList, BlurAmount = 2
            };


            m_wordbubble = blur;
        }
        private void canvas_Draw(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs args)
        {
            args.DrawingSession.DrawImage(m_wordbubble);

            args.DrawingSession.Transform = Matrix3x2.CreateRotation(3.14f * 0.25f, new Vector2(100, 100));
            args.DrawingSession.DrawImage(m_wordbubble);
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            this.canvas.RemoveFromVisualTree();
            this.canvas = null;
        }
    }
}
