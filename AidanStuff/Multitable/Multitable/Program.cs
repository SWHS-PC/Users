using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multitable
{
    class Program
    {
        static void Main(string[] args)
        {

            var a = new int[100];
            for (int i = 0; i < a.Length; i++)
            {
                a[i] = 1 << i;
            }
            foreach (var num in a)
            {
                Console.WriteLine(num);

            }

            
        }
        public static void mtable()
        {
            for (int row = 1; row <= 10; row++)
            {
                for (int col = 1; col <= 10; col++)
                {
                    //alternative option for below, Console.Write("Row{0} Col{1} = {2} ", row, col, row * col);
                    //Console.Write("{0, 4} ", row % col);
                    Console.Write("{0,5:N} ", (float)row / col);

                }
                Console.WriteLine("\n");
            }
        }
    }
}
