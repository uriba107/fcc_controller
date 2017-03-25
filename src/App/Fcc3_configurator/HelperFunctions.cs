using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using HidSharp;
using System.Drawing;
using System.Windows.Forms;
using System.IO.Ports;
using System.Diagnostics;


namespace Fcc3_configurator
{
    public partial class MainForm 
    {

        private HidDevice ConnectUSB(short VID, short PID)
        {
            var loader = new HidDeviceLoader();
            try
            {
                var device = loader.GetDevices(VID, PID).First();
                if (device != null)
                {
                    toolStripStatusLabelInfo.Text = "Connected";
                    toolStripStatusLabelColor.ForeColor = Color.Green;
                    InitRefreshTimer();
                }
                return device;
            }
            catch
            {
                toolStripStatusLabelInfo.Text = "No Device Connected";
                toolStripStatusLabelColor.ForeColor = Color.Red;
                return null;
            }
        }

        private void SendToStick(HidDevice device, byte options, short forceSetting)
        {
            byte upper = (byte)(forceSetting >> 8);
            byte lower = (byte)(forceSetting & 0xff);

            try
            {
                if (device != null)
                {
                    byte[] OutData = new byte[device.MaxOutputReportLength];
                    OutData[0] = 0; // Report ID
                    OutData[1] = options;
                    OutData[2] = upper;
                    OutData[3] = lower;
                    HidStream stream;
                    device.TryOpen(out stream);
                    stream.Write(OutData);
                }
            }
            catch
            {
                MessageBox.Show("Failed to send to device..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private byte[] ReadFromStick(HidDevice device)
        {
            byte[] Retval = new byte[3];

            if (device != null)
            {
                HidStream stream;
                byte[] buff = new byte[device.MaxInputReportLength];
                device.TryOpen(out stream);
                stream.Read(buff);
                Retval[0] = buff[7];
                Retval[1] = buff[8];
                Retval[2] = buff[9];
            }
            return Retval;

        }
        private Int16 CalcForce()
        {
            // Hardware: 9kg/f = 2Volt Delta
            // ADC: 5V = 4096
            // Middle Voltage: 2.5V

            const float DeltaVol = 2.0F;
            const float ForceRef = 9.0F;
            const float AdcMax = 4095.0F;
            const float Vref = 5.0F;
            const float BaseVoltage = 2.5F;

            decimal selectedVal = numericUserDefined.Value;
            if (!isUnitKg)
            {
                selectedVal /= Kg2lb;
            }
            float voltage = ((DeltaVol / ForceRef) * (float)selectedVal) + BaseVoltage;

            float Retval = ((AdcMax / Vref) * voltage) - (AdcMax / 2);

            return (Int16)Retval;
        }

        private decimal GetForce(Int16 force)
        {
            // Hardware: 9kg/f = 2Volt Delta
            // ADC: 5V = 4096
            // Middle Voltage: 2.5V

            const float DeltaVol = 2.0F;
            const float ForceRef = 9.0F;
            const float AdcMax = 4095.0F;
            const float Vref = 5.0F;
            const float BaseVoltage = 2.5F;

            float Retval = (((force + (AdcMax / 2)) * (Vref / AdcMax)) - BaseVoltage) * (ForceRef / DeltaVol);

            return (decimal)Math.Round(Retval, 2);
        }

        private Byte MakeOptions()
        {
            Byte RetVal = 0;
            ConfigOptions options = 0;
            if (checkBoxForceMapping.Checked)
            {
                options |= (ConfigOptions.ForceMap);
            }
            if (checkBoxRotate.Checked)
            {
                options |= ConfigOptions.RotatedSensors;
            }
            if (radioButton4Kg.Checked)
            {
                options |= ConfigOptions.Force4Kg;
            }
            if (radioButton6Kg.Checked)
            {
                options |= ConfigOptions.Force6Kg;
            }
            if (radioButton9Kg.Checked)
            {
                options |= ConfigOptions.Force9Kg;
            }
            if (radioButtonUser.Checked)
            {
                options |= ConfigOptions.ForceUserDefined;
            }
            RetVal = (byte)options;
            return RetVal;
        }
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
                ShowCurrentValues();
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
            UsbTimer.Interval = 2000; // in miliseconds
            UsbTimer.Start();
        }

        private void UsbTimer_Tick(object sender, EventArgs e)
        {
            if (Stick == null)
            {
                Stick = ConnectUSB(VID, PID);
            }

        }
        private string AutoDetectNewPort(string[] InitialPorts)
        {
            short Counter = 0;
            List<string> ProgramingPort = new List<string>();
            while (ProgramingPort.Count() == 0)
            {
                Thread.Sleep(50);
                string[] updatedPorts = SerialPort.GetPortNames();
                ProgramingPort = updatedPorts.Except(InitialPorts).ToList();
                Counter += 50;
                if (Counter > 5000)
                {
                    return "";
                }
            }
            return ProgramingPort[0];
        }

        private void uploadHex(string ComPort, string HexPath)
        {
            if (HexPath == "")
            {
                MessageBox.Show("No HEX file was selected");
                return;
            }

            //var uploader = new ArduinoSketchUploader(
            //new ArduinoSketchUploaderOptions()
            //{
            //    FileName = HexPath,
            //    PortName = ComPort,
            //    ArduinoModel = ArduinoModel.Micro,
            //});

            //uploader.UploadSketch();

            string AvrDudePath = "Avrdude";
            string AvrDudeBin = AvrDudePath + "\\avrdude.exe";
            string AvrDudeParams = "-v -patmega32u4 -cavr109  -P" + ComPort + " -b57600 -D -Uflash:w:\"" + HexPath + "\":i -C " + AvrDudePath + "\\avrdude.conf";
            //string AvrDudeBin = AvrDudePath + "\\upload_code.bat";
            //string AvrDudeParams = " " + ComPort + " " + HexPath;

            //Console.WriteLine(AvrDudeBin + " " + AvrDudeParams);
            try
            {
                Process p = new Process();

                p.StartInfo.FileName = AvrDudeBin;
                p.StartInfo.Arguments = AvrDudeParams;
                p.StartInfo.RedirectStandardError = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.UseShellExecute = false;
                p.Start();
                //string output = p.StandardOutput.ReadToEnd();
                //Console.WriteLine(output);
                //string err = p.StandardError.ReadToEnd();
                //Console.WriteLine(err);

                for (int i = 0; i < 15; i++)
                {
                    if (!p.HasExited)
                    {
                        p.Refresh();
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        break;
                    }
                }
                //p.WaitForExit(15000);
            }
            catch (Exception e)
            {
                MessageBox.Show("The Following Exception was raised:\n"+e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string BdcDecode(int VersionBin)
        {
            //#define VERSION_BCD(Major, Minor, Revision) \
            //            CPU_TO_LE16(((Major & 0xFF) << 8) | \

            //                         ((Minor & 0x0F) << 4) | \

            //                         (Revision & 0x0F))

            //byte inMajor = 4;
            //byte inMinor = 2;
            //byte inRevision = 14;

            //VersionBin = (((inMajor & 0xFF) << 8) | ((inMinor & 0x0F) << 4) | (inRevision & 0x0F));
            //Console.WriteLine(VersionBin);

            byte lower = (byte)(VersionBin & 0xFF);

            byte Major = (byte)(VersionBin >> 8);
            byte Minor = (byte)(lower >> 4);
            byte Revision = (byte)(lower & 0x0F);


            string RetVal = Major + "." + Minor + "." + Revision;
            return RetVal;
        }
    }
}
