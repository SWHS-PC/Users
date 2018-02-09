using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace File_Converter
{
    public class FileOptions
    {
        
        //Change file ending/convert file
        public static void ChangeFileProperties(int prop)
        {
            Console.WriteLine("Enter File Name or Path.");
            Console.Write("> ");

            string fileEntered = Console.ReadLine();
            string fileType = Path.GetExtension(fileEntered);
            string filePath = Path.GetDirectoryName(Directory.GetCurrentDirectory() + "\\" + fileEntered);
            string filePathFull = Directory.GetCurrentDirectory() + "\\" + fileEntered;

            Console.WriteLine("{0}\n{1}\n{2}\n{3}",fileEntered, fileType, filePath, filePathFull);
            switch (prop)
            {
                case 1:
                    Console.WriteLine("Path to: {0} \nThe File Type is: {1} \n\nEnter File type to convert to.", filePath, fileType);
                    string newFileType = Console.ReadLine();
                    string newFile = Path.ChangeExtension(fileEntered, newFileType);
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    File.Move(fileEntered, newFile);
                    Console.WriteLine("{0} converted to type {1}, new file: {2}", fileEntered, newFileType, newFile);
                    break;
                case 2:
                    Console.WriteLine("Path to: {0} \nThe File Type is: {1} \n\nEnter File type to convert to.", filePath, fileType);
                    string newFileName = Console.ReadLine();
                    File.Move(fileEntered, newFileName);
                    Console.WriteLine("{0} renamed to {1}", fileEntered, newFileName);
                    break;
            }
            
            //Close();
        }
        
        
        //print dirtree
        public static void Kringle(string subDirectory)
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
    
    }
}