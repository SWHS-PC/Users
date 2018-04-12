using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rekt
{
    public partial class MainWindow : Form
    {

        public int screenx = SystemInformation.VirtualScreen.Width, screeny = SystemInformation.VirtualScreen.Height;

        private void MainWindow_Load(object sender, EventArgs e)
        {

        }

        public MainWindow()
        {
            InitializeComponent();

            FormBorderStyle = FormBorderStyle.None;
            Size = new Size(screenx, screeny);
            Text = " ";
            Cursor cur = new Cursor(Properties.Resources.b.GetHicon());
            this.Cursor = cur;
        }

    }
}
