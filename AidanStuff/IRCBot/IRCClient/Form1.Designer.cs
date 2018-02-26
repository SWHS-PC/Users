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
            this.textBoxInput = new System.Windows.Forms.TextBox();
            this.listViewChannels = new System.Windows.Forms.ListView();
            this.flowLayoutPanelBackground = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanelLeft = new System.Windows.Forms.FlowLayoutPanel();
            this.textBoxChat = new System.Windows.Forms.TextBox();
            this.flowLayoutPanelBackground.SuspendLayout();
            this.flowLayoutPanelLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxInput
            // 
            this.textBoxInput.Location = new System.Drawing.Point(3, 719);
            this.textBoxInput.Name = "textBoxInput";
            this.textBoxInput.Size = new System.Drawing.Size(1055, 20);
            this.textBoxInput.TabIndex = 0;
            // 
            // listViewChannels
            // 
            this.listViewChannels.Location = new System.Drawing.Point(1069, 3);
            this.listViewChannels.Name = "listViewChannels";
            this.listViewChannels.Size = new System.Drawing.Size(265, 740);
            this.listViewChannels.TabIndex = 1;
            this.listViewChannels.UseCompatibleStateImageBehavior = false;
            // 
            // flowLayoutPanelBackground
            // 
            this.flowLayoutPanelBackground.Controls.Add(this.flowLayoutPanelLeft);
            this.flowLayoutPanelBackground.Controls.Add(this.listViewChannels);
            this.flowLayoutPanelBackground.Location = new System.Drawing.Point(12, 12);
            this.flowLayoutPanelBackground.Name = "flowLayoutPanelBackground";
            this.flowLayoutPanelBackground.Size = new System.Drawing.Size(1340, 745);
            this.flowLayoutPanelBackground.TabIndex = 2;
            // 
            // flowLayoutPanelLeft
            // 
            this.flowLayoutPanelLeft.Controls.Add(this.textBoxChat);
            this.flowLayoutPanelLeft.Controls.Add(this.textBoxInput);
            this.flowLayoutPanelLeft.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanelLeft.Name = "flowLayoutPanelLeft";
            this.flowLayoutPanelLeft.Size = new System.Drawing.Size(1060, 740);
            this.flowLayoutPanelLeft.TabIndex = 2;
            // 
            // textBoxChat
            // 
            this.textBoxChat.AcceptsReturn = true;
            this.textBoxChat.AcceptsTab = true;
            this.textBoxChat.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxChat.Location = new System.Drawing.Point(3, 3);
            this.textBoxChat.MaxLength = 65536;
            this.textBoxChat.Multiline = true;
            this.textBoxChat.Name = "textBoxChat";
            this.textBoxChat.ReadOnly = true;
            this.textBoxChat.Size = new System.Drawing.Size(1055, 710);
            this.textBoxChat.TabIndex = 1;
            this.textBoxChat.TextChanged += new System.EventHandler(this.ClientWindow_Load);
            // 
            // ClientWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1366, 768);
            this.Controls.Add(this.flowLayoutPanelBackground);
            this.Name = "ClientWindow";
            this.Text = "IRC Client";
            this.flowLayoutPanelBackground.ResumeLayout(false);
            this.flowLayoutPanelLeft.ResumeLayout(false);
            this.flowLayoutPanelLeft.PerformLayout();
            this.ResumeLayout(false);

        }

      


        private System.Windows.Forms.TextBox textBoxInput;
        private System.Windows.Forms.ListView listViewChannels;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelBackground;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelLeft;
        private System.Windows.Forms.TextBox textBoxChat;
    }
}

