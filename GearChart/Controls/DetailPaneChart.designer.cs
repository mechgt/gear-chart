namespace GearChart.Controls
{
    partial class DetailPaneChart
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
            this.ButtonPanel = new ZoneFiveSoftware.Common.Visuals.Panel();
            this.ZoomInButton = new ZoneFiveSoftware.Common.Visuals.Button();
            this.ZoomOutButton = new ZoneFiveSoftware.Common.Visuals.Button();
            this.ZoomChartButton = new ZoneFiveSoftware.Common.Visuals.Button();
            this.ExtraChartsButton = new ZoneFiveSoftware.Common.Visuals.Button();
            this.SaveImageButton = new ZoneFiveSoftware.Common.Visuals.Button();
            this.ChartBanner = new ZoneFiveSoftware.Common.Visuals.ActionBanner();
            this.detailMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.speedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.paceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.heartRateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cadenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.elevationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gradeStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.powerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.distanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelMain = new ZoneFiveSoftware.Common.Visuals.Panel();
            this.MainChart = new ZoneFiveSoftware.Common.Visuals.Chart.ChartBase();
            this.ExportButton = new ZoneFiveSoftware.Common.Visuals.Button();
            this.ButtonPanel.SuspendLayout();
            this.detailMenu.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonPanel
            // 
            this.ButtonPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonPanel.BackColor = System.Drawing.Color.Transparent;
            this.ButtonPanel.Border = ZoneFiveSoftware.Common.Visuals.ControlBorder.Style.Square;
            this.ButtonPanel.BorderColor = System.Drawing.Color.Gray;
            this.ButtonPanel.Controls.Add(this.ZoomInButton);
            this.ButtonPanel.Controls.Add(this.ZoomOutButton);
            this.ButtonPanel.Controls.Add(this.ZoomChartButton);
            this.ButtonPanel.Controls.Add(this.ExportButton);
            this.ButtonPanel.Controls.Add(this.ExtraChartsButton);
            this.ButtonPanel.Controls.Add(this.SaveImageButton);
            this.ButtonPanel.HeadingBackColor = System.Drawing.Color.LightBlue;
            this.ButtonPanel.HeadingFont = null;
            this.ButtonPanel.HeadingLeftMargin = 0;
            this.ButtonPanel.HeadingText = null;
            this.ButtonPanel.HeadingTextColor = System.Drawing.Color.Black;
            this.ButtonPanel.HeadingTopMargin = 0;
            this.ButtonPanel.Location = new System.Drawing.Point(0, 23);
            this.ButtonPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonPanel.Name = "ButtonPanel";
            this.ButtonPanel.Size = new System.Drawing.Size(400, 24);
            this.ButtonPanel.TabIndex = 1;
            // 
            // ZoomInButton
            // 
            this.ZoomInButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ZoomInButton.BackColor = System.Drawing.Color.Transparent;
            this.ZoomInButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
            this.ZoomInButton.CenterImage = null;
            this.ZoomInButton.DialogResult = System.Windows.Forms.DialogResult.None;
            this.ZoomInButton.HyperlinkStyle = false;
            this.ZoomInButton.ImageMargin = 2;
            this.ZoomInButton.LeftImage = null;
            this.ZoomInButton.Location = new System.Drawing.Point(374, 0);
            this.ZoomInButton.Margin = new System.Windows.Forms.Padding(0);
            this.ZoomInButton.Name = "ZoomInButton";
            this.ZoomInButton.PushStyle = true;
            this.ZoomInButton.RightImage = null;
            this.ZoomInButton.Size = new System.Drawing.Size(24, 24);
            this.ZoomInButton.TabIndex = 0;
            this.ZoomInButton.TextAlign = System.Drawing.StringAlignment.Center;
            this.ZoomInButton.TextLeftMargin = 2;
            this.ZoomInButton.TextRightMargin = 2;
            this.ZoomInButton.Click += new System.EventHandler(this.ZoomInButton_Click);
            // 
            // ZoomOutButton
            // 
            this.ZoomOutButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ZoomOutButton.BackColor = System.Drawing.Color.Transparent;
            this.ZoomOutButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
            this.ZoomOutButton.CenterImage = null;
            this.ZoomOutButton.DialogResult = System.Windows.Forms.DialogResult.None;
            this.ZoomOutButton.HyperlinkStyle = false;
            this.ZoomOutButton.ImageMargin = 2;
            this.ZoomOutButton.LeftImage = null;
            this.ZoomOutButton.Location = new System.Drawing.Point(350, 0);
            this.ZoomOutButton.Margin = new System.Windows.Forms.Padding(0);
            this.ZoomOutButton.Name = "ZoomOutButton";
            this.ZoomOutButton.PushStyle = true;
            this.ZoomOutButton.RightImage = null;
            this.ZoomOutButton.Size = new System.Drawing.Size(24, 24);
            this.ZoomOutButton.TabIndex = 0;
            this.ZoomOutButton.TextAlign = System.Drawing.StringAlignment.Center;
            this.ZoomOutButton.TextLeftMargin = 2;
            this.ZoomOutButton.TextRightMargin = 2;
            this.ZoomOutButton.Click += new System.EventHandler(this.ZoomOutButton_Click);
            // 
            // ZoomChartButton
            // 
            this.ZoomChartButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ZoomChartButton.BackColor = System.Drawing.Color.Transparent;
            this.ZoomChartButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
            this.ZoomChartButton.CenterImage = null;
            this.ZoomChartButton.DialogResult = System.Windows.Forms.DialogResult.None;
            this.ZoomChartButton.HyperlinkStyle = false;
            this.ZoomChartButton.ImageMargin = 2;
            this.ZoomChartButton.LeftImage = null;
            this.ZoomChartButton.Location = new System.Drawing.Point(326, 0);
            this.ZoomChartButton.Margin = new System.Windows.Forms.Padding(0);
            this.ZoomChartButton.Name = "ZoomChartButton";
            this.ZoomChartButton.PushStyle = true;
            this.ZoomChartButton.RightImage = null;
            this.ZoomChartButton.Size = new System.Drawing.Size(24, 24);
            this.ZoomChartButton.TabIndex = 0;
            this.ZoomChartButton.TextAlign = System.Drawing.StringAlignment.Center;
            this.ZoomChartButton.TextLeftMargin = 2;
            this.ZoomChartButton.TextRightMargin = 2;
            this.ZoomChartButton.Click += new System.EventHandler(this.ZoomFitButton_Click);
            // 
            // ExtraChartsButton
            // 
            this.ExtraChartsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ExtraChartsButton.BackColor = System.Drawing.Color.Transparent;
            this.ExtraChartsButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
            this.ExtraChartsButton.CenterImage = null;
            this.ExtraChartsButton.DialogResult = System.Windows.Forms.DialogResult.None;
            this.ExtraChartsButton.HyperlinkStyle = false;
            this.ExtraChartsButton.ImageMargin = 2;
            this.ExtraChartsButton.LeftImage = null;
            this.ExtraChartsButton.Location = new System.Drawing.Point(278, 0);
            this.ExtraChartsButton.Margin = new System.Windows.Forms.Padding(0);
            this.ExtraChartsButton.Name = "ExtraChartsButton";
            this.ExtraChartsButton.PushStyle = true;
            this.ExtraChartsButton.RightImage = null;
            this.ExtraChartsButton.Size = new System.Drawing.Size(24, 24);
            this.ExtraChartsButton.TabIndex = 0;
            this.ExtraChartsButton.TextAlign = System.Drawing.StringAlignment.Center;
            this.ExtraChartsButton.TextLeftMargin = 2;
            this.ExtraChartsButton.TextRightMargin = 2;
            this.ExtraChartsButton.Click += new System.EventHandler(this.ExtraChartsButton_Click);
            // 
            // SaveImageButton
            // 
            this.SaveImageButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveImageButton.BackColor = System.Drawing.Color.Transparent;
            this.SaveImageButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
            this.SaveImageButton.CenterImage = null;
            this.SaveImageButton.DialogResult = System.Windows.Forms.DialogResult.None;
            this.SaveImageButton.HyperlinkStyle = false;
            this.SaveImageButton.ImageMargin = 2;
            this.SaveImageButton.LeftImage = null;
            this.SaveImageButton.Location = new System.Drawing.Point(302, 0);
            this.SaveImageButton.Margin = new System.Windows.Forms.Padding(0);
            this.SaveImageButton.Name = "SaveImageButton";
            this.SaveImageButton.PushStyle = true;
            this.SaveImageButton.RightImage = null;
            this.SaveImageButton.Size = new System.Drawing.Size(24, 24);
            this.SaveImageButton.TabIndex = 0;
            this.SaveImageButton.TextAlign = System.Drawing.StringAlignment.Center;
            this.SaveImageButton.TextLeftMargin = 2;
            this.SaveImageButton.TextRightMargin = 2;
            this.SaveImageButton.Click += new System.EventHandler(this.SaveImageButton_Click);
            // 
            // ChartBanner
            // 
            this.ChartBanner.BackColor = System.Drawing.Color.Transparent;
            this.ChartBanner.ContextMenuStrip = this.detailMenu;
            this.ChartBanner.Dock = System.Windows.Forms.DockStyle.Top;
            this.ChartBanner.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ChartBanner.HasMenuButton = true;
            this.ChartBanner.Location = new System.Drawing.Point(0, 0);
            this.ChartBanner.Name = "ChartBanner";
            this.ChartBanner.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ChartBanner.Size = new System.Drawing.Size(400, 24);
            this.ChartBanner.Style = ZoneFiveSoftware.Common.Visuals.ActionBanner.BannerStyle.Header2;
            this.ChartBanner.TabIndex = 5;
            this.ChartBanner.Text = "Detail Pane Chart";
            this.ChartBanner.UseStyleFont = true;
            this.ChartBanner.MenuClicked += new System.EventHandler(this.ChartBanner_MenuClicked);
            // 
            // detailMenu
            // 
            this.detailMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.speedToolStripMenuItem,
            this.paceToolStripMenuItem,
            this.heartRateToolStripMenuItem,
            this.cadenceToolStripMenuItem,
            this.elevationToolStripMenuItem,
            this.gradeStripMenuItem,
            this.powerToolStripMenuItem,
            this.toolStripSeparator1,
            this.distanceToolStripMenuItem,
            this.timeToolStripMenuItem});
            this.detailMenu.Name = "detailMenu";
            this.detailMenu.Size = new System.Drawing.Size(130, 208);
            // 
            // speedToolStripMenuItem
            // 
            this.speedToolStripMenuItem.Name = "speedToolStripMenuItem";
            this.speedToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.speedToolStripMenuItem.Text = "Speed";
            // 
            // paceToolStripMenuItem
            // 
            this.paceToolStripMenuItem.Name = "paceToolStripMenuItem";
            this.paceToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.paceToolStripMenuItem.Text = "Pace";
            // 
            // heartRateToolStripMenuItem
            // 
            this.heartRateToolStripMenuItem.Name = "heartRateToolStripMenuItem";
            this.heartRateToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.heartRateToolStripMenuItem.Text = "Heart Rate";
            // 
            // cadenceToolStripMenuItem
            // 
            this.cadenceToolStripMenuItem.Name = "cadenceToolStripMenuItem";
            this.cadenceToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.cadenceToolStripMenuItem.Text = "Cadence";
            // 
            // elevationToolStripMenuItem
            // 
            this.elevationToolStripMenuItem.Name = "elevationToolStripMenuItem";
            this.elevationToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.elevationToolStripMenuItem.Text = "Elevation";
            // 
            // gradeStripMenuItem
            // 
            this.gradeStripMenuItem.Name = "gradeStripMenuItem";
            this.gradeStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.gradeStripMenuItem.Text = "Grade";
            // 
            // powerToolStripMenuItem
            // 
            this.powerToolStripMenuItem.Name = "powerToolStripMenuItem";
            this.powerToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.powerToolStripMenuItem.Text = "Power";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(126, 6);
            // 
            // distanceToolStripMenuItem
            // 
            this.distanceToolStripMenuItem.Name = "distanceToolStripMenuItem";
            this.distanceToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.distanceToolStripMenuItem.Text = "Distance";
            // 
            // timeToolStripMenuItem
            // 
            this.timeToolStripMenuItem.Name = "timeToolStripMenuItem";
            this.timeToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.timeToolStripMenuItem.Text = "Time";
            // 
            // panelMain
            // 
            this.panelMain.BackColor = System.Drawing.Color.Transparent;
            this.panelMain.Border = ZoneFiveSoftware.Common.Visuals.ControlBorder.Style.Round;
            this.panelMain.BorderColor = System.Drawing.Color.Gray;
            this.panelMain.Controls.Add(this.MainChart);
            this.panelMain.Controls.Add(this.ButtonPanel);
            this.panelMain.Controls.Add(this.ChartBanner);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.HeadingBackColor = System.Drawing.Color.LightBlue;
            this.panelMain.HeadingFont = null;
            this.panelMain.HeadingLeftMargin = 0;
            this.panelMain.HeadingText = null;
            this.panelMain.HeadingTextColor = System.Drawing.Color.Black;
            this.panelMain.HeadingTopMargin = 0;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Margin = new System.Windows.Forms.Padding(0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(400, 400);
            this.panelMain.TabIndex = 2;
            // 
            // MainChart
            // 
            this.MainChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.MainChart.BackColor = System.Drawing.Color.Transparent;
            this.MainChart.Border = ZoneFiveSoftware.Common.Visuals.ControlBorder.Style.None;
            this.MainChart.Location = new System.Drawing.Point(1, 47);
            this.MainChart.Name = "MainChart";
            this.MainChart.Padding = new System.Windows.Forms.Padding(5);
            this.MainChart.Size = new System.Drawing.Size(398, 350);
            this.MainChart.TabIndex = 6;
            // 
            // ExportButton
            // 
            this.ExportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ExportButton.BackColor = System.Drawing.Color.Transparent;
            this.ExportButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
            this.ExportButton.CenterImage = null;
            this.ExportButton.DialogResult = System.Windows.Forms.DialogResult.None;
            this.ExportButton.HyperlinkStyle = false;
            this.ExportButton.ImageMargin = 2;
            this.ExportButton.LeftImage = null;
            this.ExportButton.Location = new System.Drawing.Point(254, 0);
            this.ExportButton.Margin = new System.Windows.Forms.Padding(0);
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.PushStyle = true;
            this.ExportButton.RightImage = null;
            this.ExportButton.Size = new System.Drawing.Size(24, 24);
            this.ExportButton.TabIndex = 0;
            this.ExportButton.TextAlign = System.Drawing.StringAlignment.Center;
            this.ExportButton.TextLeftMargin = 2;
            this.ExportButton.TextRightMargin = 2;
            this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // DetailPaneChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelMain);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "DetailPaneChart";
            this.Size = new System.Drawing.Size(400, 400);
            this.ButtonPanel.ResumeLayout(false);
            this.detailMenu.ResumeLayout(false);
            this.panelMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ZoneFiveSoftware.Common.Visuals.Panel ButtonPanel;
        private ZoneFiveSoftware.Common.Visuals.Button ZoomOutButton;
        private ZoneFiveSoftware.Common.Visuals.Button ZoomInButton;
        private ZoneFiveSoftware.Common.Visuals.Button SaveImageButton;
        private ZoneFiveSoftware.Common.Visuals.ActionBanner ChartBanner;
        private System.Windows.Forms.ContextMenuStrip detailMenu;
        private System.Windows.Forms.ToolStripMenuItem timeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem speedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem paceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem heartRateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cadenceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem elevationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gradeStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem powerToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem distanceToolStripMenuItem;
        private ZoneFiveSoftware.Common.Visuals.Button ZoomChartButton;
        private ZoneFiveSoftware.Common.Visuals.Panel panelMain;
        private ZoneFiveSoftware.Common.Visuals.Button ExtraChartsButton;
        private ZoneFiveSoftware.Common.Visuals.Chart.ChartBase MainChart;
        private ZoneFiveSoftware.Common.Visuals.Button ExportButton;
    }
}
