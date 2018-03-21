using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Life
{
    class Program
    {
        public static int w = Console.LargestWindowWidth / 2, h = Console.LargestWindowHeight / 2;
        public static char cell = (char)183;
        public static bool[,] Map;
        public static bool[,] newMap;

        public bool this[int x, int y]
        {
            get { return Map[x, y]; }
            set { Map[x, y] = value; }
        }

        static void Main(string[] args)
        {
            Program link = new Program();
            Console.SetWindowSize(w, h);
            Console.SetWindowPosition(0, 0);
            Console.CursorVisible = false;
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear();

            Map = new bool[w, h];
            newMap = new bool[w, h];
            link.AddCell(w / 2 - 1, h / 2);
            link.AddCell(w / 2, h / 2);
            link.AddCell(w / 2 + 1, h / 2);
            link.AddCell(w / 2, h / 2 - 1);
            link.AddCell(w / 2 - 1, h / 2 + 1);
            link.AddCell(w / 2 + 1, h / 2 + 1);
            link.AddCell(w / 2, h / 2 + 2);








            while (true)
            {
                link.Draw();
                link.NextGen();
                Console.ReadLine();
                Console.Clear();

            }
        }

        public bool AddCell(int x, int y)
        {
            bool currentValue = Map[x, y];
            return Map[x, y] = !currentValue;
        }

        public void Draw()
        {
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    Console.Write(Map[x, y] ? Convert.ToString(cell) : " ");
                }
            }  
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
    }
}
