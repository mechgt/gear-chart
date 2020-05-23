namespace GearChart.Data.ActivityDocumentationComponent
{
    partial class EditForm
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
            this.lblPlotSize = new System.Windows.Forms.Label();
            this.radTime = new System.Windows.Forms.RadioButton();
            this.radDistance = new System.Windows.Forms.RadioButton();
            this.grpChartSize = new System.Windows.Forms.GroupBox();
            this.txtHeight = new ZoneFiveSoftware.Common.Visuals.TextBox();
            this.lblPx2 = new System.Windows.Forms.Label();
            this.lblPx1 = new System.Windows.Forms.Label();
            this.lblWidth = new System.Windows.Forms.Label();
            this.txtWidth = new ZoneFiveSoftware.Common.Visuals.TextBox();
            this.lblHeight = new System.Windows.Forms.Label();
            this.chkShowLaps = new System.Windows.Forms.CheckBox();
            this.btnOK = new ZoneFiveSoftware.Common.Visuals.Button();
            this.btnCancel = new ZoneFiveSoftware.Common.Visuals.Button();
            this.chkRawData = new System.Windows.Forms.CheckBox();
            this.grpChartSize.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblPlotSize
            // 
            this.lblPlotSize.AutoSize = true;
            this.lblPlotSize.Location = new System.Drawing.Point(13, 37);
            this.lblPlotSize.Name = "lblPlotSize";
            this.lblPlotSize.Size = new System.Drawing.Size(52, 13);
            this.lblPlotSize.TabIndex = 1;
            this.lblPlotSize.Text = "Plot over:";
            // 
            // radTime
            // 
            this.radTime.AutoSize = true;
            this.radTime.Location = new System.Drawing.Point(71, 35);
            this.radTime.Name = "radTime";
            this.radTime.Size = new System.Drawing.Size(85, 17);
            this.radTime.TabIndex = 3;
            this.radTime.TabStop = true;
            this.radTime.Text = "Elapsed time";
            this.radTime.UseVisualStyleBackColor = true;
            // 
            // radDistance
            // 
            this.radDistance.AutoSize = true;
            this.radDistance.Location = new System.Drawing.Point(162, 35);
            this.radDistance.Name = "radDistance";
            this.radDistance.Size = new System.Drawing.Size(67, 17);
            this.radDistance.TabIndex = 5;
            this.radDistance.TabStop = true;
            this.radDistance.Text = "Distance";
            this.radDistance.UseVisualStyleBackColor = true;
            // 
            // grpChartSize
            // 
            this.grpChartSize.Controls.Add(this.txtHeight);
            this.grpChartSize.Controls.Add(this.lblPx2);
            this.grpChartSize.Controls.Add(this.lblPx1);
            this.grpChartSize.Controls.Add(this.lblWidth);
            this.grpChartSize.Controls.Add(this.txtWidth);
            this.grpChartSize.Controls.Add(this.lblHeight);
            this.grpChartSize.Location = new System.Drawing.Point(13, 54);
            this.grpChartSize.Name = "grpChartSize";
            this.grpChartSize.Size = new System.Drawing.Size(267, 71);
            this.grpChartSize.TabIndex = 3;
            this.grpChartSize.TabStop = false;
            this.grpChartSize.Text = "Chart Size";
            // 
            // txtHeight
            // 
            this.txtHeight.AcceptsReturn = false;
            this.txtHeight.AcceptsTab = false;
            this.txtHeight.BackColor = System.Drawing.Color.White;
            this.txtHeight.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(114)))), ((int)(((byte)(108)))));
            this.txtHeight.ButtonImage = null;
            this.txtHeight.Location = new System.Drawing.Point(68, 41);
            this.txtHeight.MaxLength = 32767;
            this.txtHeight.Multiline = false;
            this.txtHeight.Name = "txtHeight";
            this.txtHeight.ReadOnly = false;
            this.txtHeight.ReadOnlyColor = System.Drawing.SystemColors.Control;
            this.txtHeight.ReadOnlyTextColor = System.Drawing.SystemColors.ControlLight;
            this.txtHeight.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtHeight.Size = new System.Drawing.Size(50, 19);
            this.txtHeight.TabIndex = 8;
            this.txtHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // lblPx2
            // 
            this.lblPx2.AutoSize = true;
            this.lblPx2.Location = new System.Drawing.Point(125, 47);
            this.lblPx2.Name = "lblPx2";
            this.lblPx2.Size = new System.Drawing.Size(18, 13);
            this.lblPx2.TabIndex = 1;
            this.lblPx2.Text = "px";
            // 
            // lblPx1
            // 
            this.lblPx1.AutoSize = true;
            this.lblPx1.Location = new System.Drawing.Point(124, 22);
            this.lblPx1.Name = "lblPx1";
            this.lblPx1.Size = new System.Drawing.Size(18, 13);
            this.lblPx1.TabIndex = 1;
            this.lblPx1.Text = "px";
            // 
            // lblWidth
            // 
            this.lblWidth.AutoSize = true;
            this.lblWidth.Location = new System.Drawing.Point(6, 22);
            this.lblWidth.Name = "lblWidth";
            this.lblWidth.Size = new System.Drawing.Size(38, 13);
            this.lblWidth.TabIndex = 1;
            this.lblWidth.Text = "Width:";
            // 
            // txtWidth
            // 
            this.txtWidth.AcceptsReturn = false;
            this.txtWidth.AcceptsTab = false;
            this.txtWidth.BackColor = System.Drawing.Color.White;
            this.txtWidth.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(114)))), ((int)(((byte)(108)))));
            this.txtWidth.ButtonImage = null;
            this.txtWidth.Location = new System.Drawing.Point(68, 16);
            this.txtWidth.MaxLength = 32767;
            this.txtWidth.Multiline = false;
            this.txtWidth.Name = "txtWidth";
            this.txtWidth.ReadOnly = false;
            this.txtWidth.ReadOnlyColor = System.Drawing.SystemColors.Control;
            this.txtWidth.ReadOnlyTextColor = System.Drawing.SystemColors.ControlLight;
            this.txtWidth.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtWidth.Size = new System.Drawing.Size(50, 19);
            this.txtWidth.TabIndex = 7;
            this.txtWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // lblHeight
            // 
            this.lblHeight.AutoSize = true;
            this.lblHeight.Location = new System.Drawing.Point(6, 47);
            this.lblHeight.Name = "lblHeight";
            this.lblHeight.Size = new System.Drawing.Size(41, 13);
            this.lblHeight.TabIndex = 1;
            this.lblHeight.Text = "Height:";
            // 
            // chkShowLaps
            // 
            this.chkShowLaps.AutoSize = true;
            this.chkShowLaps.Enabled = false;
            this.chkShowLaps.Location = new System.Drawing.Point(12, 131);
            this.chkShowLaps.Name = "chkShowLaps";
            this.chkShowLaps.Size = new System.Drawing.Size(79, 17);
            this.chkShowLaps.TabIndex = 9;
            this.chkShowLaps.Text = "Show Laps";
            this.chkShowLaps.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.Transparent;
            this.btnOK.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
            this.btnOK.CenterImage = null;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnOK.HyperlinkStyle = false;
            this.btnOK.ImageMargin = 2;
            this.btnOK.LeftImage = null;
            this.btnOK.Location = new System.Drawing.Point(39, 153);
            this.btnOK.Name = "btnOK";
            this.btnOK.PushStyle = true;
            this.btnOK.RightImage = null;
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 12;
            this.btnOK.Text = "OK";
            this.btnOK.TextAlign = System.Drawing.StringAlignment.Center;
            this.btnOK.TextLeftMargin = 2;
            this.btnOK.TextRightMargin = 2;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
            this.btnCancel.CenterImage = null;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.HyperlinkStyle = false;
            this.btnCancel.ImageMargin = 2;
            this.btnCancel.LeftImage = null;
            this.btnCancel.Location = new System.Drawing.Point(179, 153);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.PushStyle = true;
            this.btnCancel.RightImage = null;
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextAlign = System.Drawing.StringAlignment.Center;
            this.btnCancel.TextLeftMargin = 2;
            this.btnCancel.TextRightMargin = 2;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // chkRawData
            // 
            this.chkRawData.AutoSize = true;
            this.chkRawData.Checked = true;
            this.chkRawData.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRawData.Location = new System.Drawing.Point(12, 12);
            this.chkRawData.Name = "chkRawData";
            this.chkRawData.Size = new System.Drawing.Size(104, 17);
            this.chkRawData.TabIndex = 10;
            this.chkRawData.Text = "Show Raw Data";
            this.chkRawData.UseVisualStyleBackColor = true;
            // 
            // EditForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(292, 188);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.grpChartSize);
            this.Controls.Add(this.radDistance);
            this.Controls.Add(this.radTime);
            this.Controls.Add(this.lblPlotSize);
            this.Controls.Add(this.chkRawData);
            this.Controls.Add(this.chkShowLaps);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(300, 215);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(300, 215);
            this.Name = "EditForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Parameters of Gear Chart";
            this.grpChartSize.ResumeLayout(false);
            this.grpChartSize.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPlotSize;
        private System.Windows.Forms.RadioButton radTime;
        private System.Windows.Forms.RadioButton radDistance;
        private System.Windows.Forms.GroupBox grpChartSize;
        private System.Windows.Forms.CheckBox chkShowLaps;
        private ZoneFiveSoftware.Common.Visuals.Button btnOK;
        private ZoneFiveSoftware.Common.Visuals.Button btnCancel;
        private ZoneFiveSoftware.Common.Visuals.TextBox txtHeight;
        private System.Windows.Forms.Label lblPx2;
        private System.Windows.Forms.Label lblPx1;
        private System.Windows.Forms.Label lblWidth;
        private ZoneFiveSoftware.Common.Visuals.TextBox txtWidth;
        private System.Windows.Forms.Label lblHeight;
        private System.Windows.Forms.CheckBox chkRawData;
    }
}