namespace Fcc3_configurator
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.buttonApply = new System.Windows.Forms.Button();
            this.checkBoxRotate = new System.Windows.Forms.CheckBox();
            this.checkBoxForceMapping = new System.Windows.Forms.CheckBox();
            this.radioButton4Kg = new System.Windows.Forms.RadioButton();
            this.radioButton6Kg = new System.Windows.Forms.RadioButton();
            this.radioButton9Kg = new System.Windows.Forms.RadioButton();
            this.radioButtonUser = new System.Windows.Forms.RadioButton();
            this.numericUserDefined = new System.Windows.Forms.NumericUpDown();
            this.buttonDefaults = new System.Windows.Forms.Button();
            this.toolTipMainForm = new System.Windows.Forms.ToolTip(this.components);
            this.groupBoxSensitivity = new System.Windows.Forms.GroupBox();
            this.comboBoxUnit = new System.Windows.Forms.ComboBox();
            this.labelForceUser = new System.Windows.Forms.Label();
            this.labelForce9kg = new System.Windows.Forms.Label();
            this.labelForce6kg = new System.Windows.Forms.Label();
            this.labelForce4Kg = new System.Windows.Forms.Label();
            this.labelCurrentUserDefined = new System.Windows.Forms.Label();
            this.groupBoxFeatures = new System.Windows.Forms.GroupBox();
            this.labelForceMapping = new System.Windows.Forms.Label();
            this.labelSensorRotation = new System.Windows.Forms.Label();
            this.buttonAdvancedFW = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonCenter = new System.Windows.Forms.Button();
            this.tabPageUpdate = new System.Windows.Forms.TabPage();
            this.groupBoxAutoUpdate = new System.Windows.Forms.GroupBox();
            this.buttonCheckUpdates = new System.Windows.Forms.Button();
            this.linkLabelLatestAppVersion = new System.Windows.Forms.LinkLabel();
            this.linkLabelLatestFirmwareVersion = new System.Windows.Forms.LinkLabel();
            this.labelVesrion = new System.Windows.Forms.Label();
            this.labelAppVersionCurrent = new System.Windows.Forms.Label();
            this.labelVersionDetected = new System.Windows.Forms.Label();
            this.buttonAutoUpdateApp = new System.Windows.Forms.Button();
            this.labelAvailableFW = new System.Windows.Forms.Label();
            this.buttonAutoUpdateFirmware = new System.Windows.Forms.Button();
            this.labelAvailableApp = new System.Windows.Forms.Label();
            this.labelAppVersionTitle = new System.Windows.Forms.Label();
            this.groupBoxManualUpdate = new System.Windows.Forms.GroupBox();
            this.groupBoxUpdater = new System.Windows.Forms.GroupBox();
            this.checkBoxNotifyApp = new System.Windows.Forms.CheckBox();
            this.checkBoxNotifyFirmware = new System.Windows.Forms.CheckBox();
            this.tabPageSettings = new System.Windows.Forms.TabPage();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelColor = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripLabelUploadStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.notifyIconMain = new System.Windows.Forms.NotifyIcon(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.numericUserDefined)).BeginInit();
            this.groupBoxSensitivity.SuspendLayout();
            this.groupBoxFeatures.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPageUpdate.SuspendLayout();
            this.groupBoxAutoUpdate.SuspendLayout();
            this.groupBoxManualUpdate.SuspendLayout();
            this.groupBoxUpdater.SuspendLayout();
            this.tabPageSettings.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.statusStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonApply
            // 
            this.buttonApply.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonApply.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonApply.Location = new System.Drawing.Point(6, 242);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(245, 43);
            this.buttonApply.TabIndex = 0;
            this.buttonApply.Text = "&Apply Config";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // checkBoxRotate
            // 
            this.checkBoxRotate.AutoSize = true;
            this.checkBoxRotate.Location = new System.Drawing.Point(6, 19);
            this.checkBoxRotate.Name = "checkBoxRotate";
            this.checkBoxRotate.Size = new System.Drawing.Size(151, 17);
            this.checkBoxRotate.TabIndex = 1;
            this.checkBoxRotate.Text = "Sensor &Rotation Emulation";
            this.checkBoxRotate.UseVisualStyleBackColor = true;
            // 
            // checkBoxForceMapping
            // 
            this.checkBoxForceMapping.AutoSize = true;
            this.checkBoxForceMapping.Location = new System.Drawing.Point(6, 42);
            this.checkBoxForceMapping.Name = "checkBoxForceMapping";
            this.checkBoxForceMapping.Size = new System.Drawing.Size(156, 17);
            this.checkBoxForceMapping.TabIndex = 2;
            this.checkBoxForceMapping.Text = "Proportional Force &Mapping";
            this.checkBoxForceMapping.UseVisualStyleBackColor = true;
            // 
            // radioButton4Kg
            // 
            this.radioButton4Kg.AutoSize = true;
            this.radioButton4Kg.Checked = true;
            this.radioButton4Kg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.radioButton4Kg.Location = new System.Drawing.Point(6, 19);
            this.radioButton4Kg.Name = "radioButton4Kg";
            this.radioButton4Kg.Size = new System.Drawing.Size(104, 17);
            this.radioButton4Kg.TabIndex = 3;
            this.radioButton4Kg.TabStop = true;
            this.radioButton4Kg.Text = "&4.5 Kg/f (10 lb/f)";
            this.radioButton4Kg.UseVisualStyleBackColor = true;
            // 
            // radioButton6Kg
            // 
            this.radioButton6Kg.AutoSize = true;
            this.radioButton6Kg.Location = new System.Drawing.Point(6, 42);
            this.radioButton6Kg.Name = "radioButton6Kg";
            this.radioButton6Kg.Size = new System.Drawing.Size(101, 17);
            this.radioButton6Kg.TabIndex = 4;
            this.radioButton6Kg.Text = "&6 Kg/f   (13 lb/f)";
            this.radioButton6Kg.UseVisualStyleBackColor = true;
            // 
            // radioButton9Kg
            // 
            this.radioButton9Kg.AutoSize = true;
            this.radioButton9Kg.Location = new System.Drawing.Point(6, 65);
            this.radioButton9Kg.Name = "radioButton9Kg";
            this.radioButton9Kg.Size = new System.Drawing.Size(101, 17);
            this.radioButton9Kg.TabIndex = 5;
            this.radioButton9Kg.Text = "&9 Kg/f   (20 lb/f)";
            this.radioButton9Kg.UseVisualStyleBackColor = true;
            // 
            // radioButtonUser
            // 
            this.radioButtonUser.AutoSize = true;
            this.radioButtonUser.Location = new System.Drawing.Point(6, 88);
            this.radioButtonUser.Name = "radioButtonUser";
            this.radioButtonUser.Size = new System.Drawing.Size(148, 17);
            this.radioButtonUser.TabIndex = 6;
            this.radioButtonUser.Text = "&User Defined force setting";
            this.radioButtonUser.UseVisualStyleBackColor = true;
            // 
            // numericUserDefined
            // 
            this.numericUserDefined.DecimalPlaces = 1;
            this.numericUserDefined.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUserDefined.Location = new System.Drawing.Point(32, 111);
            this.numericUserDefined.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            65536});
            this.numericUserDefined.Minimum = new decimal(new int[] {
            15,
            0,
            0,
            65536});
            this.numericUserDefined.Name = "numericUserDefined";
            this.numericUserDefined.Size = new System.Drawing.Size(46, 20);
            this.numericUserDefined.TabIndex = 7;
            this.numericUserDefined.Value = new decimal(new int[] {
            30,
            0,
            0,
            65536});
            // 
            // buttonDefaults
            // 
            this.buttonDefaults.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDefaults.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonDefaults.Location = new System.Drawing.Point(6, 306);
            this.buttonDefaults.Name = "buttonDefaults";
            this.buttonDefaults.Size = new System.Drawing.Size(111, 23);
            this.buttonDefaults.TabIndex = 9;
            this.buttonDefaults.Text = "&Restore Defaults";
            this.buttonDefaults.UseVisualStyleBackColor = true;
            this.buttonDefaults.Click += new System.EventHandler(this.buttonDefaults_Click);
            // 
            // groupBoxSensitivity
            // 
            this.groupBoxSensitivity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSensitivity.Controls.Add(this.comboBoxUnit);
            this.groupBoxSensitivity.Controls.Add(this.labelForceUser);
            this.groupBoxSensitivity.Controls.Add(this.labelForce9kg);
            this.groupBoxSensitivity.Controls.Add(this.labelForce6kg);
            this.groupBoxSensitivity.Controls.Add(this.labelForce4Kg);
            this.groupBoxSensitivity.Controls.Add(this.labelCurrentUserDefined);
            this.groupBoxSensitivity.Controls.Add(this.radioButton4Kg);
            this.groupBoxSensitivity.Controls.Add(this.radioButton6Kg);
            this.groupBoxSensitivity.Controls.Add(this.radioButtonUser);
            this.groupBoxSensitivity.Controls.Add(this.numericUserDefined);
            this.groupBoxSensitivity.Controls.Add(this.radioButton9Kg);
            this.groupBoxSensitivity.Location = new System.Drawing.Point(6, 3);
            this.groupBoxSensitivity.Name = "groupBoxSensitivity";
            this.groupBoxSensitivity.Size = new System.Drawing.Size(245, 147);
            this.groupBoxSensitivity.TabIndex = 12;
            this.groupBoxSensitivity.TabStop = false;
            this.groupBoxSensitivity.Text = "Sensitivity Settings";
            // 
            // comboBoxUnit
            // 
            this.comboBoxUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxUnit.FormattingEnabled = true;
            this.comboBoxUnit.Items.AddRange(new object[] {
            "Kg/f",
            "lb/f"});
            this.comboBoxUnit.Location = new System.Drawing.Point(84, 111);
            this.comboBoxUnit.Name = "comboBoxUnit";
            this.comboBoxUnit.Size = new System.Drawing.Size(44, 21);
            this.comboBoxUnit.TabIndex = 16;
            this.comboBoxUnit.SelectedIndexChanged += new System.EventHandler(this.comboBoxUnit_SelectedIndexChanged);
            // 
            // labelForceUser
            // 
            this.labelForceUser.AutoSize = true;
            this.labelForceUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.labelForceUser.ForeColor = System.Drawing.Color.Green;
            this.labelForceUser.Location = new System.Drawing.Point(196, 90);
            this.labelForceUser.Name = "labelForceUser";
            this.labelForceUser.Size = new System.Drawing.Size(15, 13);
            this.labelForceUser.TabIndex = 13;
            this.labelForceUser.Text = "●";
            this.labelForceUser.Visible = false;
            // 
            // labelForce9kg
            // 
            this.labelForce9kg.AutoSize = true;
            this.labelForce9kg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.labelForce9kg.ForeColor = System.Drawing.Color.Green;
            this.labelForce9kg.Location = new System.Drawing.Point(196, 67);
            this.labelForce9kg.Name = "labelForce9kg";
            this.labelForce9kg.Size = new System.Drawing.Size(15, 13);
            this.labelForce9kg.TabIndex = 12;
            this.labelForce9kg.Text = "●";
            this.labelForce9kg.Visible = false;
            // 
            // labelForce6kg
            // 
            this.labelForce6kg.AutoSize = true;
            this.labelForce6kg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.labelForce6kg.ForeColor = System.Drawing.Color.Green;
            this.labelForce6kg.Location = new System.Drawing.Point(196, 44);
            this.labelForce6kg.Name = "labelForce6kg";
            this.labelForce6kg.Size = new System.Drawing.Size(15, 13);
            this.labelForce6kg.TabIndex = 11;
            this.labelForce6kg.Text = "●";
            this.labelForce6kg.Visible = false;
            // 
            // labelForce4Kg
            // 
            this.labelForce4Kg.AutoSize = true;
            this.labelForce4Kg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.labelForce4Kg.ForeColor = System.Drawing.Color.Green;
            this.labelForce4Kg.Location = new System.Drawing.Point(196, 21);
            this.labelForce4Kg.Name = "labelForce4Kg";
            this.labelForce4Kg.Size = new System.Drawing.Size(15, 13);
            this.labelForce4Kg.TabIndex = 10;
            this.labelForce4Kg.Text = "●";
            this.labelForce4Kg.Visible = false;
            // 
            // labelCurrentUserDefined
            // 
            this.labelCurrentUserDefined.AutoSize = true;
            this.labelCurrentUserDefined.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.labelCurrentUserDefined.Location = new System.Drawing.Point(157, 113);
            this.labelCurrentUserDefined.Name = "labelCurrentUserDefined";
            this.labelCurrentUserDefined.Size = new System.Drawing.Size(54, 13);
            this.labelCurrentUserDefined.TabIndex = 9;
            this.labelCurrentUserDefined.Text = "0.0 Kg/f";
            this.labelCurrentUserDefined.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // groupBoxFeatures
            // 
            this.groupBoxFeatures.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxFeatures.Controls.Add(this.labelForceMapping);
            this.groupBoxFeatures.Controls.Add(this.labelSensorRotation);
            this.groupBoxFeatures.Controls.Add(this.checkBoxRotate);
            this.groupBoxFeatures.Controls.Add(this.checkBoxForceMapping);
            this.groupBoxFeatures.Location = new System.Drawing.Point(6, 156);
            this.groupBoxFeatures.Name = "groupBoxFeatures";
            this.groupBoxFeatures.Size = new System.Drawing.Size(245, 70);
            this.groupBoxFeatures.TabIndex = 13;
            this.groupBoxFeatures.TabStop = false;
            this.groupBoxFeatures.Text = "Special Features";
            // 
            // labelForceMapping
            // 
            this.labelForceMapping.AutoSize = true;
            this.labelForceMapping.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.labelForceMapping.ForeColor = System.Drawing.Color.Red;
            this.labelForceMapping.Location = new System.Drawing.Point(181, 43);
            this.labelForceMapping.Name = "labelForceMapping";
            this.labelForceMapping.Size = new System.Drawing.Size(30, 13);
            this.labelForceMapping.TabIndex = 16;
            this.labelForceMapping.Text = "OFF";
            // 
            // labelSensorRotation
            // 
            this.labelSensorRotation.AutoSize = true;
            this.labelSensorRotation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.labelSensorRotation.ForeColor = System.Drawing.Color.Red;
            this.labelSensorRotation.Location = new System.Drawing.Point(181, 20);
            this.labelSensorRotation.Name = "labelSensorRotation";
            this.labelSensorRotation.Size = new System.Drawing.Size(30, 13);
            this.labelSensorRotation.TabIndex = 15;
            this.labelSensorRotation.Text = "OFF";
            // 
            // buttonAdvancedFW
            // 
            this.buttonAdvancedFW.Location = new System.Drawing.Point(5, 19);
            this.buttonAdvancedFW.Name = "buttonAdvancedFW";
            this.buttonAdvancedFW.Size = new System.Drawing.Size(226, 25);
            this.buttonAdvancedFW.TabIndex = 1;
            this.buttonAdvancedFW.Text = "Upload Firmware Manually";
            this.buttonAdvancedFW.UseVisualStyleBackColor = true;
            this.buttonAdvancedFW.Click += new System.EventHandler(this.buttonAdvancedFW_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonCenter);
            this.panel1.Controls.Add(this.groupBoxSensitivity);
            this.panel1.Controls.Add(this.buttonApply);
            this.panel1.Controls.Add(this.groupBoxFeatures);
            this.panel1.Controls.Add(this.buttonDefaults);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(256, 335);
            this.panel1.TabIndex = 16;
            // 
            // buttonCenter
            // 
            this.buttonCenter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCenter.Location = new System.Drawing.Point(140, 306);
            this.buttonCenter.Name = "buttonCenter";
            this.buttonCenter.Size = new System.Drawing.Size(111, 23);
            this.buttonCenter.TabIndex = 14;
            this.buttonCenter.Text = "Center";
            this.buttonCenter.UseVisualStyleBackColor = true;
            this.buttonCenter.Click += new System.EventHandler(this.buttonCenter_Click);
            // 
            // tabPageUpdate
            // 
            this.tabPageUpdate.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageUpdate.Controls.Add(this.groupBoxAutoUpdate);
            this.tabPageUpdate.Controls.Add(this.groupBoxManualUpdate);
            this.tabPageUpdate.Controls.Add(this.groupBoxUpdater);
            this.tabPageUpdate.Location = new System.Drawing.Point(4, 22);
            this.tabPageUpdate.Name = "tabPageUpdate";
            this.tabPageUpdate.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageUpdate.Size = new System.Drawing.Size(262, 341);
            this.tabPageUpdate.TabIndex = 1;
            this.tabPageUpdate.Text = "Updates";
            // 
            // groupBoxAutoUpdate
            // 
            this.groupBoxAutoUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxAutoUpdate.Controls.Add(this.buttonCheckUpdates);
            this.groupBoxAutoUpdate.Controls.Add(this.linkLabelLatestAppVersion);
            this.groupBoxAutoUpdate.Controls.Add(this.linkLabelLatestFirmwareVersion);
            this.groupBoxAutoUpdate.Controls.Add(this.labelVesrion);
            this.groupBoxAutoUpdate.Controls.Add(this.labelAppVersionCurrent);
            this.groupBoxAutoUpdate.Controls.Add(this.labelVersionDetected);
            this.groupBoxAutoUpdate.Controls.Add(this.buttonAutoUpdateApp);
            this.groupBoxAutoUpdate.Controls.Add(this.labelAvailableFW);
            this.groupBoxAutoUpdate.Controls.Add(this.buttonAutoUpdateFirmware);
            this.groupBoxAutoUpdate.Controls.Add(this.labelAvailableApp);
            this.groupBoxAutoUpdate.Controls.Add(this.labelAppVersionTitle);
            this.groupBoxAutoUpdate.Location = new System.Drawing.Point(10, 12);
            this.groupBoxAutoUpdate.Name = "groupBoxAutoUpdate";
            this.groupBoxAutoUpdate.Size = new System.Drawing.Size(243, 187);
            this.groupBoxAutoUpdate.TabIndex = 16;
            this.groupBoxAutoUpdate.TabStop = false;
            this.groupBoxAutoUpdate.Text = "Update Manager";
            // 
            // buttonCheckUpdates
            // 
            this.buttonCheckUpdates.Location = new System.Drawing.Point(5, 145);
            this.buttonCheckUpdates.Name = "buttonCheckUpdates";
            this.buttonCheckUpdates.Size = new System.Drawing.Size(226, 23);
            this.buttonCheckUpdates.TabIndex = 18;
            this.buttonCheckUpdates.Text = "Check for updates";
            this.buttonCheckUpdates.UseVisualStyleBackColor = true;
            this.buttonCheckUpdates.Click += new System.EventHandler(this.buttonCheckUpdates_Click);
            // 
            // linkLabelLatestAppVersion
            // 
            this.linkLabelLatestAppVersion.AutoSize = true;
            this.linkLabelLatestAppVersion.Location = new System.Drawing.Point(172, 97);
            this.linkLabelLatestAppVersion.Name = "linkLabelLatestAppVersion";
            this.linkLabelLatestAppVersion.Size = new System.Drawing.Size(40, 13);
            this.linkLabelLatestAppVersion.TabIndex = 16;
            this.linkLabelLatestAppVersion.TabStop = true;
            this.linkLabelLatestAppVersion.Text = "0.0.0.0";
            this.linkLabelLatestAppVersion.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelLatestAppVersion_LinkClicked);
            // 
            // linkLabelLatestFirmwareVersion
            // 
            this.linkLabelLatestFirmwareVersion.AutoSize = true;
            this.linkLabelLatestFirmwareVersion.Location = new System.Drawing.Point(181, 29);
            this.linkLabelLatestFirmwareVersion.Name = "linkLabelLatestFirmwareVersion";
            this.linkLabelLatestFirmwareVersion.Size = new System.Drawing.Size(31, 13);
            this.linkLabelLatestFirmwareVersion.TabIndex = 17;
            this.linkLabelLatestFirmwareVersion.TabStop = true;
            this.linkLabelLatestFirmwareVersion.Text = "0.0.0";
            this.linkLabelLatestFirmwareVersion.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelLatestAppVersion_LinkClicked);
            // 
            // labelVesrion
            // 
            this.labelVesrion.AutoSize = true;
            this.labelVesrion.Location = new System.Drawing.Point(3, 16);
            this.labelVesrion.Name = "labelVesrion";
            this.labelVesrion.Size = new System.Drawing.Size(142, 13);
            this.labelVesrion.TabIndex = 9;
            this.labelVesrion.Text = "Firmware version on device: ";
            // 
            // labelAppVersionCurrent
            // 
            this.labelAppVersionCurrent.AutoSize = true;
            this.labelAppVersionCurrent.Location = new System.Drawing.Point(172, 84);
            this.labelAppVersionCurrent.Name = "labelAppVersionCurrent";
            this.labelAppVersionCurrent.Size = new System.Drawing.Size(40, 13);
            this.labelAppVersionCurrent.TabIndex = 14;
            this.labelAppVersionCurrent.Text = "0.0.0.0";
            // 
            // labelVersionDetected
            // 
            this.labelVersionDetected.AutoSize = true;
            this.labelVersionDetected.Location = new System.Drawing.Point(181, 16);
            this.labelVersionDetected.Name = "labelVersionDetected";
            this.labelVersionDetected.Size = new System.Drawing.Size(31, 13);
            this.labelVersionDetected.TabIndex = 10;
            this.labelVersionDetected.Text = "0.0.0";
            // 
            // buttonAutoUpdateApp
            // 
            this.buttonAutoUpdateApp.Enabled = false;
            this.buttonAutoUpdateApp.Location = new System.Drawing.Point(5, 114);
            this.buttonAutoUpdateApp.Name = "buttonAutoUpdateApp";
            this.buttonAutoUpdateApp.Size = new System.Drawing.Size(226, 25);
            this.buttonAutoUpdateApp.TabIndex = 15;
            this.buttonAutoUpdateApp.Text = "Update Software";
            this.buttonAutoUpdateApp.UseVisualStyleBackColor = true;
            this.buttonAutoUpdateApp.Click += new System.EventHandler(this.buttonAutoUpdateApp_Click);
            // 
            // labelAvailableFW
            // 
            this.labelAvailableFW.AutoSize = true;
            this.labelAvailableFW.Location = new System.Drawing.Point(3, 29);
            this.labelAvailableFW.Name = "labelAvailableFW";
            this.labelAvailableFW.Size = new System.Drawing.Size(122, 13);
            this.labelAvailableFW.TabIndex = 1;
            this.labelAvailableFW.Text = "Latest Firmware Version:";
            // 
            // buttonAutoUpdateFirmware
            // 
            this.buttonAutoUpdateFirmware.Enabled = false;
            this.buttonAutoUpdateFirmware.Location = new System.Drawing.Point(5, 44);
            this.buttonAutoUpdateFirmware.Name = "buttonAutoUpdateFirmware";
            this.buttonAutoUpdateFirmware.Size = new System.Drawing.Size(226, 25);
            this.buttonAutoUpdateFirmware.TabIndex = 12;
            this.buttonAutoUpdateFirmware.Text = "Update Firmware";
            this.buttonAutoUpdateFirmware.UseVisualStyleBackColor = true;
            this.buttonAutoUpdateFirmware.Click += new System.EventHandler(this.buttonAutoUpdateFirmware_Click);
            // 
            // labelAvailableApp
            // 
            this.labelAvailableApp.AutoSize = true;
            this.labelAvailableApp.Location = new System.Drawing.Point(3, 97);
            this.labelAvailableApp.Name = "labelAvailableApp";
            this.labelAvailableApp.Size = new System.Drawing.Size(122, 13);
            this.labelAvailableApp.TabIndex = 0;
            this.labelAvailableApp.Text = "Latest Software Version:";
            // 
            // labelAppVersionTitle
            // 
            this.labelAppVersionTitle.AutoSize = true;
            this.labelAppVersionTitle.Location = new System.Drawing.Point(3, 84);
            this.labelAppVersionTitle.Name = "labelAppVersionTitle";
            this.labelAppVersionTitle.Size = new System.Drawing.Size(126, 13);
            this.labelAppVersionTitle.TabIndex = 13;
            this.labelAppVersionTitle.Text = "Control Software Version:";
            // 
            // groupBoxManualUpdate
            // 
            this.groupBoxManualUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxManualUpdate.Controls.Add(this.buttonAdvancedFW);
            this.groupBoxManualUpdate.Location = new System.Drawing.Point(10, 274);
            this.groupBoxManualUpdate.Name = "groupBoxManualUpdate";
            this.groupBoxManualUpdate.Size = new System.Drawing.Size(243, 61);
            this.groupBoxManualUpdate.TabIndex = 13;
            this.groupBoxManualUpdate.TabStop = false;
            this.groupBoxManualUpdate.Text = "Manual Update";
            // 
            // groupBoxUpdater
            // 
            this.groupBoxUpdater.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxUpdater.Controls.Add(this.checkBoxNotifyApp);
            this.groupBoxUpdater.Controls.Add(this.checkBoxNotifyFirmware);
            this.groupBoxUpdater.Location = new System.Drawing.Point(10, 204);
            this.groupBoxUpdater.Name = "groupBoxUpdater";
            this.groupBoxUpdater.Size = new System.Drawing.Size(243, 64);
            this.groupBoxUpdater.TabIndex = 12;
            this.groupBoxUpdater.TabStop = false;
            this.groupBoxUpdater.Text = "Update Notifications";
            // 
            // checkBoxNotifyApp
            // 
            this.checkBoxNotifyApp.AutoSize = true;
            this.checkBoxNotifyApp.Location = new System.Drawing.Point(6, 39);
            this.checkBoxNotifyApp.Name = "checkBoxNotifyApp";
            this.checkBoxNotifyApp.Size = new System.Drawing.Size(225, 17);
            this.checkBoxNotifyApp.TabIndex = 2;
            this.checkBoxNotifyApp.Text = "Notify when a software update is available";
            this.checkBoxNotifyApp.UseVisualStyleBackColor = true;
            this.checkBoxNotifyApp.CheckedChanged += new System.EventHandler(this.checkBoxNotifyApp_CheckedChanged);
            // 
            // checkBoxNotifyFirmware
            // 
            this.checkBoxNotifyFirmware.AutoSize = true;
            this.checkBoxNotifyFirmware.Location = new System.Drawing.Point(6, 19);
            this.checkBoxNotifyFirmware.Name = "checkBoxNotifyFirmware";
            this.checkBoxNotifyFirmware.Size = new System.Drawing.Size(227, 17);
            this.checkBoxNotifyFirmware.TabIndex = 0;
            this.checkBoxNotifyFirmware.Text = "Notify when a Firmware update is available";
            this.checkBoxNotifyFirmware.UseVisualStyleBackColor = true;
            this.checkBoxNotifyFirmware.CheckedChanged += new System.EventHandler(this.checkBoxNotifyFirmware_CheckedChanged);
            // 
            // tabPageSettings
            // 
            this.tabPageSettings.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageSettings.Controls.Add(this.panel1);
            this.tabPageSettings.Location = new System.Drawing.Point(4, 22);
            this.tabPageSettings.Name = "tabPageSettings";
            this.tabPageSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSettings.Size = new System.Drawing.Size(262, 341);
            this.tabPageSettings.TabIndex = 0;
            this.tabPageSettings.Text = "Settings";
            // 
            // tabControlMain
            // 
            this.tabControlMain.AllowDrop = true;
            this.tabControlMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlMain.Controls.Add(this.tabPageSettings);
            this.tabControlMain.Controls.Add(this.tabPageUpdate);
            this.tabControlMain.Location = new System.Drawing.Point(2, 12);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(270, 367);
            this.tabControlMain.TabIndex = 14;
            // 
            // statusStripMain
            // 
            this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelColor,
            this.toolStripStatusLabelInfo,
            this.toolStripLabelUploadStatus});
            this.statusStripMain.Location = new System.Drawing.Point(0, 383);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Size = new System.Drawing.Size(272, 24);
            this.statusStripMain.TabIndex = 15;
            this.statusStripMain.Text = "statusStrip1";
            // 
            // toolStripStatusLabelColor
            // 
            this.toolStripStatusLabelColor.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusLabelColor.ForeColor = System.Drawing.Color.Red;
            this.toolStripStatusLabelColor.Name = "toolStripStatusLabelColor";
            this.toolStripStatusLabelColor.Size = new System.Drawing.Size(17, 19);
            this.toolStripStatusLabelColor.Text = "●";
            this.toolStripStatusLabelColor.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // toolStripStatusLabelInfo
            // 
            this.toolStripStatusLabelInfo.Name = "toolStripStatusLabelInfo";
            this.toolStripStatusLabelInfo.Size = new System.Drawing.Size(240, 19);
            this.toolStripStatusLabelInfo.Spring = true;
            this.toolStripStatusLabelInfo.Text = "No Device Connected";
            this.toolStripStatusLabelInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripLabelUploadStatus
            // 
            this.toolStripLabelUploadStatus.Name = "toolStripLabelUploadStatus";
            this.toolStripLabelUploadStatus.Size = new System.Drawing.Size(24, 19);
            this.toolStripLabelUploadStatus.Text = "NA";
            this.toolStripLabelUploadStatus.Visible = false;
            // 
            // notifyIconMain
            // 
            this.notifyIconMain.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIconMain.Icon")));
            this.notifyIconMain.Text = "FCC Controller";
            this.notifyIconMain.Visible = true;
            this.notifyIconMain.BalloonTipClicked += new System.EventHandler(this.notifyIconMain_BalloonTipClicked);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(272, 407);
            this.Controls.Add(this.statusStripMain);
            this.Controls.Add(this.tabControlMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(100, 100);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "FCC Configurator";
            ((System.ComponentModel.ISupportInitialize)(this.numericUserDefined)).EndInit();
            this.groupBoxSensitivity.ResumeLayout(false);
            this.groupBoxSensitivity.PerformLayout();
            this.groupBoxFeatures.ResumeLayout(false);
            this.groupBoxFeatures.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tabPageUpdate.ResumeLayout(false);
            this.groupBoxAutoUpdate.ResumeLayout(false);
            this.groupBoxAutoUpdate.PerformLayout();
            this.groupBoxManualUpdate.ResumeLayout(false);
            this.groupBoxUpdater.ResumeLayout(false);
            this.groupBoxUpdater.PerformLayout();
            this.tabPageSettings.ResumeLayout(false);
            this.tabControlMain.ResumeLayout(false);
            this.statusStripMain.ResumeLayout(false);
            this.statusStripMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.CheckBox checkBoxRotate;
        private System.Windows.Forms.CheckBox checkBoxForceMapping;
        private System.Windows.Forms.RadioButton radioButton4Kg;
        private System.Windows.Forms.RadioButton radioButton6Kg;
        private System.Windows.Forms.RadioButton radioButton9Kg;
        private System.Windows.Forms.RadioButton radioButtonUser;
        private System.Windows.Forms.NumericUpDown numericUserDefined;
        private System.Windows.Forms.Button buttonDefaults;
        private System.Windows.Forms.ToolTip toolTipMainForm;
        private System.Windows.Forms.GroupBox groupBoxSensitivity;
        private System.Windows.Forms.GroupBox groupBoxFeatures;
        private System.Windows.Forms.Button buttonAdvancedFW;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabPage tabPageUpdate;
        private System.Windows.Forms.TabPage tabPageSettings;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.Label labelForceMapping;
        private System.Windows.Forms.Label labelSensorRotation;
        private System.Windows.Forms.Label labelCurrentUserDefined;
        private System.Windows.Forms.Label labelForce4Kg;
        private System.Windows.Forms.Label labelForceUser;
        private System.Windows.Forms.Label labelForce9kg;
        private System.Windows.Forms.Label labelForce6kg;
        private System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelInfo;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelColor;
        private System.Windows.Forms.Button buttonCenter;
        private System.Windows.Forms.ComboBox comboBoxUnit;
        private System.Windows.Forms.Label labelVersionDetected;
        private System.Windows.Forms.Label labelVesrion;
        private System.Windows.Forms.GroupBox groupBoxUpdater;
        private System.Windows.Forms.Label labelAvailableFW;
        private System.Windows.Forms.Label labelAvailableApp;
        private System.Windows.Forms.GroupBox groupBoxManualUpdate;
        private System.Windows.Forms.Label labelAppVersionTitle;
        private System.Windows.Forms.Label labelAppVersionCurrent;
        private System.Windows.Forms.CheckBox checkBoxNotifyApp;
        private System.Windows.Forms.CheckBox checkBoxNotifyFirmware;
        private System.Windows.Forms.Button buttonAutoUpdateApp;
        private System.Windows.Forms.Button buttonAutoUpdateFirmware;
        private System.Windows.Forms.NotifyIcon notifyIconMain;
        private System.Windows.Forms.LinkLabel linkLabelLatestFirmwareVersion;
        private System.Windows.Forms.LinkLabel linkLabelLatestAppVersion;
        private System.Windows.Forms.ToolStripStatusLabel toolStripLabelUploadStatus;
        private System.Windows.Forms.GroupBox groupBoxAutoUpdate;
        private System.Windows.Forms.Button buttonCheckUpdates;
    }
}
