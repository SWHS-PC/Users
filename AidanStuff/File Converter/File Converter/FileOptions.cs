using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace File_Converter
{
    public class FileOptions
    {
        string fileEntered { get; set; }
        public void getFileOrFolder(int forfold)
        {
            CommonOpenFileDialog FileD = new CommonOpenFileDialog();
            FileD.AllowNonFileSystemItems = true;
            if(forfold == 2)
            {
                FileD.IsFolderPicker = true;
            }

            if (FileD.ShowDialog() == CommonFileDialogResult.Ok)
            {
                fileEntered = FileD.FileName;
                return;
            }
        }

        public void ChangeFileProperties(int prop)
        {
            getFileOrFolder(1);

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

                    //debugPrint(3);//Console.WriteLine(newFileName);

                    if (!Directory.Exists(Path.GetDirectoryName(newFileName)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(newFileName));
                    }
                    File.Move(fileEntered, newFileName);
                    Console.WriteLine("{0} renamed to {1}", fileEntered, newFileName);
                    break;
            }
        }

        public static void createGodModeFolder()
        {
            string currentUser = Environment.GetEnvironmentVariable("USERNAME");
            Console.WriteLine(currentUser);
            Directory.CreateDirectory("C:\\Users\\" + currentUser + "\\Desktop\\God Mode.{ED7BA470-8E54-465E-825C-99712043E01C}");
          
        }
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