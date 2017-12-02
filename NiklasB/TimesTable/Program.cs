using System;

namespace TimesTable
{
    class Program
    {
        // This program writes the following multiplication table
        // to the console output:
        //
        //  1   2   3   4   5   6   7   8   9
        //  2   4   6   8  10  12  14  16  18
        //  3   6   9  12  15  18  21  24  27
        //  4   8  12  16  20  24  28  32  36
        //  5  10  15  20  25  30  35  40  45
        //  6  12  18  24  30  36  42  48  54
        //  7  14  21  28  35  42  49  56  63
        //  8  16  24  32  40  48  56  64  72
        //  9  18  27  36  45  54  63  72  81

        static void Main(string[] args)
        {
            // The table has nine rows, and each row has nine columns.
            // We will write it using two nested 'for' loops.
            //
            // The outer loop executes once per row. Its for statement
            // declares a 'row' variable which is initialized to 1 before
            // the first iteration and incremented after each iteration.
            // The loop executes nine times with 'row' taking the values
            // 1 through 9.

            for (int row = 1; row <= 9; row++)
            {
                // The inner loop executes once for each column of each
                // row. This loop also executes nine times (per row) with
                // the 'col' variable taking the values 1 through 9.

                for (int col = 1; col <= 9; col++)
                {
                    // We can now use the values of the 'row' and 'col' 
                    // variables to compute the value for the current cell.
                    // For example, if we're in the third column of the
                    // second row then col=3 and row=2, so the value to
                    // be written is the product (i.e., 6).

                    // We convert the value from a number to a string before
                    // writing it a bit of special syntax. If a string literal
                    // has a $ before the opening quotation mark, then the
                    // string can have expressions enclosed in curly braces.
                    // At run time, the expression and the enclosing braces
                    // are replaced with the computed value of the expression.
                    // In this case, the value of row * col is computed and
                    // converted to a string of decimal digits.
                    //
                    // The ",3" after the expression means we want the converted
                    // string to be at least three characters in length. If the
                    // value of row * col is less than three decimal digits then
                    // it is padded with spaces. This ensures that all the
                    // columns line up.

                    Console.Write($"{row * col,3} ");
                }

                // After writing all the columns of a row, use Console.WriteLine
                // to advance to a new line. Note that we used Console.Write instead
                // of Console.WriteLine above. That's so all nine columns of the same
                // row appear on one line.
                Console.WriteLine();
            }
        }
    }
}
