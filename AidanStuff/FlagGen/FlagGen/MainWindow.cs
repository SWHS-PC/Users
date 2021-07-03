using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlagGen
{
    public partial class MainWindow : Form
    {
        Graphics drawGraphics;

        Pen drawPenBlack = new System.Drawing.Pen(Color.Black, 0.5F);
        Pen drawPenYellow = new System.Drawing.Pen(Color.Yellow, 0.5F);
        SolidBrush BrushBlack = new System.Drawing.SolidBrush(Color.Black);
        SolidBrush BrushWhite = new System.Drawing.SolidBrush(Color.White);
        SolidBrush BrushRed = new System.Drawing.SolidBrush(Color.Red);
        SolidBrush BrushYellow = new System.Drawing.SolidBrush(Color.Yellow);
        SolidBrush BrushBlue = new System.Drawing.SolidBrush(Color.Blue);

        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void FlagGenWindow_Load(object sender, EventArgs e)
        {

        }

        public void DrawFlag()
        {
            drawGraphics = pictureBoxFlag.CreateGraphics();

            //drawGraphics.DrawRectangle(drawPenBlack, 10, 10, 10, 10);
            drawGraphics.FillRectangle(BrushRed, 0, 0, 650, 110);
            drawGraphics.FillRectangle(BrushYellow, 0, 110, 650, 110);
            drawGraphics.FillRectangle(BrushRed, 0, 220, 650, 110);

        }

        private void buttonGen_Click(object sender, EventArgs e)
        {
            DrawFlag();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
