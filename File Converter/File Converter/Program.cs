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
                            Kringle(Directory.GetCurrentDirectory());
                            break;
                        case "f":
                            Kringle(input[1]);
                            break;
                    }
                }
                else if(input[0] == "c")
                {
                    ConvertFile();
                }
                else
                {
                    Console.WriteLine("Choose a tool.");
                }
            }
        }

        static void ConvertFile()
        {
            Console.WriteLine("Enter File Name or Path.");
            Console.Write("> ");

            string fileEntered = Console.ReadLine();

            string fileType = Path.GetExtension(fileEntered);
            string filePath = Directory.GetCurrentDirectory() + "\\" + fileEntered;

            Console.WriteLine("Path to: {0} \nThe File Type is: {1} \n\nEnter File type to convert to.", filePath, fileType);  

            string newFileType = Console.ReadLine();

            File.Move(fileEntered, Path.ChangeExtension(fileEntered, newFileType));
            string newFile = Path.GetFileNameWithoutExtension(fileEntered) + "." + newFileType;
            Console.WriteLine("{0} converted to type {1}, new file: {2}", fileEntered, newFileType, newFile);
            //Close();
        }

        static void Kringle(string subDirectory)
        {
            var dirInfo = new DirectoryInfo(subDirectory);
            if (dirInfo.Attributes.HasFlag(FileAttributes.Hidden))
                return;

            foreach (string currentFiles in Directory.EnumerateFiles(subDirectory))
            {
                Console.WriteLine(currentFiles);
            }
            foreach (string dir in Directory.EnumerateDirectories(subDirectory))
            {
                Kringle(dir);
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
