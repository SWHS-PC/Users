using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GuiTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }
        public int Str;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (RadioButton1.IsChecked == true)
            {
                MessageBox.Show(Str.ToString());
                Str++;
                MessageBox.Show(Str.ToString());


            }
            else
            {
                RadioButton2.IsChecked = true;
                MessageBox.Show("Goodbye.");
            }
        }
    }
}
