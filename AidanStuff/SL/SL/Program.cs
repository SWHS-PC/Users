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
            
            Console.SetWindowSize(wx, wy);
            
            Controls();
        }
        public static int wx = Console.LargestWindowWidth / 2, wy = Console.LargestWindowHeight / 2;
        public static void Controls()
        {
            const string s6 =    "                         (@@)  (  )  (@)   ( )  @@    ()   @   o   @   o    @";
            const string s5 =    "                     (  )  ";
            const string s4 =    "                 (@@@@)  ";
            const string s3 =    "              (   )  ";
            const string s2 =    " ";
            const string s1 =    "           (@@@)";
            const string line3 = "       ====        ________                 ___________ ";
            const string line4 = "   _D _|  |_______/        \\__I_I_____===__|_________|  ";
            const string line5 = "    |(_)---  |   H\\________/ |   |        =|___ ___|    ";
            const string line6 = "    /     |  |   H  |  |     |   |         ||_| |_||    ";
            const string line7 = "   |      |  |   H  |__--------------------| [___] |    ";
            const string line8 = "   | ________|___H__/__|_____/[][]~\\_______|       |    ";
            const string line9 = "   |/ |   |-----------I_____I [][] []  D   |=======|    ";
            const string linea = " __/ =| o |=-~~\\  /~~\\  /~~\\  /~~\\ ____Y___________|    ";
            const string lineb = "  |/-=| ___ |=  ||    ||    ||   | _____ /~\\___/        ";
            const string linec = "   \\_/       \\O=====O=====O=====O/       \\_/            ";


            int x = wx/2, y = (wy/2)-10;

            Write(s6, s5, s4, s3, s2, s1, line3, line4, line5, line6, line7, line8, line9, linea, lineb, linec, x, y);

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
                    Write(s6, s5, s4, s3, s2, s1, line3, line4, line5, line6, line7, line8, line9, linea, lineb, linec, x, y);
                }
                //else
                //{
                //    Thread.Sleep(100);
                //}
            }
        }

        public static void Write(string s6, string s5, string s4, string s3, string s2, string s1, string line3, string line4, string line5, string line6, string line7, string line8, string line9, string linea, string lineb, string linec, int x = 0, int y = 0)
        {
            try
            {
                for (int i = 0; i < wx; i++)
                {

                    Console.Clear();

                    Console.SetCursorPosition(x, y);
                    Console.Write(s6);

                    Console.SetCursorPosition(x, y + 1);
                    Console.Write(s5);

                    Console.SetCursorPosition(x, y + 2);
                    Console.Write(s4);

                    Console.SetCursorPosition(x, y + 3);
                    Console.Write(s3);

                    Console.SetCursorPosition(x, y + 4);
                    Console.Write(s2);

                    Console.SetCursorPosition(x, y + 5);
                    Console.Write(s1);

                    Console.SetCursorPosition(x, y + 6);
                    Console.Write(line3);

                    Console.SetCursorPosition(x, y + 7);
                    Console.Write(line4);

                    Console.SetCursorPosition(x, y + 8);
                    Console.Write(line5);

                    Console.SetCursorPosition(x, y + 9);
                    Console.Write(line6);

                    Console.SetCursorPosition(x, y + 10);
                    Console.Write(line7);

                    Console.SetCursorPosition(x, y + 11);
                    Console.Write(line8);

                    Console.SetCursorPosition(x, y + 12);
                    Console.Write(line9);

                    Console.SetCursorPosition(x, y + 13);
                    Console.Write(linea);

                    Console.SetCursorPosition(x, y + 14);
                    Console.Write(lineb);

                    Console.SetCursorPosition(x, y + 15);
                    Console.Write(linec);
                    Thread.Sleep(25);
                    x--;
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
