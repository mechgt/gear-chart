/******************************************************************************

    This file is from the TrailsPlugin.

    TrailsPlugin is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    TrailsPlugin is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with TrailsPlugin.  If not, see <http://www.gnu.org/licenses/>.
******************************************************************************/

using GearChart.Utils;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;
using System.Collections.Generic;
using ZoneFiveSoftware.Common.Data;
using ZoneFiveSoftware.Common.Data.Fitness;
using ZoneFiveSoftware.Common.Visuals;
using ZoneFiveSoftware.Common.Visuals.Chart;
using ZoneFiveSoftware.Common.Data.Measurement;

namespace GearChart.Controls
{
    public partial class DetailPaneChart : UserControl
    {
        #region Fields

        private XAxisValue m_XAxisReferential = XAxisValue.Time;
        private LineChartTypes m_YAxisReferential = LineChartTypes.Speed;
        private Color m_ChartFillColor = Color.WhiteSmoke;
        private Color m_ChartLineColor = Color.LightSkyBlue;
        private Color m_ChartSelectedColor = Color.AliceBlue;
        private IActivity m_activity;
        private ActivityInfo m_ActivityInfoCache;
        private IZoneCategory m_ShownZones;
        private List<ChartDataSeries> m_MainData;

        #endregion

        #region Constructors

        public DetailPaneChart()
        {
            InitializeComponent();

            SaveImageButton.CenterImage = CommonResources.Images.Save16;
            ZoomInButton.CenterImage = CommonResources.Images.ZoomIn16;
            ZoomOutButton.CenterImage = CommonResources.Images.ZoomOut16;
            ZoomChartButton.CenterImage = Resources.Images.ZoomFit;
            ExtraChartsButton.CenterImage = Resources.Images.Charts;
            ExportButton.CenterImage = ZoneFiveSoftware.Common.Visuals.CommonResources.Images.Export16;
        }

        #endregion

        #region Enumerators

        public enum XAxisValue
        {
            Time,
            Distance,
            Grade
        }

        /// <summary>
        /// Axis data options
        /// </summary>
        public enum YAxisValue
        {
            Time,
            Distance,
            Cadence,
            Elevation,
            HeartRateBPM,
            HeartRatePercentMax,
            Power,
            Grade,
            Speed,
            Pace
        }

        /// <summary>
        /// Y-axis Options
        /// </summary>
        public enum LineChartTypes
        {
            Cadence,
            Elevation,
            HeartRateBPM,
            HeartRatePercentMax,
            Power,
            Grade,
            Speed,
            Count
        }

        #endregion

        #region Properties

        private List<ChartDataSeries> ChartData
        {
            get
            {
                if (m_MainData == null)
                {
                    m_MainData = new List<ChartDataSeries>();
                }
                return m_MainData;
            }
        }

        #endregion

        #region Toolbar button handlers

        /// <summary>
        /// Pops up the Save Image dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveImageButton_Click(object sender, EventArgs e)
        {
            SaveImage dlg = new SaveImage();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                MainChart.SaveImage(dlg.ImageSize, dlg.FileName, dlg.ImageFormat);
            }

            MainChart.Focus();
            dlg.Dispose();
        }

        /// <summary>
        /// Zoom out of the chart when clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZoomOutButton_Click(object sender, EventArgs e)
        {
            MainChart.ZoomOut();
            MainChart.Focus();
        }

        /// <summary>
        /// Zoom into the chart when clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZoomInButton_Click(object sender, EventArgs e)
        {
            MainChart.ZoomIn();
            MainChart.Focus();
        }

        /// <summary>
        /// Zoom to Fit the chart when clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZoomFitButton_Click(object sender, EventArgs e)
        {
            MainChart.AutozoomToData(true);
            MainChart.Focus();
        }

        private void ExtraChartsButton_Click(object sender, EventArgs e)
        {
            SelectChartsForm dlg = new SelectChartsForm(Options.Instance.SelectedExtraCharts, Activity);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Options.Instance.SelectedExtraCharts = dlg.SelectedCharts;

                SetupPrimaryDataSeries();
                SetupSecondaryDataSeries();
            }
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            // TODO: How to export multiple activities...?
            //IActivity activity = ZoneFiveSoftware.Common.Visuals.Util.CollectionUtils.GetFirstItemOfType<IActivity>(activities);
            IActivity activity = Activity;
            // Nothing to export if activity is empty
            if (activity == null)
            {
                return;
            }

            // Open File Save dialog to create new CSV Document
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.FileName = "GearChart " + activity.StartTime.ToLocalTime().ToString("yyyy-MM-dd");
            saveFile.Filter = "All Files (*.*)|*.*|Comma Separated Values (*.csv)|*.csv";
            saveFile.FilterIndex = 2;
            saveFile.DefaultExt = "csv";
            saveFile.OverwritePrompt = true;

            string comma = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator;

            // Cancel if user doesn't select a file
            if (saveFile.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            // Export mean-max data
            // TODO: Export related data as well as MM data
            Utilities.ExportGearTracks(Chart, saveFile.FileName);
        }

        #endregion

        public void ThemeChanged(ITheme visualTheme)
        {
            MainChart.ThemeChanged(visualTheme);
            ButtonPanel.ThemeChanged(visualTheme);
            ButtonPanel.BackColor = visualTheme.Window;
            ChartBanner.ThemeChanged(visualTheme);
            panelMain.BackColor = visualTheme.Window;
        }

        public void UICultureChanged(CultureInfo culture)
        {
            SetupAxes();
        }

        public void ZoomToData()
        {
            ZoomFitButton_Click(null, null);
        }

        public void ChartDataClear()
        {
            ChartData.Clear();
            SetupPrimaryDataSeries();
        }

        /// <summary>
        /// Returns a string to be displayed in the banner (Y-axis / x-axis)
        /// </summary>
        /// <returns>String to be displayed at the top of the chart</returns>
        public string GetViewLabel()
        {
            string xAxisLabel = string.Empty;
            string yAxisLabel = string.Empty;

            // X axis
            switch (XAxisReferential)
            {
                case XAxisValue.Distance:
                    {
                        xAxisLabel = CommonResources.Text.LabelDistance;
                        break;
                    }
                case XAxisValue.Time:
                    {
                        xAxisLabel = CommonResources.Text.LabelTime;
                        break;
                    }
                //case XAxisValue.Cadence:
                //    {
                //        xAxisLabel = CommonResources.Text.LabelCadence;
                //        break;
                //    }
                //case XAxisValue.Elevation:
                //    {
                //        xAxisLabel = CommonResources.Text.LabelElevation;
                //        break;
                //    }
                //case XAxisValue.HeartRateBPM:
                //    {
                //        xAxisLabel = CommonResources.Text.LabelHeartRate;
                //        break;
                //    }
                //case XAxisValue.HeartRatePercentMax:
                //    {
                //        xAxisLabel = CommonResources.Text.LabelHeartRate;
                //        break;
                //    }
                //case XAxisValue.Power:
                //    {
                //        xAxisLabel = CommonResources.Text.LabelPower;
                //        break;
                //    }
                //case XAxisValue.Speed:
                //    {
                //        xAxisLabel = CommonResources.Text.LabelSpeed;
                //        break;
                //    }
                //case XAxisValue.Pace:
                //    {
                //        xAxisLabel = CommonResources.Text.LabelPace;
                //        break;
                //    }
                case XAxisValue.Grade:
                    {
                        xAxisLabel = CommonResources.Text.LabelGrade;
                        break;
                    }
                default:
                    {
                        Debug.Assert(false);
                        break;
                    }
            }

            // Y axis
            MainChart.YAxis.Formatter = new Formatter.General();
            switch (YAxisReferential)
            {
                case LineChartTypes.Cadence:
                    {
                        yAxisLabel = CommonResources.Text.LabelCadence;
                        break;
                    }
                case LineChartTypes.Elevation:
                    {
                        yAxisLabel = CommonResources.Text.LabelElevation;
                        break;
                    }
                case LineChartTypes.HeartRateBPM:
                    {
                        yAxisLabel = CommonResources.Text.LabelHeartRate;
                        break;
                    }
                case LineChartTypes.HeartRatePercentMax:
                    {
                        yAxisLabel = CommonResources.Text.LabelHeartRate;
                        break;
                    }
                case LineChartTypes.Power:
                    {
                        yAxisLabel = CommonResources.Text.LabelPower;
                        break;
                    }
                case LineChartTypes.Speed:
                    {
                        yAxisLabel = CommonResources.Text.LabelSpeed;
                        break;
                    }
                //case LineChartTypes.Pace:
                //    {
                //        yAxisLabel = CommonResources.Text.LabelPace;
                //        break;
                //    }
                case LineChartTypes.Grade:
                    {
                        yAxisLabel = CommonResources.Text.LabelGrade;
                        break;
                    }
                default:
                    {
                        Debug.Assert(false);
                        break;
                    }
            }

            return yAxisLabel + " / " + xAxisLabel;
        }

        public static String GetYAxisLabel(LineChartTypes chartType, IActivity activity)
        {
            string yAxisLabel = GetShortYAxisLabel(chartType, activity);

            switch (chartType)
            {
                case LineChartTypes.Cadence:
                    {
                        yAxisLabel += " (" + CommonResources.Text.LabelRPM + ")";
                        break;
                    }
                case LineChartTypes.Elevation:
                    {
                        if (activity != null)
                        {
                            yAxisLabel += " (" + Length.LabelPlural(activity.Category.ElevationUnits) + ")";
                        }
                        break;
                    }
                case LineChartTypes.Speed:
                    {
                        if (activity != null)
                        {
                            yAxisLabel += " (" + Utils.Utils.GetSpeedUnitLabelForActivity(activity) + ")";
                        }
                        break;
                    }
                case LineChartTypes.Power:
                    {
                        yAxisLabel += " (" + CommonResources.Text.LabelWatts + ")";
                        break;
                    }
            }

            return yAxisLabel;
        }

        public static String GetShortYAxisLabel(LineChartTypes chartType, IActivity activity)
        {
            string yAxisLabel = String.Empty;

            switch (chartType)
            {
                case LineChartTypes.Cadence:
                    {
                        yAxisLabel = CommonResources.Text.LabelCadence;
                        break;
                    }
                case LineChartTypes.Elevation:
                    {
                        yAxisLabel = CommonResources.Text.LabelElevation;
                        break;
                    }
                case LineChartTypes.Grade:
                    {
                        yAxisLabel = CommonResources.Text.LabelGrade;
                        break;
                    }
                case LineChartTypes.HeartRateBPM:
                    {
                        yAxisLabel = CommonResources.Text.LabelHeartRate + " (" + CommonResources.Text.LabelBPM + ")";
                        break;
                    }
                case LineChartTypes.HeartRatePercentMax:
                    {
                        yAxisLabel = CommonResources.Text.LabelHeartRate + " (" + CommonResources.Text.LabelPercentOfMax + ")";
                        break;
                    }
                case LineChartTypes.Power:
                    {
                        yAxisLabel = CommonResources.Text.LabelPower;
                        break;
                    }
                //case LineChartTypes.Pace:
                case LineChartTypes.Speed:
                    {
                        if (activity != null && activity.Category.SpeedUnits == Speed.Units.Pace)
                        {
                            yAxisLabel = CommonResources.Text.LabelPace;
                        }
                        else
                        {
                            yAxisLabel = CommonResources.Text.LabelSpeed;
                        }
                        break;
                    }
                default:
                    {
                        Debug.Assert(false);
                        break;
                    }
            }

            return yAxisLabel;
        }

        public void SetActivity(IActivity activity)
        {
            m_activity = activity;

            if (Activity != null)
            {
                m_ActivityInfoCache = ActivityInfoCache.Instance.GetInfo(Activity);
            }
            else
            {
                m_ActivityInfoCache = null;
            }

            // We must always do this since we might have changed something in the activity
            //  like it's category
            SetupAxes();
            UpdateAxisStripes();
            SetupSecondaryDataSeries();
            ZoomToData();
        }

        public IZoneCategory ShownZones
        {
            get { return m_ShownZones; }
            set
            {
                if (m_ShownZones != value)
                {
                    m_ShownZones = value;

                    if (MainChart != null && MainChart.YAxis != null)
                    {
                        SetupAxisStripes(MainChart.YAxis, YAxisReferential);
                    }
                }
            }
        }

        private void UpdateAxisStripes()
        {
            if (Activity != null && Options.Instance.ShowZonesInChart)
            {
                switch (YAxisReferential)
                {
                    case LineChartTypes.Cadence:
                        {
                            ShownZones = PluginMain.GetApplication().Logbook.CadenceZones[0];
                            break;
                        }
                    case LineChartTypes.HeartRateBPM:
                    case LineChartTypes.HeartRatePercentMax:
                        {
                            ShownZones = Activity.Category.HeartRateZone;
                            break;
                        }
                    case LineChartTypes.Power:
                        {
                            ShownZones = PluginMain.GetApplication().Logbook.PowerZones[0];
                            break;
                        }
                    case LineChartTypes.Speed:
                        {
                            ShownZones = Activity.Category.SpeedZone;
                            break;
                        }
                }
            }

            if (MainChart != null)
            {
                SetupAxisStripes(MainChart.YAxis, YAxisReferential);
            }
        }

        /// <summary>
        /// Sets up the chart axes.  Does things such as setting up the proper labels and formats based on the currently selected data.
        /// </summary>
        private void SetupAxes()
        {
            // X axis
            switch (XAxisReferential)
            {
                case XAxisValue.Distance:
                    {
                        if (PluginMain.GetApplication() != null)
                        {
                            Length.Units distanceUnit = PluginMain.GetApplication().SystemPreferences.DistanceUnits;

                            if (Activity != null && Activity.Category != null)
                            {
                                distanceUnit = Activity.Category.DistanceUnits;
                            }

                            MainChart.XAxis.Formatter = new Formatter.General();
                            MainChart.XAxis.Label = CommonResources.Text.LabelDistance + " (" +
                                                    Length.Label(distanceUnit) + ")";
                        }
                        break;
                    }
                case XAxisValue.Time:
                    {
                        MainChart.XAxis.Formatter = new Formatter.SecondsToTime();
                        MainChart.XAxis.Label = CommonResources.Text.LabelTime;
                        break;
                    }
                case XAxisValue.Grade:
                    {
                        MainChart.XAxis.Formatter = new Formatter.Percent();
                        MainChart.XAxis.Label = CommonResources.Text.LabelGrade + " (%)";
                        break;
                    }
                default:
                    {
                        Debug.Assert(false);
                        break;
                    }
            }

            // Y axis
            MainChart.YAxis.Formatter = new Formatter.General();
            switch (YAxisReferential)
            {
                case LineChartTypes.Cadence:
                    {
                        MainChart.YAxis.Label = CommonResources.Text.LabelCadence + " (" +
                                                CommonResources.Text.LabelRPM + ")";
                        break;
                    }
                case LineChartTypes.Grade:
                    {
                        MainChart.YAxis.Formatter = new Formatter.Percent();
                        MainChart.YAxis.Label = CommonResources.Text.LabelGrade + " (%)";
                        break;
                    }
                case LineChartTypes.Elevation:
                    {
                        if (PluginMain.GetApplication() != null)
                        {
                            Length.Units elevationUnit = PluginMain.GetApplication().SystemPreferences.ElevationUnits;

                            if (Activity != null)
                            {
                                elevationUnit = Activity.Category.ElevationUnits;
                            }

                            MainChart.YAxis.Label = CommonResources.Text.LabelElevation + " (" +
                                                    Length.Label(elevationUnit) + ")";
                        }
                        break;
                    }
                case LineChartTypes.HeartRateBPM:
                    {
                        MainChart.YAxis.Label = CommonResources.Text.LabelHeartRate + " (" +
                                                CommonResources.Text.LabelBPM + ")";
                        break;
                    }
                case LineChartTypes.HeartRatePercentMax:
                    {
                        MainChart.YAxis.Label = CommonResources.Text.LabelHeartRate + " (" +
                                                CommonResources.Text.LabelPercentOfMax + ")";
                        break;
                    }
                case LineChartTypes.Power:
                    {
                        MainChart.YAxis.Label = CommonResources.Text.LabelPower + " (" +
                                                CommonResources.Text.LabelWatts + ")";
                        break;
                    }
                case LineChartTypes.Speed:
                    {
                        MainChart.YAxis.Label = CommonResources.Text.LabelSpeed + " (" +
                        Utils.Units.GetSpeedUnitLabelForActivity(Activity) + ")";
                        break;
                    }
                //case LineChartTypes.Pace:
                //    {
                //        MainChart.YAxis.Formatter = new Formatter.SecondsToTime();
                //        MainChart.YAxis.Label = CommonResources.Text.LabelPace + " (" +
                //        Utils.Units.GetPaceUnitLabelForActivity(Activity) + ")";
                //        break;
                //    }
                default:
                    {
                        Debug.Assert(false);
                        break;
                    }
            }
        }

        private void SetupAxisStripes(IAxis yAxis, LineChartTypes chartType)
        {
            if (yAxis != null && Activity != null)
            {
                yAxis.Stripes.Clear();

                if (Options.Instance.ShowZonesInChart && ShownZones != null)
                {
                    foreach (INamedLowHighZone zone in ShownZones.Zones)
                    {
                        AxisStripe stripe = new AxisStripe(zone.Low, zone.High, Color.FromArgb(16, 0, 0, 0));
                        yAxis.Stripes.Add(stripe);

                        // Setup default parameters
                        stripe.Name = zone.Name;
                        stripe.LineColor = Color.FromArgb(128, 0, 0, 0);
                        stripe.LineStyle = DashStyle.Dash;
                        stripe.LineWidth = 2;

                        // Some types need to override the low/high values
                        switch (chartType)
                        {
                            case LineChartTypes.HeartRatePercentMax:
                                {
                                    IAthleteInfoEntry lastAthleteEntry = PluginMain.GetApplication().Logbook.Athlete.InfoEntries.LastEntryAsOfDate(m_ActivityInfoCache.ActualTrackStart);

                                    // Value is in BPM so convert to the % max HR if we have the info
                                    if (!float.IsNaN(lastAthleteEntry.MaximumHeartRatePerMinute))
                                    {
                                        stripe.From = (zone.Low / lastAthleteEntry.MaximumHeartRatePerMinute) * 100;
                                        stripe.To = (zone.High / lastAthleteEntry.MaximumHeartRatePerMinute) * 100;
                                    }
                                    else
                                    {
                                        // Cancel the add, we don't have the data
                                        yAxis.Stripes.Remove(stripe);
                                    }

                                    break;
                                }
                            case LineChartTypes.Speed:
                                {
                                    stripe.From = Length.Convert(zone.Low, Length.Units.Meter, Utils.Utils.MajorLengthUnit(Activity.Category.DistanceUnits)) * Utils.Constants.SecondsPerHour;
                                    stripe.To = Length.Convert(zone.High, Length.Units.Meter, Utils.Utils.MajorLengthUnit(Activity.Category.DistanceUnits)) * Utils.Constants.SecondsPerHour; ;

                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                    }

                    if (MainChart != null)
                    {
                        MainChart.Invalidate();
                    }
                }
            }
        }

        private void SetupPrimaryDataSeries()
        {
            ChartDataSeries mainData = new ChartDataSeries(MainChart, MainChart.YAxis);

            // Clear Primary Charts
            for (int i = MainChart.DataSeries.Count - 1; i >= 0; i--)
            {
                ChartDataSeries series = MainChart.DataSeries[i];

                if (series.ValueAxis == MainChart.YAxis)
                {
                    MainChart.DataSeries.Remove(series);
                }
            }

            if (Activity != null)
            {
                // Setup main chart
                foreach (ChartDataSeries series in ChartData)
                {
                    MainChart.DataSeries.Add(series);
                }
            }

            ZoomToData();
        }

        private void SetupSecondaryDataSeries()
        {
            // Clear Secondary Charts
            for (int i = MainChart.DataSeries.Count - 1; i >= 0; i--)
            {
                ChartDataSeries series = MainChart.DataSeries[i];
                if (series.ValueAxis != MainChart.YAxis)
                {
                    MainChart.DataSeries.Remove(series);
                }
            }

            if (Activity != null)
            {
                // Setup secondary charts
                MainChart.YAxisRight.Clear();
                foreach (LineChartTypes chartType in Options.Instance.SelectedExtraCharts)
                {
                    IAxis newAxis = new RightVerticalAxis(MainChart);
                    ChartDataSeries secondaryData = new ChartDataSeries(MainChart, newAxis);

                    // Only add if the track if available
                    if (FillSingleDataSerie(chartType, secondaryData))
                    {
                        MainChart.YAxisRight.Add(newAxis);
                        MainChart.DataSeries.Add(secondaryData);

                        SetupYAxisFromType(newAxis, chartType);
                        SetupYAxisAndDataColorFromType(newAxis, secondaryData, chartType);
                    }
                }
            }

            ZoomToData();
        }

        private void SetupYAxisFromType(IAxis yAxis, LineChartTypes chartType)
        {
            IAxisFormatter yAxisFormatter = new Formatter.General();

            yAxis.Label = GetYAxisLabel(chartType, Activity);
            switch (chartType)
            {
                case LineChartTypes.Cadence:
                    {
                        break;
                    }
                case LineChartTypes.Elevation:
                    {
                        if (PluginMain.GetApplication() != null)
                        {
                            Length.Units elevationUnit = PluginMain.GetApplication().SystemPreferences.ElevationUnits;

                            if (Activity != null)
                            {
                                elevationUnit = Activity.Category.ElevationUnits;
                            }
                        }
                        break;
                    }
                case LineChartTypes.Grade:
                    {
                        ((Formatter.General)yAxisFormatter).MinPrecision = 1;
                        break;
                    }
                case LineChartTypes.HeartRateBPM:
                    {
                        break;
                    }
                case LineChartTypes.HeartRatePercentMax:
                    {
                        yAxisFormatter = new Formatter.Percent();
                        break;
                    }
                case LineChartTypes.Power:
                    {
                        break;
                    }
                case LineChartTypes.Speed:
                    {
                        if (Activity != null && Activity.Category.SpeedUnits == Speed.Units.Pace)
                        {
                            yAxisFormatter = new Formatter.SecondsToTime();
                        }
                        else
                        {
                            ((Formatter.General)yAxisFormatter).MinPrecision = 2;
                        }

                        break;
                    }
                default:
                    {
                        Debug.Assert(false);
                        break;
                    }
            }
            yAxis.Formatter = yAxisFormatter;
        }

        private void SetupYAxisAndDataColorFromType(IAxis yAxis, ChartDataSeries dataSerie, LineChartTypes chartType)
        {
            yAxis.LabelColor = Utils.Constants.LineChartTypesColors[(int)chartType];
            dataSerie.LineColor = Utils.Constants.LineChartTypesColors[(int)chartType];
        }

        private bool FillSingleDataSerie(LineChartTypes chartType, ChartDataSeries dataSerie)
        {
            INumericTimeDataSeries graphPoints = GetSmoothedActivityTrack(chartType);

            if (graphPoints.Count > 0)
            {
                TimeSpan trackStartDiffWithActivity = graphPoints.StartTime - m_ActivityInfoCache.ActualTrackStart;
                float trackSecondDifference = (float)trackStartDiffWithActivity.TotalSeconds;

                if (XAxisReferential == XAxisValue.Time)
                {
                    graphPoints = Utils.Utils.RemovePausedTimesInTrack(graphPoints, Activity);

                    foreach (ITimeValueEntry<float> entry in graphPoints)
                    {
                        float key = trackSecondDifference + entry.ElapsedSeconds;
                        if (!dataSerie.Points.ContainsKey(key))
                        {
                            dataSerie.Points.Add(key, new PointF(trackSecondDifference + entry.ElapsedSeconds, entry.Value));
                        }
                    }
                }
                else if (m_ActivityInfoCache.MovingDistanceMetersTrack != null)
                {
                    IDistanceDataTrack distanceTrack = m_ActivityInfoCache.MovingDistanceMetersTrack;
                    int pointCount = Math.Min(distanceTrack.Count, graphPoints.Count);
                    ITimeValueEntry<float> startEntry = distanceTrack.GetInterpolatedValue(graphPoints.StartTime);

                    foreach (ITimeValueEntry<float> entry in graphPoints)
                    {
                        ITimeValueEntry<float> interpolatedEntry = distanceTrack.GetInterpolatedValue(graphPoints.StartTime + new TimeSpan(0, 0, (int)entry.ElapsedSeconds));

                        if (interpolatedEntry != null)
                        {
                            float distanceAtTime = interpolatedEntry.Value;
                            float distanceValue = (float)Length.Convert(distanceAtTime, Length.Units.Meter, Activity.Category.DistanceUnits);

                            float key = trackSecondDifference + entry.ElapsedSeconds;
                            if (!dataSerie.Points.ContainsKey(key))
                            {
                                dataSerie.Points.Add(key, new PointF(distanceValue, entry.Value));
                            }
                        }
                    }
                }

                return true;
            }

            return false;
        }

        private INumericTimeDataSeries GetSmoothedActivityTrack(LineChartTypes chartType)
        {
            // Fail safe
            INumericTimeDataSeries result = new NumericTimeDataSeries();

            if (Activity != null)
            {
                switch (chartType)
                {
                    case LineChartTypes.Cadence:
                        {
                            result = m_ActivityInfoCache.SmoothedCadenceTrack;
                            break;
                        }
                    case LineChartTypes.Elevation:
                        {
                            INumericTimeDataSeries tempResult = m_ActivityInfoCache.SmoothedElevationTrack;

                            // Value is in meters so convert to the right unit
                            result = new NumericTimeDataSeries();
                            foreach (ITimeValueEntry<float> entry in tempResult)
                            {
                                double temp = Length.Convert(entry.Value, Length.Units.Meter, Activity.Category.ElevationUnits);

                                result.Add(tempResult.EntryDateTime(entry), (float)temp);
                            }
                            break;
                        }
                    case LineChartTypes.Grade:
                        {
                            result = new NumericTimeDataSeries();

                            INumericTimeDataSeries tempResult = m_ActivityInfoCache.SmoothedGradeTrack;

                            foreach (ITimeValueEntry<float> entry in tempResult)
                            {
                                result.Add(tempResult.EntryDateTime(entry), entry.Value * 100.0f);
                            }
                            break;
                        }
                    case LineChartTypes.HeartRateBPM:
                        {
                            result = m_ActivityInfoCache.SmoothedHeartRateTrack;
                            break;
                        }
                    case LineChartTypes.HeartRatePercentMax:
                        {
                            result = new NumericTimeDataSeries();

                            IAthleteInfoEntry lastAthleteEntry = PluginMain.GetApplication().Logbook.Athlete.InfoEntries.LastEntryAsOfDate(m_ActivityInfoCache.ActualTrackStart);

                            // Value is in BPM so convert to the % max HR if we have the info
                            if (!float.IsNaN(lastAthleteEntry.MaximumHeartRatePerMinute))
                            {
                                INumericTimeDataSeries tempResult = m_ActivityInfoCache.SmoothedHeartRateTrack;

                                foreach (ITimeValueEntry<float> entry in tempResult)
                                {
                                    double temp = (entry.Value / lastAthleteEntry.MaximumHeartRatePerMinute) * 100;

                                    result.Add(tempResult.EntryDateTime(entry), (float)temp);
                                }
                            }
                            break;
                        }
                    case LineChartTypes.Power:
                        {
                            result = m_ActivityInfoCache.SmoothedPowerTrack;
                            break;
                        }
                    case LineChartTypes.Speed:
                        {
                            INumericTimeDataSeries tempResult = m_ActivityInfoCache.SmoothedSpeedTrack;

                            // Value is in m/sec so convert to the right unit and to
                            //  pace if necessary
                            result = new NumericTimeDataSeries();
                            foreach (ITimeValueEntry<float> entry in tempResult)
                            {
                                double temp = Length.Convert(entry.Value, Length.Units.Meter, Utils.Utils.MajorLengthUnit(Activity.Category.DistanceUnits)) * Utils.Constants.SecondsPerHour;

                                if (Activity.Category.SpeedUnits == Speed.Units.Pace)
                                {
                                    // Convert to pace and then in second
                                    temp = Utils.Utils.SpeedToPace(temp) * Utils.Constants.SecondsPerMinute;
                                }

                                result.Add(tempResult.EntryDateTime(entry), (float)temp);
                            }
                            break;
                        }
                    default:
                        {
                            Debug.Assert(false);
                            break;
                        }
                }
            }

            return result;
        }

        [DisplayName("X Axis value")]
        public XAxisValue XAxisReferential
        {
            get { return m_XAxisReferential; }
            set
            {
                m_XAxisReferential = value;
                SetupSecondaryDataSeries();
            }
        }

        [DisplayName("Y Axis value")]
        public LineChartTypes YAxisReferential
        {
            get { return m_YAxisReferential; }
            set
            {
                m_YAxisReferential = value;
            }
        }

        public Color ChartFillColor
        {
            get { return m_ChartFillColor; }
            set
            {
                if (m_ChartFillColor != value)
                {
                    m_ChartFillColor = value;

                    foreach (ChartDataSeries dataSerie in MainChart.DataSeries)
                    {
                        dataSerie.FillColor = ChartFillColor;
                    }
                }
            }
        }

        public Color ChartLineColor
        {
            get { return m_ChartLineColor; }
            set
            {
                if (ChartLineColor != value)
                {
                    m_ChartLineColor = value;

                    foreach (ChartDataSeries dataSerie in MainChart.DataSeries)
                    {
                        dataSerie.LineColor = ChartLineColor;
                    }
                }
            }
        }

        public Color ChartSelectedColor
        {
            get { return m_ChartSelectedColor; }
            set
            {
                if (ChartSelectedColor != value)
                {
                    m_ChartSelectedColor = value;

                    foreach (ChartDataSeries dataSeries in MainChart.DataSeries)
                    {
                        dataSeries.SelectedColor = ChartSelectedColor;
                    }
                }
            }
        }

        public IActivity Activity
        {
            get { return m_activity; }
        }

        public bool BeginUpdate()
        {
            return MainChart.BeginUpdate();
        }

        public void EndUpdate()
        {
            MainChart.EndUpdate();
        }

        /// <summary>
        /// Gets the chart control.
        /// </summary>
        public ChartBase Chart
        {
            get { return MainChart; }
        }

        /// <summary>
        /// Gets the banner control.
        /// </summary>
        public ActionBanner Banner
        {
            get
            {
                return ChartBanner;
            }
        }

        /// <summary>
        /// Gets or Sets the title shown in the banner
        /// </summary>
        public string Title
        {
            get { return ChartBanner.Text; }
            set { ChartBanner.Text = value; }
        }

        /// <summary>
        /// Gets the context menu shown when the activity banner menu button is clicked.
        /// </summary>
        public ContextMenuStrip DetailMenu
        {
            get { return detailMenu; }
        }

        /// <summary>
        /// Adds a chart data series to the chart, and refreshes display.
        /// </summary>
        /// <param name="DataSeries">Data series to be added.</param>
        public void ChartDataAdd(ChartDataSeries DataSeries)
        {
            m_MainData.Add(DataSeries);
            SetupPrimaryDataSeries();
            MainChart.Refresh();
        }

        /// <summary>
        /// Display the context menu when menu (blue arrow) is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChartBanner_MenuClicked(object sender, EventArgs e)
        {
            ChartBanner.ContextMenuStrip.Width = 100;
            ChartBanner.ContextMenuStrip.Show(ChartBanner.Parent.PointToScreen(new System.Drawing.Point(ChartBanner.Right - ChartBanner.ContextMenuStrip.Width - 2, ChartBanner.Bottom + 1)));
        }
    }
}
