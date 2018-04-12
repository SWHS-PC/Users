using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomBorder
{
    public partial class MainWindow : Form
    {

        private bool mouseDown;
        private Point lastLocation;
        public int screenx = SystemInformation.VirtualScreen.Width, screeny = SystemInformation.VirtualScreen.Height;

        public MainWindow()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            Size = new Size(screenx, screeny);
            Text = " ";
            Frame.Size = new Size(screenx, screeny-30);
            CloseButton.Location = new Point(screenx - 25, 7);
            MaximizeButton.Location = new Point(screenx - 50, 7);
            MinimizeButton.Location = new Point(screenx - 75, 7);

            MaximizeButton.Image = global::CustomBorder.Properties.Resources.Maximize;

        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            
            if (mouseDown)
            {
                if (WindowState == FormWindowState.Maximized)
                {
                    WindowState = FormWindowState.Normal;
                    MaximizeButton.Image = global::CustomBorder.Properties.Resources.Maximize;
                    Location = new Point(0, 0);
                }
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MaximizeButton_Click(object sender, EventArgs e)
        {
            if(WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
                MaximizeButton.Image = global::CustomBorder.Properties.Resources.Maximize;
            }
            else if(WindowState == FormWindowState.Normal)
            {
                WindowState = FormWindowState.Maximized;
                MaximizeButton.Image = global::CustomBorder.Properties.Resources.RestoreDown;
            }
        }

        private void MinimizeButton_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;

        }

        private void FrameBorderColor(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, Frame.DisplayRectangle, Color.Black, ButtonBorderStyle.Solid);
        }

        private void runwebbrowser()
        {
            WebBrowser webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.Frame.Controls.Add(webBrowser1);
            webBrowser1.Name = "webBrowser1";
            webBrowser1.Url = new System.Uri("http://www.google.com", System.UriKind.Absolute);
            resources.ApplyResources(webBrowser1, "webBrowser1");
        }

    }
}
