using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Fcc3_configurator
{
    public partial class FormStickTracker : Form
    {
        private Point[] Trace = new Point[150];

        private System.Windows.Forms.Timer PollTimer;

        //private bool CountDownX = false;
        //private bool CountDownY = false;

        private int StickPosX = 0;
        private int StickPosY = 0;

        const Int16 MinVal = -1024;
        const Int16 MaxVal = 1023;

        FccHandeler Stick = new FccHandeler();



        public FormStickTracker()
        {
            InitializeComponent();
            GetStick();
            InitPollTimer();
        }

        public void GetStick()
        {
            if (!Stick.isConnected)
            {
                Stick.Connect();
            }
        }


        private void InitPollTimer()
        {
            PollTimer = new System.Windows.Forms.Timer();
            PollTimer.Tick += new EventHandler(PollTimer_Tick);
            PollTimer.Interval = 2; // in miliseconds
            PollTimer.Start();
        }

        private void PollTimer_Tick(object sender, EventArgs e)
        {
            try
            {

                Stick.Update();
                StickPosX = Stick.X;
                StickPosY = Stick.Y;
                for (int i = Trace.Length - 1; i > 0; i--)
                {
                    Trace[i] = Trace[i - 1];
                }  



                //Trace[0].X = map(StickPosX, Int16.MinValue, Int16.MaxValue, -100, 100);
                //Trace[0].Y = map(StickPosY,  Int16.MaxValue, Int16.MinValue, -100, 100);
                Trace[0].X = map(StickPosX, MinVal, MaxVal, -100, 100);
                Trace[0].Y = map(StickPosY, MaxVal, MinVal, -100, 100);
                labelPosX.Text = StickPosX.ToString();
                labelPosY.Text = StickPosY.ToString();

                chartStick.Series[0].Points[0] = new DataPoint(Trace[0].X,Trace[0].Y);
                chartStick.Series[1].Points.Clear();
                for (int i = 0; i < Trace.Length; i++)
                {
                    chartStick.Series[1].Points.AddXY(Trace[i].X+0.001, Trace[i].Y);
                }
            }
            catch
            {

            }
        }

        private int map(int x, int in_min, int in_max, int out_min, int out_max)
        {
                return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
            
        }

        private void buttonCenter_Click(object sender, EventArgs e)
        {
            Stick.Center();
        }
    }
}
