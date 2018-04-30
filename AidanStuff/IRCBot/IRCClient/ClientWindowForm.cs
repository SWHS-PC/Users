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

namespace IRCClient
{
    public partial class ClientWindow : Form
    {
        public string input;
        public static string nick;
        public static string chan;
        public static string user;
        public string TrimServer;
        public string TrimServerToName;
        public string GetOnlyMessage;
        public string MessageSender;
        public string ChanSent;
        public string ChanDestination;
        public int ChanNum = 0;
        List<string> ActiveChannels = new List<string>();
        string[] IRCCommands = { "PRIVMSG", "JOIN", "QUIT", "PART"};
        string[] IdsToAvoid = { "004", "005", "366", "353", "333", "332" };
        public static string ServerListFile = "configs/serverlist.txt";
        public static string[] Servers = System.IO.File.ReadAllLines(ServerListFile);
        public static string[] UsersInChannel = new string[1000];
        StreamWriter send;
        TcpClient irc;

        public ClientWindow()
        {
            InitializeComponent();
            ActiveControl = textBoxEnter;
            
            tabPageServer1.Text = "";

            int ServerItemCount = 0;
            ServerItemCount = SetServerList(ServerItemCount);

        }

        private void ToolStripMenuItemServerConnect(object sender, EventArgs e)
        {
            try
            {
                string server = Servers[0].Split(' ')[1];
                int port = Convert.ToInt32(Servers[0].Split(' ')[2]);
                nick = Servers[0].Split(' ')[3];
                user = "USER " + nick + " 0 * :" + nick;

                irc = new TcpClient(server, port);
                NetworkStream stream = irc.GetStream();
                StreamReader recieve = new StreamReader(stream);
                send = new StreamWriter(stream);

                IRCRun(send, recieve, server);
            }
            catch
            {
                SetTextColorAndAppend(Color.Red, Color.Black, "Error Connecting to Server: " + Servers[0].Split(' ')[1] + ".", 1);
            }
        }

        public async void IRCRun(StreamWriter send, StreamReader recieve, string server)
        {
            try
            {
                textBoxServer1.Text = "";
                tabPageServer1.Text = server;

                send.WriteLine("NICK " + nick);
                send.WriteLine(user);
                send.Flush();

                while ((input = await recieve.ReadLineAsync()) != null)
                {
                    Console.WriteLine(input);
                    string[] splitInput = input.Split(' ');

                    switch (splitInput[1])
                    {
                        case "NOTICE":
                            TrimServer = splitInput[0];
                            TrimServerToName = splitInput[0].TrimStart(':');
                            break;
                        case "353":
                            ActiveChannels.Add(splitInput[4]);
                            foreach (string x in ActiveChannels)
                            {
                                Console.WriteLine(x);
                            }
                            UsersInChannel[ChanNum] = Regex.Split(input.TrimStart(':', ' '), ":")[1];
                            for (int i = 0; i < UsersInChannel[ChanNum].Split(' ').Length; i++)
                            {
                          
                                    textBoxUsers.AppendText(UsersInChannel[ChanNum].Split(' ')[i] + "\r\n");
                               
                            }
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

                        int SC;
                        switch (splitInput[1])
                        {
                            case "PRIVMSG":
                                FilteredInput = MessageSender + "> " + Regex.Split(input, ChanSent + " :")[1];
                                SetTextColorAndAppend(Color.Black, Color.Black, FilteredInput,2);
                                break;
                            case "JOIN":
                                FilteredInput = MessageSender + " Joined " + ChanSent + ".";
                                if (MessageSender == nick)
                                {
                                    AddNewChan(ChanSent);
                                }
                                SetTextColorAndAppend(Color.Black, Color.Black, FilteredInput,2);
                                break;
                            case "PART":
                                FilteredInput = MessageSender + " Left " + splitInput[2] + " " + splitInput[3].Replace(":", "") + ".";
                                SetTextColorAndAppend(Color.Black, Color.Black, FilteredInput,2);
                                break;
                            case "USER":

                                break;
                            case "MODE":
                                FilteredInput = "Mode " + splitInput[3] + " was set on " + splitInput[2];
                                SetTextColorAndAppend(Color.Green, Color.Black, FilteredInput, 1);
                                break;
                        }
                        //FilteredInput = input;

                        if (splitInput[0].Replace(":", "") == TrimServerToName)
                        {
                            SetTextColorAndAppend(Color.Black, Color.Black, FilteredInput, 1);
                        }

                    }
                }
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine(e);
            }
        }

        private void SetTextColorAndAppend(Color newColor, Color resetColor, string content, int x)
        {
            textBoxServer1.Select(textBoxServer1.TextLength, 0);
            textBoxServer1.SelectionColor = newColor;
            if (x == 1)
            {
                textBoxServer1.AppendText(content + "\r\n");
            }
            else if(x == 2){
                textBoxServer1Chan[GetTab(1)].AppendText(content + "\r\n");
            }
            else if (x == 3)
            {
                textBoxServer1Chan[GetTab(3)].AppendText(content + "\r\n");
            }
            textBoxServer1.Select(textBoxServer1.TextLength, 0);
            textBoxServer1.SelectionColor = resetColor;
        }

        private void Return(object sender, KeyPressEventArgs e)
        {
            int SC = 0;
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
                        if (TextSplitChar[0] == '/' && TextSplitChar.Length > 1 )
                        {
                            send.WriteLine(TextSplit[0].TrimStart('/') + " " + TextToSend);
                        }
                        else
                        {
                            SC = 3;
                            ChanDestination = Convert.ToString(tabControl.SelectedTab.Name).Split(' ')[0];
                            string textEntered = "PRIVMSG " + ChanDestination + " " + textBoxEnter.Text;
                            send.WriteLine(textEntered);
                            SetTextColorAndAppend(Color.Black, Color.Black, nick + "> " + textBoxEnter.Text, 3);

                        }
                        send.Flush();
                    }
                    else
                    {      
                        textBoxServer1.AppendText("Connect to a server to send a message." + "\r\n");
                    }
                }
                textBoxEnter.Text = "";
            }
        }

        public void AddNewChan(string JoinedChan)
        {
            
           
                    textBoxServer1Chan[ChanNum] = new TextBox();
                    tabPageServer1Chan[ChanNum] = new TabPage();
                    tabPageServer1Chan[ChanNum].Controls.Add(textBoxServer1Chan[ChanNum]);
                    tabControl.TabPages.Add(tabPageServer1Chan[ChanNum]);

                    textBoxServer1Chan[ChanNum].AcceptsReturn = true;
                    textBoxServer1Chan[ChanNum].AcceptsTab = true;
                    textBoxServer1Chan[ChanNum].Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
                    textBoxServer1Chan[ChanNum].BackColor = SystemColors.Window;
                    textBoxServer1Chan[ChanNum].CausesValidation = false;
                    textBoxServer1Chan[ChanNum].HideSelection = false;
                    textBoxServer1Chan[ChanNum].ImeMode = ImeMode.Off;
                    textBoxServer1Chan[ChanNum].Location = new System.Drawing.Point(0, 0);
                    textBoxServer1Chan[ChanNum].MaxLength = 65536;
                    textBoxServer1Chan[ChanNum].Multiline = true;
                    textBoxServer1Chan[ChanNum].Name = JoinedChan;
                    textBoxServer1Chan[ChanNum].ReadOnly = true;
                    textBoxServer1Chan[ChanNum].ScrollBars = ScrollBars.Vertical;
                    textBoxServer1Chan[ChanNum].Size = new System.Drawing.Size(1060, 628);
                    textBoxServer1Chan[ChanNum].TabIndex = ChanNum;
                    textBoxServer1Chan[ChanNum].Text = "";
                    textBoxServer1Chan[ChanNum].SelectionStart = 0;
                    textBoxServer1Chan[ChanNum].SelectionLength = 0;
                    
                    
                    tabPageServer1Chan[ChanNum].Location = new System.Drawing.Point(4, 22);
                    tabPageServer1Chan[ChanNum].Name = JoinedChan + " " + Convert.ToString(ChanNum);
                    tabPageServer1Chan[ChanNum].Size = new System.Drawing.Size(1060, 628);
                    tabPageServer1Chan[ChanNum].TabIndex = ChanNum;
                    tabPageServer1Chan[ChanNum].UseVisualStyleBackColor = true;
                    tabPageServer1Chan[ChanNum].ResumeLayout(false);
                    tabPageServer1Chan[ChanNum].PerformLayout();
                    tabPageServer1Chan[ChanNum].Text = JoinedChan;

                    
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
                foreach(TabPage x in tabPageServer1Chan)
                {
                    if(tabPageServer1Chan[Array.IndexOf(tabPageServer1Chan, x)].Name.Split(' ')[0] == ChanSent)
                    {
                        ChanSelected = Array.IndexOf(tabPageServer1Chan, x);
                        return ChanSelected;
                    }
                }
            }
            
            if(SC == 3)
            {
                Console.WriteLine(tabControl.SelectedTab.Name);

                ChanSelected = Convert.ToInt32(Convert.ToString(tabControl.SelectedTab.Name).Split(' ')[1]);
            }

            return ChanSelected;
        }
        
        private void DiconnectFromSelectedServer(object sender, EventArgs e)
        {
            if (IsConnected)
            {
                send.WriteLine("QUIT Bye");
                send.Flush();
                textBoxServer1.Text = "Select a Server or type /server <ipaddress> <port> \r\n";
                //textBoxServer1Chan1.Visible = false;
            }
            else
            {
                textBoxServer1.Text = "You need to connect to a server before you can Disconnect \r\n";
                //textBoxServer1Chan1.Text = "You need to connect to a server before you can Disconnect \r\n";
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

        private void ClientWindow_Load(object sender, EventArgs e)
        {

        }
    }
}
