using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace IRCClient
{
    public partial class NewServer : Form
    {
        private ClientWindow mainForm;
        public NewServer(ClientWindow mainfm)
        {
            InitializeComponent();
            this.buttonNewServerSubmit.Click += new System.EventHandler(this.SetServerList);
            mainForm = mainfm;
        }

        public void SetServerList(object sender, EventArgs e)
        {
                      
                string content = textBoxHostname.Text + " " + textBoxPort.Text + " " + textBoxPrefNick.Text;
                List<string> ServersInFile = new List<string>(); 
                ServersInFile.Add(content);
                for(int i = 0; i < System.IO.File.ReadAllLines(ClientWindow.ServerListFile).Length; i++)
                {
                    ServersInFile.Add(System.IO.File.ReadAllLines(ClientWindow.ServerListFile)[i]);
                    Console.WriteLine(ServersInFile[i]);
                }
                System.IO.File.WriteAllText(ClientWindow.ServerListFile, String.Empty);

                for (int i = 0; i < ServersInFile.Count; i++)
                {
                    System.IO.File.AppendAllText(ClientWindow.ServerListFile, ServersInFile[i] + "\r\n");
                }
           
        }
        private void CloseNewServerDialogue(Object sender, EventArgs e)
        {
            this.Close();
        }

        private void NewServer_Load(object sender, EventArgs e)
        {

        }
    }
}
