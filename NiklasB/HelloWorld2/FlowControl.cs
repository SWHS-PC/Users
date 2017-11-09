//
// This module demonstrates statements that control the flow of execution.
// These include:
//   * Branch statements: if and switch.
//   * Loop statements: for, while, do..while, and foreach.
//

using System;

namespace HelloWorld
{
    class FlowControl
    {
        public static void Run()
        {
            ForLoop();
            Console.WriteLine();

            WhileLoop();
            Console.WriteLine();

            DoWhileLoop();
            Console.WriteLine();

            ForEachLoop();
            Console.WriteLine();

            ForLoopWithNestedIf();
            Console.WriteLine();

            ForLoopWithNestedSwitch();
            Console.WriteLine();

            ForEachLoopWithBreak();
            Console.WriteLine();
        }

        static void ForLoop()
        {
            Console.Write("For loop:");

            // A for statment has three parts separated by semicolon:
            //  1. An initialization statement, executed once at the beginning.
            //  2. A test expression, evaluated before each iteration.
            //  3. An increment/decrement expression, evaluated after each iteration.
            for (int i = 0; i < 5; ++i)
            {
                Console.Write(" {0}", i);
            }

            Console.WriteLine();
        }

        static void WhileLoop()
        {
            Console.Write("While loop:");

            // A while statement just has a test expression. The test expression
            // is evaluated before each iteration, and the loop continues to execute
            // as long as the test expression evaluates to true.
            //
            // The following code is exactly equivalent to the for loop in the
            // ForLoop method. The initialization statement and increment are just
            // written as separate statements.
            int i = 0;
            while (i < 5)
            {
                Console.Write(" {0}", i);
                ++i;
            }
            Console.WriteLine();
        }

        static void DoWhileLoop()
        {
            Console.Write("Do..while loop:");

            // A do..while loop is similar to a while loop except that the test
            // expression is written at the end, and is not evaluated before the
            // first iteration of the loop. As a result, the body of a do..while
            // loop is always executed at least once.
            int i = 0;
            do
            {
                Console.Write(" {0}", i);
                ++i;
            } while (i < 5);

            Console.WriteLine();
        }

        static void ForEachLoop()
        {
            Console.Write("Foreach loop:");

            // A foreach loop executes once for each element in an array, collection,
            // or anything else that can be enumerated. To demonstrate, let's define
            // an array of integers containing the first five primes.
            int[] primes = new int[] { 2, 3, 5, 7, 11 };

            // In the following loop, the loop body executes once for each element in
            // the primes array. Within the loop body, the loop variable 'n' takes the
            // value of the current array element.
            foreach (var n in primes)
            {
                Console.Write(" {0}", n);
            }

            Console.WriteLine();
        }

        static void ForLoopWithNestedIf()
        {
            Console.WriteLine("For loop with nested if:");

            for (int n = 1; n <= 4; ++n)
            {
                if (n % 2 == 0)
                {
                    Console.WriteLine("{0} is even.", n);
                }
                else
                {
                    Console.WriteLine("{0} is odd.", n);
                }
            }
        }

        static void ForLoopWithNestedSwitch()
        {
            Console.WriteLine("For loop with nested switch:");
            for (int n = 1; n <= 4; ++n)
            {
                switch (n % 2)
                {
                    case 0:
                        Console.WriteLine("{0} is even.", n);
                        break;

                    case 1:
                        Console.WriteLine("{0} is odd.", n);
                        break;

                    default:
                        Console.WriteLine("{0} is neither even nor odd. How is that possible?", n);
                        break;
                }
            }
        }

        static void ForEachLoopWithBreak()
        {
            Console.WriteLine("Foreach loop with break:");

            // This method demonstrates the break statement, which can be used within the
            // body of any kind of loop to immediately exit the loop.

            // We will use a foreach loop to scan the following sentence for repeated words.
            string sentence = "the child played in the the rain rain";

            // Keep track of the previous word; initially there is none.
            string previousWord = null;

            // Split the sentence into an array of words, and loop over that.
            foreach (var word in sentence.Split())
            {
                // Does the current word differ from the previous word?
                if (word != previousWord)
                {
                    // Output the non-repeated word.
                    Console.WriteLine("{0}", word);

                    // The current word will be the previous word for the next iteration.
                    previousWord = word;
                }
                else
                {
                    // Output the repeated word.
                    Console.WriteLine("{0} <-- repeated", word);

                    // Break out of the loop without checking for any more repeated words.
                    break;
                }
            }
        }
    }
}
