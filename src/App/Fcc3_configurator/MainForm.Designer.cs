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
            this.buttonInitialSetup = new System.Windows.Forms.Button();
            this.comboBoxUnit = new System.Windows.Forms.ComboBox();
            this.radioButtonAnalogFlcs = new System.Windows.Forms.RadioButton();
            this.ButtonReadSettings = new System.Windows.Forms.Button();
            this.radioButtonDigitalFlcs = new System.Windows.Forms.RadioButton();
            this.buttonTestStick = new System.Windows.Forms.Button();
            this.groupBoxSensitivity = new System.Windows.Forms.GroupBox();
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
            this.groupBoxUpdater = new System.Windows.Forms.GroupBox();
            this.checkBoxNotifyApp = new System.Windows.Forms.CheckBox();
            this.checkBoxNotifyFirmware = new System.Windows.Forms.CheckBox();
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
            this.groupBoxUpdater.SuspendLayout();
            this.groupBoxManualUpdate.SuspendLayout();
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
            this.buttonApply.Location = new System.Drawing.Point(9, 387);
            this.buttonApply.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(373, 70);
            this.buttonApply.TabIndex = 0;
            this.buttonApply.Text = "&Apply Config";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // checkBoxRotate
            // 
            this.checkBoxRotate.AutoSize = true;
            this.checkBoxRotate.Location = new System.Drawing.Point(9, 29);
            this.checkBoxRotate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBoxRotate.Name = "checkBoxRotate";
            this.checkBoxRotate.Size = new System.Drawing.Size(226, 24);
            this.checkBoxRotate.TabIndex = 1;
            this.checkBoxRotate.Text = "Sensor &Rotation Emulation";
            this.toolTipMainForm.SetToolTip(this.checkBoxRotate, "Emulate Sensor assembly\'s 12 degrees CW rotation");
            this.checkBoxRotate.UseVisualStyleBackColor = true;
            // 
            // checkBoxForceMapping
            // 
            this.checkBoxForceMapping.AutoSize = true;
            this.checkBoxForceMapping.Location = new System.Drawing.Point(9, 65);
            this.checkBoxForceMapping.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBoxForceMapping.Name = "checkBoxForceMapping";
            this.checkBoxForceMapping.Size = new System.Drawing.Size(230, 24);
            this.checkBoxForceMapping.TabIndex = 2;
            this.checkBoxForceMapping.Text = "Proprotional &Force Mapping";
            this.toolTipMainForm.SetToolTip(this.checkBoxForceMapping, " Deflection force ratios are derivitive from FLCS version selected");
            this.checkBoxForceMapping.UseVisualStyleBackColor = true;
            this.checkBoxForceMapping.CheckedChanged += new System.EventHandler(this.checkBoxEmulateFlcs_CheckedChanged);
            // 
            // radioButton4Kg
            // 
            this.radioButton4Kg.AutoSize = true;
            this.radioButton4Kg.Checked = true;
            this.radioButton4Kg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.radioButton4Kg.Location = new System.Drawing.Point(9, 29);
            this.radioButton4Kg.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radioButton4Kg.Name = "radioButton4Kg";
            this.radioButton4Kg.Size = new System.Drawing.Size(144, 24);
            this.radioButton4Kg.TabIndex = 3;
            this.radioButton4Kg.TabStop = true;
            this.radioButton4Kg.Text = "&4.5 Kgf (10 lbf)";
            this.toolTipMainForm.SetToolTip(this.radioButton4Kg, "Max value will be reached at selected force Setting");
            this.radioButton4Kg.UseVisualStyleBackColor = true;
            // 
            // radioButton6Kg
            // 
            this.radioButton6Kg.AutoSize = true;
            this.radioButton6Kg.Location = new System.Drawing.Point(9, 65);
            this.radioButton6Kg.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radioButton6Kg.Name = "radioButton6Kg";
            this.radioButton6Kg.Size = new System.Drawing.Size(132, 24);
            this.radioButton6Kg.TabIndex = 4;
            this.radioButton6Kg.Text = "&6 Kgf   (13 lbf)";
            this.toolTipMainForm.SetToolTip(this.radioButton6Kg, "Max value will be reached at selected force Setting");
            this.radioButton6Kg.UseVisualStyleBackColor = true;
            // 
            // radioButton9Kg
            // 
            this.radioButton9Kg.AutoSize = true;
            this.radioButton9Kg.Location = new System.Drawing.Point(9, 100);
            this.radioButton9Kg.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radioButton9Kg.Name = "radioButton9Kg";
            this.radioButton9Kg.Size = new System.Drawing.Size(132, 24);
            this.radioButton9Kg.TabIndex = 5;
            this.radioButton9Kg.Text = "&9 Kgf   (20 lbf)";
            this.toolTipMainForm.SetToolTip(this.radioButton9Kg, "Max value will be reached at selected force Setting");
            this.radioButton9Kg.UseVisualStyleBackColor = true;
            // 
            // radioButtonUser
            // 
            this.radioButtonUser.AutoSize = true;
            this.radioButtonUser.Location = new System.Drawing.Point(9, 135);
            this.radioButtonUser.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radioButtonUser.Name = "radioButtonUser";
            this.radioButtonUser.Size = new System.Drawing.Size(220, 24);
            this.radioButtonUser.TabIndex = 6;
            this.radioButtonUser.Text = "&User Defined force setting";
            this.toolTipMainForm.SetToolTip(this.radioButtonUser, "Max value will be reached at selected force Setting");
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
            this.numericUserDefined.Location = new System.Drawing.Point(48, 171);
            this.numericUserDefined.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
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
            this.numericUserDefined.Size = new System.Drawing.Size(69, 26);
            this.numericUserDefined.TabIndex = 7;
            this.toolTipMainForm.SetToolTip(this.numericUserDefined, "Set desired force limit");
            this.numericUserDefined.Value = new decimal(new int[] {
            30,
            0,
            0,
            65536});
            this.numericUserDefined.ValueChanged += new System.EventHandler(this.numericUserDefined_ValueChanged);
            // 
            // buttonDefaults
            // 
            this.buttonDefaults.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDefaults.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonDefaults.Location = new System.Drawing.Point(202, 556);
            this.buttonDefaults.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonDefaults.Name = "buttonDefaults";
            this.buttonDefaults.Size = new System.Drawing.Size(180, 40);
            this.buttonDefaults.TabIndex = 9;
            this.buttonDefaults.Text = "&Restore Defaults";
            this.toolTipMainForm.SetToolTip(this.buttonDefaults, "Restore facroty defaults");
            this.buttonDefaults.UseVisualStyleBackColor = true;
            this.buttonDefaults.Click += new System.EventHandler(this.buttonDefaults_Click);
            // 
            // buttonInitialSetup
            // 
            this.buttonInitialSetup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonInitialSetup.Location = new System.Drawing.Point(202, 511);
            this.buttonInitialSetup.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonInitialSetup.Name = "buttonInitialSetup";
            this.buttonInitialSetup.Size = new System.Drawing.Size(180, 40);
            this.buttonInitialSetup.TabIndex = 17;
            this.buttonInitialSetup.Text = "&Initial Config";
            this.toolTipMainForm.SetToolTip(this.buttonInitialSetup, "Select Hardware gains to match your FCC model (required only once) - please click" +
        " \"restore defaults\" after changing");
            this.buttonInitialSetup.UseVisualStyleBackColor = true;
            this.buttonInitialSetup.Click += new System.EventHandler(this.buttonInitialSetup_Click);
            // 
            // comboBoxUnit
            // 
            this.comboBoxUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxUnit.FormattingEnabled = true;
            this.comboBoxUnit.Items.AddRange(new object[] {
            "Kgf",
            "lbf"});
            this.comboBoxUnit.Location = new System.Drawing.Point(126, 171);
            this.comboBoxUnit.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBoxUnit.Name = "comboBoxUnit";
            this.comboBoxUnit.Size = new System.Drawing.Size(64, 28);
            this.comboBoxUnit.TabIndex = 16;
            this.toolTipMainForm.SetToolTip(this.comboBoxUnit, "Select force mesurment unit");
            this.comboBoxUnit.SelectedIndexChanged += new System.EventHandler(this.comboBoxUnit_SelectedIndexChanged);
            // 
            // radioButtonAnalogFlcs
            // 
            this.radioButtonAnalogFlcs.AutoSize = true;
            this.radioButtonAnalogFlcs.Enabled = false;
            this.radioButtonAnalogFlcs.Location = new System.Drawing.Point(9, 101);
            this.radioButtonAnalogFlcs.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radioButtonAnalogFlcs.Name = "radioButtonAnalogFlcs";
            this.radioButtonAnalogFlcs.Size = new System.Drawing.Size(129, 24);
            this.radioButtonAnalogFlcs.TabIndex = 17;
            this.radioButtonAnalogFlcs.Text = "A&nalog FLCS";
            this.toolTipMainForm.SetToolTip(this.radioButtonAnalogFlcs, "Analog FLCS ratios (P+: 180N, P-/R:80N)");
            this.radioButtonAnalogFlcs.UseVisualStyleBackColor = true;
            // 
            // ButtonReadSettings
            // 
            this.ButtonReadSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonReadSettings.Location = new System.Drawing.Point(9, 556);
            this.ButtonReadSettings.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ButtonReadSettings.Name = "ButtonReadSettings";
            this.ButtonReadSettings.Size = new System.Drawing.Size(180, 40);
            this.ButtonReadSettings.TabIndex = 14;
            this.ButtonReadSettings.Text = "Read and Save";
            this.toolTipMainForm.SetToolTip(this.ButtonReadSettings, "Read current settings from stick and store them.");
            this.ButtonReadSettings.UseVisualStyleBackColor = true;
            this.ButtonReadSettings.Click += new System.EventHandler(this.buttonCenter_Click);
            // 
            // radioButtonDigitalFlcs
            // 
            this.radioButtonDigitalFlcs.AutoSize = true;
            this.radioButtonDigitalFlcs.Checked = true;
            this.radioButtonDigitalFlcs.Enabled = false;
            this.radioButtonDigitalFlcs.Location = new System.Drawing.Point(148, 101);
            this.radioButtonDigitalFlcs.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radioButtonDigitalFlcs.Name = "radioButtonDigitalFlcs";
            this.radioButtonDigitalFlcs.Size = new System.Drawing.Size(123, 24);
            this.radioButtonDigitalFlcs.TabIndex = 18;
            this.radioButtonDigitalFlcs.TabStop = true;
            this.radioButtonDigitalFlcs.Text = "&Digital FLCS";
            this.toolTipMainForm.SetToolTip(this.radioButtonDigitalFlcs, "Digital FLCS ratios (P+: 25lbf, P: 16lbf, R:17lbf)");
            this.radioButtonDigitalFlcs.UseVisualStyleBackColor = true;
            // 
            // buttonTestStick
            // 
            this.buttonTestStick.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTestStick.Location = new System.Drawing.Point(9, 465);
            this.buttonTestStick.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonTestStick.Name = "buttonTestStick";
            this.buttonTestStick.Size = new System.Drawing.Size(373, 40);
            this.buttonTestStick.TabIndex = 18;
            this.buttonTestStick.Text = "&Test Stick";
            this.toolTipMainForm.SetToolTip(this.buttonTestStick, "Test stick range of motion");
            this.buttonTestStick.UseVisualStyleBackColor = true;
            this.buttonTestStick.Click += new System.EventHandler(this.buttonTestStick_Click);
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
            this.groupBoxSensitivity.Location = new System.Drawing.Point(9, 5);
            this.groupBoxSensitivity.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxSensitivity.Name = "groupBoxSensitivity";
            this.groupBoxSensitivity.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxSensitivity.Size = new System.Drawing.Size(373, 214);
            this.groupBoxSensitivity.TabIndex = 12;
            this.groupBoxSensitivity.TabStop = false;
            this.groupBoxSensitivity.Text = "Sensitivity Settings";
            // 
            // labelForceUser
            // 
            this.labelForceUser.AutoSize = true;
            this.labelForceUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.labelForceUser.ForeColor = System.Drawing.Color.Green;
            this.labelForceUser.Location = new System.Drawing.Point(314, 139);
            this.labelForceUser.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelForceUser.Name = "labelForceUser";
            this.labelForceUser.Size = new System.Drawing.Size(20, 20);
            this.labelForceUser.TabIndex = 13;
            this.labelForceUser.Text = "●";
            this.labelForceUser.Visible = false;
            // 
            // labelForce9kg
            // 
            this.labelForce9kg.AutoSize = true;
            this.labelForce9kg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.labelForce9kg.ForeColor = System.Drawing.Color.Green;
            this.labelForce9kg.Location = new System.Drawing.Point(314, 102);
            this.labelForce9kg.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelForce9kg.Name = "labelForce9kg";
            this.labelForce9kg.Size = new System.Drawing.Size(20, 20);
            this.labelForce9kg.TabIndex = 12;
            this.labelForce9kg.Text = "●";
            this.labelForce9kg.Visible = false;
            // 
            // labelForce6kg
            // 
            this.labelForce6kg.AutoSize = true;
            this.labelForce6kg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.labelForce6kg.ForeColor = System.Drawing.Color.Green;
            this.labelForce6kg.Location = new System.Drawing.Point(314, 68);
            this.labelForce6kg.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelForce6kg.Name = "labelForce6kg";
            this.labelForce6kg.Size = new System.Drawing.Size(20, 20);
            this.labelForce6kg.TabIndex = 11;
            this.labelForce6kg.Text = "●";
            this.labelForce6kg.Visible = false;
            // 
            // labelForce4Kg
            // 
            this.labelForce4Kg.AutoSize = true;
            this.labelForce4Kg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.labelForce4Kg.ForeColor = System.Drawing.Color.Green;
            this.labelForce4Kg.Location = new System.Drawing.Point(314, 32);
            this.labelForce4Kg.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelForce4Kg.Name = "labelForce4Kg";
            this.labelForce4Kg.Size = new System.Drawing.Size(20, 20);
            this.labelForce4Kg.TabIndex = 10;
            this.labelForce4Kg.Text = "●";
            this.labelForce4Kg.Visible = false;
            // 
            // labelCurrentUserDefined
            // 
            this.labelCurrentUserDefined.AutoSize = true;
            this.labelCurrentUserDefined.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.labelCurrentUserDefined.Location = new System.Drawing.Point(253, 174);
            this.labelCurrentUserDefined.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCurrentUserDefined.Name = "labelCurrentUserDefined";
            this.labelCurrentUserDefined.Size = new System.Drawing.Size(68, 20);
            this.labelCurrentUserDefined.TabIndex = 9;
            this.labelCurrentUserDefined.Text = "0.0 Kgf";
            this.labelCurrentUserDefined.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // groupBoxFeatures
            // 
            this.groupBoxFeatures.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxFeatures.Controls.Add(this.radioButtonDigitalFlcs);
            this.groupBoxFeatures.Controls.Add(this.radioButtonAnalogFlcs);
            this.groupBoxFeatures.Controls.Add(this.labelForceMapping);
            this.groupBoxFeatures.Controls.Add(this.labelSensorRotation);
            this.groupBoxFeatures.Controls.Add(this.checkBoxRotate);
            this.groupBoxFeatures.Controls.Add(this.checkBoxForceMapping);
            this.groupBoxFeatures.Location = new System.Drawing.Point(9, 228);
            this.groupBoxFeatures.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxFeatures.Name = "groupBoxFeatures";
            this.groupBoxFeatures.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxFeatures.Size = new System.Drawing.Size(373, 141);
            this.groupBoxFeatures.TabIndex = 13;
            this.groupBoxFeatures.TabStop = false;
            this.groupBoxFeatures.Text = "Special Features";
            // 
            // labelForceMapping
            // 
            this.labelForceMapping.AutoSize = true;
            this.labelForceMapping.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.labelForceMapping.ForeColor = System.Drawing.Color.Red;
            this.labelForceMapping.Location = new System.Drawing.Point(234, 66);
            this.labelForceMapping.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelForceMapping.Name = "labelForceMapping";
            this.labelForceMapping.Size = new System.Drawing.Size(104, 20);
            this.labelForceMapping.TabIndex = 16;
            this.labelForceMapping.Text = "FLCS: OFF";
            // 
            // labelSensorRotation
            // 
            this.labelSensorRotation.AutoSize = true;
            this.labelSensorRotation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.labelSensorRotation.ForeColor = System.Drawing.Color.Red;
            this.labelSensorRotation.Location = new System.Drawing.Point(291, 31);
            this.labelSensorRotation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSensorRotation.Name = "labelSensorRotation";
            this.labelSensorRotation.Size = new System.Drawing.Size(45, 20);
            this.labelSensorRotation.TabIndex = 15;
            this.labelSensorRotation.Text = "OFF";
            // 
            // buttonAdvancedFW
            // 
            this.buttonAdvancedFW.Location = new System.Drawing.Point(8, 29);
            this.buttonAdvancedFW.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonAdvancedFW.Name = "buttonAdvancedFW";
            this.buttonAdvancedFW.Size = new System.Drawing.Size(339, 39);
            this.buttonAdvancedFW.TabIndex = 1;
            this.buttonAdvancedFW.Text = "Upload Firmware Manually";
            this.buttonAdvancedFW.UseVisualStyleBackColor = true;
            this.buttonAdvancedFW.Click += new System.EventHandler(this.buttonAdvancedFW_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonCenter);
            this.panel1.Controls.Add(this.buttonTestStick);
            this.panel1.Controls.Add(this.buttonInitialSetup);
            this.panel1.Controls.Add(this.ButtonReadSettings);
            this.panel1.Controls.Add(this.groupBoxSensitivity);
            this.panel1.Controls.Add(this.buttonApply);
            this.panel1.Controls.Add(this.groupBoxFeatures);
            this.panel1.Controls.Add(this.buttonDefaults);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(4, 5);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(389, 607);
            this.panel1.TabIndex = 16;
            // 
            // buttonCenter
            // 
            this.buttonCenter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCenter.Location = new System.Drawing.Point(9, 511);
            this.buttonCenter.Name = "buttonCenter";
            this.buttonCenter.Size = new System.Drawing.Size(180, 40);
            this.buttonCenter.TabIndex = 19;
            this.buttonCenter.Text = "&Center";
            this.toolTipMainForm.SetToolTip(this.buttonCenter, "Read current stick location as center (not persistant)");
            this.buttonCenter.UseVisualStyleBackColor = true;
            this.buttonCenter.Click += new System.EventHandler(this.buttonCenter_Click);
            // 
            // tabPageUpdate
            // 
            this.tabPageUpdate.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageUpdate.Controls.Add(this.groupBoxAutoUpdate);
            this.tabPageUpdate.Controls.Add(this.groupBoxManualUpdate);
            this.tabPageUpdate.Location = new System.Drawing.Point(4, 29);
            this.tabPageUpdate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageUpdate.Name = "tabPageUpdate";
            this.tabPageUpdate.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageUpdate.Size = new System.Drawing.Size(397, 617);
            this.tabPageUpdate.TabIndex = 1;
            this.tabPageUpdate.Text = "Updates";
            // 
            // groupBoxAutoUpdate
            // 
            this.groupBoxAutoUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxAutoUpdate.Controls.Add(this.buttonCheckUpdates);
            this.groupBoxAutoUpdate.Controls.Add(this.linkLabelLatestAppVersion);
            this.groupBoxAutoUpdate.Controls.Add(this.groupBoxUpdater);
            this.groupBoxAutoUpdate.Controls.Add(this.linkLabelLatestFirmwareVersion);
            this.groupBoxAutoUpdate.Controls.Add(this.labelVesrion);
            this.groupBoxAutoUpdate.Controls.Add(this.labelAppVersionCurrent);
            this.groupBoxAutoUpdate.Controls.Add(this.labelVersionDetected);
            this.groupBoxAutoUpdate.Controls.Add(this.buttonAutoUpdateApp);
            this.groupBoxAutoUpdate.Controls.Add(this.labelAvailableFW);
            this.groupBoxAutoUpdate.Controls.Add(this.buttonAutoUpdateFirmware);
            this.groupBoxAutoUpdate.Controls.Add(this.labelAvailableApp);
            this.groupBoxAutoUpdate.Controls.Add(this.labelAppVersionTitle);
            this.groupBoxAutoUpdate.Location = new System.Drawing.Point(15, 19);
            this.groupBoxAutoUpdate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxAutoUpdate.Name = "groupBoxAutoUpdate";
            this.groupBoxAutoUpdate.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxAutoUpdate.Size = new System.Drawing.Size(364, 480);
            this.groupBoxAutoUpdate.TabIndex = 16;
            this.groupBoxAutoUpdate.TabStop = false;
            this.groupBoxAutoUpdate.Text = "Update Manager";
            // 
            // buttonCheckUpdates
            // 
            this.buttonCheckUpdates.Location = new System.Drawing.Point(8, 425);
            this.buttonCheckUpdates.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonCheckUpdates.Name = "buttonCheckUpdates";
            this.buttonCheckUpdates.Size = new System.Drawing.Size(339, 39);
            this.buttonCheckUpdates.TabIndex = 18;
            this.buttonCheckUpdates.Text = "Check for updates";
            this.buttonCheckUpdates.UseVisualStyleBackColor = true;
            this.buttonCheckUpdates.Click += new System.EventHandler(this.buttonCheckUpdates_Click);
            // 
            // linkLabelLatestAppVersion
            // 
            this.linkLabelLatestAppVersion.AutoSize = true;
            this.linkLabelLatestAppVersion.Location = new System.Drawing.Point(262, 176);
            this.linkLabelLatestAppVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.linkLabelLatestAppVersion.Name = "linkLabelLatestAppVersion";
            this.linkLabelLatestAppVersion.Size = new System.Drawing.Size(57, 20);
            this.linkLabelLatestAppVersion.TabIndex = 16;
            this.linkLabelLatestAppVersion.TabStop = true;
            this.linkLabelLatestAppVersion.Text = "0.0.0.0";
            this.linkLabelLatestAppVersion.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelLatestAppVersion_LinkClicked);
            // 
            // groupBoxUpdater
            // 
            this.groupBoxUpdater.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxUpdater.Controls.Add(this.checkBoxNotifyApp);
            this.groupBoxUpdater.Controls.Add(this.checkBoxNotifyFirmware);
            this.groupBoxUpdater.Location = new System.Drawing.Point(8, 319);
            this.groupBoxUpdater.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxUpdater.Name = "groupBoxUpdater";
            this.groupBoxUpdater.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxUpdater.Size = new System.Drawing.Size(339, 96);
            this.groupBoxUpdater.TabIndex = 12;
            this.groupBoxUpdater.TabStop = false;
            this.groupBoxUpdater.Text = "Update Notifications";
            // 
            // checkBoxNotifyApp
            // 
            this.checkBoxNotifyApp.AutoSize = true;
            this.checkBoxNotifyApp.Location = new System.Drawing.Point(9, 60);
            this.checkBoxNotifyApp.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBoxNotifyApp.Name = "checkBoxNotifyApp";
            this.checkBoxNotifyApp.Size = new System.Drawing.Size(224, 24);
            this.checkBoxNotifyApp.TabIndex = 2;
            this.checkBoxNotifyApp.Text = "Notify on software updates";
            this.checkBoxNotifyApp.UseVisualStyleBackColor = true;
            this.checkBoxNotifyApp.CheckedChanged += new System.EventHandler(this.checkBoxNotifyApp_CheckedChanged);
            // 
            // checkBoxNotifyFirmware
            // 
            this.checkBoxNotifyFirmware.AutoSize = true;
            this.checkBoxNotifyFirmware.Location = new System.Drawing.Point(9, 29);
            this.checkBoxNotifyFirmware.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBoxNotifyFirmware.Name = "checkBoxNotifyFirmware";
            this.checkBoxNotifyFirmware.Size = new System.Drawing.Size(228, 24);
            this.checkBoxNotifyFirmware.TabIndex = 0;
            this.checkBoxNotifyFirmware.Text = "Notify on Firmware updates";
            this.checkBoxNotifyFirmware.UseVisualStyleBackColor = true;
            this.checkBoxNotifyFirmware.CheckedChanged += new System.EventHandler(this.checkBoxNotifyFirmware_CheckedChanged);
            // 
            // linkLabelLatestFirmwareVersion
            // 
            this.linkLabelLatestFirmwareVersion.AutoSize = true;
            this.linkLabelLatestFirmwareVersion.Location = new System.Drawing.Point(271, 49);
            this.linkLabelLatestFirmwareVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.linkLabelLatestFirmwareVersion.Name = "linkLabelLatestFirmwareVersion";
            this.linkLabelLatestFirmwareVersion.Size = new System.Drawing.Size(44, 20);
            this.linkLabelLatestFirmwareVersion.TabIndex = 17;
            this.linkLabelLatestFirmwareVersion.TabStop = true;
            this.linkLabelLatestFirmwareVersion.Text = "0.0.0";
            this.linkLabelLatestFirmwareVersion.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelLatestAppVersion_LinkClicked);
            // 
            // labelVesrion
            // 
            this.labelVesrion.AutoSize = true;
            this.labelVesrion.Location = new System.Drawing.Point(4, 29);
            this.labelVesrion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelVesrion.Name = "labelVesrion";
            this.labelVesrion.Size = new System.Drawing.Size(211, 20);
            this.labelVesrion.TabIndex = 9;
            this.labelVesrion.Text = "Firmware Version on device: ";
            // 
            // labelAppVersionCurrent
            // 
            this.labelAppVersionCurrent.AutoSize = true;
            this.labelAppVersionCurrent.Location = new System.Drawing.Point(262, 156);
            this.labelAppVersionCurrent.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelAppVersionCurrent.Name = "labelAppVersionCurrent";
            this.labelAppVersionCurrent.Size = new System.Drawing.Size(57, 20);
            this.labelAppVersionCurrent.TabIndex = 14;
            this.labelAppVersionCurrent.Text = "0.0.0.0";
            // 
            // labelVersionDetected
            // 
            this.labelVersionDetected.AutoSize = true;
            this.labelVersionDetected.Location = new System.Drawing.Point(271, 29);
            this.labelVersionDetected.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelVersionDetected.Name = "labelVersionDetected";
            this.labelVersionDetected.Size = new System.Drawing.Size(44, 20);
            this.labelVersionDetected.TabIndex = 10;
            this.labelVersionDetected.Text = "0.0.0";
            // 
            // buttonAutoUpdateApp
            // 
            this.buttonAutoUpdateApp.Enabled = false;
            this.buttonAutoUpdateApp.Location = new System.Drawing.Point(8, 210);
            this.buttonAutoUpdateApp.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonAutoUpdateApp.Name = "buttonAutoUpdateApp";
            this.buttonAutoUpdateApp.Size = new System.Drawing.Size(339, 69);
            this.buttonAutoUpdateApp.TabIndex = 15;
            this.buttonAutoUpdateApp.Text = "Update Software";
            this.buttonAutoUpdateApp.UseVisualStyleBackColor = true;
            this.buttonAutoUpdateApp.Click += new System.EventHandler(this.buttonAutoUpdateApp_Click);
            // 
            // labelAvailableFW
            // 
            this.labelAvailableFW.AutoSize = true;
            this.labelAvailableFW.Location = new System.Drawing.Point(4, 49);
            this.labelAvailableFW.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelAvailableFW.Name = "labelAvailableFW";
            this.labelAvailableFW.Size = new System.Drawing.Size(185, 20);
            this.labelAvailableFW.TabIndex = 1;
            this.labelAvailableFW.Text = "Latest Firmware Version:";
            // 
            // buttonAutoUpdateFirmware
            // 
            this.buttonAutoUpdateFirmware.Enabled = false;
            this.buttonAutoUpdateFirmware.Location = new System.Drawing.Point(8, 76);
            this.buttonAutoUpdateFirmware.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonAutoUpdateFirmware.Name = "buttonAutoUpdateFirmware";
            this.buttonAutoUpdateFirmware.Size = new System.Drawing.Size(339, 69);
            this.buttonAutoUpdateFirmware.TabIndex = 12;
            this.buttonAutoUpdateFirmware.Text = "Update Firmware";
            this.buttonAutoUpdateFirmware.UseVisualStyleBackColor = true;
            this.buttonAutoUpdateFirmware.Click += new System.EventHandler(this.buttonAutoUpdateFirmware_Click);
            // 
            // labelAvailableApp
            // 
            this.labelAvailableApp.AutoSize = true;
            this.labelAvailableApp.Location = new System.Drawing.Point(9, 176);
            this.labelAvailableApp.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelAvailableApp.Name = "labelAvailableApp";
            this.labelAvailableApp.Size = new System.Drawing.Size(184, 20);
            this.labelAvailableApp.TabIndex = 0;
            this.labelAvailableApp.Text = "Latest Software Version:";
            // 
            // labelAppVersionTitle
            // 
            this.labelAppVersionTitle.AutoSize = true;
            this.labelAppVersionTitle.Location = new System.Drawing.Point(9, 156);
            this.labelAppVersionTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelAppVersionTitle.Name = "labelAppVersionTitle";
            this.labelAppVersionTitle.Size = new System.Drawing.Size(190, 20);
            this.labelAppVersionTitle.TabIndex = 13;
            this.labelAppVersionTitle.Text = "Control Software Version:";
            // 
            // groupBoxManualUpdate
            // 
            this.groupBoxManualUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxManualUpdate.Controls.Add(this.buttonAdvancedFW);
            this.groupBoxManualUpdate.Location = new System.Drawing.Point(15, 522);
            this.groupBoxManualUpdate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxManualUpdate.Name = "groupBoxManualUpdate";
            this.groupBoxManualUpdate.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxManualUpdate.Size = new System.Drawing.Size(364, 85);
            this.groupBoxManualUpdate.TabIndex = 13;
            this.groupBoxManualUpdate.TabStop = false;
            this.groupBoxManualUpdate.Text = "Manual Update";
            // 
            // tabPageSettings
            // 
            this.tabPageSettings.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageSettings.Controls.Add(this.panel1);
            this.tabPageSettings.Location = new System.Drawing.Point(4, 29);
            this.tabPageSettings.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageSettings.Name = "tabPageSettings";
            this.tabPageSettings.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageSettings.Size = new System.Drawing.Size(397, 617);
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
            this.tabControlMain.Location = new System.Drawing.Point(3, 14);
            this.tabControlMain.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(405, 650);
            this.tabControlMain.TabIndex = 14;
            // 
            // statusStripMain
            // 
            this.statusStripMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelColor,
            this.toolStripStatusLabelInfo,
            this.toolStripLabelUploadStatus});
            this.statusStripMain.Location = new System.Drawing.Point(0, 665);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Padding = new System.Windows.Forms.Padding(1, 0, 21, 0);
            this.statusStripMain.Size = new System.Drawing.Size(408, 33);
            this.statusStripMain.TabIndex = 15;
            this.statusStripMain.Text = "statusStrip1";
            // 
            // toolStripStatusLabelColor
            // 
            this.toolStripStatusLabelColor.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusLabelColor.ForeColor = System.Drawing.Color.Red;
            this.toolStripStatusLabelColor.Name = "toolStripStatusLabelColor";
            this.toolStripStatusLabelColor.Size = new System.Drawing.Size(24, 28);
            this.toolStripStatusLabelColor.Text = "●";
            this.toolStripStatusLabelColor.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // toolStripStatusLabelInfo
            // 
            this.toolStripStatusLabelInfo.Name = "toolStripStatusLabelInfo";
            this.toolStripStatusLabelInfo.Size = new System.Drawing.Size(362, 28);
            this.toolStripStatusLabelInfo.Spring = true;
            this.toolStripStatusLabelInfo.Text = "No Device Connected";
            this.toolStripStatusLabelInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripLabelUploadStatus
            // 
            this.toolStripLabelUploadStatus.Name = "toolStripLabelUploadStatus";
            this.toolStripLabelUploadStatus.Size = new System.Drawing.Size(37, 28);
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
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(408, 698);
            this.Controls.Add(this.statusStripMain);
            this.Controls.Add(this.tabControlMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(100, 100);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
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
            this.groupBoxUpdater.ResumeLayout(false);
            this.groupBoxUpdater.PerformLayout();
            this.groupBoxManualUpdate.ResumeLayout(false);
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
        private System.Windows.Forms.Button ButtonReadSettings;
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
        private System.Windows.Forms.Button buttonInitialSetup;
        private System.Windows.Forms.RadioButton radioButtonDigitalFlcs;
        private System.Windows.Forms.RadioButton radioButtonAnalogFlcs;
        private System.Windows.Forms.Button buttonTestStick;
        private System.Windows.Forms.Button buttonCenter;
    }
}
