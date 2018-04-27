using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FPSCounter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            FPS.Text = "0";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
        private void Run()
        {
            while(true){
                FPS.Text = Convert.ToString(GetFps());

            }
        }

        DateTime _lastCheckTime = DateTime.Now;
        long _frameCount = 0;

        void OnMapUpdated()
        {
            Interlocked.Increment(ref _frameCount);
        }

        double GetFps()
        {
            double secondsElapsed = (DateTime.Now - _lastCheckTime).TotalSeconds;
            long count = Interlocked.Exchange(ref _frameCount, 0);
            double fps = count / secondsElapsed;
            _lastCheckTime = DateTime.Now;
            return fps;
        }
    }
}
