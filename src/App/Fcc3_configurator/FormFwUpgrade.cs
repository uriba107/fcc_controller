using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using HexHelper;

namespace Fcc3_configurator
{
    public partial class FormManualFirmwareUpgrade : Form
    {
        private static volatile FccHandeler Stick = new FccHandeler();
        private HexUpdater Uploader;

        public FormManualFirmwareUpgrade()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            if (!Stick.isConnected)
            {
                Stick.Connect();
            }
            Uploader = new HexUpdater();
            comboBoxSelectCom.DataSource = Uploader.AvailablePorts;
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

        private void ShowUploadStatus(bool isSuccess)
        {
            if (isSuccess)
            {
                toolStripStatusLabelUploadStatus.Text = "FW upgrade has finished successfuly";
                toolStripStatusLabelUploadStatus.ForeColor = Color.Green;
                toolStripStatusLabelUploadStatus.Visible = true;
            }
            else
            {
                toolStripStatusLabelUploadStatus.Text = "FW upgrade had failed";
                toolStripStatusLabelUploadStatus.ForeColor = Color.Red;
                toolStripStatusLabelUploadStatus.Visible = true;
            }
            if (Stick == null)
            {
                Stick = new FccHandeler();
                Stick.Connect();
            }
        }
        private void buttonUpdateFw_Click(object sender, EventArgs e)
        {
            // TODO: Add COM port detection stuff here
            buttonUploadArduino.Enabled = false;
            buttonUpdateFw.Enabled = false;

            string HexPath = textBoxHexPath.Text;

            bool status = Stick.UpgradeFirmware(HexPath);
            ShowUploadStatus(status);

            if (!status)
            {
                buttonUpdateFw.Enabled = true;
            }
        }

        private void textBoxHexPath_TextChanged(object sender, EventArgs e)
        {
            buttonUpdateFw.Enabled = (File.Exists(textBoxHexPath.Text) && Stick.isConnected) ? true : false;
            try
            {
                buttonUploadArduino.Enabled = (File.Exists(textBoxHexPath.Text) && comboBoxSelectCom.SelectedValue.ToString() != null) ? true : false;
            } catch { }
        }

        private void buttonUploadArduino_Click(object sender, EventArgs e)
        {
            buttonUploadArduino.Enabled = false;
            buttonUpdateFw.Enabled = false;
            string HexPath = textBoxHexPath.Text;
            string ComPort = comboBoxSelectCom.SelectedValue.ToString();

            ShowUploadStatus(Uploader.UploadToArduino(ComPort,HexPath));

                            buttonUploadArduino.Enabled = true;

        }

        private void comboBoxSelectCom_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonUploadArduino.Enabled = (File.Exists(textBoxHexPath.Text) && comboBoxSelectCom.SelectedValue.ToString() != "") ? true : false;

        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
