using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factor
{
    class Program
    {
        static void Main(string[] args)
        {
            uint value = 0;
            if (args.Length == 1 && uint.TryParse(args[0], out value))
            {
                Factor(value);
            }
            else
            {
                Console.WriteLine("Factor <number>");
            }
        }

        static void Factor(uint value)
        {
            bool haveFactors = false;

            for (uint factor = 2;               // start with 2
                factor * factor <= value;       // continue while factor <= sqrt(value)
                factor += 1 + (factor & 1))     // add 1 to factor if it's even, 2 if it's odd
            {
                // Is the value divisible by factor?
                if (value % factor == 0)
                {
                    // Divide value by factor as many times as we can evenly,
                    // and keep track of the count.
                    uint count = 0;
                    do
                    {
                        value /= factor;
                        ++count;
                    } while (value % factor == 0);

                    // Precede factors after the first with *.
                    if (haveFactors)
                    {
                        Console.Write(" * ");
                    }
                    haveFactors = true;

                    // Output the factor.
                    Console.Write(factor);

                    // If we divided more than once, the count is the exponent.
                    if (count > 1)
                    {
                        Console.Write($"^{count}");
                    }
                }
            }

            if (!haveFactors)
            {
                Console.Write(value);
            }
            else if (value > 1)
            {
                Console.Write($" * {value}");
            }

            Console.WriteLine();
        }
    }
}
