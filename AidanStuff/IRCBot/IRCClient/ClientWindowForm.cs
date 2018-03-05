﻿using System;
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
using System.Text.RegularExpressions;

namespace IRCClient
{
    public partial class ClientWindow : Form
    {
        public string input;
        public static string nick;
        public static string chan = "#test";
        public static string user;
        public string TrimServer;
        public string TrimServerToName;
        public string GetOnlyMessage;
        List<string> ActiveChannels = new List<string>();
        string[] IRCCommands = { "PRIVMSG", "JOIN", "QUIT", "NICK", "PART", "USER", "PING", "PONG" };
        string[] IdsToAvoid = { "004", "005", "366", "353" };
        public string MessageSender;
        public static string ServerListFile = "configs/serverlist.txt";
        public static string[] Servers = System.IO.File.ReadAllLines(ServerListFile);
        StreamWriter send;
        TcpClient irc;

        
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
            try
            {
                Invoke(new MethodInvoker(delegate() { textBoxChat.Text = ""; }));
                send.WriteLine("NICK " + nick);
                send.WriteLine(user);
                send.Flush();

                while ((input = await recieve.ReadLineAsync()) != null)
                {
                    string[] splitInput = input.Split(' ');

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

                    if (splitInput[1] == "NOTICE")
                    {
                        TrimServer = splitInput[0];
                        TrimServerToName = splitInput[0].TrimStart(':');
                    }

                    if (!IdsToAvoid.Any(splitInput[1].Contains))
                    {
                        string FilteredInput = input.Replace(TrimServer, "");

                        if (Int32.TryParse(splitInput[1], out int LineIds) == true)
                        {
                            string TrimLineId = Convert.ToString(LineIds);
                            FilteredInput = FilteredInput.Replace(TrimLineId + " ", "");
                        }

                        if (FilteredInput.Split(' ')[1] == "00" + nick)
                        {
                            FilteredInput = FilteredInput.Replace(splitInput[1], "");
                        }

                        if (splitInput[2] == nick)


                            FilteredInput = FilteredInput.Replace(":" + TrimServerToName, "");

                        //FilteredInput = TrimServerToName + "> " + FilteredInput.Replace(nick + " ", "").Replace(":", "").Replace(" = ", " ")
                        //FilteredInput = input;

                        //get data for PRIVMSG lines, which is a majority of the lines
                        if (IRCCommands.Any(splitInput[1].Contains))
                        {
                            MessageSender = Regex.Split(splitInput[0].TrimStart(':'), "!")[0];
                            GetOnlyMessage = input.Replace(splitInput[0] + " " + splitInput[1] + " " + splitInput[2] + " :", "");
                        }

                        FilteredInput = TrimServerToName + ">" + FilteredInput;
                        FilteredInput = FilteredInput.Replace(TrimServerToName + "> " + nick + " ",TrimServerToName + "> ");
                        FilteredInput = FilteredInput.Replace(TrimServerToName + "> 00" + nick + " ",TrimServerToName + "> ");
                        switch (splitInput[1])
                        {
                            case "PRIVMSG":
                                FilteredInput = MessageSender + "> " + GetOnlyMessage;
                                break;
                            case "JOIN":
                                FilteredInput = MessageSender + " Joined " + splitInput[2].Replace(":", "");
                                break;
                            case "PART":
                                break;
                            case "USER":
                                break;
                            case "MODE":
                                FilteredInput = "Mode " + splitInput[3] + " was set on " + splitInput[2];
                                break;
                        }


                        Console.WriteLine(input);
                        Invoke(new MethodInvoker(delegate() { textBoxChat.AppendText(FilteredInput + "\r\n"); }));
                    }
                }
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine(e);
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
                
                string TextToSend;
                string[] TextSplit = textBoxEnter.Text.Split(' ');
                char[] TextSplitChar = textBoxEnter.Text.Split(' ')[0].ToCharArray();
                

                if (textBoxEnter.Text != "")
                {
                    if (TextSplit.Length > 1)
                    {
                        TextToSend = TextSplit[1];
                    }
                    else
                    {
                        TextToSend = "";
                    }
                    if (IsConnected == true)
                    {
                        if (TextSplitChar[0] == '/' && TextSplitChar.Length > 1 && IRCCommands.Any(TextSplit[0].TrimStart('/').Contains))
                        {
                            send.WriteLine(TextSplit[0].TrimStart('/') + " " + TextToSend);

                            if (TextSplit[0].TrimStart('/') == "join")
                            {
                                chan = TextSplit[1];
                            }
                        }
                        else
                        {
                            string textEntered = "PRIVMSG " + chan + " " + textBoxEnter.Text;
                            send.WriteLine(textEntered);
                            //send.WriteLine("PRIVMSG " + ActiveChan + " " + textBoxEnter.Text);
                            textBoxChat.AppendText(nick + "> " + textBoxEnter.Text +"\r\n");
                        }
                        send.Flush();
                    }
                    else
                    {
                        textBoxChat.AppendText("Connect to a server to send a message." + "\r\n");
                    }
                }
                
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

            irc = new TcpClient(server, port);
            NetworkStream stream = irc.GetStream();
            StreamReader recieve = new StreamReader(stream);
            send = new StreamWriter(stream);

            Thread IRCThread = new Thread(() => IRCRun(send, recieve, server));
            IRCThread.Start();
        }

        private void DiconnectFromSelectedServer(object sender, EventArgs e)
        {
            if (IsConnected)
            {
                send.WriteLine("QUIT Bye");
                send.Flush();
                textBoxChat.Text = "Select a Server or type /server <ipaddress> <port> \r\n";
            }
            else
            {
                textBoxChat.Text = "You need to connect to a server before you can Disconnect \r\n";
            }
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
        public bool IsConnected
        {
            get
            {
                try
                {
                    if (irc != null && irc.Client != null && irc.Client.Connected)
                    {
                        
                        if (irc.Client.Poll(0, SelectMode.SelectRead))
                        {
                            byte[] buff = new byte[1];
                            if (irc.Client.Receive(buff, SocketFlags.Peek) == 0)
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
