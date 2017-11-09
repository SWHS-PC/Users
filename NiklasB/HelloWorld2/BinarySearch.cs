//
// This module demonstrates the binary search algorithm, which is very similar to
// the guessing game algorithm in GuessingGame.cs.
//
// Binary search finds a given value in an array of values (in this case strings).
// The array must be sorted. This enables us to quickly narrow down the choices
// by eliminating half of the array with each iteration of the loop.
//
// This module also demonstrates the use of methods. The binary search algorithm
// is implemented by a method named FindString. We say the method "encapsulates"
// the algorithm because the method hides the complexity of the algorithm behind
// a simplified interface (i.e., the method's name, parameters, and return value).
//

using System;

namespace HelloWorld
{
    class BinarySearch
    {
        public static void Run()
        {
            Console.WriteLine("Enter a name and I'll tell you if it's a fruit.");
            Console.WriteLine("Press ENTER without typing a name to exit.");

            for (;;)
            {
                // Display a prompt and read a string from the console.
                Console.Write("\n> ");
                string name = Console.ReadLine();

                // Exit the loop if the string is empty.
                if (name.Length == 0)
                    break;

                // Call the FindString method, which implements the binary search algorithm.
                // The m_fruits member variable is a sorted (i.e., alphabetical) array of fruit names.
                int matchIndex;
                if (FindString(name, m_fruits, out matchIndex))
                {
                    Console.WriteLine("{0} is fruit #{1}.", name, matchIndex);
                }
                else
                {
                    Console.WriteLine("{0} is not a fruit, but would be at index {1}.", name, matchIndex);
                }
            }
        }

        /// <summary>
        /// FindString finds the specified string in a sorted array of strings using the binary
        /// search algorithm.
        /// </summary>
        /// <param name="value">String to find.</param>
        /// <param name="sortedStrings">Sorted array of strings to search.</param>
        /// <param name="matchIndex">Output parameter that receives the index of the specified
        /// string in the array if the return value is true. If the return value is false, this
        /// receives the index where the string would have been found.</param>
        /// <returns>Returns true if the specified string is found, or false if not.</returns>
        public static bool FindString(string value, string[] sortedStrings, out int matchIndex)
        {
            // Range of potential matches within sortedStrings.
            // Initially, this is the entire array.
            int beginIndex = 0;                     // index of first potential match
            int endIndex = sortedStrings.Length;    // index one past the last potential match

            // Search for the specified value by repeatedly narrowing the range of potential
            // matches until we find a match or the range is empty.
            while (beginIndex < endIndex) 
            {
                // Compare the given value with the one in the middle of the range.
                int middleIndex = (beginIndex + endIndex) / 2;
                string middleValue = sortedStrings[middleIndex];
                int comparison = string.Compare(value, middleValue);

                if (comparison > 0)
                {
                    // The given value is greater than middleValue.
                    // Narrow the range to only include strings after middleIndex.
                    beginIndex = middleIndex + 1;
                }
                else if (comparison < 0)
                {
                    // The given value is less than middleValue.
                    // Narrow the range to only include strings before middleIndex.
                    endIndex = middleIndex;
                }
                else
                {
                    // We found the match.
                    // Store its index in the output parameter and return true.
                    matchIndex = middleIndex;
                    return true;
                }
            }

            // The range of possible matches is now empty, so there is no match.
            // Store the index where the string *would* be in the output parameter,
            // and return false.
            matchIndex = beginIndex;
            return false;
        }

        // Sorted (i.e., alphabetical) array of fruit names.
        static readonly string[] m_fruits = new string[]
        {
            "apple",
            "apricot",
            "banana",
            "barberry",
            "blackberry",
            "blueberry",
            "boysenberry",
            "cantaloupe",
            "cherry",
            "cranberry",
            "date",
            "fig",
            "grape",
            "grapefruit",
            "honeydew",
            "kiwi",
            "lemon",
            "lime",
            "mango",
            "nectarine",
            "orange",
            "peach",
            "pear",
            "pineapple",
            "plum",
            "raspberry",
            "strawberry",
            "tomato",
            "watermelon"
        };
    }
}
