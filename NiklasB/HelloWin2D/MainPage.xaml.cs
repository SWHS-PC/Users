using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Numerics;

namespace HelloWin2D
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Graph_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(GraphPage));
        }

        private void BouncingBall_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BouncingBallPage));
        }

        private void SpaceWar_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SpaceWarPage));
        }
    }
}
