using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aidan___Fizzbuzz
{
    class FizzBuzz
    {
    
        [Flags]
        enum FizzFlags
        {
            None = 0,
            // Define Fizz, Buzz, and Boom flags as different powers of 2.
            // Because they are different powers of 2 they correspond to
            // different bits (i.e., binary digits), so each flag can be set
            // without affecting the other flags.
            Fizz = 0x01,
            Buzz = 0x02,
            Boom = 0x04,
            // Defined named values for combinations of two or more flags.
            FizzBuzz = Fizz | Buzz,
            FizzBoom = Fizz | Boom,
            BuzzBoom = Buzz | Boom,
            FizzBuzzBoom = Fizz | Buzz | Boom
        }

        static void Main(string[] args)
        {
            for (int i = 1; i <= 105; i++)
            {
                FizzFlags flags = 0;
 
                // Use the bitwise OR operator to set the individual bits of the flags
                // variable based on the value of i.
                if (i % 3 == 0)
                {
                    flags |= FizzFlags.Fizz;
                }
                if (i % 5 == 0)
                {
                    flags |= FizzFlags.Buzz;
                }
                if (i % 7 == 0)
                {
                    flags |= FizzFlags.Boom;
                }
                // At this point the value of flags is the combination of the bits we
                // set. For example, if i is divisible by both 3 and 5 then we've set both
                // the Fizz and Buzz bits, so flags is the combination (bitwise OR) of
                // those, namely FizzBuzz.
                switch (flags)
                {
                    case FizzFlags.None:
                    //I would write none here except i want it to print the number if its not divisible by those numbers
                        Console.WriteLine(i);
                        break;

                    case FizzFlags.Fizz:
                        Console.WriteLine("Fizz");
                        break;
 
                    case FizzFlags.Buzz:
                        Console.WriteLine("Buzz");
                        break;

                    case FizzFlags.Boom:
                        Console.WriteLine("Boom");
                        break;

                    case FizzFlags.FizzBuzz:
                        Console.WriteLine("FizzBuzz");
                        break;

                    case FizzFlags.FizzBoom:
                        Console.WriteLine("FizzBoom");
                        break;

                    case FizzFlags.FizzBuzzBoom:
                        Console.WriteLine("FizzBuzzBoom");
                        break;
                }
            }
        // public static void Run()
        // {
        //     //uses a for loop to loop i from 1 to 105
        //     for (int i = 1; i <= 105; i++)
        //     {
        //         //boolean variables for i % 3, 5, and 7, % meaning remainder of i divided by 3 equalling 0
        //         bool fizz = i % 3 == 0;
        //         bool buzz = i % 5 == 0;
        //         bool boom = i % 7 == 0;

        //         //if statements where it writes Fizz, Buzz and Boom for the divisions of the i var
        //         if (fizz)
        //             Console.WriteLine("Fizz");
        //         else if (buzz)
        //             Console.WriteLine("Buzz");
        //         else if (boom)
        //             Console.WriteLine("Boom");
        //         //for where i equals more than one of the boolean variables
        //         else if (fizz && buzz)
        //             Console.WriteLine("FizzBuzz");
        //         else if (fizz && boom)
        //             Console.WriteLine("Fizz...Boom");
        //         else if (fizz && buzz && boom)
        //             Console.WriteLine("FizzBuzzBoom");
        //         //105 is the only instance of FizzBuzzBoom
        //         //writes the i variable if the remainder isnt equal to the booleans
        //         else
        //             Console.WriteLine(i);
        //     }
        //     //allowing user to close program upon pressing enter
        //     Console.WriteLine("\r\nPress Enter to Close");
        //     //(char)13 is the enter key
        //     if (Console.ReadKey().KeyChar == (char)13)
        //     {
        //         return;
        //     }
        // }
    }
}
