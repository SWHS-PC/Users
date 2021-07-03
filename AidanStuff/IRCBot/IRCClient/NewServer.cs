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
            mainForm = mainfm;
        }

        
        private void CloseNewServerDialogue(Object sender, EventArgs e)
        {
            string content = "$ " + textBoxHostname.Text + " " + textBoxPort.Text + " " + textBoxPrefNick.Text;
            foreach(string x in textBoxAutoJC.Text.Split(' '))
            {
                content = content + " " + x;
            }
            System.IO.File.AppendAllText(ClientWindow.ServerListFile, content + "\r\n");
            mainForm.SetServerList(0);
            this.Close();
        }

        private void NewServer_Load(object sender, EventArgs e)
        {

        }
    }
}
