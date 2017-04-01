using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Diagnostics;

namespace HexHelper
{
    public class HexUpdater
    {
        public string[] AvailablePorts
        {
            get { return SerialPort.GetPortNames(); }
        }

        public void TriggerBootLoader(string ComPort)
        {
            SerialPort _serialPort = new SerialPort();
            _serialPort.PortName = ComPort;
            _serialPort.BaudRate = 1200;
            _serialPort.Open();
            Thread.Sleep(50);
            _serialPort.Close();
            Thread.Sleep(50);
        }

        public bool UploadToArduino(string ComPort, string HexFile)
        {
            bool isSuccess = false;
            string[] initialPorts = AvailablePorts;

            TriggerBootLoader(ComPort);

            string ProgPort = AutoDetectNewPort(initialPorts);
            if (ProgPort != "")
            {
                isSuccess = uploadHex(ProgPort, HexFile);
            }
            return isSuccess;
        }

        public string AutoDetectNewPort(string[] InitialPorts)
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

        public bool uploadHex(string ComPort, string HexPath)
        {
            if (HexPath == "")
            {
                //MessageBox.Show("No HEX file was selected");
                return false;
            }

            if (!File.Exists(HexPath))
            {
                //MessageBox.Show("Could not read Hex file");
                return false;
            }

            string AvrDudePath = "Avrdude";
            string AvrDudeBin = AvrDudePath + "\\avrdude.exe";
            string AvrDudeParams = "-v -patmega32u4 -cavr109  -P" + ComPort + " -b57600 -D -Uflash:w:\"" + HexPath + "\":i -C " + AvrDudePath + "\\avrdude.conf";

            try
            {
                Process p = new Process();

                p.StartInfo.FileName = AvrDudeBin;
                p.StartInfo.Arguments = AvrDudeParams;
                p.StartInfo.RedirectStandardError = false;
                p.StartInfo.RedirectStandardOutput = false;
                p.StartInfo.UseShellExecute = false;
                p.Start();


                for (int i = 0; i < 15; i++)
                {
                    if (!p.HasExited)
                    {
                        p.Refresh();
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        int RetVal = p.ExitCode;
                        //string output = p.StandardOutput.ReadToEnd();
                        //File.WriteAllText(AvrDudePath + @"\upload.log", output);
                        if (RetVal == 0)
                        {
                            return true;
                        }
                        else
                        {                
                            //string err = p.StandardError.ReadToEnd();
                            //File.WriteAllText(AvrDudePath + @"\upload.err", output);
                            return false;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show("The Following Exception was raised:\n" + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return false;
        }

    }
}
