using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using ZoneFiveSoftware.Common.Data.Fitness;
using ZoneFiveSoftware.Common.Data.Measurement;
using ZoneFiveSoftware.Common.Visuals;
using ZoneFiveSoftware.Common.Visuals.Chart;
using ZoneFiveSoftware.Common.Data;

namespace GearChart
{
    /// <summary>
    /// Generic utilities class that can be used on many projects
    /// </summary>
    static class Utilities
    {
        /// <summary>
        /// Refresh the SportTracks Calendar with the selected activities
        /// </summary>
        /// <param name="activities">These activity dates will be highlighted on the calendar</param>
        public static void RefreshCalendar(IList<IActivity> activities)
        {
            IList<DateTime> dates = new List<DateTime>();
            foreach (IActivity activity in activities)
            {
                dates.Add(activity.StartTime.ToLocalTime().Date);
            }

            PluginMain.GetApplication().Calendar.SetHighlightedDates(dates);
        }

        /// <summary>
        /// Export GearChart data to .csv file.
        /// </summary>
        /// <remarks>NOTE: This is specific to Gear Chart and may not work properly for other track types.</remarks>
        public static void ExportGearTracks(ChartBase chart, string name)
        {
            ChartDataSeries rawSeries = null, estimateSeries = null;

            foreach (ChartDataSeries series in chart.DataSeries)
            {
                if (series.LineColor == Color.Blue)
                    estimateSeries = series;
                else if (series.LineColor == Color.Red)
                    rawSeries = series;
            }

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                System.IO.StreamWriter writer = new System.IO.StreamWriter(name);

                // Write Header
                if (estimateSeries != null && rawSeries != null)
                {
                    writer.WriteLine(chart.XAxis.Label + ", Raw, Estimate");
                    foreach (PointF item in rawSeries.Points.Values)
                    {
                        // Write data
                        float x = item.X;
                        writer.WriteLine(item.X + ", " + item.Y + ", " + estimateSeries.GetYValueAtX(ref x));
                    }
                }
                else if (rawSeries != null)
                {
                    writer.WriteLine(chart.XAxis.Label + ", Raw");
                    foreach (PointF item in rawSeries.Points.Values)
                    {
                        // Write data
                        writer.WriteLine(item.X + ", " + item.Y + ", ");
                    }
                }
                else
                {
                    writer.WriteLine(chart.XAxis.Label + ", Estimate");
                    foreach (PointF item in estimateSeries.Points.Values)
                    {
                        // Write data
                        writer.WriteLine(item.X + ", " + item.Y);
                    }
                }
                writer.Close();
                MessageDialog.Show(CommonResources.Text.MessageExportComplete, Resources.Strings.GearSelection, MessageBoxButtons.OK);
            }
            catch { }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

        }

        /// <summary>
        /// Export track to .csv file
        /// </summary>
        /// <param name="track"></param>
        public static void ExportTrack(INumericTimeDataSeries track, string name)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                System.IO.StreamWriter writer = new System.IO.StreamWriter(name);

                // Write Header
                writer.WriteLine(track.StartTime.ToLocalTime() + ", " + System.IO.Path.GetFileNameWithoutExtension(name));
                writer.WriteLine("Seconds, Value");

                foreach (ITimeValueEntry<float> item in track)
                {
                    // Write data
                    writer.WriteLine(item.ElapsedSeconds + ", " + item.Value);
                }

                writer.Close();
                MessageDialog.Show(CommonResources.Text.MessageExportComplete, Resources.Strings.GearSelection, MessageBoxButtons.OK);
            }
            catch { }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// Format timespan for display.  Eliminates all leading zeros.
        /// </summary>
        /// <param name="span">Timespan to prepare for display</param>
        /// <returns>Formatted string (##:#0:00)</returns>
        public static string FormatTimeSpan(TimeSpan span)
        {
            string displayTime = string.Empty;

            if (span.TotalHours >= 1)
            {
                // Hours & minutes
                displayTime = (span.Days * 24 + span.Hours).ToString("#0") + ":" +
                              span.Minutes.ToString("00") + ":";
            }
            //else if (span.Minutes < 10)
            //{
            //    // Single digit minutes
            //    displayTime = span.Minutes.ToString("#0") + ":";
            //}
            else
            {
                // Double digit minutes
                displayTime = span.Minutes.ToString("#0") + ":";
            }

            displayTime = displayTime +
                          span.Seconds.ToString("00");

            return displayTime;
        }

        /// <summary>
        /// Open a popup treelist.
        /// </summary>
        /// <typeparam name="T">The type of items to be listed</typeparam>
        /// <param name="theme">Visual Theme</param>
        /// <param name="items">Items to be listed</param>
        /// <param name="control">The control that the list will appear attached to</param>
        /// <param name="selected">selected item</param>
        /// <param name="selectHandler">Handler that will handle when an item is clicked</param>
        public static void OpenListPopup<T>(ITheme theme, IList<T> items, System.Windows.Forms.Control control, T selected, TreeListPopup.ItemSelectedEventHandler selectHandler)
        {
            TreeListPopup popup = new TreeListPopup();
            popup.ThemeChanged(theme);
            popup.Tree.Columns.Add(new TreeList.Column());
            popup.Tree.RowData = items;
            if (selected != null)
            {
                popup.Tree.Selected = new object[] { selected };
            }

            popup.ItemSelected += delegate(object sender, TreeListPopup.ItemSelectedEventArgs e)
            {
                if (e.Item is T)
                {
                    selectHandler((T)e.Item, e);
                }
            };
            popup.Popup(control.Parent.RectangleToScreen(control.Bounds));
        }

        /// <summary>
        /// Open a context menu.
        /// </summary>
        /// <param name="theme">Visual Theme</param>
        /// <param name="items">Items to be listed</param>
        /// <param name="mouse"></param>
        /// <param name="selectHandler">Handler that will handle when an item is clicked</param>
        public static void OpenContextPopup(ITheme theme, ToolStripItemCollection items, MouseEventArgs mouse, ToolStripItemClickedEventHandler selectHandler)
        {
            ContextMenuStrip menuStrip = new ContextMenuStrip();

            menuStrip.Items.AddRange(items);

            menuStrip.ItemClicked += delegate(object sender, ToolStripItemClickedEventArgs e)
            {
                selectHandler(e.ClickedItem, e);
            };
            menuStrip.Show(mouse.Location);
        }
    }
}
