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
using System.Threading;

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

            AddNewTab(server, xx, 1);

            IRCRun(send[xx], arecieve[xx], server, anick[xx], auser[xx], xx);
            anick[xx+1] = "test";
            
            xx++;
        }

        public async void IRCRun(StreamWriter send, StreamReader recieve, string server, string nick, string user, int sid)
        {
            try
            {
               
                

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
                        case "001":
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
                                    AddNewTab(ChanSent, sid, 0);

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

                userListChan[ChanNum].AppendText(UsersInChannel[ChanNum].Split(' ')[i] + "\r\n");

            }
        }

        private void SetTextColorAndAppend(Color newColor, Color resetColor, string content, int x, int sid)
        {
            //textBoxChat.Select(textBoxChat.TextLength, 0);
            //textBoxChat.SelectionColor = newColor;
            
            if (x == 1)
            {
                //fix
                textBoxChat[sid].AppendText(time + ": " + content + "\r\n");
            }
            else if (x == 2)
            {
                Debug.WriteLine("Debug!: 5");
                textBoxChat[GetTab(1)].AppendText(time + ": " + content + "\r\n");
            }
            else if (x == 3)
            {
                textBoxChat[GetTab(3)].AppendText(time + ": " + content + "\r\n");
            }
            //textBoxChat.Select(textBoxChat.TextLength, 0);
            //textBoxChat.SelectionColor = resetColor;
        }

        private void Return(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                int SC = 0;
                int sid = GetServer();
                e.Handled = true;

                string TextToSend;
                string[] TextSplit = textBoxEnter.Text.Split(' ');
                char[] TextSplitChar = textBoxEnter.Text.Split(' ')[0].ToCharArray();
                Debug.WriteLine("DebugTextSplitChar: " + TextSplitChar[0] + TextSplitChar.Length);
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
                            string text = textBoxEnter.Text;
                            if (TextSplitChar[0] == '/' && TextSplitChar.Length > 1)
                            {
                                text = TextSplit[0].TrimStart('/') + " " + TextToSend;
                                
                            }
                            send[sid].WriteLine(text);
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
                        //todo
                        //textBoxChat.AppendText("Connect to a server to send a message." + "\r\n");
                    }
                }
                textBoxEnter.Text = "";
            }
        }

        public void AddNewTab(string JoinedChan, int sid, int type)
        {
            textBoxChat[ChanNum] = new RichTextBox();
            tabPageChan[ChanNum] = new TabPage();
            userListChan[ChanNum] = new TextBox();

            tabPageChan[sid].Controls.Add(textBoxChat[sid]);
            tabControl.TabPages.Add(tabPageChan[sid]);

            if (type == 0)
            {
                tabPageChan[ChanNum].Controls.Add(userListChan[ChanNum]);
                
                textBoxChat[ChanNum].AcceptsTab = true;
                textBoxChat[ChanNum].Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
                textBoxChat[ChanNum].BackColor = SystemColors.Window;
                textBoxChat[ChanNum].BorderStyle = System.Windows.Forms.BorderStyle.None;
                textBoxChat[ChanNum].CausesValidation = false;
                textBoxChat[ChanNum].HideSelection = false;
                textBoxChat[ChanNum].ImeMode = ImeMode.Off;
                textBoxChat[ChanNum].Location = new System.Drawing.Point(0, 0);
                textBoxChat[ChanNum].MaxLength = 65536;
                textBoxChat[ChanNum].Multiline = true;
                textBoxChat[ChanNum].Name = JoinedChan;
                textBoxChat[ChanNum].ReadOnly = true;
                textBoxChat[ChanNum].ScrollBars = RichTextBoxScrollBars.Vertical;
                textBoxChat[ChanNum].Size = new System.Drawing.Size(1085, 625);
                textBoxChat[ChanNum].TabIndex = ChanNum;
                textBoxChat[ChanNum].Text = "";
                textBoxChat[ChanNum].SelectionStart = 0;
                textBoxChat[ChanNum].SelectionLength = 0;

                userListChan[ChanNum].AcceptsTab = true;
                userListChan[ChanNum].Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Right)));
                userListChan[ChanNum].BackColor = System.Drawing.Color.White;
                userListChan[ChanNum].BorderStyle = System.Windows.Forms.BorderStyle.None;
                userListChan[ChanNum].CausesValidation = false;
                userListChan[ChanNum].HideSelection = false;
                userListChan[ChanNum].ImeMode = System.Windows.Forms.ImeMode.Off;
                userListChan[ChanNum].Location = new System.Drawing.Point(1086, 0);
                userListChan[ChanNum].MaxLength = 65536;
                userListChan[ChanNum].Multiline = true;
                userListChan[ChanNum].Name = "textBoxUsers";
                userListChan[ChanNum].ReadOnly = true;
                userListChan[ChanNum].ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
                userListChan[ChanNum].Size = new System.Drawing.Size(232, 625);
                userListChan[ChanNum].TabIndex = ChanNum;

                tabPageChan[ChanNum].Location = new System.Drawing.Point(4, 22);
                tabPageChan[ChanNum].Name = JoinedChan + " " + Convert.ToString(ChanNum) + " " + Convert.ToString(sid);
                tabPageChan[ChanNum].Size = new System.Drawing.Size(1060, 628);
                tabPageChan[ChanNum].TabIndex = ChanNum;
                tabPageChan[ChanNum].UseVisualStyleBackColor = true;
                tabPageChan[ChanNum].ResumeLayout(false);
                tabPageChan[ChanNum].PerformLayout();
                tabPageChan[ChanNum].Text = JoinedChan;
            }

            if(type == 1)
            {
                textBoxChat[sid].AcceptsTab = true;
                textBoxChat[sid].Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
                textBoxChat[sid].BackColor = System.Drawing.Color.White;
                textBoxChat[sid].BorderStyle = System.Windows.Forms.BorderStyle.None;
                textBoxChat[sid].CausesValidation = false;
                textBoxChat[sid].HideSelection = false;
                textBoxChat[sid].ImeMode = System.Windows.Forms.ImeMode.NoControl;
                textBoxChat[sid].Location = new System.Drawing.Point(0, 0);
                textBoxChat[sid].MaxLength = 65536;
                textBoxChat[sid].Name = "textBoxServer" + Convert.ToString(sid);
                textBoxChat[sid].ReadOnly = true;
                textBoxChat[sid].ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
                textBoxChat[sid].Size = new System.Drawing.Size(1318, 625);
                textBoxChat[sid].TabIndex = ChanNum;
                textBoxChat[sid].Text = "";

                tabPageChan[sid].Location = new System.Drawing.Point(4, 22);
                tabPageChan[sid].Name = Convert.ToString(JoinedChan) + " " + Convert.ToString(ChanNum) + " " + Convert.ToString(sid);
                tabPageChan[sid].Size = new System.Drawing.Size(1060, 628);
                tabPageChan[sid].TabIndex = ChanNum;
                tabPageChan[sid].UseVisualStyleBackColor = true;
                tabPageChan[sid].ResumeLayout(false);
                tabPageChan[sid].PerformLayout();
                tabPageChan[sid].Text = JoinedChan;
            }

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
                foreach (TabPage x in tabPageChan)
                {
                    Debug.WriteLine("Debug!: 7");
                    //Debug.WriteLine("Debug: " + x);
                    if (x.Name.Split(' ')[0] == ChanSent)
                    {
                        ChanSelected = Array.IndexOf(tabPageChan, x);
                        Debug.WriteLine("Debug!: 8");
                        return ChanSelected;
                    }
                }
            }
            if (SC == 2)
            {
                //Debug.WriteLine("Debug22: " + tabControl.SelectedTab.Name);
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
                foreach (RichTextBox x in textBoxChat)
                {
                    if(x != null)
                        textBoxChat[Array.IndexOf(textBoxChat, x)].Dispose();
                }
                foreach (TextBox x in userListChan)
                {
                    if (x != null)
                        userListChan[Array.IndexOf(userListChan, x)].Dispose();
                }
                foreach (TabPage x in tabPageChan)
                {
                    if (x != null)
                        tabPageChan[Array.IndexOf(tabPageChan, x)].Dispose();
                }
                
            }
            else
            {
                //todo
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

