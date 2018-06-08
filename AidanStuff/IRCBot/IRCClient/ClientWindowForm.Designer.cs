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
            this.textBoxEnter = new System.Windows.Forms.TextBox();
            this.menuStripTopNav = new System.Windows.Forms.MenuStrip();
            this.serverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serversToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disconnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ServerListSeperator = new System.Windows.Forms.ToolStripSeparator();
            this.openServerListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.menuStripTopNav.SuspendLayout();
            this.SuspendLayout();
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
            this.textBoxEnter.Size = new System.Drawing.Size(1326, 20);
            this.textBoxEnter.TabIndex = 3;
            this.textBoxEnter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Return);
            // 
            // menuStripTopNav
            // 
            this.menuStripTopNav.BackColor = System.Drawing.SystemColors.Control;
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
            this.newServerToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.newServerToolStripMenuItem.Text = "New Server";
            this.newServerToolStripMenuItem.Click += new System.EventHandler(this.OpenClientWindowNewServer);
            // 
            // serversToolStripMenuItem
            // 
            this.serversToolStripMenuItem.Name = "serversToolStripMenuItem";
            this.serversToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.serversToolStripMenuItem.Text = "Servers";
            // 
            // disconnectToolStripMenuItem
            // 
            this.disconnectToolStripMenuItem.Name = "disconnectToolStripMenuItem";
            this.disconnectToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
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
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Location = new System.Drawing.Point(12, 41);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1326, 651);
            this.tabControl.TabIndex = 5;
            // 
            // ClientWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1350, 730);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.textBoxEnter);
            this.Controls.Add(this.menuStripTopNav);
            this.MainMenuStrip = this.menuStripTopNav;
            this.Name = "ClientWindow";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Blade IRC";
            this.menuStripTopNav.ResumeLayout(false);
            this.menuStripTopNav.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
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

        public System.Windows.Forms.ToolStripMenuItem[] ServerItems = new System.Windows.Forms.ToolStripMenuItem[10];

        public System.Windows.Forms.TextBox[] userListChan = new System.Windows.Forms.TextBox[1000];
        public System.Windows.Forms.RichTextBox[] textBoxChat = new System.Windows.Forms.RichTextBox[1000];
        public System.Windows.Forms.TabPage[] tabPageChan = new System.Windows.Forms.TabPage[1000];
        
    }
}

