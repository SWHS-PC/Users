namespace FlagGen
{
    partial class MainWindow
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.pictureBoxFlag = new System.Windows.Forms.PictureBox();
            this.buttonGen = new System.Windows.Forms.Button();
            this.tabControlFlagOptions = new System.Windows.Forms.TabControl();
            this.tabPage1Style = new System.Windows.Forms.TabPage();
            this.tabPage2Colors = new System.Windows.Forms.TabPage();
            this.tabPage3Misc = new System.Windows.Forms.TabPage();
            this.listView1 = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFlag)).BeginInit();
            this.tabControlFlagOptions.SuspendLayout();
            this.tabPage1Style.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBoxFlag
            // 
            this.pictureBoxFlag.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxFlag.Location = new System.Drawing.Point(12, 12);
            this.pictureBoxFlag.Name = "pictureBoxFlag";
            this.pictureBoxFlag.Size = new System.Drawing.Size(652, 332);
            this.pictureBoxFlag.TabIndex = 0;
            this.pictureBoxFlag.TabStop = false;
            // 
            // buttonGen
            // 
            this.buttonGen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonGen.Location = new System.Drawing.Point(13, 426);
            this.buttonGen.Name = "buttonGen";
            this.buttonGen.Size = new System.Drawing.Size(75, 23);
            this.buttonGen.TabIndex = 1;
            this.buttonGen.Text = "Generate";
            this.buttonGen.UseVisualStyleBackColor = true;
            this.buttonGen.Click += new System.EventHandler(this.buttonGen_Click);
            // 
            // tabControlFlagOptions
            // 
            this.tabControlFlagOptions.Controls.Add(this.tabPage1Style);
            this.tabControlFlagOptions.Controls.Add(this.tabPage2Colors);
            this.tabControlFlagOptions.Controls.Add(this.tabPage3Misc);
            this.tabControlFlagOptions.Location = new System.Drawing.Point(670, 12);
            this.tabControlFlagOptions.Name = "tabControlFlagOptions";
            this.tabControlFlagOptions.SelectedIndex = 0;
            this.tabControlFlagOptions.Size = new System.Drawing.Size(302, 332);
            this.tabControlFlagOptions.TabIndex = 2;
            // 
            // tabPage1Style
            // 
            this.tabPage1Style.Controls.Add(this.label1);
            this.tabPage1Style.Controls.Add(this.listView1);
            this.tabPage1Style.Location = new System.Drawing.Point(4, 22);
            this.tabPage1Style.Name = "tabPage1Style";
            this.tabPage1Style.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1Style.Size = new System.Drawing.Size(294, 306);
            this.tabPage1Style.TabIndex = 0;
            this.tabPage1Style.Text = "Style";
            this.tabPage1Style.UseVisualStyleBackColor = true;
            // 
            // tabPage2Colors
            // 
            this.tabPage2Colors.Location = new System.Drawing.Point(4, 22);
            this.tabPage2Colors.Name = "tabPage2Colors";
            this.tabPage2Colors.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2Colors.Size = new System.Drawing.Size(294, 306);
            this.tabPage2Colors.TabIndex = 1;
            this.tabPage2Colors.Text = "Colors";
            this.tabPage2Colors.UseVisualStyleBackColor = true;
            // 
            // tabPage3Misc
            // 
            this.tabPage3Misc.Location = new System.Drawing.Point(4, 22);
            this.tabPage3Misc.Name = "tabPage3Misc";
            this.tabPage3Misc.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3Misc.Size = new System.Drawing.Size(294, 306);
            this.tabPage3Misc.TabIndex = 2;
            this.tabPage3Misc.Text = "Misc.";
            this.tabPage3Misc.UseVisualStyleBackColor = true;
            // 
            // listView1
            // 
            this.listView1.Location = new System.Drawing.Point(6, 26);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(282, 57);
            this.listView1.SmallImageList = this.imageList1;
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.SmallIcon;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Flag Styles";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "next.png");
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(984, 461);
            this.Controls.Add(this.tabControlFlagOptions);
            this.Controls.Add(this.buttonGen);
            this.Controls.Add(this.pictureBoxFlag);
            this.Name = "MainWindow";
            this.Text = "Flag Generator";
            this.Load += new System.EventHandler(this.FlagGenWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFlag)).EndInit();
            this.tabControlFlagOptions.ResumeLayout(false);
            this.tabPage1Style.ResumeLayout(false);
            this.tabPage1Style.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxFlag;
        private System.Windows.Forms.Button buttonGen;
        private System.Windows.Forms.TabControl tabControlFlagOptions;
        private System.Windows.Forms.TabPage tabPage1Style;
        private System.Windows.Forms.TabPage tabPage2Colors;
        private System.Windows.Forms.TabPage tabPage3Misc;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ImageList imageList1;
    }
}

