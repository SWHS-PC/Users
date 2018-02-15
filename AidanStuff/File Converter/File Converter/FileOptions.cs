using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Windows.Forms;

namespace File_Converter
{
    public class FileOptions
    {
        string fileEntered { get; set; }
        public void getFile()
        {  
            OpenFileDialog FileD = new OpenFileDialog();
            FileD.FilterIndex = 2;
            FileD.RestoreDirectory = true;

            
            if (FileD.ShowDialog() == DialogResult.OK)
            {
                fileEntered = FileD.FileName;
                return;
            }
        }
        public void getFolder()
        {
            FolderBrowserDialog FolderD = new FolderBrowserDialog();

            if (FolderD.ShowDialog() == DialogResult.OK)
            {
                fileEntered = FolderD.SelectedPath;
                return;
            }
        }

        //Change file ending/convert file
        public void ChangeFileProperties(int prop)
        {
            getFile();

            //debugPrint(1);//Console.WriteLine(fileEntered);

            string fileType = Path.GetExtension(fileEntered);
            string fileDirectoryPath = Path.GetDirectoryName(fileEntered);
            
            //debugPrint(2);// Console.WriteLine("{0}\n{1}",fileEntered, fileDirectoryPath);
            switch (prop)
            {
                case 1:
                    Console.WriteLine("Path to: {0} \nThe File Type is: {1} \n\nEnter File type to convert to.", fileEntered, fileType);
                    string newFileType = Console.ReadLine();
                    string newFile = Path.ChangeExtension(fileEntered, newFileType);
                    FileSystem.CopyDirectory(fileEntered, newFile, UIOption.AllDialogs);
                    Console.WriteLine("{0} converted to type {1}, new file: {2}", fileEntered, newFileType, newFile);
                    break;
                case 2:
                    Console.WriteLine("Enter new file name:");
                    string newFileName = fileDirectoryPath + "\\" + Console.ReadLine() + fileType;

                    //FileAttributes NF = File.GetAttributes(fileEntered);
                    //debugPrint(3);//Console.WriteLine(newFileName);

                    if (!Directory.Exists(Path.GetDirectoryName(newFileName)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(newFileName));
                    }
                    File.Move(fileEntered, newFileName);    
                    Console.WriteLine("{0} renamed to {1}", fileEntered, newFileName);
                    break;
            }
            
            //could add the close function but its a while loop
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