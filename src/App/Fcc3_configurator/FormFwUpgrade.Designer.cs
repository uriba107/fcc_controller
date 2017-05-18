namespace Fcc3_configurator
{
    partial class FormManualFirmwareUpgrade
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormManualFirmwareUpgrade));
            this.groupBoxControllerUpdate = new System.Windows.Forms.GroupBox();
            this.buttonUpdateFw = new System.Windows.Forms.Button();
            this.groupBoxHex = new System.Windows.Forms.GroupBox();
            this.labelPath = new System.Windows.Forms.Label();
            this.textBoxHexPath = new System.Windows.Forms.TextBox();
            this.buttonBrowsHex = new System.Windows.Forms.Button();
            this.groupBoxArduino = new System.Windows.Forms.GroupBox();
            this.labelComport = new System.Windows.Forms.Label();
            this.comboBoxSelectCom = new System.Windows.Forms.ComboBox();
            this.buttonUploadArduino = new System.Windows.Forms.Button();
            this.statusStripUpgrade = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelUploadStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.buttonClose = new System.Windows.Forms.Button();
            this.groupBoxControllerUpdate.SuspendLayout();
            this.groupBoxHex.SuspendLayout();
            this.groupBoxArduino.SuspendLayout();
            this.statusStripUpgrade.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxControllerUpdate
            // 
            this.groupBoxControllerUpdate.Controls.Add(this.buttonUpdateFw);
            this.groupBoxControllerUpdate.Location = new System.Drawing.Point(11, 98);
            this.groupBoxControllerUpdate.Name = "groupBoxControllerUpdate";
            this.groupBoxControllerUpdate.Size = new System.Drawing.Size(257, 56);
            this.groupBoxControllerUpdate.TabIndex = 14;
            this.groupBoxControllerUpdate.TabStop = false;
            this.groupBoxControllerUpdate.Text = "Update FCC Controller";
            // 
            // buttonUpdateFw
            // 
            this.buttonUpdateFw.Enabled = false;
            this.buttonUpdateFw.Location = new System.Drawing.Point(16, 19);
            this.buttonUpdateFw.Name = "buttonUpdateFw";
            this.buttonUpdateFw.Size = new System.Drawing.Size(226, 25);
            this.buttonUpdateFw.TabIndex = 1;
            this.buttonUpdateFw.Text = "Upload Firmware To Controller";
            this.buttonUpdateFw.UseVisualStyleBackColor = true;
            this.buttonUpdateFw.Click += new System.EventHandler(this.buttonUpdateFw_Click);
            // 
            // groupBoxHex
            // 
            this.groupBoxHex.Controls.Add(this.labelPath);
            this.groupBoxHex.Controls.Add(this.textBoxHexPath);
            this.groupBoxHex.Controls.Add(this.buttonBrowsHex);
            this.groupBoxHex.Location = new System.Drawing.Point(12, 12);
            this.groupBoxHex.Name = "groupBoxHex";
            this.groupBoxHex.Size = new System.Drawing.Size(256, 80);
            this.groupBoxHex.TabIndex = 15;
            this.groupBoxHex.TabStop = false;
            this.groupBoxHex.Text = "Select Hex file:";
            // 
            // labelPath
            // 
            this.labelPath.AutoSize = true;
            this.labelPath.Location = new System.Drawing.Point(15, 22);
            this.labelPath.Name = "labelPath";
            this.labelPath.Size = new System.Drawing.Size(109, 13);
            this.labelPath.TabIndex = 11;
            this.labelPath.Text = "Select HEX to upload";
            // 
            // textBoxHexPath
            // 
            this.textBoxHexPath.AllowDrop = true;
            this.textBoxHexPath.Location = new System.Drawing.Point(15, 38);
            this.textBoxHexPath.Name = "textBoxHexPath";
            this.textBoxHexPath.Size = new System.Drawing.Size(195, 20);
            this.textBoxHexPath.TabIndex = 9;
            this.textBoxHexPath.TextChanged += new System.EventHandler(this.textBoxHexPath_TextChanged);
            // 
            // buttonBrowsHex
            // 
            this.buttonBrowsHex.Location = new System.Drawing.Point(216, 36);
            this.buttonBrowsHex.Name = "buttonBrowsHex";
            this.buttonBrowsHex.Size = new System.Drawing.Size(25, 23);
            this.buttonBrowsHex.TabIndex = 10;
            this.buttonBrowsHex.Text = "...";
            this.buttonBrowsHex.UseVisualStyleBackColor = true;
            this.buttonBrowsHex.Click += new System.EventHandler(this.buttonBrowsHex_Click);
            // 
            // groupBoxArduino
            // 
            this.groupBoxArduino.Controls.Add(this.labelComport);
            this.groupBoxArduino.Controls.Add(this.comboBoxSelectCom);
            this.groupBoxArduino.Controls.Add(this.buttonUploadArduino);
            this.groupBoxArduino.Location = new System.Drawing.Point(12, 160);
            this.groupBoxArduino.Name = "groupBoxArduino";
            this.groupBoxArduino.Size = new System.Drawing.Size(257, 94);
            this.groupBoxArduino.TabIndex = 15;
            this.groupBoxArduino.TabStop = false;
            this.groupBoxArduino.Text = "Upload To Arduino";
            // 
            // labelComport
            // 
            this.labelComport.AutoSize = true;
            this.labelComport.Location = new System.Drawing.Point(15, 20);
            this.labelComport.Name = "labelComport";
            this.labelComport.Size = new System.Drawing.Size(128, 13);
            this.labelComport.TabIndex = 3;
            this.labelComport.Text = "Select Arduino COM Port:";
            // 
            // comboBoxSelectCom
            // 
            this.comboBoxSelectCom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSelectCom.FormattingEnabled = true;
            this.comboBoxSelectCom.Location = new System.Drawing.Point(149, 17);
            this.comboBoxSelectCom.Name = "comboBoxSelectCom";
            this.comboBoxSelectCom.Size = new System.Drawing.Size(92, 21);
            this.comboBoxSelectCom.TabIndex = 2;
            this.comboBoxSelectCom.SelectedIndexChanged += new System.EventHandler(this.comboBoxSelectCom_SelectedIndexChanged);
            // 
            // buttonUploadArduino
            // 
            this.buttonUploadArduino.Enabled = false;
            this.buttonUploadArduino.Location = new System.Drawing.Point(15, 44);
            this.buttonUploadArduino.Name = "buttonUploadArduino";
            this.buttonUploadArduino.Size = new System.Drawing.Size(226, 25);
            this.buttonUploadArduino.TabIndex = 1;
            this.buttonUploadArduino.Text = "Upload Firmware To Arduino";
            this.buttonUploadArduino.UseVisualStyleBackColor = true;
            this.buttonUploadArduino.Click += new System.EventHandler(this.buttonUploadArduino_Click);
            // 
            // statusStripUpgrade
            // 
            this.statusStripUpgrade.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelUploadStatus});
            this.statusStripUpgrade.Location = new System.Drawing.Point(0, 295);
            this.statusStripUpgrade.Name = "statusStripUpgrade";
            this.statusStripUpgrade.Size = new System.Drawing.Size(280, 22);
            this.statusStripUpgrade.TabIndex = 16;
            this.statusStripUpgrade.Text = "statusStrip1";
            // 
            // toolStripStatusLabelUploadStatus
            // 
            this.toolStripStatusLabelUploadStatus.Name = "toolStripStatusLabelUploadStatus";
            this.toolStripStatusLabelUploadStatus.Size = new System.Drawing.Size(265, 17);
            this.toolStripStatusLabelUploadStatus.Spring = true;
            this.toolStripStatusLabelUploadStatus.Text = "Upgrade N/A";
            this.toolStripStatusLabelUploadStatus.Visible = false;
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(110, 260);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 17;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // FormManualFirmwareUpgrade
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(280, 317);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.statusStripUpgrade);
            this.Controls.Add(this.groupBoxArduino);
            this.Controls.Add(this.groupBoxHex);
            this.Controls.Add(this.groupBoxControllerUpdate);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormManualFirmwareUpgrade";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Firmware Upgrade";
            this.groupBoxControllerUpdate.ResumeLayout(false);
            this.groupBoxHex.ResumeLayout(false);
            this.groupBoxHex.PerformLayout();
            this.groupBoxArduino.ResumeLayout(false);
            this.groupBoxArduino.PerformLayout();
            this.statusStripUpgrade.ResumeLayout(false);
            this.statusStripUpgrade.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxControllerUpdate;
        private System.Windows.Forms.Button buttonUpdateFw;
        private System.Windows.Forms.GroupBox groupBoxHex;
        private System.Windows.Forms.Label labelPath;
        private System.Windows.Forms.TextBox textBoxHexPath;
        private System.Windows.Forms.Button buttonBrowsHex;
        private System.Windows.Forms.GroupBox groupBoxArduino;
        private System.Windows.Forms.Button buttonUploadArduino;
        private System.Windows.Forms.StatusStrip statusStripUpgrade;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelUploadStatus;
        private System.Windows.Forms.Label labelComport;
        private System.Windows.Forms.ComboBox comboBoxSelectCom;
        private System.Windows.Forms.Button buttonClose;
    }
}