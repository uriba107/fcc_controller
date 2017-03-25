using System;

using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.IO.Ports;
using HidSharp;



namespace Fcc3_configurator
{
    public partial class MainForm : Form
    {
        const decimal Kg2lb = 2.20462M;

        const short VID = 0x1029;
        const short PID = 0x2345;

        public HidDevice Stick;

        private System.Windows.Forms.Timer RefreshTimer;
        private System.Windows.Forms.Timer UsbTimer;

        private bool isUnitKg;

        [Flags]
        public enum ConfigOptions
        {
            RotatedSensors = 0x01,
            ForceMap = 0x02,
            CenterStick = 0x04,
            RebootDevice = 0x08,
            Force4Kg = 0x10,
            Force6Kg = 0x20,
            Force9Kg = 0x40,
            ForceUserDefined = 0x80,
            ForceAll = 0xF0,
        };


        public MainForm()
        {
            InitializeComponent();
            this.Text += " - " + Application.ProductVersion;
            LoadLastSettings();
            Stick = ConnectUSB(VID, PID);
            InitUsbTimer();
        }

        // Button functions

        private void buttonApply_Click(object sender, EventArgs e)
        {
            buttonApply.Enabled = false;
            SaveCurrentSettings();
            SendToStick(Stick,MakeOptions(), CalcForce());
            buttonApply.Enabled = true;
        }

        private void buttonDefaults_Click(object sender, EventArgs e)
        {
            ResetStickDefaults();
        }

        private void buttonUpdateFw_Click(object sender, EventArgs e)
        {

            // TODO: Add COM port detection stuff here
            buttonUpdateFw.Enabled = false;

            //List initial ports

            string HexPath = textBoxHexPath.Text;
            
            string[] AvailablePorts = SerialPort.GetPortNames();

            // Trigger reboot for COM port to appear
            if (Stick != null)
            {
                RefreshTimer.Stop();
                UsbTimer.Stop();
                SendToStick(Stick, (byte)ConfigOptions.RebootDevice, 0);
            }

            Stick = null;

            string ProgPort = AutoDetectNewPort(AvailablePorts);
            if (ProgPort != "")
            {
                uploadHex(ProgPort, HexPath);
            }
            // TODO: Add once COM port detected, fire AVRdude
            buttonUpdateFw.Enabled = true;

            InitUsbTimer();
        }

        private void buttonBrowsHex_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialogFirmWare = new OpenFileDialog();
            openFileDialogFirmWare.Filter = "HEX Files|*.hex";
            openFileDialogFirmWare.Title = "Select a HEX File";

            // Show the Dialog.
            // If the user clicked OK in the dialog and
            // a .CUR file was selected, open it.
            if (openFileDialogFirmWare.ShowDialog() == DialogResult.OK)
            {
                textBoxHexPath.Text = openFileDialogFirmWare.FileName;
            }
        }

        private void buttonCenter_Click(object sender, EventArgs e)
        {
            SendToStick(Stick, (byte)ConfigOptions.CenterStick, 0);
        }

        // Functions

        private void SaveCurrentSettings()
        {
            Properties.Settings.Default.gOptions = MakeOptions();
            Properties.Settings.Default.gUserDefinedForce = numericUserDefined.Value;
            Properties.Settings.Default.gForceUnitlbf = comboBoxUnit.SelectedIndex;
            Properties.Settings.Default.Save();
        }

        private void ResetStickDefaults()
        {
            Properties.Settings.Default.gOptions = (byte)ConfigOptions.Force4Kg;
            Properties.Settings.Default.gForceUnitlbf = 0;
            Properties.Settings.Default.gUserDefinedForce = 3.0M;
            Properties.Settings.Default.Save();

            LoadLastSettings();
            SendToStick(Stick, MakeOptions(), CalcForce());
        }

        private void LoadLastSettings()
        {
            isUnitKg = (comboBoxUnit.SelectedIndex == 0) ? true : false;

            comboBoxUnit.SelectedIndex = Properties.Settings.Default.gForceUnitlbf;

            numericUserDefined.Value = Properties.Settings.Default.gUserDefinedForce;

            byte SavedOptions = Properties.Settings.Default.gOptions;

            checkBoxForceMapping.Checked = ((SavedOptions & (byte)ConfigOptions.ForceMap) != 0) ? true : false;
            checkBoxRotate.Checked = ((SavedOptions & (byte)ConfigOptions.RotatedSensors) != 0) ? true : false;
            radioButton4Kg.Checked = ((SavedOptions & (byte)ConfigOptions.Force4Kg) != 0) ? true : false;
            radioButton6Kg.Checked = ((SavedOptions & (byte)ConfigOptions.Force6Kg) != 0) ? true : false;
            radioButton9Kg.Checked = ((SavedOptions & (byte)ConfigOptions.Force9Kg) != 0) ? true : false;
            radioButtonUser.Checked = ((SavedOptions & (byte)ConfigOptions.ForceUserDefined) != 0) ? true : false;
        }

        private void ShowCurrentValues()
        {
            byte[] buff = new byte[3];
            buff = ReadFromStick(Stick);
            decimal force = GetForce((Int16)((buff[1] << 8) | buff[2]));
            if (isUnitKg)
            {
                labelCurrentUserDefined.Text = force + " Kg/f";
            } else
            {
                labelCurrentUserDefined.Text = Math.Round(force * Kg2lb,2) + " Lb/f";
            }

            labelVersionDetected.Text = BdcDecode(Stick.ProductVersion);
                   
            byte opts = buff[0];
            if ((opts & (byte)ConfigOptions.ForceMap) != 0 )
            {
                labelForceMapping.Text = "ON";
                labelForceMapping.ForeColor = Color.Green;
            } else
            {
                labelForceMapping.Text = "OFF";
                labelForceMapping.ForeColor = Color.Red;
            }

            if ((opts & (byte)ConfigOptions.RotatedSensors) != 0)
            {
                labelSensorRotation.Text = "ON";
                labelSensorRotation.ForeColor = Color.Green;
            }
            else
            {
                labelSensorRotation.Text = "OFF";
                labelSensorRotation.ForeColor = Color.Red;
            }


            labelForce4Kg.Visible = ((opts & (byte)ConfigOptions.Force4Kg) != 0) ? true : false;
            labelForce6kg.Visible = ((opts & (byte)ConfigOptions.Force6Kg) != 0) ? true : false;
            labelForce9kg.Visible = ((opts & (byte)ConfigOptions.Force9Kg) != 0) ? true : false;
            labelForceUser.Visible = ((opts & (byte)ConfigOptions.ForceUserDefined) != 0) ? true : false;
        }

        private void comboBoxUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool newUnitKg = (comboBoxUnit.SelectedIndex == 0) ? true : false;
            if (newUnitKg != isUnitKg) //Only do on change
            {
                if (newUnitKg)
                {
                    decimal newValue = numericUserDefined.Value / Kg2lb;
                    numericUserDefined.Value = Math.Max(numericUserDefined.Minimum, Math.Min(numericUserDefined.Maximum, newValue));
                }
                else
                {
                    decimal newValue = numericUserDefined.Value * Kg2lb;
                    numericUserDefined.Value = Math.Max(numericUserDefined.Minimum, Math.Min(numericUserDefined.Maximum, newValue));
                }
            }
            isUnitKg = newUnitKg;

            if (isUnitKg)
            {
                numericUserDefined.Maximum = 9.0M;
                numericUserDefined.Minimum = 1.5M;
            } else {
                numericUserDefined.Maximum = 20.0M;
                numericUserDefined.Minimum = 3.0M;
            }
        }

        private void textBoxHexPath_TextChanged(object sender, EventArgs e)
        {
            buttonUpdateFw.Enabled = (File.Exists(textBoxHexPath.Text)) ? true : false;
   
        }
    }
}