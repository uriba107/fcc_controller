using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using HidSharp;
using System.Drawing;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
using System.Diagnostics;
using GithubHelper;
using HexHelper;

namespace Fcc3_configurator
{
    public partial class MainForm
    {
        private GithubUpdater updater = new GithubUpdater("uriba107", "fcc_controller");
        private HexUpdater uploader = new HexUpdater();
        private bool isNotified = false;



        public void InitRefreshTimer()
        {
            RefreshTimer = new System.Windows.Forms.Timer();
            RefreshTimer.Tick += new EventHandler(RefreshTimer_Tick);
            RefreshTimer.Interval = 250; // in miliseconds
            RefreshTimer.Start();
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                Stick.Update();
                ShowCurrentValues();
                RefreshUpdateTab();
            }
            catch
            {
                Console.WriteLine("Lost the device");
                RefreshTimer.Stop();
                Stick = null;

            }
        }

        public void InitUsbTimer()
        {
            UsbTimer = new System.Windows.Forms.Timer();
            UsbTimer.Tick += new EventHandler(UsbTimer_Tick);
            UsbTimer.Interval = 1000; // in miliseconds
            UsbTimer.Start();
        }

        private void UsbTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Stick == null)
                {
                    Stick = new FccHandeler();
                }
                if (!Stick.isConnected)
                {
                    Stick.Connect();
                    InitRefreshTimer();
                }

            }
            catch
            {
                Console.WriteLine("here we are");
            }
        }


        private Byte MakeOptions()
        {
            Byte RetVal = 0;
            FccHandeler.ConfigOptions options = 0;
            if (checkBoxForceMapping.Checked)
            {
                if (radioButtonAnalogFlcs.Checked)
                {
                    options |= (FccHandeler.ConfigOptions.AnaloglFlcs);
                }
                if (radioButtonDigitalFlcs.Checked)
                {
                    options |= (FccHandeler.ConfigOptions.DigitalFlcs);
                }
            }
            if (checkBoxRotate.Checked)
            {
                options |= FccHandeler.ConfigOptions.RotatedSensors;
            }
            if (radioButton4Kg.Checked)
            {
                options |= FccHandeler.ConfigOptions.Force4Kg;
            }
            if (radioButton6Kg.Checked)
            {
                options |= FccHandeler.ConfigOptions.Force6Kg;
            }
            if (radioButton9Kg.Checked)
            {
                options |= FccHandeler.ConfigOptions.Force9Kg;
            }
            if (radioButtonUser.Checked)
            {
                options |= FccHandeler.ConfigOptions.ForceUserDefined;
            }
            RetVal = (byte)options;
            return RetVal;
        }


        private bool UpgradeFirmware(string HexPath)
        {
            bool isSuccess = false;

            //List initial ports
            if (Stick.isConnected)
            {
                //RefreshTimer.Stop();
                //UsbTimer.Stop();
                isSuccess = Stick.UpgradeFirmware(HexPath);
            }
            //InitUsbTimer();
   
            return isSuccess;
        }


        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                RunUpdateManager();
            }
            catch
            {
                return;
            }
        }

        public void InitUpdateTimer()
        {
            RunUpdateManager();
            UpdateTimer = new System.Windows.Forms.Timer();
            UpdateTimer.Tick += new EventHandler(UpdateTimer_Tick);
            UpdateTimer.Interval = (int)TimeSpan.FromDays(7).TotalMilliseconds;
            //UpdateTimer.Interval = 7*24*60*60*1000; // 7 days * 24hours * 60 Mins * 60 Secs - in miliseconds
            UpdateTimer.Start();
        }
        public void RunUpdateManager()
        {
            isNotified = false;
            Thread worker = new System.Threading.Thread(delegate ()
            {
                updater.Update();

                //bool result = updater.GetFirmware(@"C:\Temp1");
                //Console.WriteLine(result);
            });
            worker.Start();
        }


    }
}
