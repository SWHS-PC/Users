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
                        case "dir":
                            FileOptions.Kringle(Directory.GetCurrentDirectory());
                            break;
                        case "ftree":
                            FileOptions.Kringle(input[1]);
                            break;
                        case "mv":
                            FileOptions.ChangeFileProperties(2);
                            break;
                    }
                }

                else if (input[0] == "c")
                {

                    FileOptions.ChangeFileProperties(1);
                }
                else if (input[0] == "html" || input[0] == "HTML")
                {
                    File_Converter.Convert.TextToHtml(input);
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
