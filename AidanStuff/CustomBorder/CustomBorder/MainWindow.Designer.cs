namespace CustomBorder
{
    partial class MainWindow
    {
 
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        //

        public System.ComponentModel.ComponentResourceManager resources;
        private void InitializeComponent()
        {
            resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.Frame = new System.Windows.Forms.Panel();
            this.MinimizeButton = new System.Windows.Forms.PictureBox();
            this.MaximizeButton = new System.Windows.Forms.PictureBox();
            this.CloseButton = new System.Windows.Forms.PictureBox();
            this.Frame.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MinimizeButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaximizeButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CloseButton)).BeginInit();
            this.SuspendLayout();
            // 
            // Frame
            // 
            this.Frame.BackColor = System.Drawing.Color.White;
            this.Frame.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.Frame, "Frame");
            this.Frame.Name = "Frame";
            this.Frame.Paint += new System.Windows.Forms.PaintEventHandler(this.FrameBorderColor);
            // 
            // MinimizeButton
            // 
            this.MinimizeButton.BackColor = System.Drawing.Color.Transparent;
            this.MinimizeButton.Image = global::CustomBorder.Properties.Resources.Minimize;
            resources.ApplyResources(this.MinimizeButton, "MinimizeButton");
            this.MinimizeButton.Name = "MinimizeButton";
            this.MinimizeButton.TabStop = false;
            this.MinimizeButton.Click += new System.EventHandler(this.MinimizeButton_Click);
            // 
            // MaximizeButton
            // 
            this.MaximizeButton.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.MaximizeButton, "MaximizeButton");
            this.MaximizeButton.Name = "MaximizeButton";
            this.MaximizeButton.TabStop = false;
            this.MaximizeButton.Click += new System.EventHandler(this.MaximizeButton_Click);
            // 
            // CloseButton
            // 
            this.CloseButton.BackColor = System.Drawing.Color.Transparent;
            this.CloseButton.Image = global::CustomBorder.Properties.Resources.Exit;
            resources.ApplyResources(this.CloseButton, "CloseButton");
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.TabStop = false;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // webBrowser1
            // 
            
            // 
            // MainWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.MinimizeButton);
            this.Controls.Add(this.MaximizeButton);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.Frame);
            this.Name = "MainWindow";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);
            this.Frame.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MinimizeButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaximizeButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CloseButton)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel Frame;
        private System.Windows.Forms.PictureBox CloseButton;
        private System.Windows.Forms.PictureBox MaximizeButton;
        private System.Windows.Forms.PictureBox MinimizeButton;
    }
}

