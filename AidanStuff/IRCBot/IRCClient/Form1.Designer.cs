﻿namespace IRCClient
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
            this.textBoxChat.Cursor = System.Windows.Forms.Cursors.Default;
            this.textBoxChat.HideSelection = false;
            this.textBoxChat.Location = new System.Drawing.Point(12, 12);
            this.textBoxChat.MaxLength = 65536;
            this.textBoxChat.Multiline = true;
            this.textBoxChat.Name = "textBoxChat";
            this.textBoxChat.ReadOnly = true;
            this.textBoxChat.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxChat.Size = new System.Drawing.Size(1010, 680);
            this.textBoxChat.TabIndex = 1;
            this.textBoxChat.Text = " ";
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Location = new System.Drawing.Point(1028, 12);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(310, 706);
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
            this.textBoxEnter.Multiline = false;
            this.textBoxEnter.Name = "textBoxEnter";
            this.textBoxEnter.Size = new System.Drawing.Size(1010, 20);
            this.textBoxEnter.TabIndex = 3;
            this.textBoxEnter.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Return);
            // 
            // ClientWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1350, 730);
            this.Controls.Add(this.textBoxEnter);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.textBoxChat);
            this.Name = "ClientWindow";
            this.Text = "IRC Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.TextBox textBoxChat;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.TextBox textBoxEnter;
        //private System.Windows.Forms.
    }
}

