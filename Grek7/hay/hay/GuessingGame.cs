//
// Everyone knows the children's guessing game, "Pick a number between 1 and 100."
// There is an optimal method of guessing in which each guess eliminates half of
// the possible numbers. This module demonstrates how to translate that guessing
// method into a computer algorithm.
//

using System;

namespace hay

{
    class GuessingGame
    {
        public static void Run()
        {
            Console.Write(
                "Hi, let's play a game!\n" +
                "You pick a number between 1 and 100, and I'll try to guess it.\n" +
                "Answer each guess by pressing one of the following keys:\n" +
                "\n" +
                "  g - answer is greater than my guess\n" +
                "  l - answer is less than my guess\n" +
                "  e - answer is equal to my guess\n" +
                "  q - quit\n"
                );

            int minValue = 1;
            int maxValue = 100;

            while (minValue < maxValue)
            {
                int guess = (minValue + maxValue) / 2;

                Console.Write("\n{0}? ", guess);

                var keyChar = Console.ReadKey().KeyChar;

                if (keyChar == 'g')
                {
                    minValue = guess + 1;
                }
                else if (keyChar == 'l')
                {
                    maxValue = guess - 1;
                }
                else if (keyChar == 'e')
                {
                    minValue = guess;
                    maxValue = guess;
                }
                else if (keyChar == 'q' || keyChar == 'x')
                {
                    return;
                }
            }

            Console.WriteLine("\n\nThe answer is {0}!", minValue);
        }
    }
}
