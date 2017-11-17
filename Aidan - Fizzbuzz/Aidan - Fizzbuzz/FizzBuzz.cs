﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aidan___Fizzbuzz
{
    class FizzBuzz
    {
        public static void Run()
        {
            //uses a for loop to loop i from 1 to 105
            for (int i = 1; i <= 105; i++)
            {
                //boolean variables for i % 3, 5, and 7, % meaning remainder of i divided by 3 equalling 0
                bool fizz = i % 3 == 0;
                bool buzz = i % 5 == 0;
                bool boom = i % 7 == 0;

                //if statements where it writes Fizz, Buzz and Boom for the divisions of the i var
                if (fizz)
                    Console.WriteLine("Fizz");
                else if (buzz)
                    Console.WriteLine("Buzz");
                else if (boom)
                    Console.WriteLine("Boom");
                //for where i equals more than one of the boolean variables
                else if (fizz && buzz)
                    Console.WriteLine("FizzBuzz");
                else if (fizz && boom)
                    Console.WriteLine("Fizz...Boom");
                else if (fizz && buzz && boom)
                    Console.WriteLine("FizzBuzzBoom");
                //105 is the only instance of FizzBuzzBoom
                //writes the i variable if the remainder isnt equal to the booleans
                else
                    Console.WriteLine(i);
            }
            //allowing user to close program upon pressing enter
            Console.WriteLine("\r\nPress Enter to Close");
            //(char)13 is the enter key
            if (Console.ReadKey().KeyChar == (char)13)
            {
                return;
            }
        }
    }
}
