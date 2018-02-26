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
    public partial class ClientWindow : Form
    {
        public ClientWindow()
        {
            InitializeComponent();
            
            string line = "a";
            textBoxChat.Text = IRCClient.IRC.input;
            
        }

        private void ClientWindow_Load(object sender, EventArgs e)
        {

        }
    }
}
