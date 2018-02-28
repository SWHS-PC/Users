using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SL
{
    class sl
    {
        public static void Run()
        {
           
            Console.SetBufferSize(wx*2, wy);
            Console.SetWindowPosition(wx / 4, 0);

            Controls();

            Thread.Sleep(50000);
        }
        public static int wx = Console.LargestWindowWidth / 2, wy = Console.LargestWindowHeight / 2;
        public static void Controls()
        {
            List<string> lines = new List<string>();

            lines.Add("                         (@@)  (  )  (@)   ( )  @@    ()   @   o   @   o    @");
            lines.Add("                     (  )  ");
            lines.Add("                 (@@@@)  ");
            lines.Add("              (   )  ");
            lines.Add("           (@@@)                                                                         ");
            lines.Add("       ====        ________                 ___________                                  ");
            lines.Add("   _D _|  |_______/        \\__I_I_____===__|_________|                                  ");
            lines.Add("    |(_)---  |   H\\________/ |   |        =|___ ___|       _________________            ");
            lines.Add("    /     |  |   H  |  |     |   |         ||_| |_||      _|                \\_____A     ");
            lines.Add("   |      |  |   H  |__--------------------| [___] |    =|                        |      ");
            lines.Add("   | ________|___H__/__|_____/[][]~\\_______|       |    -|                        |     ");
            lines.Add("   |/ |   |-----------I_____I [][] []  D   |=======|_____|________________________|_     ");
            lines.Add("__ / =| o |=-~~\\  /~~\\  /~~\\  /~~\\ ____Y___________|__|__________________________|_  ");
            lines.Add("  |/-=| ___ |=  ||    ||    ||    |_____ /~\\___/          |_D__D__D_|  |_D__D__D_|      ");
            lines.Add("   \\_/       \\O=====O=====O=====O/       \\_/               \\_/   \\_/    \\_/   \\_/ ");


            Console.Read();
            int x = wx, y = (wy / 3);

            Write(lines, x, y);

        }

        public static void Write(List<string> lines, int x, int y)
        {
            try
            {
                for (int i = 0; i < x; i++)
                {
                    Console.Clear();
                    for (int l = 0; l < lines.Count; l++)
                    {
                        Console.SetCursorPosition(x - i, y + l);
                        Console.Write(lines[l]);
                    }
                    Thread.Sleep(35);
                }

            }
            catch (Exception e)
            {
                Console.Write(e);
                Thread.Sleep(1000);
            }
        }
    }
}
