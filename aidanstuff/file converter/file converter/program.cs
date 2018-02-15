﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace File_Converter
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            while (true)
            {
                Console.WriteLine("Enter a command, dir, ftree, mv, c, html\n>");
                string[] input = Console.ReadLine().Split(' ');

                InputSelection(input);
            }
        }


        public static void InputSelection(string[] input)
        {
            switch (input[0])
            {
                case "dir":
                    FileOptions.Kringle(Directory.GetCurrentDirectory());
                    break;
                case "ftree":
                    if (input.Length == 1)
                    {
                        Console.WriteLine("Specify a directory.");
                    }
                    FileOptions.Kringle(input[1]);
                    break;
                case "mv":
                    FileOptions.ChangeFileProperties(2);
                    break;
                case "c":
                    FileOptions.ChangeFileProperties(1);
                    break;
                case "html":
                    //specifying File_Converter Namespace because Convert.cs is also the name of a System.IO class
                    File_Converter.Convert.TextToHtml(input);
                    break;
                case "":
                    Console.WriteLine("Choose a tool.");
                    break;
            }
        }

        public static void Close()
        {
            Console.WriteLine("\nPress Space to Close...");
            if (Console.ReadKey().KeyChar == (char)13)
            {
                return;
            }
        }
    }
}