using System;
<<<<<<< Aidan
using System.Collections;
=======
using System.Collections.Generic;
>>>>>>> master
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
<<<<<<< Aidan
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;

namespace ConsoleApp2
{
    class Program : System.Windows.Forms.Form
    {
        private const int MF_BYCOMMAND = 0x00000000;
        private const int MF_BYPOSITION = 0x00000400;
        public const int SC_CLOSE = 0xF060;
        public const int SC_MAXIMIZE = 0xF030;
        public const int SC_MINIMIZE = 0xF020;
        public const int SC_MOVE = 0xF010;
        public const int SC_MOUSEMENU = 0xF090;
        public const int SC_KEYMENU = 0xF100;
        public const int SC_DEFAULT = 0xF160;
        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();


        static void Main()
        {
         
            

            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_CLOSE, MF_BYCOMMAND);
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MAXIMIZE, MF_BYCOMMAND);
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MINIMIZE, MF_BYCOMMAND);
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MOVE, MF_BYCOMMAND);

            Console.Title = "Rect!";
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Black;
            if (Environment.Is64BitOperatingSystem){string a = "64Bit"; Console.WriteLine(a); }
            Console.WriteLine(Environment.MachineName);
            Console.WriteLine(Environment.OSVersion);
            Console.WriteLine(Environment.ProcessorCount);
            Console.WriteLine(Environment.UserName);
            Console.WriteLine(Environment.Version);

            String str;
            String nl = Environment.NewLine;
            //
            Console.WriteLine();
            Console.WriteLine("-- Environment members --");

            //  Invoke this sample with an arbitrary set of command line arguments.
            Console.WriteLine("CommandLine: {0}", Environment.CommandLine);

            String[] arguments = Environment.GetCommandLineArgs();
            Console.WriteLine("GetCommandLineArgs: {0}", String.Join(", ", arguments));

            //  <-- Keep this information secure! -->
            Console.WriteLine("CurrentDirectory: {0}", Environment.CurrentDirectory);

            Console.WriteLine("ExitCode: {0}", Environment.ExitCode);

            Console.WriteLine("HasShutdownStarted: {0}", Environment.HasShutdownStarted);

            //  <-- Keep this information secure! -->
            Console.WriteLine("MachineName: {0}", Environment.MachineName);

            Console.WriteLine("NewLine: {0}  first line{0}  second line{0}  third line",
                                  Environment.NewLine);

            Console.WriteLine("OSVersion: {0}", Environment.OSVersion.ToString());

            Console.WriteLine("StackTrace: '{0}'", Environment.StackTrace);

            //  <-- Keep this information secure! -->
            Console.WriteLine("SystemDirectory: {0}", Environment.SystemDirectory);

            Console.WriteLine("TickCount: {0}", Environment.TickCount);

            //  <-- Keep this information secure! -->
            Console.WriteLine("UserDomainName: {0}", Environment.UserDomainName);

            Console.WriteLine("UserInteractive: {0}", Environment.UserInteractive);

            //  <-- Keep this information secure! -->
            Console.WriteLine("UserName: {0}", Environment.UserName);

            Console.WriteLine("Version: {0}", Environment.Version.ToString());

            Console.WriteLine("WorkingSet: {0}", Environment.WorkingSet);

            //  No example for Exit(exitCode) because doing so would terminate this example.

            //  <-- Keep this information secure! -->
            String query = "My system drive is %SystemDrive% and my system root is %SystemRoot%";
            str = Environment.ExpandEnvironmentVariables(query);
            Console.WriteLine("ExpandEnvironmentVariables: {0}  {1}", nl, str);

            Console.WriteLine("GetEnvironmentVariable: {0}  My temporary directory is {1}.", nl,
                                   Environment.GetEnvironmentVariable("TEMP"));

            Console.WriteLine("GetEnvironmentVariables: ");
            IDictionary environmentVariables = Environment.GetEnvironmentVariables();
            foreach (DictionaryEntry de in environmentVariables)
            {
                Console.WriteLine("  {0} = {1}", de.Key, de.Value);
            }

            Console.WriteLine("GetFolderPath: {0}",
                         Environment.GetFolderPath(Environment.SpecialFolder.System));

            String[] drives = Environment.GetLogicalDrives();
            Console.WriteLine("GetLogicalDrives: {0}", String.Join(", ", drives));

            begin:
            string code = Console.ReadLine();
            if (code == "new form")
            {
                Application.EnableVisualStyles();
                Application.Run(new Program()); 
                goto begin;
            }

        }

        private void InitializeComponent() 
        {
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(367, 188);
            this.Name = "Program";
            this.ResumeLayout(false);
        }
=======

namespace ConsoleApp2
{
    class Program
    {
        static void Main()
        {
            string input= Console.ReadLine();
            if (input == "" || input == "wd")
            {
                Kringle(Directory.GetCurrentDirectory());
            }
            else
            {
                Kringle(input);
            }
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
        
>>>>>>> master
    }
}
