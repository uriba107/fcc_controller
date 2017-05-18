using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.IO.Ports;
using HidSharp;
//using System.IO.Compression;




namespace Fcc3_configurator
{
    public partial class MainForm : Form
    {

        const ushort VID = 0x1029;
        const ushort PID = 0xF16C;

        private volatile FccHandeler Stick = new FccHandeler();

        private System.Windows.Forms.Timer RefreshTimer;
        private System.Windows.Forms.Timer UsbTimer;
        private System.Windows.Forms.Timer UpdateTimer;

        private bool useKg;

        public MainForm()
        {
            InitializeComponent();
            this.Text += " - " + Application.ProductVersion;
            labelAppVersionCurrent.Text = Application.ProductVersion;
            LoadLastSettings();
            InitUsbTimer();
            InitUpdateTimer();
        }

        // Button functions

        private void buttonApply_Click(object sender, EventArgs e)
        {
            buttonApply.Enabled = false;
            SaveCurrentSettings();
            AppendChanges();
            Stick.ApplyChanges();
            buttonApply.Enabled = true;
        }

        private void buttonDefaults_Click(object sender, EventArgs e)
        {
            ResetStickDefaults();
        }


        private void buttonCenter_Click(object sender, EventArgs e)
        {
          ReadStickSettings();
            //Stick.Center();
        }

        // Functions
        private void ReadStickSettings()
        {
          Properties.Settings.Default.UseNewFccGain = Stick.isFccWhGains;
          Properties.Settings.Default.Save();

            checkBoxRotate.Checked = Stick.isSensorRotated;
          bool DigitalFlcs = Stick.isDigitalFlcs;
          bool AnalogFlcs = Stick.isAnalogFlcs;
          checkBoxForceMapping.Checked = (AnalogFlcs || DigitalFlcs);
          radioButtonAnalogFlcs.Checked = AnalogFlcs;
          radioButtonDigitalFlcs.Checked = DigitalFlcs;

          radioButton4Kg.Checked = Stick.Use4KgForce;
          radioButton6Kg.Checked = Stick.Use6KgForce;
          radioButton9Kg.Checked = Stick.Use9KgForce;
          radioButtonUser.Checked = Stick.UseCustomForce;

          bool isUnitKg = (comboBoxUnit.SelectedIndex == 0) ? true : false;
          numericUserDefined.Value = Stick.GetCurrentForce(isUnitKg);

          SaveCurrentSettings();
        }

        private void SaveCurrentSettings()
        {
            Properties.Settings.Default.gOptions = MakeOptions();
            Properties.Settings.Default.gUserDefinedForce = numericUserDefined.Value;
            Properties.Settings.Default.gForceUnitlbf = comboBoxUnit.SelectedIndex;
            Properties.Settings.Default.Save();
        }

        private void ResetStickDefaults()
        {
            Properties.Settings.Default.gOptions = (byte)FccHandeler.ConfigOptions.Force4Kg;
            Properties.Settings.Default.gForceUnitlbf = 0;
            Properties.Settings.Default.gUserDefinedForce = 3.0M;
            Properties.Settings.Default.Save();

            LoadLastSettings();
            AppendChanges();
            Stick.ApplyChanges();
        }

        private void AppendChanges()
        {
            bool isKgSelected = (comboBoxUnit.SelectedIndex == 0) ? true : false;

            if (checkBoxForceMapping.Checked)
            {
                if (radioButtonAnalogFlcs.Checked)
                {
                    Stick.isAnalogFlcs = true;
                }
                if (radioButtonDigitalFlcs.Checked)
                {
                    Stick.isDigitalFlcs = true;
                }
            }
            else
            {
                Stick.isAnalogFlcs = false;
                Stick.isDigitalFlcs = false;
            }
            Stick.isSensorRotated = checkBoxRotate.Checked;
            Stick.isFccWhGains = Properties.Settings.Default.UseNewFccGain;

            Stick.SetCustomForce(numericUserDefined.Value, isKgSelected);

            Stick.Use4KgForce = radioButton4Kg.Checked;
            Stick.Use6KgForce = radioButton6Kg.Checked;
            Stick.Use9KgForce = radioButton9Kg.Checked;
            Stick.UseCustomForce = radioButtonUser.Checked;

        }
        private void LoadLastSettings()
        {
            comboBoxUnit.SelectedIndex = Properties.Settings.Default.gForceUnitlbf;
            useKg = (comboBoxUnit.SelectedIndex == 0) ? true : false;

            numericUserDefined.Value = Properties.Settings.Default.gUserDefinedForce;

            byte SavedOptions = Properties.Settings.Default.gOptions;

            bool DigitalFlcs = ((SavedOptions & (byte)FccHandeler.ConfigOptions.DigitalFlcs) != 0) ? true : false;
            bool AnalogFlcs = ((SavedOptions & (byte)FccHandeler.ConfigOptions.AnaloglFlcs) != 0) ? true : false;
            checkBoxForceMapping.Checked = (AnalogFlcs || DigitalFlcs);
            radioButtonAnalogFlcs.Checked = AnalogFlcs;
            radioButtonDigitalFlcs.Checked = DigitalFlcs;

            checkBoxRotate.Checked = ((SavedOptions & (byte)FccHandeler.ConfigOptions.RotatedSensors) != 0) ? true : false;
            radioButton4Kg.Checked = ((SavedOptions & (byte)FccHandeler.ConfigOptions.Force4Kg) != 0) ? true : false;
            radioButton6Kg.Checked = ((SavedOptions & (byte)FccHandeler.ConfigOptions.Force6Kg) != 0) ? true : false;
            radioButton9Kg.Checked = ((SavedOptions & (byte)FccHandeler.ConfigOptions.Force9Kg) != 0) ? true : false;
            radioButtonUser.Checked = ((SavedOptions & (byte)FccHandeler.ConfigOptions.ForceUserDefined) != 0) ? true : false;
            checkBoxNotifyApp.Checked = Properties.Settings.Default.notifyApp;
            checkBoxNotifyFirmware.Checked = Properties.Settings.Default.notifyFirmware;
        }

        private void ShowCurrentValues()
        {
            bool isUnitKg = (comboBoxUnit.SelectedIndex == 0) ? true : false;

            if (isUnitKg)
            {
                labelCurrentUserDefined.Text = Stick.GetCurrentForce(isUnitKg) + " Kgf";
            }
            else
            {
                labelCurrentUserDefined.Text = Stick.GetCurrentForce(isUnitKg) + " Lbf";
            }

            labelVersionDetected.Text = Stick.FirmwareVersion;

            if (Stick.isEmulatingFlcs)
            {
                if (Stick.isAnalogFlcs)
                {
                    labelForceMapping.Text = "FLCS: Analog";
                }
                if (Stick.isDigitalFlcs)
                {
                    labelForceMapping.Text = "FLCS: Digital";
                }
                labelForceMapping.ForeColor = Color.Green;
            }
            else
            {
                labelForceMapping.Text = "FLCS: OFF";
                labelForceMapping.ForeColor = Color.Red;
            }

            if (Stick.isSensorRotated)
            {
                labelSensorRotation.Text = "ON";
                labelSensorRotation.ForeColor = Color.Green;
            }
            else
            {
                labelSensorRotation.Text = "OFF";
                labelSensorRotation.ForeColor = Color.Red;
            }


            labelForce4Kg.Visible = Stick.Use4KgForce;
            labelForce6kg.Visible = Stick.Use6KgForce;
            labelForce9kg.Visible = Stick.Use9KgForce;
            labelForceUser.Visible = Stick.UseCustomForce;

            if (Stick.isConnected)
            {
                string rev = (Stick.isFccWhGains) ? "Warthog" : "Cougar";

                toolStripStatusLabelInfo.Text = "FCC (" + rev + ") Connected";

                toolStripStatusLabelColor.ForeColor = Color.Green;

                buttonTestStick.Enabled = true;
            }
            else
            {
                toolStripStatusLabelInfo.Text = "No Device Connected";
                toolStripStatusLabelColor.ForeColor = Color.Red;
                buttonTestStick.Enabled = false;

            }
        }

        private void comboBoxUnit_SelectedIndexChanged(object sender, EventArgs e)
        {

            bool isKgSelected = (comboBoxUnit.SelectedIndex == 0) ? true : false;
            if (isKgSelected != useKg) //Only do on change
            {
                if (isKgSelected)
                {
                    decimal newValue = numericUserDefined.Value / FccHandeler.KgInLb;
                    if (Stick.isFccWhGains)
                    {
                        numericUserDefined.Maximum = 9.0M;
                    }
                    else
                    {
                        numericUserDefined.Maximum = 12.0M;
                    }
                    numericUserDefined.Minimum = 1.5M;
                    numericUserDefined.Value = Math.Max(numericUserDefined.Minimum, Math.Min(numericUserDefined.Maximum, newValue));
                }
                else
                {
                    decimal newValue = numericUserDefined.Value * FccHandeler.KgInLb;
                    if (Stick.isFccWhGains)
                    {
                        numericUserDefined.Maximum = 20.0M;
                    }
                    else
                    {
                        numericUserDefined.Maximum = 26.5M;
                    }
                    numericUserDefined.Minimum = 3.0M;
                    numericUserDefined.Value = Math.Max(numericUserDefined.Minimum, Math.Min(numericUserDefined.Maximum, newValue));
                }
            }
            useKg = isKgSelected;
        }

        private void numericUserDefined_ValueChanged(object sender, EventArgs e)
        {
            bool isKgSelected = (comboBoxUnit.SelectedIndex == 0) ? true : false;

            if (isKgSelected)
            {
                if (Stick.isFccWhGains)
                {
                    numericUserDefined.Maximum = 9.0M;
                }
                else
                {
                    numericUserDefined.Maximum = 12.0M;
                }
                numericUserDefined.Minimum = 1.5M;
                numericUserDefined.Value = Math.Max(numericUserDefined.Minimum, Math.Min(numericUserDefined.Maximum, numericUserDefined.Value));
            }
            else
            {
                if (Stick.isFccWhGains)
                {
                    numericUserDefined.Maximum = 20.0M;
                }
                else
                {
                    numericUserDefined.Maximum = 26.5M;
                }
                numericUserDefined.Minimum = 3.0M;
                numericUserDefined.Value = Math.Max(numericUserDefined.Minimum, Math.Min(numericUserDefined.Maximum, numericUserDefined.Value));
            }
        }

        private void SetCustomForceLimit()
        {
        }

        private void buttonAutoUpdateFirmware_Click(object sender, EventArgs e)
        {
            buttonAutoUpdateFirmware.Enabled = false;

            string AppDir = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            string TargetDir = AppDir + @"\firmware";
            Directory.CreateDirectory(TargetDir);
            string HexPath = TargetDir + "\\" + updater.FirmwareFileName;

            if (updater.DownloadFirmware(TargetDir))
            {

                ShowUploadStatus(UpgradeFirmware(HexPath));
            }
            RunUpdateManager();
        }

        private void buttonAutoUpdateApp_Click(object sender, EventArgs e)
        {
            string UserTempDir = Path.GetTempPath();

            bool isDownloaded = updater.DownloadApp(UserTempDir);
            if (isDownloaded)
            {
                ProcessStartInfo InstallLauncher = new ProcessStartInfo();
                InstallLauncher.Arguments = "/C ping 127.0.0.1 -n 2 > Nul & " + UserTempDir + @"\" + updater.AppFileName;
                InstallLauncher.WindowStyle = ProcessWindowStyle.Hidden;
                InstallLauncher.CreateNoWindow = false;
                InstallLauncher.FileName = "cmd.exe";
                Process.Start(InstallLauncher);
                Application.Exit();
            }
        }

        private void RefreshUpdateTab()
        {
            linkLabelLatestFirmwareVersion.Text = updater.FirmwareVersion;
            linkLabelLatestAppVersion.Text = updater.AppVersion;

            labelAppVersionCurrent.Text = Application.ProductVersion;
            labelVersionDetected.Text = Stick.FirmwareVersion;

            linkLabelLatestFirmwareVersion.Visible = true;
            linkLabelLatestAppVersion.Visible = true;


            if (updater.CheckFirmware(Stick.FirmwareVersion) && Stick.FirmwareVersion != "0.0.0")
            {
                labelVersionDetected.ForeColor = Color.Red;
                linkLabelLatestFirmwareVersion.ForeColor = SystemColors.ControlText;
                buttonAutoUpdateFirmware.Enabled = true;
                if (checkBoxNotifyFirmware.Checked)
                {
                    if (!isNotified)
                    {
                        notifyIconMain.ShowBalloonTip(1000, "FCC update is Available", "There is an update for you. please check the update tab for more details.", ToolTipIcon.Info);
                    }
                    isNotified = true;
                }

            }
            else
            {
                linkLabelLatestFirmwareVersion.ForeColor = Color.Green;
                labelVersionDetected.ForeColor = Color.Green;
                buttonAutoUpdateFirmware.Enabled = false;

            }
            if (updater.CheckApp(Application.ProductVersion))
            {
                labelAppVersionCurrent.ForeColor = Color.Red;
                linkLabelLatestAppVersion.ForeColor = SystemColors.ControlText;
                buttonAutoUpdateApp.Enabled = true;
                if (checkBoxNotifyApp.Checked)
                {
                    if (!isNotified)
                    {
                        notifyIconMain.ShowBalloonTip(15000, "FCC update is Available", "There is an update for you. please check the update tab for more details.", ToolTipIcon.Info);
                    }
                    isNotified = true;
                }
            }
            else
            {
                labelAppVersionCurrent.ForeColor = Color.Green;
                linkLabelLatestAppVersion.ForeColor = Color.Green;
                buttonAutoUpdateApp.Enabled = false;
            }
        }

        private void linkLabelLatestAppVersion_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@updater.Changelog);
        }

        private void notifyIconMain_BalloonTipClicked(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            tabControlMain.SelectedTab = tabPageUpdate;
        }

        private void checkBoxNotifyFirmware_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.notifyFirmware = checkBoxNotifyFirmware.Checked;
            Properties.Settings.Default.Save();
        }

        private void checkBoxNotifyApp_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.notifyApp = checkBoxNotifyApp.Checked;
            Properties.Settings.Default.Save();
        }




        private void ShowUploadStatus(bool isSuccess)
        {
            if (isSuccess)
            {
                toolStripLabelUploadStatus.Text = "Upgrade OK";
                toolStripLabelUploadStatus.ForeColor = Color.Green;
                toolStripLabelUploadStatus.Visible = true;
            }
            else
            {
                toolStripLabelUploadStatus.Text = "Upgrade Failed";
                toolStripLabelUploadStatus.ForeColor = Color.Red;
                toolStripLabelUploadStatus.Visible = true;
            }
        }

        private void buttonCheckUpdates_Click(object sender, EventArgs e)
        {
            RunUpdateManager();

        }



        private void checkBoxEmulateFlcs_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonAnalogFlcs.Enabled = checkBoxForceMapping.Checked;
            radioButtonDigitalFlcs.Enabled = checkBoxForceMapping.Checked;

            if (checkBoxForceMapping.Checked)
            {
                byte SavedOptions = Properties.Settings.Default.gOptions;

                radioButtonAnalogFlcs.Checked = ((SavedOptions & (byte)FccHandeler.ConfigOptions.AnaloglFlcs) != 0) ? true : false;
                radioButtonDigitalFlcs.Checked = ((SavedOptions & (byte)FccHandeler.ConfigOptions.DigitalFlcs) != 0) ? true : false;

                if (!radioButtonAnalogFlcs.Checked && !radioButtonDigitalFlcs.Checked)
                {
                    radioButtonDigitalFlcs.Checked = true;
                }
            }
        }

        private void buttonAdvancedFW_Click(object sender, EventArgs e)
        {
            var FwUpgradeForm = new FormManualFirmwareUpgrade();
            FwUpgradeForm.FormClosed += new FormClosedEventHandler(FwUpgradeForm_closed);
            FwUpgradeForm.StartPosition = FormStartPosition.CenterParent;
            FwUpgradeForm.Show();
            this.Hide();
        }

        private void FwUpgradeForm_closed(object sender, EventArgs e)
        {
            this.Show();
        }
        private void buttonInitialSetup_Click(object sender, EventArgs e)
        {
            var FormHardware = new FormAdvancedHardware();
            FormHardware.FormClosed += new FormClosedEventHandler(FormHardware_closed);
            FormHardware.StartPosition = FormStartPosition.CenterParent;
            FormHardware.Show();
            this.Hide();
        }

        private void FormHardware_closed(object sender, EventArgs e)
        {

            ResetStickDefaults();
            this.Show();
        }

        private void buttonTestStick_Click(object sender, EventArgs e)
        {
            var StickTracer = new FormStickTracker();
            StickTracer.FormClosed += new FormClosedEventHandler(FormStickTracker_closed);
            StickTracer.StartPosition = FormStartPosition.CenterParent;
            StickTracer.Show();
            this.Hide();
        }

        private void FormStickTracker_closed(object sender, EventArgs e)
        {
            this.Show();
        }
    }
}
