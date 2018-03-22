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

namespace LifeGui
{
    public partial class DrawForm : Form
    {
        System.Drawing.Graphics drawGraphics;
        System.Drawing.Pen drawPenBlack = new System.Drawing.Pen(Color.Black, 0.5F);
        System.Drawing.SolidBrush drawBrushBlack = new System.Drawing.SolidBrush(Color.Black);
        System.Drawing.SolidBrush drawBrushWhite = new System.Drawing.SolidBrush(Color.White);

        public static int w = 75, h = 25;
        public static bool[,] Map;
        public static bool[,] newMap;
        public bool this[int x, int y]
        {
            get { return Map[x, y]; }
            set { Map[x, y] = value; }
        }
        public bool clicked;


        public DrawForm()
        {
            InitializeComponent();

            Map = new bool[w, h];
            newMap = new bool[w, h];
            AddCell(w / 2 - 1, h / 2);
            AddCell(w / 2, h / 2);
            AddCell(w / 2 + 1, h / 2);
            AddCell(w / 2, h / 2 - 1);
            AddCell(w / 2 - 1, h / 2 + 1);
            AddCell(w / 2 + 1, h / 2 + 1);
            AddCell(w / 2, h / 2 + 2);
            
        }

        public void NextGen()
        {
            newMap = new bool[w, h];
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    int numberOfNeighbors = IsNeighbor(Map, i, j, -1, 0)
                     + IsNeighbor(Map, i, j, -1, 1)
                     + IsNeighbor(Map, i, j, 0, 1)
                     + IsNeighbor(Map, i, j, 1, 1)
                     + IsNeighbor(Map, i, j, 1, 0)
                     + IsNeighbor(Map, i, j, 1, -1)
                     + IsNeighbor(Map, i, j, 0, -1)
                     + IsNeighbor(Map, i, j, -1, -1);

                    bool shouldLive = false;
                    bool isAlive = Map[i, j];

                    if (isAlive && (numberOfNeighbors == 2 || numberOfNeighbors == 3))
                    {
                        shouldLive = true;
                    }
                    else if (!isAlive && numberOfNeighbors == 3)
                    {
                        shouldLive = true;
                    }

                    newMap[i, j] = shouldLive;
                }
            }
            Map = newMap;
        }

        static int IsNeighbor(bool[,] Map, int x, int y, int offsetx, int offsety)
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

        private void DrawForm_Load(object sender, EventArgs e)
        {
            
        }

        private void DrawClick(object sender, EventArgs e)
        {
            drawGraphics = PictureBox1.CreateGraphics();

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    drawGraphics.DrawRectangle(drawPenBlack, x * 10, y * 10, 10, 10);
                }
            }
        }

        

        private void DrawNext(object sender, EventArgs e)
        {
            Draw();
            NextGen();
        }

        private void Draw()
        {
            drawGraphics = PictureBox1.CreateGraphics();
            
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    drawGraphics.FillRectangle(Map[x, y] ? drawBrushBlack : drawBrushWhite, (x * 10) + 1, (y * 10) + 1, 9, 9);
                }
            }
        }
    }
}
