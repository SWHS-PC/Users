using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;

namespace LifeGui
{
    public partial class DrawForm : Form
    {
        System.Drawing.Graphics drawGraphics;
        System.Drawing.Pen drawPenBlack = new System.Drawing.Pen(Color.Black, 0.5F);
        System.Drawing.Pen drawPenYellow = new System.Drawing.Pen(Color.Yellow, 0.5F);
        System.Drawing.SolidBrush drawBrushBlack = new System.Drawing.SolidBrush(Color.Black);
        System.Drawing.SolidBrush drawBrushWhite = new System.Drawing.SolidBrush(Color.White);

        public static int w = 84, h = 33;
        public static bool[,] Map;
        public static bool[,] newMap;
        public static bool[,] oldMap;
        public bool this[int x, int y] { get { return Map[x, y]; } set { Map[x, y] = value; } }
        public bool clicked;
        public bool autoRun = false;
        System.Threading.Thread t;
        public int genCount;

        public DrawForm()
        {
            InitializeComponent();

            labelGeneration.Text = "Generation: " + Convert.ToString(genCount);
            
            Map = new bool[w, h];
            newMap = new bool[w, h];
            oldMap = new bool[w, h];
        }

        private void DrawNext(object sender, EventArgs e)
        {
            NextGen();
            Draw();
            genCount++;
            labelGeneration.Text = "Generation: " + Convert.ToString(genCount);
        }

        public void NextGen()
        {
            oldMap = Map;
            newMap = new bool[w, h];
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    int Neighbors = FindNeighbor(Map, i, j, -1, 0) + FindNeighbor(Map, i, j, -1, 1) + FindNeighbor(Map, i, j, 0, 1) + FindNeighbor(Map, i, j, 1, 1) + FindNeighbor(Map, i, j, 1, 0) + FindNeighbor(Map, i, j, 1, -1) + FindNeighbor(Map, i, j, 0, -1) + FindNeighbor(Map, i, j, -1, -1);

                    bool KeepLiving = false;
                    bool isAlive = Map[i, j];

                    if (isAlive && (Neighbors == 2 || Neighbors == 3))
                    {
                        KeepLiving = true;
                    }
                    else if (!isAlive && Neighbors == 3)
                    {
                        KeepLiving = true;
                    }

                    newMap[i, j] = KeepLiving;
                }
            }
            Map = newMap;
        }

        static int FindNeighbor(bool[,] Map, int x, int y, int offsetx, int offsety)
        {
            int result = 0;
            int proposedOffsetX = x + offsetx;
            int proposedOffsetY = y + offsety;
            bool outOfBounds = proposedOffsetX < 0 || proposedOffsetX >= w | proposedOffsetY < 0 || proposedOffsetY >= h;
            if (!outOfBounds)
            {
                result = Map[x + offsetx, y + offsety] ? 1 : 0;
            }
            return result;
        }

        public bool AddCell(int x, int y)
        {
            bool currentValue = Map[x, y];
            return Map[x, y] = !currentValue;
        }

        private void DrawClick(object sender, EventArgs e)
        {
            Map = new bool[w, h];
            newMap = new bool[w, h];

            drawGraphics = PictureBoxMap.CreateGraphics();

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    drawGraphics.DrawRectangle(drawPenBlack, x * 10, y * 10, 10, 10);
                    drawGraphics.FillRectangle(Map[x, y] ? drawBrushBlack : drawBrushWhite, (x * 10) + 1, (y * 10) + 1, 9, 9);
                }
            }
            genCount = 0;
            labelGeneration.Text = "Generation: " + Convert.ToString(genCount);
        }

        private void Draw()
        {
            drawGraphics = PictureBoxMap.CreateGraphics();
            
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    if (oldMap[x, y] == true)
                    {
                        drawGraphics.FillRectangle(drawBrushWhite, (x * 10) + 1, (y * 10) + 1, 9, 9);
                    }
                    if (Map[x, y] == true)
                    {
                        drawGraphics.FillRectangle(drawBrushBlack, (x * 10) + 1, (y * 10) + 1, 9, 9);
                    }
                }
            }
        }

        private void ClickCell(object sender, MouseEventArgs e)
        {
            drawGraphics = PictureBoxMap.CreateGraphics();

            int cx = e.Location.X, cy = e.Location.Y;
            AddCell((cx - 1) / 10, (cy - 1) / 10);

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    drawGraphics.FillRectangle(Map[x, y] ? drawBrushBlack : drawBrushWhite, (x * 10) + 1, (y * 10) + 1, 9, 9);
                }
            }
        }

        private void AutoRun(object sender, EventArgs e)
        {
            if (autoRun == true)
            {
                autoRun = false;
            }
            else
            {
                autoRun = true;
                t = new System.Threading.Thread(AutoRunThread);
                t.Start();
            }

        }
        private void AutoRunThread()
        {
            while (autoRun == true)
            {
                MethodInvoker mi = delegate () {
                    NextGen();
                    Draw();
                    Thread.Sleep(100);
                    genCount++;
                    labelGeneration.Text = "Generation: " + Convert.ToString(genCount);
                };
                this.Invoke(mi);
                
            }
        }
        private void DoNothing(){ }
        private void DrawForm_Load(object sender, EventArgs e)
        {}

        private void labelGeneration_Click(object sender, EventArgs e)
        {

        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            this.buttonSet.Image = Properties.Resources.;
        }

        //private void button1_MouseEnter(object sender, EventArgs e)
        //{
        //    this.buttonSet.Image = Properties.Resources._hover;
        //}

        //private void button1_MouseDown(object sender, MouseEventArgs e)
        //{
        //    this.buttonSet.Image = Properties.Resources._clicked;
        //}

        //private void button1_MouseUp(object sender, MouseEventArgs e)
        //{
        //    this.buttonSet.Image = Properties.Resources._default;
        //}
    }
}
