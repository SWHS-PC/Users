using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Shell
{
    class Program
    {
        [DllImport("winmm.dll", EntryPoint = "mciSendString")]
        public static extern int mciSendStringA(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);
        public static string file;
        public static string[] driveLetter = {"E", "F", "G"};
        
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Enter a command");
                string a = Console.ReadLine();
                if (a == "run")
                {
                    file = Console.ReadLine();
                    string dir = Console.ReadLine(); 
                    string arg = Console.ReadLine();
                    Process start = new Process();
                    start.StartInfo.FileName = file;
                    start.StartInfo.WorkingDirectory = dir;
                    start.StartInfo.Arguments = arg;
                    //start.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    start.Start();
                }

                if (a == "kill")
                {
                    try
                    {
                        Process[] proc = Process.GetProcessesByName(file);
                        foreach (Process x in proc)
                        {
                            Console.WriteLine(x);
                        }
                        Console.WriteLine("Kill Process?(y/n)");
                        string kill = Console.ReadLine();
                        if (kill == "y")
                        {
                            proc[0].Kill();
                        }
                    }
                    catch { }
                }

                if(a == "rekt")
                {
                    while (true)
                    {
                        foreach (string x in driveLetter)
                        {
                            mciSendStringA("open " + x + ": type CDaudio alias drive" + x, "", 0, 0);
                            mciSendStringA("set drive" + x + " door open", "", 0, 0);

                            mciSendStringA("open " + x + ": type CDaudio alias drive" + x, "", 0, 0);
                            mciSendStringA("set drive" + x + " door closed", "", 0, 0);
                        }
                    }
                }
            }
        }

        
    }
}


//start.StartInfo.FileName = @"cscript";
//start.StartInfo.WorkingDirectory = @"c:\";
//start.StartInfo.Arguments = " //B //Nologo a.vbs";

//Process.Start("C:\\a.exe");
//ProcessStartInfo start = new ProcessStartInfo();
//start.FileName = "C:\\a.exe";
//Process.Start(start);

//Process.Start(@"cscript //B //Nologo c:\scripts\vbscript.vbs");