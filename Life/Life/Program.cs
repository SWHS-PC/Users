using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life
{
    class Program
    {
        public static int w = Console.LargestWindowWidth / 2, h = Console.LargestWindowHeight / 2;
        public static char cell = (char)183;
        public static int[] Neighbours;
        

        static void Main(string[] args)
        {
            Console.SetWindowSize(w, h);
            Console.SetWindowPosition(0, 0);
            Console.CursorVisible = false;
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear();



            Neighbours = { 0, 0, 0, 0, 0, 0, 0, 0 };
            //TopLeft Top TopRight Left Right  BottomLeft Bottom BottomRight

            int x = 0;
            Console.SetCursorPosition((Console.WindowWidth / 2), (Console.WindowHeight / 2));
            Console.Write(cell);

            while (true)
            {
                Draw(x);
                Console.Read();
                x++;
            }
            
        }
        public static void Draw(int it)
        {
            
                Console.SetCursorPosition((Console.WindowWidth / 2)-it, (Console.WindowHeight / 2));
                Console.Write(cell);
            
        }
    }
}
