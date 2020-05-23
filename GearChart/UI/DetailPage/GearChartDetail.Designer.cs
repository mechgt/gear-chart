namespace GearChart.UI.DetailPage
{
    partial class GearChartDetail
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GearChartDetail));
            this.pnlMain = new ZoneFiveSoftware.Common.Visuals.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeListStats = new ZoneFiveSoftware.Common.Visuals.TreeList();
            this.detailPaneChart = new GearChart.Controls.DetailPaneChart();
            this.pnlMain.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.Transparent;
            this.pnlMain.Border = ZoneFiveSoftware.Common.Visuals.ControlBorder.Style.None;
            this.pnlMain.BorderColor = System.Drawing.Color.Gray;
            this.pnlMain.Controls.Add(this.splitContainer1);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.HeadingBackColor = System.Drawing.Color.LightBlue;
            this.pnlMain.HeadingFont = null;
            this.pnlMain.HeadingLeftMargin = 0;
            this.pnlMain.HeadingText = null;
            this.pnlMain.HeadingTextColor = System.Drawing.Color.Black;
            this.pnlMain.HeadingTopMargin = 3;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(374, 415);
            this.pnlMain.TabIndex = 2;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeListStats);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.detailPaneChart);
            this.splitContainer1.Size = new System.Drawing.Size(374, 415);
            this.splitContainer1.SplitterDistance = 120;
            this.splitContainer1.TabIndex = 3;
            // 
            // treeListStats
            // 
            this.treeListStats.BackColor = System.Drawing.Color.Transparent;
            this.treeListStats.Border = ZoneFiveSoftware.Common.Visuals.ControlBorder.Style.SmallRoundShadow;
            this.treeListStats.CheckBoxes = false;
            this.treeListStats.DefaultIndent = 15;
            this.treeListStats.DefaultRowHeight = -1;
            this.treeListStats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeListStats.HeaderRowHeight = 21;
            this.treeListStats.Location = new System.Drawing.Point(0, 0);
            this.treeListStats.MultiSelect = false;
            this.treeListStats.Name = "treeListStats";
            this.treeListStats.NumHeaderRows = ZoneFiveSoftware.Common.Visuals.TreeList.HeaderRows.Auto;
            this.treeListStats.NumLockedColumns = 0;
            this.treeListStats.RowAlternatingColors = true;
            this.treeListStats.RowHotlightColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(10)))), ((int)(((byte)(36)))), ((int)(((byte)(106)))));
            this.treeListStats.RowHotlightColorText = System.Drawing.SystemColors.HighlightText;
            this.treeListStats.RowHotlightMouse = true;
            this.treeListStats.RowSelectedColor = System.Drawing.SystemColors.Highlight;
            this.treeListStats.RowSelectedColorText = System.Drawing.SystemColors.HighlightText;
            this.treeListStats.RowSeparatorLines = true;
            this.treeListStats.ShowLines = false;
            this.treeListStats.ShowPlusMinus = false;
            this.treeListStats.Size = new System.Drawing.Size(374, 120);
            this.treeListStats.TabIndex = 2;
            this.treeListStats.ColumnClicked += new ZoneFiveSoftware.Common.Visuals.TreeList.ColumnEventHandler(this.treeListStats_ColumnClicked);
            // 
            // detailPaneChart
            // 
            this.detailPaneChart.ChartFillColor = System.Drawing.Color.WhiteSmoke;
            this.detailPaneChart.ChartLineColor = System.Drawing.Color.LightSkyBlue;
            this.detailPaneChart.ChartSelectedColor = System.Drawing.Color.AliceBlue;
            this.detailPaneChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.detailPaneChart.Location = new System.Drawing.Point(0, 0);
            this.detailPaneChart.Margin = new System.Windows.Forms.Padding(0);
            this.detailPaneChart.Name = "detailPaneChart";
            this.detailPaneChart.ShownZones = null;
            this.detailPaneChart.Size = new System.Drawing.Size(374, 291);
            this.detailPaneChart.TabIndex = 0;
            this.detailPaneChart.Title = "Detail Pane Chart";
            this.detailPaneChart.XAxisReferential = GearChart.Controls.DetailPaneChart.XAxisValue.Time;
            this.detailPaneChart.YAxisReferential = GearChart.Controls.DetailPaneChart.LineChartTypes.Speed;
            // 
            // GearChartDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlMain);
            this.Name = "GearChartDetail";
            this.Size = new System.Drawing.Size(374, 415);
            this.pnlMain.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ZoneFiveSoftware.Common.Visuals.Panel pnlMain;
        private Controls.DetailPaneChart detailPaneChart;
        private ZoneFiveSoftware.Common.Visuals.TreeList treeListStats;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}
