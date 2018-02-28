using System;
using System.IO;

namespace TimesTable
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                // No command line argument, so write to the console.
                WriteTables(Console.Out);
            }
            else
            {
                // At least one command line argument. Assume the first
                // argument is a file name and create a stream writer for it.
                using (var output = new StreamWriter(args[0]))
                {
                    // Write the tables to the file.
                    WriteTables(output);
                }
            }
        }

        static void WriteTables(TextWriter output)
        {
            // Write a decimal multiplication table (decimal format string and radix 10).
            WriteTimesTable("{0,4}", 10, output);

            // Write a hexadecimal multiplication table (hexadecimal format string and radix 16).
            WriteTimesTable("{0,4:x}", 16, output);
        }


        static void WriteTimesTable(
            string formatString, 
            int radix, 
            TextWriter output
            )
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
