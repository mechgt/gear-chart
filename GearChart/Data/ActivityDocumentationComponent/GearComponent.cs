using System;
using System.Collections.Generic;
using System.Text;
using OldManBiking.ActivityDocumentationPlugin.Data;
using System.Windows.Forms;
using ZoneFiveSoftware.Common.Data.Fitness;
using ZoneFiveSoftware.Common.Data;
using ZoneFiveSoftware.Common.Visuals.Chart;
using GearChart.UI.DetailPage;

namespace GearChart.Data.ActivityDocumentationComponent
{
    /// <summary>
    /// An example for a component that can be added to the ActivityDocumentation plugin
    /// </summary>
    class GearComponent : IActivityDocumentationComponent
    {
        /// <summary>
        /// Create a component for a certain activity
        /// usually you will simply store the activity in a member variable for later use
        /// </summary>
        /// <param name="activity">the activity</param>
        public GearComponent(IActivity activity)
        {
            this.activity = activity;
        }

        #region IActivityDocumentationComponent Member

        /// <summary>
        /// See the comment for IActivityDocumentationComponent
        /// </summary>
        public string DisplayName
        {
            get { return "Gear Chart"; }
        }

        /// <summary>
        /// Popup edit dialog box and collect options
        /// </summary>
        public bool Edit()
        {
            EditForm edit = new EditForm();
            edit.ChartHeight = height;
            edit.ChartWidth = width;
            edit.ShowLaps = showLaps;
            edit.ShowRawData = showRawData;
            edit.Time = time;

            if (edit.ShowDialog() == DialogResult.OK)
            {
                width = edit.ChartWidth;
                height = edit.ChartHeight;
                showLaps = edit.ShowLaps;
                time = edit.Time;
                showRawData = edit.ShowRawData;

                edit.Dispose();

                return true;
            }
            else
            {
                edit.Dispose();

                return false;
            }
        }

        /// <summary>
        /// See the comment for IActivityDocumentationComponent
        /// </summary>
        public bool Enabled
        {
            // only enabled for activities with a start time
            get { return activity.HasStartTime; }
        }

        /// <summary>
        /// See the comment for IActivityDocumentationComponent
        /// </summary>
        public string GetHtml(out IList<System.Drawing.Image> images)
        {
            // return the image of a chart and other graphical output
            images = new List<System.Drawing.Image>();
            string html;
            string tempImgFile = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetTempFileName());

            bool rawData = GearChart.Options.Instance.ShowRawGearData;
            bool estData = GearChart.Options.Instance.ShowEstGearData;
            GearChartDetail.ChartBasis type = GearChart.Options.Instance.GearChartType;

            List<GearChart.Controls.DetailPaneChart.LineChartTypes> extraCharts = GearChart.Options.Instance.SelectedExtraCharts;

            GearChart.Options.Instance.ShowRawGearData = showRawData;
            GearChart.Options.Instance.ShowEstGearData = true;

            GearChart.Options.Instance.SelectedExtraCharts.Clear();
            if (time)
            {
                GearChart.Options.Instance.GearChartType = GearChartDetail.ChartBasis.Gear_Time;
            }
            else
            {
                GearChart.Options.Instance.GearChartType = GearChartDetail.ChartBasis.Gear_Distance;
            }

            GearChartDetail control = GearChartPage.Instance.CreatePageControl() as GearChartDetail;
            // TODO: There's an issue with the auto-zoom.  Now (with .Focus() in RefreshPage method) it will show right first time, but weird zoom next time. 
            control.SelectNextControl(control.Chart, true, true, true, true);
            control.RefreshPage();
            //control.Chart.Focus();
            
            control.Chart.BackColor = System.Drawing.Color.White;
            control.Chart.YAxis.Stripes.Clear();

            control.Chart.SaveImage(new System.Drawing.Size(width, height), tempImgFile, System.Drawing.Imaging.ImageFormat.Png);

            GearChart.Options.Instance.ShowRawGearData = rawData;
            GearChart.Options.Instance.ShowEstGearData = estData;
            GearChart.Options.Instance.SelectedExtraCharts = extraCharts;

            System.Drawing.Image chartImage = System.Drawing.Image.FromFile(tempImgFile);
            images.Add(chartImage);

            // {0} is a placeholder for the first image added to the images list
            // TODO: Write Min/Max/Avg Values... or not.
            html = "<H4 style=\"COLOR: #FF0000\">Gear Chart (including stopped times)</H4>";
            /*
            html += "<TABLE>\n<TBODY>\n<TR>\n" +
                "<TD class=caption>Min.</TD>\n" +
                "<TD class=caption>Max.</TD>\n" +
                "<TD class=caption>Avg.</TD></TR>\n" +
                "<TR><TD>0.0 rpm</TD>" +
                "<TD>99.6 rpm</TD>" +
                "<TD>65.5 rpm</TD></TR></TBODY></TABLE>";
            */
            html += "<BR><IMG src=\"{0}\"/>";
            return html;
            //return "<B>Gear Chart: <BR><IMG src=\"{0}\"/></B><BR>";
        }

        /// <summary>
        /// See the comment for IActivityDocumentationComponent
        /// </summary>
        public OldManBiking.SporttracksPlugins.Data.IGroup Group
        {
            // you could instantiate a Group class and return it to let the component appear
            // in a subfolder of the components menu
            get { return null; }
        }

        /// <summary>
        /// See the comment for IActivityDocumentationComponent
        /// </summary>
        public System.Drawing.Image Image
        {
            get { return ZoneFiveSoftware.Common.Visuals.CommonResources.Images.Settings16; }
        }

        /// <summary>
        /// See the comment for IActivityDocumentationComponent
        /// </summary>
        public string Parameters
        {
            get
            {
                // for this example, we let the framework save the current date time as a parameter, when we're
                // called for the first time, i.e. the parameters haven't been set before
                // Parameters don't make much sense, when the component doesn't show a real config
                // dialog when Edit() is called
                return showLaps + "|" +
                       time + "|" +
                       showRawData + "|" +
                       width.ToString() + "|" +
                       height.ToString();
            }
            set
            {
                parameters = value;
                string[] values = value.Split('|');

                if (values.Length >= 5)
                {
                    bool.TryParse(values[0], out showLaps);
                    bool.TryParse(values[1], out time);
                    bool.TryParse(values[2], out showRawData);

                    int.TryParse(values[3], out width);
                    int.TryParse(values[4], out height);
                }
                else
                {
                    // Set Default values
                    showLaps = false;
                    time = true;
                    showRawData = true;
                    width = 800;
                    height = 240;
                }
            }
        }

        /// <summary>
        /// See the comment for IActivityDocumentationComponent
        /// </summary>
        public string ReferenceId
        {
            get { return RefId; }
        }

        /// <summary>
        /// See the comment for IActivityDocumentationComponent
        /// </summary>
        public bool SupportsEdit
        {
            // This component supports Edit although the Edit method is not a very brilliant one
            get { return true; }
        }

        /// <summary>
        /// See the comment for IActivityDocumentationComponent
        /// </summary>
        public string ToolTip
        {
            // well, in real use this should use resources to be multilingual
            get { return "Gear Component Tooltip ..."; }
        }

        #endregion

        // unique ID (-> http://www.guidgen.com )
        // This has been updated by mechgt
        internal static readonly String RefId = "d0527ce5-ae91-4cd8-aa19-739f2240ab82";

        private bool showLaps;
        private bool time;
        private bool showRawData;
        private int width;
        private int height;

        private IActivity activity;
        private String parameters;
    }
}
