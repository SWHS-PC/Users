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
            Console.SetBufferSize(wx*2, wy);
            Console.SetWindowPosition(wx / 4, 0);

            Controls();
            Thread.Sleep(500);

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


            Console.Read();
            int x = wx, y = (wy/3);       
                                  
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
