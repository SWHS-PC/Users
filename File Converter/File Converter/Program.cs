using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace File_Converter
{
    class Program
    {
        static void Main()
        {
            while (true)
            {
                Console.WriteLine("Enter a command(k for Kringle: wd, f | c for Convert File)\n>");
                string userInput = Console.ReadLine();
                string[] input = userInput.Split(' ');

                if (input[0] == "k" && input.Length > 1)
                {
                    switch (input[1])
                    {
                        case "wd":
                            FileOptions.Kringle(Directory.GetCurrentDirectory());
                            break;
                        case "f":
                            FileOptions.Kringle(input[1]);
                            break;
                    }
                }
                else if(input[0] == "c")
                {
                    FileOptions.ChangeFileEnding();
                }
                else
                {
                    Console.WriteLine("Choose a tool.");
                }
            }
        }


        //public static void Close()
        //{
        //    Console.WriteLine("\nPress Space to Close...");
        //    if (Console.ReadKey().KeyChar == (char)13)
        //    {
        //        return;
        //    }
        //}
    }
}
