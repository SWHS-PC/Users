using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 1; i <= 10; i++)
            {
                Console.WriteLine("Hello");
            }

            Console.WriteLine("\n\nPress Enter to Close");
            if (Console.ReadKey().KeyChar == (char)13)
            {
                return;
            }
        }
    }
}
