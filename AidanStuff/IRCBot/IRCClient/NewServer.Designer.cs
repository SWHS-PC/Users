
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.labelChan = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonNewServerSubmit
            // 
            this.buttonNewServerSubmit.Location = new System.Drawing.Point(116, 234);
            this.buttonNewServerSubmit.Name = "buttonNewServerSubmit";
            this.buttonNewServerSubmit.Size = new System.Drawing.Size(75, 23);
            this.buttonNewServerSubmit.TabIndex = 0;
            this.buttonNewServerSubmit.Text = "Add Server";
            this.buttonNewServerSubmit.UseVisualStyleBackColor = true;
            this.buttonNewServerSubmit.Click += new System.EventHandler(this.CloseNewServerDialogue);
            // 
            // textBoxHostname
            // 
            this.textBoxHostname.Location = new System.Drawing.Point(107, 46);
            this.textBoxHostname.Name = "textBoxHostname";
            this.textBoxHostname.Size = new System.Drawing.Size(217, 20);
            this.textBoxHostname.TabIndex = 1;
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(362, 46);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(46, 20);
            this.textBoxPort.TabIndex = 2;
            // 
            // textBoxPrefNick
            // 
            this.textBoxPrefNick.Location = new System.Drawing.Point(107, 81);
            this.textBoxPrefNick.Name = "textBoxPrefNick";
            this.textBoxPrefNick.Size = new System.Drawing.Size(100, 20);
            this.textBoxPrefNick.TabIndex = 3;
            // 
            // textBoxAltNick
            // 
            this.textBoxAltNick.Location = new System.Drawing.Point(264, 81);
            this.textBoxAltNick.Name = "textBoxAltNick";
            this.textBoxAltNick.Size = new System.Drawing.Size(100, 20);
            this.textBoxAltNick.TabIndex = 4;
            // 
            // labelHostname
            // 
            this.labelHostname.AutoSize = true;
            this.labelHostname.Location = new System.Drawing.Point(12, 49);
            this.labelHostname.Name = "labelHostname";
            this.labelHostname.Size = new System.Drawing.Size(89, 13);
            this.labelHostname.TabIndex = 5;
            this.labelHostname.Text = "Server Hostname";
            // 
            // labelPort
            // 
            this.labelPort.AutoSize = true;
            this.labelPort.Location = new System.Drawing.Point(330, 49);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(26, 13);
            this.labelPort.TabIndex = 6;
            this.labelPort.Text = "Port";
            // 
            // labelPrefNick
            // 
            this.labelPrefNick.AutoSize = true;
            this.labelPrefNick.Location = new System.Drawing.Point(12, 84);
            this.labelPrefNick.Name = "labelPrefNick";
            this.labelPrefNick.Size = new System.Drawing.Size(75, 13);
            this.labelPrefNick.TabIndex = 7;
            this.labelPrefNick.Text = "Preffered Nick";
            // 
            // labelAltNick
            // 
            this.labelAltNick.AutoSize = true;
            this.labelAltNick.Location = new System.Drawing.Point(214, 83);
            this.labelAltNick.Name = "labelAltNick";
            this.labelAltNick.Size = new System.Drawing.Size(44, 13);
            this.labelAltNick.TabIndex = 8;
            this.labelAltNick.Text = "Alt Nick";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(107, 119);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 9;
            // 
            // labelChan
            // 
            this.labelChan.AutoSize = true;
            this.labelChan.Location = new System.Drawing.Point(12, 122);
            this.labelChan.Name = "labelChan";
            this.labelChan.Size = new System.Drawing.Size(30, 13);
            this.labelChan.TabIndex = 10;
            this.labelChan.Text = "temp";
            // 
            // NewServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(665, 288);
            this.Controls.Add(this.labelChan);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.labelAltNick);
            this.Controls.Add(this.labelPrefNick);
            this.Controls.Add(this.labelPort);
            this.Controls.Add(this.labelHostname);
            this.Controls.Add(this.textBoxAltNick);
            this.Controls.Add(this.textBoxPrefNick);
            this.Controls.Add(this.textBoxPort);
            this.Controls.Add(this.textBoxHostname);
            this.Controls.Add(this.buttonNewServerSubmit);
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
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label labelChan;
    }
}