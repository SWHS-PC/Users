using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public static string nick;
        public static string chan = "#test";
        public static string user;
        public string TrimServer = " ";
        List<string> ActiveChannels = new List<string>();
        
        public static string ServerListFile = "configs\\serverlist.txt";
        public static string[] Servers = System.IO.File.ReadAllLines(ServerListFile);
        StreamWriter send;

        
        public ClientWindow()
        {
            InitializeComponent();
            ActiveControl = textBoxEnter;
            Server1.Text = Servers[0].Split(' ')[0];
            Console.WriteLine(Servers[1].Split(' ')[0]);
            Server2.Text = Servers[1].Split(' ')[0];
            Server3.Text = Servers[2].Split(' ')[0];
            Server4.Text = Servers[3].Split(' ')[0];
            Server5.Text = Servers[4].Split(' ')[0];
            textBoxChat.SelectionStart = 0;
            textBoxChat.SelectionLength = 0;
        }

        public async void IRCRun(StreamWriter send, StreamReader recieve, string server)
        {
            Invoke(new MethodInvoker(delegate ()
            {
                textBoxChat.Text = "";
            }));
            send.WriteLine("NICK " + nick);
            send.WriteLine(user);
            send.Flush();

            while ((input = await recieve.ReadLineAsync()) != null)
            {
                string[] splitInput = input.Split(' ');
                
                if (splitInput[1] == "NOTICE")
                {
                    TrimServer = splitInput[0];
                }

                if (splitInput[1] == "005" || splitInput[1] == "004")
                {
                    continue;
                }
                else
                {
                    string FilteredInput = input.Replace(TrimServer, "");

                    if (Int32.TryParse(splitInput[1], out int LineIds) == true)
                    {
                        string TrimLineId = Convert.ToString(LineIds);
                        FilteredInput = FilteredInput.Replace(TrimLineId + " ", "");
                    }
                    if (FilteredInput.Split(' ').Any("00".Contains))
                    {
                        FilteredInput = FilteredInput.Replace("00", "");
                    }
                    FilteredInput = FilteredInput.Replace(nick + " ", "");
                    FilteredInput = FilteredInput.Replace(":", "");
                    FilteredInput = FilteredInput.Replace(" = ", " ");
                    //FilteredInput = input;
                    if (splitInput[0].Split('!')[0] == ":" + nick)
                    {
                        continue;
                    }
                    
                    else
                    {
                        Console.WriteLine(input);
                        Invoke(new MethodInvoker(delegate ()
                        {
                            textBoxChat.AppendText(FilteredInput + "\r\n");
                        }));
                        switch (splitInput[1])
                        {
                            case "376":
                                send.WriteLine("JOIN " + chan);
                                send.Flush();
                                break;
                            case "422":
                                send.WriteLine("JOIN " + chan);
                                send.Flush();
                                break;
                            case "353":
                                ActiveChannels.Add(splitInput[4]);
                                Console.WriteLine(ActiveChannels[0]);
                                break;
                            case "333":
                                break;
                        }

                        if (splitInput[0] == "PING")
                        {
                            string reply = splitInput[1];
                            send.WriteLine("PONG " + reply);
                            send.Flush();
                        }
                    }
                }            
            }
        }
        private void ClientWindow_Load(object sender, EventArgs e)
        {

        }

        private void Return(object sender, KeyPressEventArgs e)
        {
            
            if (e.KeyChar == (char)Keys.Return)
            {
                e.Handled = true;
                //Console.WriteLine(textBoxEnter.Text.ToString() + "gayyyyyyyyyyyyyyyyyyyyyyyyy");
                if (textBoxEnter.Text.Split(' ')[0].ToCharArray()[0] == '/')
                {
                    switch (textBoxEnter.Text.Split(' ')[0])
                    {
                        case "/join":
                            send.WriteLine("JOIN " + textBoxEnter.Text.Split(' ')[1]);
                            chan = textBoxEnter.Text.Split(' ')[1];
                            break;
                        case "/quit":
                            send.WriteLine("QUIT " + textBoxEnter.Text.Split(' ')[1]);
                            break;
                    }
                }
                else
                {
                    send.WriteLine("PRIVMSG " + chan + " " + textBoxEnter.Text);
                }
                send.Flush();
                textBoxEnter.Text = "";
            }
            
        }

        private void OpenClientWindowNewServer(object sender, EventArgs e)
        {
            NewServer LoadNewServer = new NewServer(this);
            LoadNewServer.Show();
        }

        private void OpenClientWindowPreferences(object sender, EventArgs e)
        {

        }

        private void ToolStripMenuItemServerConnect(string server, int port)
        {
            
            user = "USER " + nick + " 0 * :" + nick;

            TcpClient irc = new TcpClient(server, port);
            NetworkStream stream = irc.GetStream();
            StreamReader recieve = new StreamReader(stream);
            send = new StreamWriter(stream);

            Thread IRCThread = new Thread(() => IRCRun(send, recieve, server));
            IRCThread.Start();
        }

        private void DiconnectFromSelectedServer(object sender, EventArgs e)
        {
            send.WriteLine("QUIT Bye");
            send.Flush();
            textBoxChat.Text = "Select a Server or type /server <ipaddress> <port>";
        }

        private void Server1Con(object sender, EventArgs e)
        {
            string server = Servers[0].Split(' ')[0];
            int port = Convert.ToInt32(Servers[0].Split(' ')[1]);
            nick = Servers[0].Split(' ')[2];
            ToolStripMenuItemServerConnect(server, port);
        }
        private void Server2Con(object sender, EventArgs e)
        {
            string server = Servers[1].Split(' ')[0];
            int port = Convert.ToInt32(Servers[1].Split(' ')[1]);
            nick = Servers[1].Split(' ')[2];
            ToolStripMenuItemServerConnect(server, port);
        }
        private void Server3Con(object sender, EventArgs e)
        {
            string server = Servers[2].Split(' ')[0];
            int port = Convert.ToInt32(Servers[2].Split(' ')[1]);
            nick = Servers[2].Split(' ')[2];
            ToolStripMenuItemServerConnect(server, port);
        }
        private void Server4Con(object sender, EventArgs e)
        {
            string server = Servers[3].Split(' ')[0];
            int port = Convert.ToInt32(Servers[3].Split(' ')[1]);
            nick = Servers[3].Split(' ')[2];
            ToolStripMenuItemServerConnect(server, port);
        }
        private void Server5Con(object sender, EventArgs e)
        {
            string server = Servers[4].Split(' ')[0];
            int port = Convert.ToInt32(Servers[4].Split(' ')[1]);
            nick = Servers[4].Split(' ')[2];
            ToolStripMenuItemServerConnect(server, port);
        }
    }
}
