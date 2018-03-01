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

namespace IRCClient
{
    public partial class ClientWindow : Form
    {
        public string input;                                    
        const string server = "irc.orderofthetilde.net";
        const int port = 6667;
        const string nick = "CSClient";
        const string chan = "#test";
        const string user = "USER " + nick + " 0 * :" + nick;
        public static TcpClient irc = new TcpClient(server, port);
        public static NetworkStream stream = irc.GetStream();
        public static StreamReader recieve = new StreamReader(stream);
        public StreamWriter send = new StreamWriter(stream);
        List<string> ActiveChannels = new List<string>();
        public static List<string> Servers = new List<string>();

        public ClientWindow()
        {
            InitializeComponent();

            var IRCThread = new Thread(new ThreadStart(IRCRun));
            IRCThread.Start();
        }
        public void IRCRun()
        {          
            send.WriteLine("NICK " + nick);
            send.WriteLine(user);
            send.Flush();

            while ((input = recieve.ReadLine()) != null)
            {
                string[] splitInput = input.Split(' ');

                if (splitInput[1] == "005" || splitInput[1] == "004")
                {
                    continue;
                }
                else
                {
                    string FilteredInput = input.Replace(":" + server, "");
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
                send.WriteLine("PRIVMSG " + chan + " " + textBoxEnter.Text);
                send.Flush();
                textBoxEnter.Text = "";
            } 
        }

        private void OpenClientWindowNewServer(object sender, EventArgs e)
        {
            NewServer LoadNewServer = new NewServer();
            LoadNewServer.Show();
        }

        private void OpenClientWindowPreferences(object sender, EventArgs e)
        {

        }
    }
}
