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
        public static int screenx = SystemInformation.VirtualScreen.Width, screeny = SystemInformation.VirtualScreen.Height;
        public int rdscreenx = (screenx / 2) + (screenx / 4), rdscreeny = (screeny / 2) + (screeny / 4);

        public MainWindow()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            Text = " ";
            setSizeAndPos(0);
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
                    setSizeAndPos(1);
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
                setSizeAndPos(0);
            }
            else if(WindowState == FormWindowState.Normal)
            {
                WindowState = FormWindowState.Maximized;
                MaximizeButton.Image = global::CustomBorder.Properties.Resources.RestoreDown;
                setSizeAndPos(1);
            }
        }

        private void MinimizeButton_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void FrameBorderColor(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, Frame.DisplayRectangle, Color.Blue, ButtonBorderStyle.Solid);
        }

        //private void runwebbrowser()
        //{
        //    WebBrowser webBrowser1 = new System.Windows.Forms.WebBrowser();
        //    this.Frame.Controls.Add(webBrowser1);
        //    webBrowser1.Name = "webBrowser1";
        //    webBrowser1.Url = new System.Uri("http://www.google.com", System.UriKind.Absolute);
        //    resources.ApplyResources(webBrowser1, "webBrowser1");
        //}
        private void setSizeAndPos(int screentype)
        {
            int x = 0, y = 0;
            switch (screentype)
            {
                case 0:
                    x = rdscreenx;
                    y = rdscreeny;
                    Location = new Point(screenx / 8, screeny / 8);
                    break;
                case 1:
                    x = screenx;
                    y = screeny;
                    break;
            }
            
            Size = new Size(x, y);
            Frame.Size = new Size(x, y - 30);
            Frame.Invalidate();
            CloseButton.Location = new Point(x - 25, 7);
            MaximizeButton.Location = new Point(x - 50, 7);
            MinimizeButton.Location = new Point(x - 75, 7);
        }
    }
}
