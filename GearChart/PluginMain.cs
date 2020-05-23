using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;
using GearChart.Controls;
using GearChart.Data.ActivityDocumentationComponent;
using ZoneFiveSoftware.Common.Data;
using ZoneFiveSoftware.Common.Visuals.Fitness;
using ZoneFiveSoftware.Common.Data.Fitness;
using ZoneFiveSoftware.Common.Data.GPS;
using ZoneFiveSoftware.Common.Visuals;
using GearChart.Data.FilteredStatisticsPlugin;

namespace GearChart
{
    class PluginMain : IPlugin
    {
        # region Fields

        private static IApplication m_App;
        private static ActivityDocumentationPluginProxy m_ActivityDocumentationPluginProxy;

        # endregion

        #region Constructor

        public PluginMain()
        {
            if (m_ActivityDocumentationPluginProxy == null)
            {
                m_ActivityDocumentationPluginProxy = new ActivityDocumentationPluginProxy();
                m_ActivityDocumentationPluginProxy.Register(new ADComponentFactory());
            }
        }

        # endregion

        /// <summary>
        /// Plugin product Id as listed in license application
        /// </summary>
        internal static string ProductId
        {
            get
            {
                return "gs";
            }
        }

        /// <summary>
        /// Number of days in history that data will be displayed.
        /// This is the amount of data that will be displayed, not a number of days for a trial version.
        /// </summary>
        internal static int EvalDays
        {
            get { return 90; }
        }

        internal static string SupportEmail
        {
            get
            {
                return "support@mechgt.com";
            }
        }

        #region IPlugin Members

        public IApplication Application
        {
            set
            {
                if (m_App != null)
                {
                    m_App.PropertyChanged -= new PropertyChangedEventHandler(AppPropertyChanged);
                }

                m_App = value;

                if (m_App != null)
                {
                    m_App.PropertyChanged += new PropertyChangedEventHandler(AppPropertyChanged);
                }
            }
        }

        public System.Guid Id
        {
            get { return GUIDs.PluginMain; }
        }

        public string Name
        {
            get { return Resources.Strings.GearSelection; }
        }

        public void ReadOptions(XmlDocument xmlDoc, XmlNamespaceManager nsmgr, XmlElement pluginNode)
        {
            List<DetailPaneChart.LineChartTypes> displayedCharts = new List<DetailPaneChart.LineChartTypes>();
            XmlElement element;
            XmlNodeList list;

            float WheelCircumferenceMeters = 0;
            List<float> BigGears = new List<float>();
            List<float> SmallGears = new List<float>();

            // General Settings
            list = pluginNode.GetElementsByTagName("Options");
            if (list.Count > 0)
            {
                element = list[0] as XmlElement;
                if (element.HasAttribute(Constants.WheelCircumference))
                {
                    WheelCircumferenceMeters = float.Parse(ReadXmlAttribute(element, Constants.WheelCircumference));
                }
            }

            // Read extended gear charts
            list = pluginNode.GetElementsByTagName(Constants.GearCharts);
            if (list.Count > 0)
            {
                element = list[0] as XmlElement;
                bool rawData, estData;
                if (!bool.TryParse(ReadXmlAttribute(element, "rawData"), out rawData))
                {
                    // Set default value if option not found
                    rawData = true;
                }

                if (!bool.TryParse(ReadXmlAttribute(element, "estData"), out estData))
                {
                    // Set default value if option not found
                    estData = true;
                }
                Options.Instance.ShowRawGearData = rawData;
                Options.Instance.ShowEstGearData = estData;

                int value;
                if (int.TryParse(ReadXmlAttribute(element, "treeHeight"), out value))
                {
                    Options.Instance.TreeHeight = value;
                }

                string chartType = ReadXmlAttribute(element, "chartType");
                if (!string.IsNullOrEmpty(chartType))
                {
                    Options.Instance.GearChartType = (GearChart.UI.DetailPage.GearChartDetail.ChartBasis)Enum.Parse(typeof(GearChart.UI.DetailPage.GearChartDetail.ChartBasis), chartType);
                }

                foreach (XmlElement item in list[0].ChildNodes)
                {
                    DetailPaneChart.LineChartTypes chart = (DetailPaneChart.LineChartTypes)Enum.Parse(typeof(DetailPaneChart.LineChartTypes), ReadXmlAttribute(item, "id"));
                    Options.Instance.SelectedExtraCharts.Add(chart);
                }
            }

            // Read tree column settings
            list = pluginNode.GetElementsByTagName(Constants.GearColumns);
            if (list.Count > 0)
            {
                List<TreeColumnDefinition> columns = new List<TreeColumnDefinition>();
                foreach (XmlElement item in list[0].ChildNodes)
                {
                    try
                    {
                        string id = ReadXmlAttribute(item, "id");
                        string text = ReadXmlAttribute(item, "text");
                        int width = int.Parse(ReadXmlAttribute(item, "width"));
                        StringAlignment align = (StringAlignment)Enum.Parse(typeof(StringAlignment), ReadXmlAttribute(item, "align"));

                        columns.Add(new TreeColumnDefinition(id, text, width, align));
                    }
                    catch { }
                }

                Options.Instance.SetTreeColumns(columns);
            }
        }

        public string Version
        {
            get { return GetType().Assembly.GetName().Version.ToString(3); }
        }

        public void WriteOptions(XmlDocument xmlDoc, XmlElement pluginNode)
        {
            XmlElement element;
            pluginNode.Attributes.Append(XmlAttributeSetting(xmlDoc, "name", "Gear Selection"));

            // General Options
            element = xmlDoc.CreateElement("Options");
            pluginNode.AppendChild(element);

            // Save Selected Gear Options
            element = xmlDoc.CreateElement(Constants.GearColumns);
            pluginNode.AppendChild(element);

            // Columns
            foreach (TreeColumnDefinition column in Options.Instance.GetTreeColumns())
            {
                element = xmlDoc.CreateElement("Item");
                pluginNode.LastChild.AppendChild(element);

                element.Attributes.Append(XmlAttributeSetting(xmlDoc, "id", column.Id));
                element.Attributes.Append(XmlAttributeSetting(xmlDoc, "text", column.Text));
                element.Attributes.Append(XmlAttributeSetting(xmlDoc, "width", column.Width.ToString()));
                element.Attributes.Append(XmlAttributeSetting(xmlDoc, "align", column.Alignment.ToString()));
            }

            // Save Chart Options
            element = xmlDoc.CreateElement(Constants.GearCharts);
            pluginNode.AppendChild(element);

            element.Attributes.Append(XmlAttributeSetting(xmlDoc, "rawData", Options.Instance.ShowRawGearData.ToString()));
            element.Attributes.Append(XmlAttributeSetting(xmlDoc, "estData", Options.Instance.ShowEstGearData.ToString()));
            element.Attributes.Append(XmlAttributeSetting(xmlDoc, "chartType", Options.Instance.GearChartType.ToString()));
            element.Attributes.Append(XmlAttributeSetting(xmlDoc, "treeHeight", Options.Instance.TreeHeight.ToString()));

            // Selected Charts
            foreach (int chart in Options.Instance.SelectedExtraCharts)
            {
                element = xmlDoc.CreateElement("Item");
                pluginNode.LastChild.AppendChild(element);

                element.Attributes.Append(XmlAttributeSetting(xmlDoc, "id", chart.ToString()));
            }
        }

        private static XmlAttribute XmlAttributeSetting(XmlDocument xmlDoc, string name, string value)
        {
            XmlAttribute attribute;

            attribute = xmlDoc.CreateAttribute(name);
            attribute.Value = value;

            return attribute;
        }

        /// <summary>
        /// Reads the named attribute from a given element.
        /// </summary>
        /// <param name="element">XML element containing the attribute to be read</param>
        /// <param name="name">Attribute name</param>
        /// <returns>Returns the read value, or an empty string if the value does not exist</returns>
        private static string ReadXmlAttribute(XmlElement element, string name)
        {
            if (element.HasAttribute(name))
            {
                string value = element.GetAttribute(name);
                return value;
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion

        public static IApplication GetApplication()
        {
            return m_App;
        }

        void AppPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Logbook")
            {
                // Register our data track providers to Filtered Statistics plugin
                if (ActivityDataTrackControllerWrapper.Instance.IsPluginInstalled &&
                    ActivityDataTrackControllerWrapper.Instance.RegisterMethodAvailable &&
                    !m_DataTrackProviderRegistered)
                {
                    ActivityDataTrackControllerWrapper.Instance.RegisterDataTrackProvider(new GearsDataTrackProvider());
                    ActivityDataTrackControllerWrapper.Instance.RegisterDataTrackProvider(new RawGearsDataTrackProvider());
                    m_DataTrackProviderRegistered = true;
                }

                // Register our filter criteria provider
                if (FilterCriteriaControllerWrapper.Instance.IsPluginInstalled &&
                    FilterCriteriaControllerWrapper.Instance.RegisterMethodAvailable &&
                    !m_FilterCriteriaProviderRegistered)
                {
                    FilterCriteriaControllerWrapper.Instance.RegisterFilterCriteriaProvider(new GearFilterCriteriasProvider());
                    m_FilterCriteriaProviderRegistered = true;
                }

                // Register our data track providers to Filtered Statistics plugin
                if (StatisticsControllerWrapper.Instance.IsPluginInstalled &&
                    StatisticsControllerWrapper.Instance.RegisterMethodAvailable &&
                    !m_StatisticsProviderRegistered)
                {
                    StatisticsControllerWrapper.Instance.RegisterProvider(new GearStatisticsProvider());
                    m_StatisticsProviderRegistered = true;
                }

                if (m_CurrentLogbook != null)
                {
                    m_CurrentLogbook.PropertyChanged -= Logbook_DataChanged;
                }

                if (LogbookChanged != null)
                {
                    LogbookChanged(this, m_CurrentLogbook, m_App.Logbook);
                }

                m_CurrentLogbook = m_App.Logbook;

                if (m_CurrentLogbook != null)
                {
                    m_CurrentLogbook.PropertyChanged += Logbook_DataChanged;
                }

                Options.LoadSettings();
            }
        }

        void Logbook_DataChanged(object sender, PropertyChangedEventArgs e)
        {
            // TODO: Check ST3 migration: Logbook_DataChanged
            if (e.GetType().FullName == "ZoneFiveSoftware.SportTracks.Data.ZoneCategory")
            {
                if (ZoneCategoryChanged != null)
                {
                    ZoneCategoryChanged(this, (IZoneCategory)sender);
                }
            }
            else if (e.GetType().FullName == "ZoneFiveSoftware.SportTracks.Data.ActivityCategory")
            {
                if (ActivityCategoryChanged != null)
                {
                    ActivityCategoryChanged(this, (IActivityCategory)sender);
                }
            }
        }

        public delegate void LogbookChangedEventHandler(object sender, ILogbook oldLogbook, ILogbook newLogbook);
        public static event LogbookChangedEventHandler LogbookChanged;

        public delegate void ZoneCategoryChangedEventHandler(object sender, IZoneCategory category);
        public static event ZoneCategoryChangedEventHandler ZoneCategoryChanged;

        public delegate void ActivityCategoryChangedEventHandler(object sender, IActivityCategory category);
        public static event ActivityCategoryChangedEventHandler ActivityCategoryChanged;

        private static ILogbook m_CurrentLogbook = null;

        private bool m_DataTrackProviderRegistered = false;
        private bool m_FilterCriteriaProviderRegistered = false;
        private bool m_StatisticsProviderRegistered = false;
    }
}
