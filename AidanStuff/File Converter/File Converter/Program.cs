using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.IO;
using System.Windows.Forms;

namespace File_Converter
{
    class Program
    {
        [STAThread]

        static void Main(string[] args)
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
            FileOptions newFO = new FileOptions();
            switch (input[0])
            {

                //print current directory
                case "dir":
                    FileOptions.Kringle(Directory.GetCurrentDirectory());
                    break;
                //print directory tree

                case "ftree":
                    if (input.Length == 1)
                    {
                        Console.WriteLine("Specify a directory.");
                    }
                    FileOptions.Kringle(input[1]);
                    break;

                //change file name
                case "mv":
                    newFO.ChangeFileProperties(2);
                    break;
                //change file type
                case "c":
                    newFO.ChangeFileProperties(1);
                    break;
                //convert .txt/blank file to html

                case "html":
                    //specifying File_Converter Namespace because Convert.cs is also the name of a System.IO class
                    File_Converter.Convert.TextToHtml(input);
                    break;

                //blank entry catching
                case "":
                    Console.WriteLine("Choose a tool.");
                    break;
                //create the god mode folder on the user desktop
                case "GM":
                    FileOptions.createGodModeFolder();
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
