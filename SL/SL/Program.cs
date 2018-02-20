using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SL
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            
            Controls();
        }
        public static void Controls()
        {
            const string line1 = "*";
            const string line2 = "*";
            const string line3 = " ==== ________                ___________ ";
            const string line4 = "  _D _|  |_______/        \\__I_I_____===__|_________| ";
            const string line5 = "   |(_)---  |   H\\________/ |   |        =|___ ___|   ";
            const string line6 = "   /     |  |   H  |  |     |   |         ||_| |_||   ";
            const string line7 = "  |      |  |   H  |__--------------------| [___] |   ";
            const string line8 = "  | ________|___H__/__|_____/[][]~\\_______|       |   ";
            const string line9 = "  |/ |   |-----------I_____I [][] []  D   |=======|__ ";


            int x = 0, y = 0;

            Write(line1, line2);

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    var command = Console.ReadKey().Key;

                    switch (command)
                    {
                        case ConsoleKey.DownArrow:
                            y++;
                            break;
                        case ConsoleKey.UpArrow:
                            if (y > 0)
                            {
                                y--;
                            }
                            break;
                        case ConsoleKey.LeftArrow:
                            if (x > 0)
                            {
                                x--;
                            }
                            break;
                        case ConsoleKey.RightArrow:
                            x++;
                            break;
                    }
                    Write(line1, line2, x, y);
                }
                //else
                //{
                //    Thread.Sleep(100);
                //}
            }
        }

        public static void Write(string line1, string line2, int x = 0, int y = 0)
        {
            try
            {
                
                Console.Clear();

                Console.SetCursorPosition(x, y);
                Console.Write(line1);

                Console.SetCursorPosition(x, y+1);
                Console.Write(line2);
            }
            catch (Exception)
            {
            }
        }
    }
}
