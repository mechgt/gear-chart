using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ZoneFiveSoftware.Common.Data;
using ZoneFiveSoftware.Common.Data.Fitness;
using ZoneFiveSoftware.Common.Visuals;
using ZoneFiveSoftware.Common.Visuals.Chart;
using ZoneFiveSoftware.Common.Data.GPS;
using ZoneFiveSoftware.Common.Data.Measurement;
using System.IO;
using GearChart.Common;

namespace GearChart.UI.DetailPage
{
    public partial class GearChartDetail : UserControl
    {
        #region Fields

        private static INumericTimeDataSeries gearSelection;
        private static ChartBasis chartType = ChartBasis.Gear_Time;
        private bool currentSortDirection;
        private string currentSortColumnId = string.Empty;
        private static IActivity activity;

        #endregion

        #region Enumerations

        public enum ChartBasis
        {
            Gear_Distance,
            Gear_Time
        }

        #endregion

        #region Constructor

        public GearChartDetail()
        {
            InitializeComponent();

            splitContainer1.SplitterDistance = Options.Instance.TreeHeight;

            if (Options.Instance.GearChartType == ChartBasis.Gear_Distance)
            {
                detailPaneChart.Title = Resources.Strings.GearRatioText + " / " + CommonResources.Text.LabelDistance;
                detailPaneChart.XAxisReferential = GearChart.Controls.DetailPaneChart.XAxisValue.Distance;
            }
            else
            {
                detailPaneChart.Title = Resources.Strings.GearRatioText + " / " + CommonResources.Text.LabelTime;
                detailPaneChart.XAxisReferential = GearChart.Controls.DetailPaneChart.XAxisValue.Time;
            }

            SetupMenuItems();

            treeListStats.ColumnResized += treeListStats_ColumnResized;
            splitContainer1.SplitterMoved += splitContainer1_SplitterMoved;
        }

        #endregion

        #region Properties

        public IActivity Activity
        {
            set
            {
                activity = value;
                detailPaneChart.SetActivity(activity);
                RefreshPage();
            }
        }

        /// <summary>
        /// Gets a value describing the selected chart type
        /// </summary>
        public static ChartBasis ChartType
        {
            get { return chartType; }
            set
            {
                chartType = value;
            }
        }

        #endregion

        #region Event Handlers

        private void treeListStats_ColumnResized(object sender, TreeList.ColumnEventArgs e)
        {
            List<TreeColumnDefinition> columns = new List<TreeColumnDefinition>();
            foreach (TreeList.Column column in treeListStats.Columns)
            {
                columns.Add(new TreeColumnDefinition(column.Id, column.Text, column.Width, column.TextAlign));
            }
            Options.Instance.SetTreeColumns(columns);
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            Options.Instance.TreeHeight = splitContainer1.SplitterDistance;
        }

        private void treeListStats_ColumnClicked(object sender, TreeList.ColumnEventArgs e)
        {
            SortTreeList(e.Column.Id, true);
        }

        #endregion

        #region Methods

        public void RefreshPage()
        {
            // Clear Chart
            ChartBase chart = detailPaneChart.Chart;
            detailPaneChart.ChartDataClear();
            gearSelection = new NumericTimeDataSeries();

            if (activity != null)
            {
                // Get Gear Track (raw data)
                gearSelection = GearUtils.GetRawGearTrack(activity);
                if (chartType == ChartBasis.Gear_Time)
                {
                    gearSelection = GearUtils.RemovePausedTimesInTrack(gearSelection, activity);
                }

                if (gearSelection.Count != 0)
                {
                    // Smooth data series
                    float min, max;
                    gearSelection = ZoneFiveSoftware.Common.Data.Algorithm.NumericTimeDataSeries.Smooth(gearSelection, Constants.GearTrackSmoothing, out max, out min);
                    //gearSelection = ZoneFiveSoftware.Common.Data.Algorithm.NumericTimeDataSeries.Smooth(gearSelection, 10, out max, out min);

                    //*********************************************
                    // Initialize variables for activity grade impact, and full category impact
                    ChartDataSeries rawGears = new ChartDataSeries(chart, chart.YAxis);

                    // ChartDataSeries
                    rawGears = PopulateDataSeries(gearSelection, rawGears, ChartType, activity);
                    rawGears.ChartType = ChartDataSeries.Type.Line;
                    rawGears.LineColor = Color.Red;
                    rawGears.ValueAxis = chart.YAxis;

                    if (Options.Instance.ShowRawGearData)
                    {
                        detailPaneChart.ChartDataAdd(rawGears);
                    }

                    // Guess gears
                    string equipmentId = Options.Instance.GetGearEquipmentId(activity);
                    List<SprocketCombo> gearRatios = Options.Instance.GetSprocketCombos(equipmentId);
                    INumericTimeDataSeries gearGuess = GearUtils.GuessGears(gearSelection, gearRatios);
                    ChartDataSeries guessSeries = new ChartDataSeries(chart, chart.YAxis);

                    if (Options.Instance.ShowEstGearData)
                    {
                        detailPaneChart.ChartDataAdd(guessSeries);
                    }

                    // ChartDataSeries
                    guessSeries = PopulateDataSeries(gearGuess, guessSeries, ChartType, activity);
                    guessSeries.ChartType = ChartDataSeries.Type.Line;
                    guessSeries.LineColor = Color.Blue;
                    guessSeries.ValueAxis = chart.YAxis;
                    chart.YAxis.LabelColor = rawGears.LineColor;

                    //ExportDataSeriesToCSV(guessSeries);
                    //ExportDataSeriesToCSV(rawGears);

                    if (Options.Instance.GearChartType == ChartBasis.Gear_Time)
                    {
                        chart.XAxis.Formatter = new Formatter.SecondsToTime();
                        chart.XAxis.Label = CommonResources.Text.LabelTime;
                    }
                    else
                    {
                        chart.XAxis.Formatter = new Formatter.General();
                        chart.XAxis.Label = CommonResources.Text.LabelDistance + " (" + activity.Category.DistanceUnits + ")";
                    }

                    Options.Instance.ShowZonesInChart = false;
                    chart.YAxis.Label = "m/Rev (ratio)";  // TODO: Localize this string
                }
            }

            chart.AutozoomToData(true);
            chart.Focus();

            // Update Tree
            RefreshTree();
        }

        internal ChartBase Chart
        {
            get { return detailPaneChart.Chart; }
        }

        private void RefreshTree()
        {
            List<GearStats> rowData = new List<GearStats>();

            if (gearSelection.Count > 0)
            {
                string equipmentId = Options.Instance.GetGearEquipmentId(activity);
                List<SprocketCombo> sprockets = Options.Instance.GetSprocketCombos(equipmentId);

                foreach (SprocketCombo combo in sprockets)
                {
                    INumericTimeDataSeries guessGears = new NumericTimeDataSeries(GearUtils.GuessGears(gearSelection, sprockets));
                    guessGears = GearUtils.RemovePausedTimesInTrack(guessGears, activity);
                    GearStats item = new GearStats(guessGears, combo.GearRatio, activity);
                    rowData.Add(item);
                }

                // Add 'Total' Line
                rowData.Add(new GearStats(rowData));
            }

            treeListStats.Columns.Clear();
            foreach (TreeList.Column column in GetTreeColumns())
            {
                treeListStats.Columns.Add(column);
            }

            treeListStats.RowDataRenderer = new MyRowDataRenderer(treeListStats);
            treeListStats.RowData = rowData;
        }

        private IList<TreeList.Column> GetTreeColumns()
        {
            IList<TreeList.Column> columns = new List<TreeList.Column>();

            // Restore column widths from saved settings
            foreach (TreeColumnDefinition item in Options.Instance.GetTreeColumns())
            {
                columns.Add(new TreeList.Column(item.Id, item.Text, item.Width, item.Alignment));
                columns[columns.Count - 1].CanSelect = true;
            }

            return columns;
        }

        /// <summary>
        /// Populate a dataseries with data from a NumericTimeDataSeries
        /// </summary>
        /// <param name="timeDataSeries">NumericTimeDataSeries containing data to add to chart data series</param>
        /// <param name="chartDataSeries">Dataseries to add data to</param>
        /// <param name="chartType">Chart basis (time or distance)</param>
        /// <returns>Populated dataseries</returns>
        private static ChartDataSeries PopulateDataSeries(INumericTimeDataSeries timeDataSeries, ChartDataSeries chartDataSeries, ChartBasis chartType, IActivity activity)
        {
            switch (chartType)
            {
                case ChartBasis.Gear_Distance:
                    ActivityInfo info = ActivityInfoCache.Instance.GetInfo(activity);
                    IDistanceDataTrack track = GearUtils.GetDistanceTrack(activity);
                    float test = 0;

                    foreach (ITimeValueEntry<float> entry in timeDataSeries)
                    {
                        DateTime time = track.EntryDateTime(entry);
                        float distance = track.GetInterpolatedValue(time).Value;
                        distance = (float)Length.Convert(distance, Length.Units.Meter, activity.Category.DistanceUnits);
                        PointF point = new PointF(distance, entry.Value);
                        if (!chartDataSeries.Points.ContainsKey(point.X))
                        {
                            chartDataSeries.Points.Add(point.X, point);
                        }
                        else if (test != point.X)
                        {
                            test = point.X;
                        }
                    }

                    break;

                case ChartBasis.Gear_Time:
                    foreach (ITimeValueEntry<float> entry in timeDataSeries)
                    {
                        PointF point = new PointF(entry.ElapsedSeconds, entry.Value);
                        chartDataSeries.Points.Add(point.X, point);
                    }

                    break;

                default:

                    break;
            }

            return chartDataSeries;
        }

        private void ExportDataSeriesToCSV(ChartDataSeries series)
        {
            if (!Directory.Exists(@"C:\ST exports\"))
            {
                Directory.CreateDirectory(@"C:\ST exports");
            }

            System.IO.StreamWriter file = new System.IO.StreamWriter("c:\\ST exports\\RawGears.csv", false);

            file.WriteLine("basis, raw, guess");
            foreach (PointF point in series.Points.Values)
            {
                file.WriteLine(point.X + ", " + point.Y);
            }

            file.Close();
        }

        public void ThemeChanged(ITheme visualTheme)
        {
            detailPaneChart.ThemeChanged(visualTheme);
            treeListStats.ThemeChanged(visualTheme);
        }

        #endregion

        #region MenuItem Setup and Handlers

        private void SetupMenuItems()
        {
            detailPaneChart.DetailMenu.Items.Clear();

            ToolStripMenuItem item1 = new ToolStripMenuItem(Resources.Strings.GearSelection + " / " + CommonResources.Text.LabelTime, null, detailMenu_Click);
            item1.Tag = ChartBasis.Gear_Time;
            detailPaneChart.DetailMenu.Items.Add(item1);

            ToolStripMenuItem item2 = new ToolStripMenuItem(Resources.Strings.GearSelection + " / " + CommonResources.Text.LabelDistance, null, detailMenu_Click);
            item2.Tag = ChartBasis.Gear_Distance;
            detailPaneChart.DetailMenu.Items.Add(item2);

            detailPaneChart.DetailMenu.Items.Add(new ToolStripSeparator());

            // TODO: Localize
            ToolStripMenuItem itemRaw = new ToolStripMenuItem("Show raw data", null, rawEstData_Click);
            itemRaw.Checked = Options.Instance.ShowRawGearData;
            itemRaw.Tag = "Raw";
            detailPaneChart.DetailMenu.Items.Add(itemRaw);

            // TODO: Localize
            ToolStripMenuItem itemEst = new ToolStripMenuItem("Show Est. gears", null, rawEstData_Click);
            itemEst.Checked = Options.Instance.ShowEstGearData;
            itemEst.Tag = "Est";
            detailPaneChart.DetailMenu.Items.Add(itemEst);

            // Setup current options
            if (Options.Instance.GearChartType == ChartBasis.Gear_Distance)
            {
                item2.Checked = true;
            }
            else
            {
                item1.Checked = true;
            }

            itemRaw.Checked = Options.Instance.ShowRawGearData;
            itemEst.Checked = Options.Instance.ShowEstGearData;
        }

        /// <summary>
        /// Handler for when an item in the banner detail menu is selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void detailMenu_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem selected = sender as ToolStripMenuItem;

            for (int i = 0; i < detailPaneChart.DetailMenu.Items.Count; i++)
            {
                ToolStripMenuItem item = detailPaneChart.DetailMenu.Items[i] as ToolStripMenuItem;

                if (item != null)
                {
                    if (item != selected)
                    {
                        item.Checked = false;
                    }
                    else
                    {
                        item.Checked = true;
                    }
                }
                else
                {
                    // ToolStrip Separator encountered.  Stop evaluating
                    break;
                }
            }

            ChartType = (ChartBasis)selected.Tag;
            detailPaneChart.Title = selected.Text;

            switch (chartType)
            {
                case ChartBasis.Gear_Distance:
                    detailPaneChart.XAxisReferential = GearChart.Controls.DetailPaneChart.XAxisValue.Distance;
                    break;

                case ChartBasis.Gear_Time:
                    detailPaneChart.XAxisReferential = GearChart.Controls.DetailPaneChart.XAxisValue.Time;
                    break;
            }

            RefreshPage();
        }

        private void rawEstData_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            if (item != null)
            {
                switch (item.Tag as string)
                {
                    case "Raw":
                        Options.Instance.ShowRawGearData = !Options.Instance.ShowRawGearData;
                        item.Checked = Options.Instance.ShowRawGearData;
                        break;
                    case "Est":
                        Options.Instance.ShowEstGearData = !Options.Instance.ShowEstGearData;
                        item.Checked = Options.Instance.ShowEstGearData;
                        break;
                }
            }

            RefreshPage();
        }

        #endregion

        /// <summary>
        /// Sort activities in TreeActivity table.
        /// </summary>
        /// <param name="columnId">Column to sort by.</param>
        /// <param name="reSort">Re-sort/invert sort of table.  For example, TRUE if column clicked, FALSE if maintain existing sort order.</param>
        private void SortTreeList(string columnId, bool reSort)
        {
            TreeList tree = treeListStats;

            // Exit if chart is empty
            if (tree.RowData != null)
            {
                // Sort only the data currently in the treeList
                List<GearStats> data = (List<GearStats>)tree.RowData;

                // Create a comparer instance
                GearStatsComparer comparer = new GearStatsComparer();

                switch (columnId)
                {
                    case "Time":
                        comparer.ComparisonMethod = GearStatsComparer.ComparisonType.Time;
                        break;
                    case "CoastingTime":
                        comparer.ComparisonMethod = GearStatsComparer.ComparisonType.Coasting;
                        break;
                    case "PercentTime":
                        comparer.ComparisonMethod = GearStatsComparer.ComparisonType.PercentTime;
                        break;
                    case "Power":
                        comparer.ComparisonMethod = GearStatsComparer.ComparisonType.Power;
                        break;
                    case "Cadence":
                        comparer.ComparisonMethod = GearStatsComparer.ComparisonType.Cadence;
                        break;
                    case "Distance":
                        comparer.ComparisonMethod = GearStatsComparer.ComparisonType.Distance;
                        break;
                    case "PercentDistance":
                        comparer.ComparisonMethod = GearStatsComparer.ComparisonType.PercentDistance;
                        break;
                    case "Ascend":
                        comparer.ComparisonMethod = GearStatsComparer.ComparisonType.Ascend;
                        break;
                    case "Descend":
                        comparer.ComparisonMethod = GearStatsComparer.ComparisonType.Descend;
                        break;
                    case "Speed":
                        comparer.ComparisonMethod = GearStatsComparer.ComparisonType.Speed;
                        break;
                    case "Grade":
                        comparer.ComparisonMethod = GearStatsComparer.ComparisonType.Grade;
                        break;
                    case "HR":
                        comparer.ComparisonMethod = GearStatsComparer.ComparisonType.HR;
                        break;
                    default:
                    case "Ratio":
                        comparer.ComparisonMethod = GearStatsComparer.ComparisonType.Ratio;
                        break;
                }

                // Set sort direction
                if ((reSort && !currentSortDirection && columnId == currentSortColumnId) || (!reSort && currentSortDirection))
                {
                    // Sort ascending - same column clicked, invert sort order OR don't resort and already Ascending
                    currentSortColumnId = columnId;
                    currentSortDirection = true; // Ascending = True, Descending = False
                    comparer.SortOrder = GearStatsComparer.Order.Ascending;
                    tree.SetSortIndicator(columnId, true);
                }
                else
                {
                    // Sort descending
                    currentSortColumnId = columnId;
                    currentSortDirection = false; // Ascending = True, Descending = False
                    comparer.SortOrder = GearStatsComparer.Order.Descending;
                    tree.SetSortIndicator(columnId, false);
                }

                // Sort and display activities
                data.Sort(comparer);
                tree.RowData = data;
            }
        }

    }

    class MyRowDataRenderer : TreeList.DefaultRowDataRenderer
    {
        public MyRowDataRenderer(TreeList tree)
            : base(tree)
        {

        }

        protected override FontStyle GetCellFontStyle(object element, TreeList.Column column)
        {
            if (IsTotalRow(element)) return FontStyle.Bold;
            return FontStyle.Regular;
        }

        protected override TreeList.DefaultRowDataRenderer.RowDecoration GetRowDecoration(object element)
        {
            if (IsTotalRow(element)) return RowDecoration.TopLineSingle;
            return RowDecoration.None;
        }

        private bool IsTotalRow(object element)
        {
            GearStats item = element as GearStats;
            if (item != null && item.Ratio == "Total")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
