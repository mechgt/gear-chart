namespace GearChart.UI.GearFilterCriteria
{
    partial class SelectGearsEquipmentDialog
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
            this.Cancel_Button = new ZoneFiveSoftware.Common.Visuals.Button();
            this.OKButton = new ZoneFiveSoftware.Common.Visuals.Button();
            this.EquipmentComboBox = new System.Windows.Forms.ComboBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.SelectEquipmentLabel = new System.Windows.Forms.Label();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Cancel_Button
            // 
            this.Cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel_Button.BackColor = System.Drawing.Color.Transparent;
            this.Cancel_Button.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
            this.Cancel_Button.CenterImage = null;
            this.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel_Button.HyperlinkStyle = false;
            this.Cancel_Button.ImageMargin = 2;
            this.Cancel_Button.LeftImage = null;
            this.Cancel_Button.Location = new System.Drawing.Point(288, 47);
            this.Cancel_Button.Margin = new System.Windows.Forms.Padding(3, 3, 15, 3);
            this.Cancel_Button.Name = "Cancel_Button";
            this.Cancel_Button.PushStyle = true;
            this.Cancel_Button.RightImage = null;
            this.Cancel_Button.Size = new System.Drawing.Size(75, 23);
            this.Cancel_Button.TabIndex = 7;
            this.Cancel_Button.Text = "Cancel";
            this.Cancel_Button.TextAlign = System.Drawing.StringAlignment.Center;
            this.Cancel_Button.TextLeftMargin = 2;
            this.Cancel_Button.TextRightMargin = 2;
            this.Cancel_Button.Click += new System.EventHandler(this.Cancel_Button_Click);
            // 
            // OKButton
            // 
            this.OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKButton.BackColor = System.Drawing.Color.Transparent;
            this.OKButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
            this.OKButton.CenterImage = null;
            this.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKButton.HyperlinkStyle = false;
            this.OKButton.ImageMargin = 2;
            this.OKButton.LeftImage = null;
            this.OKButton.Location = new System.Drawing.Point(204, 47);
            this.OKButton.Margin = new System.Windows.Forms.Padding(3, 3, 6, 3);
            this.OKButton.Name = "OKButton";
            this.OKButton.PushStyle = true;
            this.OKButton.RightImage = null;
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 6;
            this.OKButton.Text = "OK";
            this.OKButton.TextAlign = System.Drawing.StringAlignment.Center;
            this.OKButton.TextLeftMargin = 2;
            this.OKButton.TextRightMargin = 2;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // EquipmentComboBox
            // 
            this.EquipmentComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.EquipmentComboBox.FormattingEnabled = true;
            this.EquipmentComboBox.Location = new System.Drawing.Point(74, 3);
            this.EquipmentComboBox.Name = "EquipmentComboBox";
            this.EquipmentComboBox.Size = new System.Drawing.Size(164, 21);
            this.EquipmentComboBox.TabIndex = 8;
            this.EquipmentComboBox.SelectedIndexChanged += new System.EventHandler(this.EquipmentComboBox_SelectedIndexChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Controls.Add(this.SelectEquipmentLabel);
            this.flowLayoutPanel1.Controls.Add(this.EquipmentComboBox);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(351, 29);
            this.flowLayoutPanel1.TabIndex = 9;
            // 
            // SelectEquipmentLabel
            // 
            this.SelectEquipmentLabel.AutoSize = true;
            this.SelectEquipmentLabel.Location = new System.Drawing.Point(3, 6);
            this.SelectEquipmentLabel.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.SelectEquipmentLabel.Name = "SelectEquipmentLabel";
            this.SelectEquipmentLabel.Size = new System.Drawing.Size(65, 13);
            this.SelectEquipmentLabel.TabIndex = 9;
            this.SelectEquipmentLabel.Text = "Gear setup :";
            // 
            // SelectGearsEquipmentDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(369, 73);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.Cancel_Button);
            this.Controls.Add(this.OKButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectGearsEquipmentDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select gears setup";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ZoneFiveSoftware.Common.Visuals.Button Cancel_Button;
        private ZoneFiveSoftware.Common.Visuals.Button OKButton;
        private System.Windows.Forms.ComboBox EquipmentComboBox;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label SelectEquipmentLabel;
    }
}