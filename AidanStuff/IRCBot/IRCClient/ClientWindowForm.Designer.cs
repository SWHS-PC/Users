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
            this.textBoxServer1 = new System.Windows.Forms.TextBox();
            this.textBoxUsers = new System.Windows.Forms.TextBox();
            this.textBoxEnter = new System.Windows.Forms.TextBox();
            this.menuStripTopNav = new System.Windows.Forms.MenuStrip();
            this.serverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serversToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ServerListSeperator = new System.Windows.Forms.ToolStripSeparator();
            this.openServerListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disconnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageServer1 = new System.Windows.Forms.TabPage();
            this.menuStripTopNav.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPageServer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxServer1
            // 
            this.textBoxServer1.AcceptsReturn = true;
            this.textBoxServer1.AcceptsTab = true;
            this.textBoxServer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxServer1.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxServer1.CausesValidation = false;
            this.textBoxServer1.HideSelection = false;
            this.textBoxServer1.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.textBoxServer1.Location = new System.Drawing.Point(0, 0);
            this.textBoxServer1.MaxLength = 65536;
            this.textBoxServer1.Multiline = true;
            this.textBoxServer1.Name = "textBoxServer1";
            this.textBoxServer1.ReadOnly = true;
            this.textBoxServer1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxServer1.Size = new System.Drawing.Size(1060, 628);
            this.textBoxServer1.TabIndex = 1;
            this.textBoxServer1.Text = "Select a Server or type /server <ipaddress> <port> \r\n";
            // 
            // textBoxUsers
            // 
            this.textBoxUsers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxUsers.Location = new System.Drawing.Point(1086, 38);
            this.textBoxUsers.Name = "textBoxUsers";
            this.textBoxUsers.Size = new System.Drawing.Size(252, 680);
            this.textBoxUsers.TabIndex = 2;
            this.textBoxUsers.Text = "";
            this.textBoxUsers.AcceptsReturn = true;
            this.textBoxUsers.AcceptsTab = true;
            this.textBoxUsers.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxUsers.CausesValidation = false;
            this.textBoxUsers.HideSelection = false;
            this.textBoxUsers.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.textBoxUsers.MaxLength = 65536;
            this.textBoxUsers.Multiline = true;
            this.textBoxUsers.ReadOnly = true;
            this.textBoxUsers.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxUsers.Text = "";
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
            this.serversToolStripMenuItem.Name = "serversToolStripMenuItem";
            this.serversToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.serversToolStripMenuItem.Text = "Servers";
            
           
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
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageServer1);
            this.tabControl.Location = new System.Drawing.Point(12, 38);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1068, 654);
            this.tabControl.TabIndex = 5;
            // 
            // tabPageServer1
            // 
            this.tabPageServer1.Controls.Add(this.textBoxServer1);
            this.tabPageServer1.Location = new System.Drawing.Point(4, 22);
            this.tabPageServer1.Name = "tabPageServer1";
            this.tabPageServer1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageServer1.Size = new System.Drawing.Size(1060, 628);
            this.tabPageServer1.TabIndex = 0;
            this.tabPageServer1.UseVisualStyleBackColor = true;
            // 
            // ClientWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1350, 730);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.textBoxEnter);
            this.Controls.Add(this.textBoxUsers);
            this.Controls.Add(this.menuStripTopNav);
            this.MainMenuStrip = this.menuStripTopNav;
            this.Name = "ClientWindow";
            this.Text = "IRC Client";
            this.menuStripTopNav.ResumeLayout(false);
            this.menuStripTopNav.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabPageServer1.ResumeLayout(false);
            this.tabPageServer1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.TextBox textBoxServer1;
        private System.Windows.Forms.TextBox textBoxUsers;
        private System.Windows.Forms.TextBox textBoxEnter;
        private System.Windows.Forms.MenuStrip menuStripTopNav;
        private System.Windows.Forms.ToolStripMenuItem serverToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newServerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem serversToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem preferencesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator ServerListSeperator;
        private System.Windows.Forms.ToolStripMenuItem openServerListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disconnectToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageServer1;

        public System.Windows.Forms.ToolStripMenuItem[] ServerItems = new System.Windows.Forms.ToolStripMenuItem[10];

        public System.Windows.Forms.TextBox[] textBoxServer1Chan = new System.Windows.Forms.TextBox[1000];
        public System.Windows.Forms.TabPage[] tabPageServer1Chan = new System.Windows.Forms.TabPage[1000];


    }
}

