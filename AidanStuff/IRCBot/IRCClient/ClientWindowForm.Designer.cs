namespace IRCClient
{
    partial class ClientWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

       
        private void InitializeComponent()
        {
            this.textBoxChat = new System.Windows.Forms.TextBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.textBoxEnter = new System.Windows.Forms.TextBox();
            this.menuStripTopNav = new System.Windows.Forms.MenuStrip();
            this.serverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serversToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Server1 = new System.Windows.Forms.ToolStripMenuItem();
            this.Server2 = new System.Windows.Forms.ToolStripMenuItem();
            this.Server3 = new System.Windows.Forms.ToolStripMenuItem();
            this.Server4 = new System.Windows.Forms.ToolStripMenuItem();
            this.Server5 = new System.Windows.Forms.ToolStripMenuItem();
            this.ServerListSeperator = new System.Windows.Forms.ToolStripSeparator();
            this.openServerListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disconnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripTopNav.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxChat
            // 
            this.textBoxChat.AcceptsReturn = true;
            this.textBoxChat.AcceptsTab = true;
            this.textBoxChat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxChat.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxChat.CausesValidation = false;
            this.textBoxChat.HideSelection = false;
            this.textBoxChat.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.textBoxChat.Location = new System.Drawing.Point(12, 38);
            this.textBoxChat.MaxLength = 65536;
            this.textBoxChat.Multiline = true;
            this.textBoxChat.Name = "textBoxChat";
            this.textBoxChat.ReadOnly = true;
            this.textBoxChat.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxChat.Size = new System.Drawing.Size(1068, 654);
            this.textBoxChat.TabIndex = 1;
            this.textBoxChat.Text = "Select a Server or type /server <ipaddress> <port> \r\n";
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Location = new System.Drawing.Point(1086, 38);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(252, 680);
            this.listView1.TabIndex = 2;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // textBoxEnter
            // 
            this.textBoxEnter.AcceptsReturn = true;
            this.textBoxEnter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxEnter.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBoxEnter.ImeMode = System.Windows.Forms.ImeMode.On;
            this.textBoxEnter.Location = new System.Drawing.Point(12, 698);
            this.textBoxEnter.Name = "textBoxEnter";
            this.textBoxEnter.Size = new System.Drawing.Size(1068, 20);
            this.textBoxEnter.TabIndex = 3;
            this.textBoxEnter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Return);
            // 
            // menuStripTopNav
            // 
            this.menuStripTopNav.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.menuStripTopNav.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.serverToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.menuStripTopNav.Location = new System.Drawing.Point(0, 0);
            this.menuStripTopNav.Name = "menuStripTopNav";
            this.menuStripTopNav.Size = new System.Drawing.Size(1350, 24);
            this.menuStripTopNav.TabIndex = 4;
            this.menuStripTopNav.Text = "menuStrip1";
            // 
            // serverToolStripMenuItem
            // 
            this.serverToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newServerToolStripMenuItem,
            this.serversToolStripMenuItem,
            this.disconnectToolStripMenuItem});
            this.serverToolStripMenuItem.Name = "serverToolStripMenuItem";
            this.serverToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.serverToolStripMenuItem.Text = "Server";
            // 
            // newServerToolStripMenuItem
            // 
            this.newServerToolStripMenuItem.Name = "newServerToolStripMenuItem";
            this.newServerToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.newServerToolStripMenuItem.Text = "New Server";
            this.newServerToolStripMenuItem.Click += new System.EventHandler(this.OpenClientWindowNewServer);
            // 
            // serversToolStripMenuItem
            // 
            this.serversToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Server1,
            this.Server2,
            this.Server3,
            this.Server4,
            this.Server5,
            this.ServerListSeperator,
            this.openServerListToolStripMenuItem});
            this.serversToolStripMenuItem.Name = "serversToolStripMenuItem";
            this.serversToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.serversToolStripMenuItem.Text = "Servers";
            // 
            // Server1
            // 
            this.Server1.Name = "Server1";
            this.Server1.Size = new System.Drawing.Size(159, 22);
            this.Server1.Click += new System.EventHandler(this.Server1Con);
            // 
            // Server2
            // 
            this.Server2.Name = "Server2";
            this.Server2.Size = new System.Drawing.Size(159, 22);
            this.Server2.Click += new System.EventHandler(this.Server2Con);
            // 
            // Server3
            // 
            this.Server3.Name = "Server3";
            this.Server3.Size = new System.Drawing.Size(159, 22);
            this.Server3.Click += new System.EventHandler(this.Server3Con);
            // 
            // Server4
            // 
            this.Server4.Name = "Server4";
            this.Server4.Size = new System.Drawing.Size(159, 22);
            this.Server4.Click += new System.EventHandler(this.Server4Con);
            // 
            // Server5
            // 
            this.Server5.Name = "Server5";
            this.Server5.Size = new System.Drawing.Size(159, 22);
            this.Server5.Click += new System.EventHandler(this.Server5Con);
            // 
            // ServerListSeperator
            // 
            this.ServerListSeperator.Name = "ServerListSeperator";
            this.ServerListSeperator.Size = new System.Drawing.Size(156, 6);
            // 
            // openServerListToolStripMenuItem
            // 
            this.openServerListToolStripMenuItem.Name = "openServerListToolStripMenuItem";
            this.openServerListToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.openServerListToolStripMenuItem.Text = "Open Server List";
            // 
            // disconnectToolStripMenuItem
            // 
            this.disconnectToolStripMenuItem.Name = "disconnectToolStripMenuItem";
            this.disconnectToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.disconnectToolStripMenuItem.Text = "Disconnect";
            this.disconnectToolStripMenuItem.Click += new System.EventHandler(this.DiconnectFromSelectedServer);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.preferencesToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // preferencesToolStripMenuItem
            // 
            this.preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
            this.preferencesToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.preferencesToolStripMenuItem.Text = "Preferences";
            this.preferencesToolStripMenuItem.Click += new System.EventHandler(this.OpenClientWindowPreferences);
            // 
            // ClientWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1350, 730);
            this.Controls.Add(this.textBoxEnter);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.textBoxChat);
            this.Controls.Add(this.menuStripTopNav);
            this.MainMenuStrip = this.menuStripTopNav;
            this.Name = "ClientWindow";
            this.Text = "IRC Client";
            this.Load += new System.EventHandler(this.ClientWindow_Load);
            this.menuStripTopNav.ResumeLayout(false);
            this.menuStripTopNav.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.TextBox textBoxChat;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.TextBox textBoxEnter;
        private System.Windows.Forms.MenuStrip menuStripTopNav;
        private System.Windows.Forms.ToolStripMenuItem serverToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newServerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem serversToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem preferencesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Server2;
        private System.Windows.Forms.ToolStripMenuItem Server3;
        private System.Windows.Forms.ToolStripMenuItem Server4;
        private System.Windows.Forms.ToolStripMenuItem Server5;
        public System.Windows.Forms.ToolStripMenuItem Server1;
        private System.Windows.Forms.ToolStripSeparator ServerListSeperator;
        private System.Windows.Forms.ToolStripMenuItem openServerListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disconnectToolStripMenuItem;
    }
}

