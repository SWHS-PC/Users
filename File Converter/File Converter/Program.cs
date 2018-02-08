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
        static void main()
        {
            string input = Console.ReadLine();
            if (input == "k wd")
            {
                Kringle(Directory.GetCurrentDirectory());
            }
            else if(input == "k")
            {
                Kringle(input);
            }
            else
            {
                Console.WriteLine("Choose a tool");
            }
        }

        static void ConvertFile(string[] args)
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
            Close();
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
