using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace IRCClient
{
    public partial class ClientWindow : Form
    {
        public string input;
        List<string> DisplayInput = new List<string>(); 
        string server = "4bit.pw";
        int port = 6667;
        string nick = "abot";
        string chan = "#GRP";
        string user = "USER abot 0 * :abot";

        public ClientWindow()
        {
            InitializeComponent();

            var demoThread = new Thread(new ThreadStart(IRCStart));

            demoThread.Start();
            


        }
        private void IRCStart()
        {
            using (var irc = new TcpClient(server, port))
            using (var stream = irc.GetStream())
            using (var recieve = new StreamReader(stream))
            using (var send = new StreamWriter(stream))
            {
                send.WriteLine("NICK " + nick);
                send.WriteLine(user);
                send.Flush();

                while (true)
                {

                    while ((input = recieve.ReadLine()) != null)
                    {

                        DisplayInput.Add(input);
                        Invoke(new MethodInvoker(delegate () 
                        {
                            textBoxChat.Text = "";
                            for (int i = 0; i < DisplayInput.Count; i++)
                            {

                                textBoxChat.Text += DisplayInput[i] + "\r\n";
                            }
                        }));

                        string[] splitInput = input.Split(' ');

                        if (splitInput[0] == "PING")
                        {
                            string reply = splitInput[1];
                            send.WriteLine("PONG " + reply);
                            send.Flush();
                        }
                        else if (splitInput[1] == "376" || splitInput[1] == "422")
                        {
                            send.WriteLine("JOIN " + chan);
                        }
                    }
                }
            }
        }

        private void ClientWindow_Load(object sender, EventArgs e)
        {

        }
        
    }
}
