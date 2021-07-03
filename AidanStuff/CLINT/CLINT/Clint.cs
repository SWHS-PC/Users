using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CLINT
{
    public partial class Clint : Form
    {
        [DllImport("winmm.dll", EntryPoint = "mciSendString")]
        public static extern int mciSendStringA(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);
        public static string file;
        public static string[] driveLetter = { "E", "F", "G" };
        Process start;
        public Clint()
        {
            InitializeComponent();
        }

        private void Run(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                try
                {

                    if (Input.Text.Split(' ').Length > 0)
                    {
                        string[] InputArr = Input.Text.Split(' ');
                        if (InputArr[0] == "run")
                        {
                            start = new Process();
                            file = InputArr[1];
                            start.StartInfo.FileName = file;
                            if (InputArr.Length > 2)
                            {
                                string arg = Regex.Split(Input.Text, InputArr[1])[1];
                                start.StartInfo.Arguments = arg;
                            }
                            //start.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                            start.Start();
                            MessageBox.Show("The Process you started was: " + Convert.ToString(start) + ". \r\n File path: " + file + ".", "Process Info");
                        }
                    }

                    if (Input.Text == "kill")
                    {
                        try
                        {
                            DialogResult dialogResult = MessageBox.Show("Kill Process " + file + "?", "Kill?", MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                            {
                                start.Kill();
                            }
                            else if (dialogResult == DialogResult.No)
                            {
                                return;
                            }
                            
                            
                        }
                        catch { }
                    }

                    //This opens all present cd drives on computer and closes them repeatedly
                    if (Input.Text == "rekt")
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
                catch { }
            }
        }
    }
}


