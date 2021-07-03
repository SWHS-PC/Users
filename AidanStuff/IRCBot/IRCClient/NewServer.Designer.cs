
using System.Windows.Forms;

namespace IRCClient
{
    partial class NewServer
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonNewServerSubmit = new System.Windows.Forms.Button();
            this.textBoxHostname = new System.Windows.Forms.TextBox();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.textBoxPrefNick = new System.Windows.Forms.TextBox();
            this.textBoxAltNick = new System.Windows.Forms.TextBox();
            this.labelHostname = new System.Windows.Forms.Label();
            this.labelPort = new System.Windows.Forms.Label();
            this.labelPrefNick = new System.Windows.Forms.Label();
            this.labelAltNick = new System.Windows.Forms.Label();
            this.textBoxAutoJC = new System.Windows.Forms.TextBox();
            this.labelChan = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonNewServerSubmit
            // 
            this.buttonNewServerSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonNewServerSubmit.Location = new System.Drawing.Point(89, 195);
            this.buttonNewServerSubmit.Name = "buttonNewServerSubmit";
            this.buttonNewServerSubmit.Size = new System.Drawing.Size(120, 23);
            this.buttonNewServerSubmit.TabIndex = 0;
            this.buttonNewServerSubmit.Text = "Add Server";
            this.buttonNewServerSubmit.UseVisualStyleBackColor = true;
            this.buttonNewServerSubmit.Click += new System.EventHandler(this.CloseNewServerDialogue);
            // 
            // textBoxHostname
            // 
            this.textBoxHostname.Location = new System.Drawing.Point(15, 65);
            this.textBoxHostname.Name = "textBoxHostname";
            this.textBoxHostname.Size = new System.Drawing.Size(217, 20);
            this.textBoxHostname.TabIndex = 1;
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(248, 65);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(46, 20);
            this.textBoxPort.TabIndex = 2;
            // 
            // textBoxPrefNick
            // 
            this.textBoxPrefNick.Location = new System.Drawing.Point(15, 104);
            this.textBoxPrefNick.Name = "textBoxPrefNick";
            this.textBoxPrefNick.Size = new System.Drawing.Size(99, 20);
            this.textBoxPrefNick.TabIndex = 3;
            // 
            // textBoxAltNick
            // 
            this.textBoxAltNick.Location = new System.Drawing.Point(120, 104);
            this.textBoxAltNick.Name = "textBoxAltNick";
            this.textBoxAltNick.Size = new System.Drawing.Size(100, 20);
            this.textBoxAltNick.TabIndex = 4;
            // 
            // labelHostname
            // 
            this.labelHostname.AutoSize = true;
            this.labelHostname.Location = new System.Drawing.Point(12, 49);
            this.labelHostname.Name = "labelHostname";
            this.labelHostname.Size = new System.Drawing.Size(92, 13);
            this.labelHostname.TabIndex = 5;
            this.labelHostname.Text = "Server Hostname:";
            // 
            // labelPort
            // 
            this.labelPort.AutoSize = true;
            this.labelPort.Location = new System.Drawing.Point(245, 49);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(29, 13);
            this.labelPort.TabIndex = 6;
            this.labelPort.Text = "Port:";
            // 
            // labelPrefNick
            // 
            this.labelPrefNick.AutoSize = true;
            this.labelPrefNick.Location = new System.Drawing.Point(12, 88);
            this.labelPrefNick.Name = "labelPrefNick";
            this.labelPrefNick.Size = new System.Drawing.Size(78, 13);
            this.labelPrefNick.TabIndex = 7;
            this.labelPrefNick.Text = "Preffered Nick:";
            // 
            // labelAltNick
            // 
            this.labelAltNick.AutoSize = true;
            this.labelAltNick.Location = new System.Drawing.Point(117, 88);
            this.labelAltNick.Name = "labelAltNick";
            this.labelAltNick.Size = new System.Drawing.Size(47, 13);
            this.labelAltNick.TabIndex = 8;
            this.labelAltNick.Text = "Alt Nick:";
            // 
            // textBoxAutoJC
            // 
            this.textBoxAutoJC.Location = new System.Drawing.Point(15, 143);
            this.textBoxAutoJC.Name = "textBoxAutoJC";
            this.textBoxAutoJC.Size = new System.Drawing.Size(217, 20);
            this.textBoxAutoJC.TabIndex = 9;
            // 
            // labelChan
            // 
            this.labelChan.AutoSize = true;
            this.labelChan.Location = new System.Drawing.Point(12, 127);
            this.labelChan.Name = "labelChan";
            this.labelChan.Size = new System.Drawing.Size(95, 13);
            this.labelChan.TabIndex = 10;
            this.labelChan.Text = "Autojoin Channels:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.DimGray;
            this.label1.Location = new System.Drawing.Point(238, 146);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(274, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "List channel names without # and seperated by a space.";
            // 
            // NewServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(548, 246);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelChan);
            this.Controls.Add(this.textBoxAutoJC);
            this.Controls.Add(this.labelAltNick);
            this.Controls.Add(this.labelPrefNick);
            this.Controls.Add(this.labelPort);
            this.Controls.Add(this.labelHostname);
            this.Controls.Add(this.textBoxAltNick);
            this.Controls.Add(this.textBoxPrefNick);
            this.Controls.Add(this.textBoxPort);
            this.Controls.Add(this.textBoxHostname);
            this.Controls.Add(this.buttonNewServerSubmit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "NewServer";
            this.Text = "NewServer";
            this.Load += new System.EventHandler(this.NewServer_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonNewServerSubmit;
        private System.Windows.Forms.TextBox textBoxHostname;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.TextBox textBoxPrefNick;
        private System.Windows.Forms.TextBox textBoxAltNick;
        private System.Windows.Forms.Label labelHostname;
        private System.Windows.Forms.Label labelPort;
        private System.Windows.Forms.Label labelPrefNick;
        private System.Windows.Forms.Label labelAltNick;
        private System.Windows.Forms.TextBox textBoxAutoJC;
        private System.Windows.Forms.Label labelChan;
        private Label label1;
    }
}