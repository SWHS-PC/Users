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
            List<string> lines = new List<string>();
            
            lines.Add("                         (@@)  (  )  (@)   ( )  @@    ()   @   o   @   o    @");
            lines.Add("                     (  )  ");
            lines.Add("                 (@@@@)  ");
            lines.Add("              (   )  ");
            lines.Add("           (@@@)");
            lines.Add("       ====        ________                 ___________  ");
            lines.Add("   _D _|  |_______/        \\__I_I_____===__|_________|  ");
            lines.Add("    |(_)---  |   H\\________/ |   |        =|___ ___|    ");
            lines.Add("    /     |  |   H  |  |     |   |         ||_| |_||     ");
            lines.Add("   |      |  |   H  |__--------------------| [___] |     ");
            lines.Add("   | ________|___H__/__|_____/[][]~\\_______|       |    ");
            lines.Add("   |/ |   |-----------I_____I [][] []  D   |=======|     ");
            lines.Add("__ / =| o |=-~~\\  /~~\\  /~~\\  /~~\\ ____Y___________| ");
            lines.Add("  |/-=| ___ |=  ||    ||    ||   | _____ /~\\___/        ");
            lines.Add("   \\_/       \\O=====O=====O=====O/       \\_/          ");


            int x = wx/2, y = (wy/2)-10;       
                                  
            Write(lines, x, y); 

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
                    Write(lines, x, y);
                }
            }
        }

        public static void Write(List<string> lines, int x = 0, int y = 0)
        {
            try
            {
                for (int i = 0; i < wx; i++)
                {
                    Console.Clear();
                    for (int l = 0; l < lines.Count; l++)
                    {
                        Console.SetCursorPosition(x - i, y + l);
                        Console.Write(lines[l]);
                    }
                    Thread.Sleep(50);
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
