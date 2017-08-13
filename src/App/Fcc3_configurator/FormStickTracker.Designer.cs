namespace Fcc3_configurator
{
    partial class FormStickTracker
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint1 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0D, 0D);
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormStickTracker));
            this.panelData = new System.Windows.Forms.Panel();
            this.labelPosY = new System.Windows.Forms.Label();
            this.labelPosX = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.chartStick = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.buttonCenter = new System.Windows.Forms.Button();
            this.panelData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartStick)).BeginInit();
            this.SuspendLayout();
            // 
            // panelData
            // 
            this.panelData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelData.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panelData.Controls.Add(this.buttonCenter);
            this.panelData.Controls.Add(this.labelPosY);
            this.panelData.Controls.Add(this.labelPosX);
            this.panelData.Controls.Add(this.label3);
            this.panelData.Controls.Add(this.label2);
            this.panelData.Controls.Add(this.label1);
            this.panelData.Location = new System.Drawing.Point(-2, 655);
            this.panelData.Name = "panelData";
            this.panelData.Size = new System.Drawing.Size(650, 49);
            this.panelData.TabIndex = 2;
            // 
            // labelPosY
            // 
            this.labelPosY.AutoSize = true;
            this.labelPosY.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPosY.ForeColor = System.Drawing.Color.White;
            this.labelPosY.Location = new System.Drawing.Point(350, 14);
            this.labelPosY.Name = "labelPosY";
            this.labelPosY.Size = new System.Drawing.Size(63, 25);
            this.labelPosY.TabIndex = 4;
            this.labelPosY.Text = "-1000";
            this.labelPosY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelPosX
            // 
            this.labelPosX.AutoSize = true;
            this.labelPosX.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPosX.ForeColor = System.Drawing.Color.White;
            this.labelPosX.Location = new System.Drawing.Point(231, 14);
            this.labelPosX.Name = "labelPosX";
            this.labelPosX.Size = new System.Drawing.Size(63, 25);
            this.labelPosX.TabIndex = 3;
            this.labelPosX.Text = "-1000";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(304, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 29);
            this.label3.TabIndex = 2;
            this.label3.Text = "Y:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(194, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 29);
            this.label2.TabIndex = 1;
            this.label2.Text = "X:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(179, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Stick Position:";
            // 
            // chartStick
            // 
            this.chartStick.BackColor = System.Drawing.SystemColors.Info;
            chartArea1.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea1.AxisX.Interval = 200D;
            chartArea1.AxisX.LabelStyle.Enabled = false;
            chartArea1.AxisX.MajorGrid.Interval = 100D;
            chartArea1.AxisX.Maximum = 100D;
            chartArea1.AxisX.Minimum = -100D;
            chartArea1.AxisX.MinorGrid.Enabled = true;
            chartArea1.AxisX.MinorGrid.Interval = 25D;
            chartArea1.AxisX.MinorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea1.AxisX2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea1.AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea1.AxisY.Interval = 200D;
            chartArea1.AxisY.LabelStyle.Enabled = false;
            chartArea1.AxisY.MajorGrid.Interval = 100D;
            chartArea1.AxisY.Maximum = 100D;
            chartArea1.AxisY.Minimum = -100D;
            chartArea1.AxisY.MinorGrid.Enabled = true;
            chartArea1.AxisY.MinorGrid.Interval = 25D;
            chartArea1.AxisY.MinorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea1.AxisY2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea1.Name = "ChartAreaStick";
            this.chartStick.ChartAreas.Add(chartArea1);
            this.chartStick.Location = new System.Drawing.Point(-2, 0);
            this.chartStick.Name = "chartStick";
            series1.ChartArea = "ChartAreaStick";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series1.CustomProperties = "IsXAxisQuantitative=True";
            series1.MarkerSize = 6;
            series1.Name = "SeriesCurrent";
            series1.Points.Add(dataPoint1);
            series2.ChartArea = "ChartAreaStick";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series2.Color = System.Drawing.Color.Black;
            series2.CustomProperties = "EmptyPointValue=Zero";
            series2.IsVisibleInLegend = false;
            series2.Name = "SeriesTrace";
            this.chartStick.Series.Add(series1);
            this.chartStick.Series.Add(series2);
            this.chartStick.Size = new System.Drawing.Size(650, 649);
            this.chartStick.TabIndex = 0;
            this.chartStick.Text = "chart1";
            // 
            // buttonCenter
            // 
            this.buttonCenter.Location = new System.Drawing.Point(451, 9);
            this.buttonCenter.Name = "buttonCenter";
            this.buttonCenter.Size = new System.Drawing.Size(187, 35);
            this.buttonCenter.TabIndex = 5;
            this.buttonCenter.Text = "Center";
            this.buttonCenter.UseVisualStyleBackColor = true;
            this.buttonCenter.Click += new System.EventHandler(this.buttonCenter_Click);
            // 
            // FormStickTracker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(648, 705);
            this.Controls.Add(this.chartStick);
            this.Controls.Add(this.panelData);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormStickTracker";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormStickTracker";
            this.panelData.ResumeLayout(false);
            this.panelData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartStick)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panelData;
        private System.Windows.Forms.Label labelPosY;
        private System.Windows.Forms.Label labelPosX;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartStick;
        private System.Windows.Forms.Button buttonCenter;
    }
}