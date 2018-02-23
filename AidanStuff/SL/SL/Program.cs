using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SL
{
    class Program
    {

        public static int wx = Console.BufferWidth = Console.LargestWindowWidth / 2, wy = Console.BufferHeight = Console.LargestWindowHeight / 2;
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(wx, wy);
            Console.SetWindowPosition(0, 0);
            Console.WindowLeft = Console.WindowTop = 0;

            Console.WriteLine("Enter a command, sl or matrix>");
            string input = Console.ReadLine();
            switch (input)
            {
                case "sl":
                    sl.Run();
                    break;
                case "matrix":
                    matrix.Run();
                    break;
            }
        }
    }
}
