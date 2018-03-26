using System.Drawing;
using System.Windows.Forms;

namespace LifeGui
{
    partial class DrawForm
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
            this.buttonNext = new System.Windows.Forms.Button();
            this.labelInfo = new System.Windows.Forms.Label();
            this.labelGeneration = new System.Windows.Forms.Label();
            this.PictureBoxMap = new System.Windows.Forms.PictureBox();
            this.buttonSet = new System.Windows.Forms.Button();
            this.buttonStart = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxMap)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonNext
            // 
            this.buttonNext.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.buttonNext.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonNext.Location = new System.Drawing.Point(174, 363);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(75, 23);
            this.buttonNext.TabIndex = 2;
            this.buttonNext.Text = "Next";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.DrawNext);
            // 
            // labelInfo
            // 
            this.labelInfo.AutoSize = true;
            this.labelInfo.Location = new System.Drawing.Point(582, 368);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(266, 13);
            this.labelInfo.TabIndex = 4;
            this.labelInfo.Text = "Conway\'s Game of Life - Remade In C# by Aidan Dona";
            // 
            // labelGeneration
            // 
            this.labelGeneration.AutoSize = true;
            this.labelGeneration.Location = new System.Drawing.Point(255, 368);
            this.labelGeneration.Name = "labelGeneration";
            this.labelGeneration.Size = new System.Drawing.Size(62, 13);
            this.labelGeneration.TabIndex = 5;
            this.labelGeneration.Text = "Generation:";
            this.labelGeneration.Click += new System.EventHandler(this.labelGeneration_Click);
            // 
            // PictureBoxMap
            // 
            this.PictureBoxMap.BackColor = System.Drawing.Color.White;
            this.PictureBoxMap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PictureBoxMap.Location = new System.Drawing.Point(9, 9);
            this.PictureBoxMap.Margin = new System.Windows.Forms.Padding(0);
            this.PictureBoxMap.Name = "PictureBoxMap";
            this.PictureBoxMap.Size = new System.Drawing.Size(842, 332);
            this.PictureBoxMap.TabIndex = 0;
            this.PictureBoxMap.TabStop = false;
            this.PictureBoxMap.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ClickCell);
            // 
            // buttonSet
            // 
            this.buttonSet.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.buttonSet.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonSet.Location = new System.Drawing.Point(12, 363);
            this.buttonSet.Name = "buttonSet";
            this.buttonSet.Size = new System.Drawing.Size(75, 23);
            this.buttonSet.TabIndex = 6;
            this.buttonSet.Text = "Set";
            this.buttonSet.UseVisualStyleBackColor = true;
            this.buttonSet.Click += new System.EventHandler(this.DrawClick);
            // 
            // buttonStart
            // 
            this.buttonStart.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.buttonStart.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonStart.Location = new System.Drawing.Point(93, 363);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 23);
            this.buttonStart.TabIndex = 7;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.AutoRun);
            // 
            // DrawForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(861, 410);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.buttonSet);
            this.Controls.Add(this.labelGeneration);
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.PictureBoxMap);
            this.Name = "DrawForm";
            this.Text = "Conway\'s Game Of Life";
            this.Load += new System.EventHandler(this.DrawForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxMap)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox PictureBoxMap;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.Label labelGeneration;
        private Button buttonSet;
        private Button buttonStart;
    }
}

