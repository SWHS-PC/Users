﻿using System.Drawing;

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
            this.PictureBoxMap = new System.Windows.Forms.PictureBox();
            this.buttonSet = new System.Windows.Forms.Button();
            this.buttonNext = new System.Windows.Forms.Button();
            this.buttonStartAuto = new System.Windows.Forms.Button();
            this.labelInfo = new System.Windows.Forms.Label();
            this.labelGeneration = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxMap)).BeginInit();
            this.SuspendLayout();
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
            this.buttonSet.Location = new System.Drawing.Point(12, 363);
            this.buttonSet.Name = "buttonSet";
            this.buttonSet.Size = new System.Drawing.Size(75, 23);
            this.buttonSet.TabIndex = 1;
            this.buttonSet.Text = "Set";
            this.buttonSet.UseVisualStyleBackColor = true;
            this.buttonSet.Click += new System.EventHandler(this.DrawClick);
            // 
            // buttonNext
            // 
            this.buttonNext.Location = new System.Drawing.Point(93, 363);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(75, 23);
            this.buttonNext.TabIndex = 2;
            this.buttonNext.Text = "Next";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.DrawNext);
            // 
            // buttonStartAuto
            // 
            this.buttonStartAuto.Location = new System.Drawing.Point(174, 363);
            this.buttonStartAuto.Name = "buttonStartAuto";
            this.buttonStartAuto.Size = new System.Drawing.Size(75, 23);
            this.buttonStartAuto.TabIndex = 3;
            this.buttonStartAuto.Text = "Start";
            this.buttonStartAuto.UseVisualStyleBackColor = true;
            this.buttonStartAuto.Click += new System.EventHandler(this.AutoRun);
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
            this.labelGeneration.Location = new System.Drawing.Point(256, 368);
            this.labelGeneration.Name = "labelGeneration";
            this.labelGeneration.Size = new System.Drawing.Size(62, 13);
            this.labelGeneration.TabIndex = 5;
            this.labelGeneration.Text = "Generation:";
            // 
            // DrawForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(861, 410);
            this.Controls.Add(this.labelGeneration);
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.buttonStartAuto);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.buttonSet);
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
        private System.Windows.Forms.Button buttonSet;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Button buttonStartAuto;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.Label labelGeneration;
    }
}

