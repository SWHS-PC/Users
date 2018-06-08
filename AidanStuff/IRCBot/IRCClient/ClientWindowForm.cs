using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace IRCClient
{
    public partial class ClientWindow : Form
    {
        public string[] input = new string[100];
        public static string nick;
        public static string user;
        public string TrimServer;
        public string TrimServerToName;
        public string GetOnlyMessage;
        public string MessageSender;
        public string ChanSent;
        public string ChanDestination;
        public string time;
        public int ChanNum = 0;
        public int xx = 0;
        public int tempsid;
        List<string> ActiveChannels = new List<string>();
        string[] IRCCommands = { "PRIVMSG", "JOIN", "QUIT", "PART" };
        string[] IdsToAvoid = { "004", "005", "366", "353", "333", "332" };
        public static string ServerListFile = "configs/serverlist.txt";
        public static string[] Servers = System.IO.File.ReadAllLines(ServerListFile);
        public static string[] UsersInChannel = new string[1000];
        //StreamWriter send;
        //TcpClient irc;
        public static string[] anick = new string[100];
        public static string[] auser = new string[100];
        StreamWriter[] send = new StreamWriter[100];
        TcpClient[] irc = new TcpClient[100];
        NetworkStream[] astream = new NetworkStream[100];
        StreamReader[] arecieve = new StreamReader[100];

        public ClientWindow()
        {
            InitializeComponent();
            ActiveControl = textBoxEnter;

            tabPageServer1.Text = "";

            int ServerItemCount = 0;
            ServerItemCount = SetServerList(ServerItemCount);
            anick[0] = Servers[0].Split(' ')[3];
        }

        //private void a()
        //{
        //    try
        //    {
        //        string server = Servers[0].Split(' ')[1];
        //        int port = Convert.ToInt32(Servers[0].Split(' ')[2]);
        //        nick = Servers[0].Split(' ')[3];
        //        user = "USER " + nick + " 0 * :" + nick;

        //        irc = new TcpClient(server, port);
        //        NetworkStream stream = irc.GetStream();
        //        StreamReader recieve = new StreamReader(stream);
        //        send = new StreamWriter(stream);

        //        //IRCRun(send, recieve, server);
        //    }
        //    catch
        //    {
        //        SetTextColorAndAppend(Color.Red, Color.Black, "Error Connecting to Server: " + Servers[0].Split(' ')[1] + ".", 1);
        //    }
        //}

        
        private void ToolStripMenuItemServerConnect(object sender, EventArgs e)
        {
            
            string server = Servers[0].Split(' ')[1];
            int port = Convert.ToInt32(Servers[0].Split(' ')[2]);
            //anick[x] = Servers[0].Split(' ')[3];
            auser[xx] = "USER " + anick[0] + " 0 * :" + anick[0];

            irc[xx] = new TcpClient(server, port);
            astream[xx] = irc[xx].GetStream();
            arecieve[xx] = new StreamReader(astream[xx]);
            send[xx] = new StreamWriter(astream[xx]);

            AddNewServerTab(xx, server);

            IRCRun(send[xx], arecieve[xx], server, anick[xx], auser[xx], xx);
            anick[xx+1] = "test";
            
            xx++;
        }

        public async void IRCRun(StreamWriter send, StreamReader recieve, string server, string nick, string user, int sid)
        {
            try
            {
                textBoxNewServer[sid].Text = "";
                tabPageNewServer[sid].Text = server;

                send.WriteLine("NICK " + nick);
                send.WriteLine(user);
                send.Flush();

                while ((input[sid] = await recieve.ReadLineAsync()) != null)
                {
                    Debug.WriteLine(sid);
                    Debug.WriteLine("Debug: " + input[sid]);
                    string[] splitInput = input[sid].Split(' ');
                    //TODO
                    // add ("h:mm:ss tt") to preferences
                    time = DateTime.Now.ToString("hh:mm:ss tt");

                    switch (splitInput[1])
                    {
                        case "NOTICE":
                            TrimServer = splitInput[0];
                            TrimServerToName = splitInput[0].TrimStart(':');
                            break;
                        case "353":
                            ActiveChannels.Add(splitInput[4]);
                            ChanSent = splitInput[4];
                            SetUsersInChan(sid);
                            break;

                    }

                    if (splitInput[0] == "PING")
                    {
                        string reply = splitInput[1];
                        send.WriteLine("PONG " + reply);
                        send.Flush();
                    }

                    if (!IdsToAvoid.Any(splitInput[1].Contains) && splitInput.Length > 1)
                    {
                        string FilteredInput = input[sid].Replace(TrimServer, "");

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
                        {
                            FilteredInput = FilteredInput.Replace(":" + TrimServerToName, "");
                        }

                        if (IRCCommands.Any(splitInput[1].Contains))
                        {
                            MessageSender = Regex.Split(splitInput[0].TrimStart(':'), "!")[0];
                            ChanSent = splitInput[2].Replace(":", "");
                        }

                        FilteredInput = TrimServerToName + ">" + FilteredInput;
                        FilteredInput = FilteredInput.Replace(TrimServerToName + "> " + nick + " ", TrimServerToName + "> ");
                        FilteredInput = FilteredInput.Replace(TrimServerToName + "> 00" + nick + " ", TrimServerToName + "> ");

                        
                        switch (splitInput[1])
                        {
                            case "PRIVMSG":
                                FilteredInput = MessageSender + "> " + Regex.Split(input[sid], ChanSent + " :")[1];
                                SetTextColorAndAppend(Color.Black, Color.Black, FilteredInput, 2, sid);
                                break;
                            case "JOIN":
                                FilteredInput = MessageSender + " Joined " + ChanSent + ".";
                                Debug.WriteLine("Debug!: 1 " + FilteredInput);
                                if (MessageSender == nick)
                                {
                                    Debug.WriteLine("Debug!: 2");
                                    AddNewChan(ChanSent, sid);

                                }
                                Debug.WriteLine("Debug!: 3");
                                SetTextColorAndAppend(Color.Black, Color.Black, FilteredInput, 2, sid);
                                Debug.WriteLine("Debug!: 4");
                                break;
                            case "PART":
                                FilteredInput = MessageSender + " Left " + splitInput[2] + " " + splitInput[3].Replace(":", "") + ".";
                                SetTextColorAndAppend(Color.Black, Color.Black, FilteredInput, 2, sid);
                                break;
                            case "USER":

                                break;
                            case "MODE":
                                FilteredInput = "Mode " + splitInput[3] + " was set on " + splitInput[2];
                                SetTextColorAndAppend(Color.Green, Color.Black, FilteredInput, 1, sid);
                                break;
                        }
                        //FilteredInput = input;

                        if (splitInput[0].Replace(":", "") == TrimServerToName)
                        {
                            //Debug.WriteLine("Debug!: " + FilteredInput);
                            SetTextColorAndAppend(Color.Black, Color.Black, FilteredInput, 1, sid);
                        }

                    }
                }
            }
            catch (IndexOutOfRangeException e)
            {
                Debug.WriteLine("Debug: " + e);
                //input = "Error connecting to server";
                //SetTextColorAndAppend(Color.Black, Color.Black, input, 1);
            }
        }
        private void SetUsersInChan(int sid)
        {
            ChanNum = GetTab(1);
            //Debug.WriteLine("Debug: " + Regex.Split(input.TrimStart(':', ' '), ":")[1].Split(' ').Length);
            UsersInChannel[ChanNum] = Regex.Split(input[sid].TrimStart(':', ' '), ":")[1];
            for (int i = 0; i < UsersInChannel[ChanNum].Split(' ').Length; i++)
            {

                userListServer1Chan[ChanNum].AppendText(UsersInChannel[ChanNum].Split(' ')[i] + "\r\n");

            }
        }

        private void SetTextColorAndAppend(Color newColor, Color resetColor, string content, int x, int sid)
        {
            textBoxServer1.Select(textBoxServer1.TextLength, 0);
            textBoxServer1.SelectionColor = newColor;
            if (x == 1)
            {
                textBoxNewServer[sid].AppendText(time + ": " + content + "\r\n");
            }
            else if (x == 2)
            {
                Debug.WriteLine("Debug!: 5");
                textBoxServer1Chan[GetTab(1)].AppendText(time + ": " + content + "\r\n");
            }
            else if (x == 3)
            {
                textBoxServer1Chan[GetTab(3)].AppendText(time + ": " + content + "\r\n");
            }
            textBoxServer1.Select(textBoxServer1.TextLength, 0);
            textBoxServer1.SelectionColor = resetColor;
        }

        private void Return(object sender, KeyPressEventArgs e)
        {
            int SC = 0;
            int sid = GetServer();
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
                    tempsid = sid;
                    if (IsConnected == true)
                    {
                        if (Convert.ToString(tabControl.SelectedTab.Text) == TrimServerToName)
                        {
                             send[sid].WriteLine(textBoxEnter.Text);
                        }
                        else
                        {
                            SC = 3;
                            ChanDestination = Convert.ToString(tabControl.SelectedTab.Name).Split(' ')[0];
                            string textEntered = "PRIVMSG " + ChanDestination + " :" + textBoxEnter.Text;
                            Debug.WriteLine("Debug: <" + textEntered + ">");
                            if (TextSplitChar.Length > 0)
                            {
                                if (TextSplitChar[0] == '/' && TextSplitChar.Length > 1)
                                {
                                    textEntered = TextSplit[0].TrimStart('/') + " " + TextToSend;
                                    send[sid].WriteLine(textEntered);
                                }
                                else
                                {
                                    send[sid].WriteLine(textEntered);
                                    SetTextColorAndAppend(Color.Black, Color.Black, nick + "> " + textBoxEnter.Text, 3, sid);
                                }
                            }
                            else
                            {
                                send[sid].WriteLine(textEntered);
                                SetTextColorAndAppend(Color.Black, Color.Black, nick + "> " + textBoxEnter.Text, 3, sid);
                            }
                        }
                        send[sid].Flush();
                    }
                    else
                    {
                        textBoxServer1.AppendText("Connect to a server to send a message." + "\r\n");
                    }
                }
                textBoxEnter.Text = "";
            }
        }

        public void AddNewChan(string JoinedChan, int sid)
        {
            textBoxServer1Chan[ChanNum] = new RichTextBox();
            tabPageServer1Chan[ChanNum] = new TabPage();
            userListServer1Chan[ChanNum] = new TextBox();

            tabPageServer1Chan[ChanNum].Controls.Add(textBoxServer1Chan[ChanNum]);
            tabPageServer1Chan[ChanNum].Controls.Add(userListServer1Chan[ChanNum]);
            tabControl.TabPages.Add(tabPageServer1Chan[ChanNum]);

            textBoxServer1Chan[ChanNum].AcceptsTab = true;
            textBoxServer1Chan[ChanNum].Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            textBoxServer1Chan[ChanNum].BackColor = SystemColors.Window;
            textBoxServer1Chan[ChanNum].BorderStyle = System.Windows.Forms.BorderStyle.None;
            textBoxServer1Chan[ChanNum].CausesValidation = false;
            textBoxServer1Chan[ChanNum].HideSelection = false;
            textBoxServer1Chan[ChanNum].ImeMode = ImeMode.Off;
            textBoxServer1Chan[ChanNum].Location = new System.Drawing.Point(0, 0);
            textBoxServer1Chan[ChanNum].MaxLength = 65536;
            textBoxServer1Chan[ChanNum].Multiline = true;
            textBoxServer1Chan[ChanNum].Name = JoinedChan;
            textBoxServer1Chan[ChanNum].ReadOnly = true;
            textBoxServer1Chan[ChanNum].ScrollBars = RichTextBoxScrollBars.Vertical;
            textBoxServer1Chan[ChanNum].Size = new System.Drawing.Size(1085, 625);
            textBoxServer1Chan[ChanNum].TabIndex = ChanNum;
            textBoxServer1Chan[ChanNum].Text = "";
            textBoxServer1Chan[ChanNum].SelectionStart = 0;
            textBoxServer1Chan[ChanNum].SelectionLength = 0;

            userListServer1Chan[ChanNum].AcceptsTab = true;
            userListServer1Chan[ChanNum].Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Right)));
            userListServer1Chan[ChanNum].BackColor = System.Drawing.Color.White;
            userListServer1Chan[ChanNum].BorderStyle = System.Windows.Forms.BorderStyle.None;
            userListServer1Chan[ChanNum].CausesValidation = false;
            userListServer1Chan[ChanNum].HideSelection = false;
            userListServer1Chan[ChanNum].ImeMode = System.Windows.Forms.ImeMode.Off;
            userListServer1Chan[ChanNum].Location = new System.Drawing.Point(1086, 0);
            userListServer1Chan[ChanNum].MaxLength = 65536;
            userListServer1Chan[ChanNum].Multiline = true;
            userListServer1Chan[ChanNum].Name = "textBoxUsers";
            userListServer1Chan[ChanNum].ReadOnly = true;
            userListServer1Chan[ChanNum].ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            userListServer1Chan[ChanNum].Size = new System.Drawing.Size(232, 625);
            userListServer1Chan[ChanNum].TabIndex = ChanNum;

            tabPageServer1Chan[ChanNum].Location = new System.Drawing.Point(4, 22);
            tabPageServer1Chan[ChanNum].Name = JoinedChan + " " + Convert.ToString(ChanNum) + " " + Convert.ToString(sid);
            tabPageServer1Chan[ChanNum].Size = new System.Drawing.Size(1060, 628);
            tabPageServer1Chan[ChanNum].TabIndex = ChanNum;
            tabPageServer1Chan[ChanNum].UseVisualStyleBackColor = true;
            tabPageServer1Chan[ChanNum].ResumeLayout(false);
            tabPageServer1Chan[ChanNum].PerformLayout();
            tabPageServer1Chan[ChanNum].Text = JoinedChan;


            ChanNum++;

        }

        public void AddNewServerTab(int sid, string server)
        {
            tabPageNewServer[sid] = new TabPage();
            textBoxNewServer[sid] = new RichTextBox();

            tabPageNewServer[sid].Controls.Add(textBoxNewServer[sid]);
            tabControl.TabPages.Add(tabPageNewServer[sid]);

            textBoxNewServer[sid].AcceptsTab = true;
            textBoxNewServer[sid].Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            textBoxNewServer[sid].BackColor = System.Drawing.Color.White;
            textBoxNewServer[sid].BorderStyle = System.Windows.Forms.BorderStyle.None;
            textBoxNewServer[sid].CausesValidation = false;
            textBoxNewServer[sid].HideSelection = false;
            textBoxNewServer[sid].ImeMode = System.Windows.Forms.ImeMode.NoControl;
            textBoxNewServer[sid].Location = new System.Drawing.Point(0, 0);
            textBoxNewServer[sid].MaxLength = 65536;
            textBoxNewServer[sid].Name = "textBoxServer" + Convert.ToString(sid);
            textBoxNewServer[sid].ReadOnly = true;
            textBoxNewServer[sid].ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            textBoxNewServer[sid].Size = new System.Drawing.Size(1318, 625);
            textBoxNewServer[sid].TabIndex = ChanNum;
            textBoxNewServer[sid].Text = "";

            tabPageNewServer[sid].Location = new System.Drawing.Point(4, 22);
            tabPageNewServer[sid].Name = Convert.ToString(ChanNum) + " " + Convert.ToString(sid);
            tabPageNewServer[sid].Size = new System.Drawing.Size(1060, 628);
            tabPageNewServer[sid].TabIndex = ChanNum;
            tabPageNewServer[sid].UseVisualStyleBackColor = true;
            tabPageNewServer[sid].ResumeLayout(false);
            tabPageNewServer[sid].PerformLayout();
            tabPageNewServer[sid].Text = server;
            ChanNum++;
        }

        public int SetServerList(int ServerItemCount)
        {
            Servers = System.IO.File.ReadAllLines(ServerListFile);

            foreach (string x in Servers)
            {
                if (x.Split(' ')[0] == "$")
                {
                    ServerItems[ServerItemCount] = new System.Windows.Forms.ToolStripMenuItem();
                    ServerItems[ServerItemCount].Name = Servers[ServerItemCount].Split(' ')[1];
                    ServerItems[ServerItemCount].Text = Servers[ServerItemCount].Split(' ')[1];
                    ServerItems[ServerItemCount].Size = new System.Drawing.Size(159, 22);
                    ServerItems[ServerItemCount].Click += new System.EventHandler(this.ToolStripMenuItemServerConnect);
                    serversToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[]
                    {
                        ServerItems[ServerItemCount]
                    });
                    ServerItemCount++;
                }
            }

            serversToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[]
            {
                ServerListSeperator,
                openServerListToolStripMenuItem
            });
            return ServerItemCount;
        }

        private int GetTab(int SC)
        {
            int ChanSelected = 0;
            if (SC == 1)
            {
                Debug.WriteLine("Debug!: 6");
                foreach (TabPage x in tabPageServer1Chan)
                {
                    Debug.WriteLine("Debug!: 7");
                    //Debug.WriteLine("Debug: " + x);
                    if (x.Name.Split(' ')[0] == ChanSent)
                    {
                        ChanSelected = Array.IndexOf(tabPageServer1Chan, x);
                        Debug.WriteLine("Debug!: 8");
                        return ChanSelected;
                    }
                }
            }
            if (SC == 2)
            {
                ChanSelected = Convert.ToInt32(Convert.ToString(tabControl.SelectedTab.Name).Split(' ')[2]);
            }
            if (SC == 3)
            {
                //Debug.WriteLine("Debug: " + tabControl.SelectedTab.Name);

                ChanSelected = Convert.ToInt32(Convert.ToString(tabControl.SelectedTab.Name).Split(' ')[1]);
            }

            return ChanSelected;
        }
        private int GetServer()
        {
            int sid = 0;

            sid = GetTab(2);

            return sid;
        }
        public void DiconnectFromSelectedServer(object sender, EventArgs e)
        {
            int sid = GetServer();
            tempsid = sid;
            if (IsConnected)
            {
                send[sid].WriteLine("QUIT Bye");
                send[sid].Flush();
                textBoxServer1.Text = "Select a Server or type /server <ipaddress> <port> \r\n";
                tabPageServer1.Text = " ";

                foreach (RichTextBox x in textBoxServer1Chan)
                {
                    if(x != null)
                        textBoxServer1Chan[Array.IndexOf(textBoxServer1Chan, x)].Dispose();
                }
                foreach (TextBox x in userListServer1Chan)
                {
                    if (x != null)
                        userListServer1Chan[Array.IndexOf(userListServer1Chan, x)].Dispose();
                }
                foreach (TabPage x in tabPageServer1Chan)
                {
                    if (x != null)
                        tabPageServer1Chan[Array.IndexOf(tabPageServer1Chan, x)].Dispose();
                }
                
            }
            else
            {
                textBoxServer1.Text = "You need to connect to a server before you can Disconnect \r\n";
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
        public bool IsConnected
        {
            get
            {
                try
                {
                    if (irc[tempsid] != null && irc[tempsid].Client != null && irc[tempsid].Client.Connected)
                    {
                        if (irc[tempsid].Client.Poll(0, SelectMode.SelectRead))
                        {
                            byte[] buff = new byte[1];
                            if (irc[tempsid].Client.Receive(buff, SocketFlags.Peek) == 0)
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

