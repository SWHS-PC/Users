using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IRCClient
{
    public partial class NewServer : Form
    {
        
        public NewServer()
        {
            InitializeComponent();
        }

        private void AddNewServer(object sender, EventArgs e)
        {
            ClientWindow.Servers.Add(textBoxHostname.Text + " " + textBoxPort.Text + " " + textBoxPrefNick.Text);
        }
    }
}
