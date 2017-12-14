using System;
using System.IO;

namespace TimesTable
{
    class Program
    {
        static void Main(string[] args)
        {
            // Write a decimal multiplication table (decimal format string and radix 10).
            WriteTimesTable("{0,4}", 10, Console.Out);

            // Write a hexadecimal multiplication table (hexadecimal format string and radix 16).
            WriteTimesTable("{0,4:x}", 16, Console.Out);
        }


        static void WriteTimesTable(string formatString, int radix, TextWriter output)
        {
            // Outer loop executes once for each row.
            for (int row = 1; row < radix; row++)
            {
                // Inner loop executes once for each column in the current row.
                for (int col = 1; col < radix; col++)
                {
                    // Output the value for this table cell using the specified format string.
                    output.Write(formatString, row * col);
                }

                // Begin a new line at the end of the row.
                output.WriteLine();
            }

            // Output a blank line at the end of the table.
            output.WriteLine();
        }
    }
}
