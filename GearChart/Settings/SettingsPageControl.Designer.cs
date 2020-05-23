// <copyright file="SettingsPageControl.Designer.cs" company="N/A">
// Copyright (c) 2008 All Right Reserved
// </copyright>
// <author>mechgt</author>
// <email>mechgt@gmail.com</email>
// <date>2008-12-23</date>
namespace GearChart.Settings
{
    partial class SettingsPageControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.grpGearSettings = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.treeSmall = new ZoneFiveSoftware.Common.Visuals.TreeList();
            this.lblM = new System.Windows.Forms.Label();
            this.SmallRemoveButton = new ZoneFiveSoftware.Common.Visuals.Button();
            this.SmallAddButton = new ZoneFiveSoftware.Common.Visuals.Button();
            this.lblWheelCirc = new System.Windows.Forms.Label();
            this.txtSmall = new ZoneFiveSoftware.Common.Visuals.TextBox();
            this.txtCircum = new ZoneFiveSoftware.Common.Visuals.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.treeBig = new ZoneFiveSoftware.Common.Visuals.TreeList();
            this.BigAddButton = new ZoneFiveSoftware.Common.Visuals.Button();
            this.BigRemoveButton = new ZoneFiveSoftware.Common.Visuals.Button();
            this.txtBig = new ZoneFiveSoftware.Common.Visuals.TextBox();
            this.btnEquipOpen = new ZoneFiveSoftware.Common.Visuals.Button();
            this.txtEquip = new ZoneFiveSoftware.Common.Visuals.TextBox();
            this.toolTipSmooth = new System.Windows.Forms.ToolTip(this.components);
            this.toolTipHelp = new System.Windows.Forms.ToolTip(this.components);
            this.pnlTopLeft = new System.Windows.Forms.Panel();
            this.grpGearSettings.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlTopLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpGearSettings
            // 
            this.grpGearSettings.Controls.Add(this.panel2);
            this.grpGearSettings.Controls.Add(this.panel1);
            this.grpGearSettings.Controls.Add(this.btnEquipOpen);
            this.grpGearSettings.Controls.Add(this.txtEquip);
            this.grpGearSettings.Location = new System.Drawing.Point(0, 0);
            this.grpGearSettings.MinimumSize = new System.Drawing.Size(249, 68);
            this.grpGearSettings.Name = "grpGearSettings";
            this.grpGearSettings.Size = new System.Drawing.Size(339, 388);
            this.grpGearSettings.TabIndex = 3;
            this.grpGearSettings.TabStop = false;
            this.grpGearSettings.Text = "Gear Settings";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.panel2.Controls.Add(this.treeSmall);
            this.panel2.Controls.Add(this.lblM);
            this.panel2.Controls.Add(this.SmallRemoveButton);
            this.panel2.Controls.Add(this.SmallAddButton);
            this.panel2.Controls.Add(this.lblWheelCirc);
            this.panel2.Controls.Add(this.txtSmall);
            this.panel2.Controls.Add(this.txtCircum);
            this.panel2.Location = new System.Drawing.Point(6, 146);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(327, 236);
            this.panel2.TabIndex = 4;
            // 
            // treeSmall
            // 
            this.treeSmall.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.treeSmall.BackColor = System.Drawing.Color.Transparent;
            this.treeSmall.Border = ZoneFiveSoftware.Common.Visuals.ControlBorder.Style.SmallRoundShadow;
            this.treeSmall.CheckBoxes = false;
            this.treeSmall.DefaultIndent = 15;
            this.treeSmall.DefaultRowHeight = -1;
            this.treeSmall.HeaderRowHeight = 21;
            this.treeSmall.Location = new System.Drawing.Point(3, 3);
            this.treeSmall.MultiSelect = false;
            this.treeSmall.Name = "treeSmall";
            this.treeSmall.NumHeaderRows = ZoneFiveSoftware.Common.Visuals.TreeList.HeaderRows.One;
            this.treeSmall.NumLockedColumns = 0;
            this.treeSmall.RowAlternatingColors = true;
            this.treeSmall.RowHotlightColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(10)))), ((int)(((byte)(36)))), ((int)(((byte)(106)))));
            this.treeSmall.RowHotlightColorText = System.Drawing.SystemColors.HighlightText;
            this.treeSmall.RowHotlightMouse = true;
            this.treeSmall.RowSelectedColor = System.Drawing.SystemColors.Highlight;
            this.treeSmall.RowSelectedColorText = System.Drawing.SystemColors.HighlightText;
            this.treeSmall.RowSeparatorLines = true;
            this.treeSmall.ShowLines = false;
            this.treeSmall.ShowPlusMinus = false;
            this.treeSmall.Size = new System.Drawing.Size(146, 229);
            this.treeSmall.TabIndex = 20;
            // 
            // lblM
            // 
            this.lblM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblM.AutoSize = true;
            this.lblM.Location = new System.Drawing.Point(235, 217);
            this.lblM.Name = "lblM";
            this.lblM.Size = new System.Drawing.Size(23, 13);
            this.lblM.TabIndex = 23;
            this.lblM.Text = "mm";
            // 
            // SmallRemoveButton
            // 
            this.SmallRemoveButton.BackColor = System.Drawing.Color.Transparent;
            this.SmallRemoveButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
            this.SmallRemoveButton.CenterImage = null;
            this.SmallRemoveButton.DialogResult = System.Windows.Forms.DialogResult.None;
            this.SmallRemoveButton.HyperlinkStyle = false;
            this.SmallRemoveButton.ImageMargin = 2;
            this.SmallRemoveButton.LeftImage = null;
            this.SmallRemoveButton.Location = new System.Drawing.Point(152, 32);
            this.SmallRemoveButton.Name = "SmallRemoveButton";
            this.SmallRemoveButton.PushStyle = true;
            this.SmallRemoveButton.RightImage = null;
            this.SmallRemoveButton.Size = new System.Drawing.Size(75, 23);
            this.SmallRemoveButton.TabIndex = 21;
            this.SmallRemoveButton.Text = "Remove";
            this.SmallRemoveButton.TextAlign = System.Drawing.StringAlignment.Center;
            this.SmallRemoveButton.TextLeftMargin = 2;
            this.SmallRemoveButton.TextRightMargin = 2;
            this.SmallRemoveButton.Click += new System.EventHandler(this.SmallRemoveButton_Click);
            // 
            // SmallAddButton
            // 
            this.SmallAddButton.BackColor = System.Drawing.Color.Transparent;
            this.SmallAddButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
            this.SmallAddButton.CenterImage = null;
            this.SmallAddButton.DialogResult = System.Windows.Forms.DialogResult.None;
            this.SmallAddButton.HyperlinkStyle = false;
            this.SmallAddButton.ImageMargin = 2;
            this.SmallAddButton.LeftImage = null;
            this.SmallAddButton.Location = new System.Drawing.Point(152, 3);
            this.SmallAddButton.Name = "SmallAddButton";
            this.SmallAddButton.PushStyle = true;
            this.SmallAddButton.RightImage = null;
            this.SmallAddButton.Size = new System.Drawing.Size(75, 23);
            this.SmallAddButton.TabIndex = 21;
            this.SmallAddButton.Text = "Add";
            this.SmallAddButton.TextAlign = System.Drawing.StringAlignment.Center;
            this.SmallAddButton.TextLeftMargin = 2;
            this.SmallAddButton.TextRightMargin = 2;
            this.SmallAddButton.Click += new System.EventHandler(this.SmallAddButton_Click);
            // 
            // lblWheelCirc
            // 
            this.lblWheelCirc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblWheelCirc.AutoSize = true;
            this.lblWheelCirc.Location = new System.Drawing.Point(149, 195);
            this.lblWheelCirc.Name = "lblWheelCirc";
            this.lblWheelCirc.Size = new System.Drawing.Size(109, 13);
            this.lblWheelCirc.TabIndex = 23;
            this.lblWheelCirc.Text = "Wheel Circumference";
            // 
            // txtSmall
            // 
            this.txtSmall.AcceptsReturn = false;
            this.txtSmall.AcceptsTab = false;
            this.txtSmall.BackColor = System.Drawing.Color.White;
            this.txtSmall.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(114)))), ((int)(((byte)(108)))));
            this.txtSmall.ButtonImage = null;
            this.txtSmall.Location = new System.Drawing.Point(233, 7);
            this.txtSmall.MaxLength = 32767;
            this.txtSmall.Multiline = false;
            this.txtSmall.Name = "txtSmall";
            this.txtSmall.ReadOnly = false;
            this.txtSmall.ReadOnlyColor = System.Drawing.SystemColors.Control;
            this.txtSmall.ReadOnlyTextColor = System.Drawing.SystemColors.ControlLight;
            this.txtSmall.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtSmall.Size = new System.Drawing.Size(72, 19);
            this.txtSmall.TabIndex = 22;
            this.txtSmall.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtSmall.ButtonClick += new System.EventHandler(this.SmallAddButton_Click);
            this.txtSmall.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.digitValidator);
            // 
            // txtCircum
            // 
            this.txtCircum.AcceptsReturn = false;
            this.txtCircum.AcceptsTab = false;
            this.txtCircum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtCircum.BackColor = System.Drawing.Color.White;
            this.txtCircum.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(114)))), ((int)(((byte)(108)))));
            this.txtCircum.ButtonImage = null;
            this.txtCircum.Location = new System.Drawing.Point(152, 211);
            this.txtCircum.MaxLength = 32767;
            this.txtCircum.Multiline = false;
            this.txtCircum.Name = "txtCircum";
            this.txtCircum.ReadOnly = false;
            this.txtCircum.ReadOnlyColor = System.Drawing.SystemColors.Control;
            this.txtCircum.ReadOnlyTextColor = System.Drawing.SystemColors.ControlLight;
            this.txtCircum.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtCircum.Size = new System.Drawing.Size(81, 19);
            this.txtCircum.TabIndex = 22;
            this.txtCircum.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtCircum.Leave += new System.EventHandler(this.txtCircum_Leave);
            this.txtCircum.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.digitValidator);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.treeBig);
            this.panel1.Controls.Add(this.BigAddButton);
            this.panel1.Controls.Add(this.BigRemoveButton);
            this.panel1.Controls.Add(this.txtBig);
            this.panel1.Location = new System.Drawing.Point(6, 48);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(327, 95);
            this.panel1.TabIndex = 4;
            // 
            // treeBig
            // 
            this.treeBig.BackColor = System.Drawing.Color.Transparent;
            this.treeBig.Border = ZoneFiveSoftware.Common.Visuals.ControlBorder.Style.SmallRoundShadow;
            this.treeBig.CheckBoxes = false;
            this.treeBig.DefaultIndent = 15;
            this.treeBig.DefaultRowHeight = -1;
            this.treeBig.HeaderRowHeight = 21;
            this.treeBig.Location = new System.Drawing.Point(3, 3);
            this.treeBig.MultiSelect = false;
            this.treeBig.Name = "treeBig";
            this.treeBig.NumHeaderRows = ZoneFiveSoftware.Common.Visuals.TreeList.HeaderRows.One;
            this.treeBig.NumLockedColumns = 0;
            this.treeBig.RowAlternatingColors = true;
            this.treeBig.RowHotlightColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(10)))), ((int)(((byte)(36)))), ((int)(((byte)(106)))));
            this.treeBig.RowHotlightColorText = System.Drawing.SystemColors.HighlightText;
            this.treeBig.RowHotlightMouse = true;
            this.treeBig.RowSelectedColor = System.Drawing.SystemColors.Highlight;
            this.treeBig.RowSelectedColorText = System.Drawing.SystemColors.HighlightText;
            this.treeBig.RowSeparatorLines = true;
            this.treeBig.ShowLines = false;
            this.treeBig.ShowPlusMinus = false;
            this.treeBig.Size = new System.Drawing.Size(146, 89);
            this.treeBig.TabIndex = 20;
            // 
            // BigAddButton
            // 
            this.BigAddButton.BackColor = System.Drawing.Color.Transparent;
            this.BigAddButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
            this.BigAddButton.CenterImage = null;
            this.BigAddButton.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BigAddButton.HyperlinkStyle = false;
            this.BigAddButton.ImageMargin = 2;
            this.BigAddButton.LeftImage = null;
            this.BigAddButton.Location = new System.Drawing.Point(152, 3);
            this.BigAddButton.Name = "BigAddButton";
            this.BigAddButton.PushStyle = true;
            this.BigAddButton.RightImage = null;
            this.BigAddButton.Size = new System.Drawing.Size(75, 23);
            this.BigAddButton.TabIndex = 21;
            this.BigAddButton.Text = "Add";
            this.BigAddButton.TextAlign = System.Drawing.StringAlignment.Center;
            this.BigAddButton.TextLeftMargin = 2;
            this.BigAddButton.TextRightMargin = 2;
            this.BigAddButton.Click += new System.EventHandler(this.BigAddButton_Click);
            // 
            // BigRemoveButton
            // 
            this.BigRemoveButton.BackColor = System.Drawing.Color.Transparent;
            this.BigRemoveButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
            this.BigRemoveButton.CenterImage = null;
            this.BigRemoveButton.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BigRemoveButton.HyperlinkStyle = false;
            this.BigRemoveButton.ImageMargin = 2;
            this.BigRemoveButton.LeftImage = null;
            this.BigRemoveButton.Location = new System.Drawing.Point(152, 32);
            this.BigRemoveButton.Name = "BigRemoveButton";
            this.BigRemoveButton.PushStyle = true;
            this.BigRemoveButton.RightImage = null;
            this.BigRemoveButton.Size = new System.Drawing.Size(75, 23);
            this.BigRemoveButton.TabIndex = 21;
            this.BigRemoveButton.Text = "Remove";
            this.BigRemoveButton.TextAlign = System.Drawing.StringAlignment.Center;
            this.BigRemoveButton.TextLeftMargin = 2;
            this.BigRemoveButton.TextRightMargin = 2;
            this.BigRemoveButton.Click += new System.EventHandler(this.BigRemoveButton_Click);
            // 
            // txtBig
            // 
            this.txtBig.AcceptsReturn = false;
            this.txtBig.AcceptsTab = false;
            this.txtBig.BackColor = System.Drawing.Color.White;
            this.txtBig.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(114)))), ((int)(((byte)(108)))));
            this.txtBig.ButtonImage = null;
            this.txtBig.Location = new System.Drawing.Point(233, 7);
            this.txtBig.MaxLength = 32767;
            this.txtBig.Multiline = false;
            this.txtBig.Name = "txtBig";
            this.txtBig.ReadOnly = false;
            this.txtBig.ReadOnlyColor = System.Drawing.SystemColors.Control;
            this.txtBig.ReadOnlyTextColor = System.Drawing.SystemColors.ControlLight;
            this.txtBig.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtBig.Size = new System.Drawing.Size(72, 19);
            this.txtBig.TabIndex = 22;
            this.txtBig.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtBig.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.digitValidator);
            // 
            // btnEquipOpen
            // 
            this.btnEquipOpen.BackColor = System.Drawing.Color.Transparent;
            this.btnEquipOpen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnEquipOpen.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
            this.btnEquipOpen.CenterImage = null;
            this.btnEquipOpen.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnEquipOpen.HyperlinkStyle = false;
            this.btnEquipOpen.ImageMargin = 2;
            this.btnEquipOpen.LeftImage = null;
            this.btnEquipOpen.Location = new System.Drawing.Point(315, 20);
            this.btnEquipOpen.Name = "btnEquipOpen";
            this.btnEquipOpen.PushStyle = true;
            this.btnEquipOpen.RightImage = null;
            this.btnEquipOpen.Size = new System.Drawing.Size(17, 17);
            this.btnEquipOpen.TabIndex = 10;
            this.btnEquipOpen.TextAlign = System.Drawing.StringAlignment.Center;
            this.btnEquipOpen.TextLeftMargin = 2;
            this.btnEquipOpen.TextRightMargin = 2;
            this.btnEquipOpen.Click += new System.EventHandler(this.btnEquipOpen_Click);
            // 
            // txtEquip
            // 
            this.txtEquip.AcceptsReturn = false;
            this.txtEquip.AcceptsTab = false;
            this.txtEquip.BackColor = System.Drawing.Color.White;
            this.txtEquip.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(114)))), ((int)(((byte)(108)))));
            this.txtEquip.ButtonImage = null;
            this.txtEquip.Location = new System.Drawing.Point(6, 19);
            this.txtEquip.MaxLength = 32767;
            this.txtEquip.Multiline = false;
            this.txtEquip.Name = "txtEquip";
            this.txtEquip.ReadOnly = false;
            this.txtEquip.ReadOnlyColor = System.Drawing.SystemColors.Control;
            this.txtEquip.ReadOnlyTextColor = System.Drawing.SystemColors.ControlLight;
            this.txtEquip.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtEquip.Size = new System.Drawing.Size(327, 19);
            this.txtEquip.TabIndex = 9;
            this.txtEquip.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // toolTipSmooth
            // 
            this.toolTipSmooth.AutoPopDelay = 3000;
            this.toolTipSmooth.InitialDelay = 0;
            this.toolTipSmooth.ReshowDelay = 100;
            // 
            // toolTipHelp
            // 
            this.toolTipHelp.AutoPopDelay = 10000;
            this.toolTipHelp.InitialDelay = 500;
            this.toolTipHelp.ReshowDelay = 100;
            // 
            // pnlTopLeft
            // 
            this.pnlTopLeft.Controls.Add(this.grpGearSettings);
            this.pnlTopLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTopLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlTopLeft.Name = "pnlTopLeft";
            this.pnlTopLeft.Size = new System.Drawing.Size(500, 650);
            this.pnlTopLeft.TabIndex = 5;
            // 
            // SettingsPageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlTopLeft);
            this.Name = "SettingsPageControl";
            this.Size = new System.Drawing.Size(500, 650);
            this.grpGearSettings.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.pnlTopLeft.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpGearSettings;
        private System.Windows.Forms.ToolTip toolTipSmooth;
        private System.Windows.Forms.ToolTip toolTipHelp;
        private System.Windows.Forms.Panel pnlTopLeft;
        private ZoneFiveSoftware.Common.Visuals.TextBox txtSmall;
        private ZoneFiveSoftware.Common.Visuals.TextBox txtBig;
        private ZoneFiveSoftware.Common.Visuals.Button BigRemoveButton;
        private ZoneFiveSoftware.Common.Visuals.Button SmallAddButton;
        private ZoneFiveSoftware.Common.Visuals.Button SmallRemoveButton;
        private ZoneFiveSoftware.Common.Visuals.Button BigAddButton;
        private ZoneFiveSoftware.Common.Visuals.TreeList treeBig;
        private ZoneFiveSoftware.Common.Visuals.TreeList treeSmall;
        private System.Windows.Forms.Label lblWheelCirc;
        private ZoneFiveSoftware.Common.Visuals.TextBox txtCircum;
        private System.Windows.Forms.Label lblM;
        private ZoneFiveSoftware.Common.Visuals.Button btnEquipOpen;
        private ZoneFiveSoftware.Common.Visuals.TextBox txtEquip;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
    }
}
