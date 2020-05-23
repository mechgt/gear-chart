using System.Collections.Generic;
using System.ComponentModel;
using ZoneFiveSoftware.Common.Visuals;
using ZoneFiveSoftware.Common.Data.Fitness;
using System.Drawing;
using System.Xml;
using System.Diagnostics;
using System;
using GearChart.Common;
using ZoneFiveSoftware.Common.Data.Measurement;

namespace GearChart
{
    class Options
    {
        #region Fields

        public event OptionsChangedEventHandler OptionsChanged;

        private bool showRawData = true;
        private bool showEstData = true;

        private static int treeHeight;

        private List<TreeColumnDefinition> m_GearTreeColumns;

        private static Options instance;

        private Color m_ChartFillColor = Color.WhiteSmoke;
        private Color m_ChartLineColor = Color.LightSkyBlue;
        private Color m_ChartSelectedColor = Color.AliceBlue;
        private List<Controls.DetailPaneChart.LineChartTypes> m_SelectedCharts = new List<Controls.DetailPaneChart.LineChartTypes>();
        private List<BikeGearSetup> bikeSetupList = new List<BikeGearSetup>();

        private bool m_ShowZonesInChart = false;

        #endregion

        # region Generic region

        public Options()
        {
        }

        protected void TriggerOptionsChangedEvent(string propertyName)
        {
            if (OptionsChanged != null)
            {
                OptionsChanged(new PropertyChangedEventArgs(propertyName));
            }
        }

        public static Options Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Options();
                }

                return instance;
            }
        }

        #endregion

        #region User Global Options (Properties)

        public bool ShowRawGearData
        {
            get { return showRawData; }
            set
            {
                if (showRawData != value)
                {
                    showRawData = value;

                    TriggerOptionsChangedEvent("ShowRawData");
                }
            }
        }

        public bool ShowEstGearData
        {
            get { return showEstData; }
            set
            {
                if (showEstData != value)
                {
                    showEstData = value;

                    TriggerOptionsChangedEvent("showEstData");
                }
            }
        }

        public bool ShowZonesInChart
        {
            get { return m_ShowZonesInChart; }
            set
            {
                m_ShowZonesInChart = value;
            }
        }

        public List<Controls.DetailPaneChart.LineChartTypes> SelectedExtraCharts
        {
            get
            {
                return m_SelectedCharts;
            }
            set
            {
                SelectedExtraCharts.Clear();
                SelectedExtraCharts.AddRange(value);

                TriggerOptionsChangedEvent("SelectedCharts");
            }
        }

        public UI.DetailPage.GearChartDetail.ChartBasis GearChartType
        {
            get { return UI.DetailPage.GearChartDetail.ChartType; }
            set { UI.DetailPage.GearChartDetail.ChartType = value; }
        }

        public int TreeHeight
        {
            get
            {
                if (treeHeight != 0)
                {
                    return treeHeight;
                }
                else
                {
                    return 120;
                }
            }
            set { treeHeight = value; }
        }

        #endregion

        #region Logbook Specific Settings (Properties)

        #endregion

        #region Save to Logbook

        /// <summary>
        /// Initiates a save to the logbook
        /// </summary>
        public void StoreSettings()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement pluginElement = xmlDoc.CreateElement("GearChart");
            xmlDoc.AppendChild(pluginElement);

            XmlElement element = xmlDoc.CreateElement(Constants.BikeList);
            pluginElement.AppendChild(element);

            foreach (BikeGearSetup bike in bikeSetupList)
            {
                if (bike.Valid)
                {
                    // Bike header
                    element = xmlDoc.CreateElement(Constants.Bike);
                    element.Attributes.Append(XmlAttributeSetting(xmlDoc, Constants.WheelCircumference, bike.WheelCircumferenceMeters.ToString()));
                    element.Attributes.Append(XmlAttributeSetting(xmlDoc, Constants.Id, bike.Id));
                    pluginElement.LastChild.AppendChild(element);

                    // Big Gears
                    element = xmlDoc.CreateElement(Constants.BigGears);
                    pluginElement.LastChild.LastChild.AppendChild(element);
                    foreach (float gear in bike.BigGears)
                    {
                        element = xmlDoc.CreateElement(Constants.Item);
                        element.Attributes.Append(XmlAttributeSetting(xmlDoc, Constants.Gear, gear.ToString()));
                        pluginElement.LastChild.LastChild.LastChild.AppendChild(element);
                    }

                    // Small Gears
                    element = xmlDoc.CreateElement(Constants.SmallGears);
                    pluginElement.LastChild.LastChild.AppendChild(element);
                    foreach (float gear in bike.SmallGears)
                    {
                        element = xmlDoc.CreateElement(Constants.Item);
                        element.Attributes.Append(XmlAttributeSetting(xmlDoc, Constants.Gear, gear.ToString()));
                        pluginElement.LastChild.LastChild.LastChild.AppendChild(element);
                    }
                }
            }

            string settings = xmlDoc.OuterXml;

            PluginMain.GetApplication().Logbook.SetExtensionText(GUIDs.PluginMain, settings.ToString());
            PluginMain.GetApplication().Logbook.Modified = true;
        }

        private static XmlAttribute XmlAttributeSetting(XmlDocument xmlDoc, string name, string value)
        {
            XmlAttribute attribute;

            attribute = xmlDoc.CreateAttribute(name);
            attribute.Value = value;

            return attribute;
        }

        /// <summary>
        /// Load user settings from logbook
        /// </summary>
        public static void LoadSettings()
        {
            ILogbook logbook = PluginMain.GetApplication().Logbook;
            XmlDocument pluginNode = new XmlDocument();
            XmlNodeList list, bikeList;

            if (logbook != null && loaded == false)
            {
                string rawXmlString = logbook.GetExtensionText(GUIDs.PluginMain);

                if (rawXmlString != string.Empty)
                {
                    pluginNode.LoadXml(rawXmlString);
                    bikeList = pluginNode.GetElementsByTagName(Constants.BikeList);

                    //  Decode settings data from logbook
                    try
                    {
                        foreach (XmlElement bike in bikeList[0].ChildNodes)
                        {
                            // Wheel Circumference
                            float wheelCircum, gear;
                            float.TryParse(ReadXmlAttribute(bike, Constants.WheelCircumference), out wheelCircum);
                            string id = ReadXmlAttribute(bike, Constants.Id);

                            // Read Big gear settings
                            list = bike.GetElementsByTagName(Constants.BigGears);
                            if (list.Count > 0)
                            {
                                foreach (XmlElement item in list[0].ChildNodes)
                                {
                                    float.TryParse(ReadXmlAttribute(item, Constants.Gear), out gear);
                                    Options.Instance.AddBigGear(id, gear);
                                }
                            }

                            // Read Small gear settings
                            list = bike.GetElementsByTagName(Constants.SmallGears);
                            if (list.Count > 0)
                            {
                                foreach (XmlElement item in list[0].ChildNodes)
                                {
                                    float.TryParse(ReadXmlAttribute(item, Constants.Gear), out gear);
                                    Options.Instance.AddSmallGear(id, gear);
                                }
                            }

                            // Store Wheel Circumference settings
                            Instance.SetWheelCircumference(id, wheelCircum);
                        }

                        loaded = true;
                    }
                    catch (Exception error)
                    {

                    }
                }
            }
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

        /// <summary>
        /// Gets or sets a value indicating whether the settings have been loaded from the logbook.
        /// </summary>
        public static bool Loaded
        {
            get
            {
                return loaded;
            }

            set
            {
                loaded = value;
            }
        }
        private static bool loaded;

        #endregion

        #region Custom Supporting Methods

        public List<TreeColumnDefinition> GetTreeColumns()
        {
            if (m_GearTreeColumns == null || m_GearTreeColumns.Count < 13)
            {
                m_GearTreeColumns = new List<TreeColumnDefinition>();
                m_GearTreeColumns.Add(new TreeColumnDefinition("Ratio", Resources.Strings.GearRatioText + "(" + Length.LabelAbbr(Length.Units.Meter) + Resources.Strings.PerRevText + ")", Constants.DefaultColumnWidth, StringAlignment.Near));
                m_GearTreeColumns.Add(new TreeColumnDefinition("Time", CommonResources.Text.LabelTime, Constants.DefaultColumnWidth, StringAlignment.Far));
                m_GearTreeColumns.Add(new TreeColumnDefinition("PercentTime", "(%) " + CommonResources.Text.LabelTime, Constants.DefaultColumnWidth, StringAlignment.Far));
                m_GearTreeColumns.Add(new TreeColumnDefinition("Power", CommonResources.Text.LabelPower, Constants.DefaultColumnWidth, StringAlignment.Far));
                m_GearTreeColumns.Add(new TreeColumnDefinition("Cadence", CommonResources.Text.LabelCadence, Constants.DefaultColumnWidth, StringAlignment.Far));
                m_GearTreeColumns.Add(new TreeColumnDefinition("Distance", CommonResources.Text.LabelDistance, Constants.DefaultColumnWidth, StringAlignment.Far));
                m_GearTreeColumns.Add(new TreeColumnDefinition("PercentDistance", "(%) " + CommonResources.Text.LabelDistance, Constants.DefaultColumnWidth, StringAlignment.Far));
                m_GearTreeColumns.Add(new TreeColumnDefinition("Ascend", CommonResources.Text.LabelAscending, Constants.DefaultColumnWidth, StringAlignment.Far));
                m_GearTreeColumns.Add(new TreeColumnDefinition("Descend", CommonResources.Text.LabelDescending, Constants.DefaultColumnWidth, StringAlignment.Far));
                m_GearTreeColumns.Add(new TreeColumnDefinition("Speed", CommonResources.Text.LabelSpeed, Constants.DefaultColumnWidth, StringAlignment.Far));
                m_GearTreeColumns.Add(new TreeColumnDefinition("Grade", CommonResources.Text.LabelGrade + " (%)", Constants.DefaultColumnWidth, StringAlignment.Far));
                m_GearTreeColumns.Add(new TreeColumnDefinition("HR", CommonResources.Text.LabelHeartRate, Constants.DefaultColumnWidth, StringAlignment.Far));
                m_GearTreeColumns.Add(new TreeColumnDefinition("CoastingTime", Resources.Strings.Coasting, Constants.DefaultColumnWidth, StringAlignment.Far));
            }

            return m_GearTreeColumns;
        }

        /// <summary>
        /// Gets all the properly configures equipment ReferenceIds
        /// </summary>
        /// <returns>All equipment ReferenceIds defined.</returns>
        public IList<string> GetGearEquipmentIds()
        {
            List<string> result = new List<string>();

            foreach (BikeGearSetup bike in bikeSetupList)
            {
                if (bike.BigGears.Count > 0 &&
                    bike.SmallGears.Count > 0 &&
                    bike.WheelCircumferenceMeters != float.NaN)
                {
                    // Fully configured bike found
                    result.Add(bike.Id);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the equipment ReferenceId for the first FULLY configured piece of equipment
        /// </summary>
        /// <param name="activity">Activity to search</param>
        /// <returns>Equipment ReferenceId.  If multiple pieces of equipment are configured 
        /// for this activity, only the first one will be returned.</returns>
        public string GetGearEquipmentId(IActivity activity)
        {
            if (activity.EquipmentUsed.Count > 0)
            {
                foreach (IEquipmentItem item in activity.EquipmentUsed)
                {
                    foreach (BikeGearSetup bike in bikeSetupList)
                    {
                        if (bike.Id == item.ReferenceId &&
                            bike.BigGears.Count > 0 &&
                            bike.SmallGears.Count > 0 &&
                            bike.WheelCircumferenceMeters != float.NaN)
                        {
                            // Fully configured bike found
                            return bike.Id;
                        }
                    }
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Gets a list of the chainring gears for a specific bike, or an empty list if setup not found.
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <returns>Returns a list of big gears, or an empty list if none found</returns>
        public List<float> GetBigGears(string equipmentId)
        {
            foreach (BikeGearSetup item in bikeSetupList)
            {
                if (equipmentId == item.Id)
                {
                    return item.BigGears;
                }
            }

            return new List<float>();
        }

        /// <summary>
        /// Adds a specific gear to an equipment's gear list
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <param name="gear"></param>
        public void AddBigGear(string equipmentId, float gear)
        {
            foreach (BikeGearSetup bike in bikeSetupList)
            {
                if (bike.Id == equipmentId)
                {
                    if (!bike.BigGears.Contains(gear))
                    {
                        bike.BigGears.Add(gear);

                        GearChart.Common.Data.TriggerBikeSetupChangedEvent(this, bike.Id);
                    }
                    return;
                }
            }

            BikeGearSetup newBike = new BikeGearSetup(equipmentId);
            newBike.BigGears.Add(gear);
            bikeSetupList.Add(newBike);
            GearChart.Common.Data.TriggerBikeSetupChangedEvent(this, newBike.Id);
        }

        /// <summary>
        /// Removes a specific gear from an equipment's gear list
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <param name="gear"></param>
        public void RemoveBigGear(string equipmentId, float gear)
        {

            foreach (BikeGearSetup bike in bikeSetupList)
            {
                if (bike.Id == equipmentId)
                {
                    bike.BigGears.Remove(gear);

                    GearChart.Common.Data.TriggerBikeSetupChangedEvent(this, bike.Id);
                }
            }
        }

        /// <summary>
        /// Gets a list of the cassette gears for a specific bike, or an empty list if setup not found.
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <returns>Returns a list of small gears, or an empty list if none found</returns>
        public List<float> GetSmallGears(string equipmentId)
        {
            foreach (BikeGearSetup item in bikeSetupList)
            {
                if (equipmentId == item.Id)
                {
                    return item.SmallGears;
                }
            }

            return new List<float>();
        }

        /// <summary>
        /// Removes a specific gear from an equipment's gear list
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <param name="gear"></param>
        public void RemoveSmallGear(string equipmentId, float gear)
        {

            foreach (BikeGearSetup bike in bikeSetupList)
            {
                if (bike.Id == equipmentId)
                {
                    bike.SmallGears.Remove(gear);

                    GearChart.Common.Data.TriggerBikeSetupChangedEvent(this, bike.Id);
                }
            }
        }

        /// <summary>
        /// Adds a specific gear to an equipment's gear list
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <param name="gear"></param>
        public void AddSmallGear(string equipmentId, float gear)
        {
            foreach (BikeGearSetup bike in bikeSetupList)
            {
                if (bike.Id == equipmentId)
                {
                    if (!bike.SmallGears.Contains(gear))
                    {
                        bike.SmallGears.Add(gear);

                        GearChart.Common.Data.TriggerBikeSetupChangedEvent(this, bike.Id);
                    }
                    return;
                }
            }

            BikeGearSetup newBike = new BikeGearSetup(equipmentId);
            newBike.BigGears.Add(gear);
            bikeSetupList.Add(newBike);
            GearChart.Common.Data.TriggerBikeSetupChangedEvent(this, newBike.Id);
        }

        /// <summary>
        /// Gets a list of the gear ratios for a specific bike, or an empty list if setup not found.
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <returns>Returns a list of gear ratios, or an empty list if none found</returns>
        public List<float> GetGearRatios(string equipmentId)
        {
            foreach (BikeGearSetup item in bikeSetupList)
            {
                if (equipmentId == item.Id)
                {
                    return item.GearRatios;
                }
            }

            return new List<float>();
        }

        /// <summary>
        /// Gets a list of the gear ratios for a specific bike, or an empty list if setup not found.
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <returns>Returns a list of gear ratios, or an empty list if none found</returns>
        public List<SprocketCombo> GetSprocketCombos(string equipmentId)
        {
            foreach (BikeGearSetup item in bikeSetupList)
            {
                if (equipmentId == item.Id)
                {
                    return item.Sprockets;
                }
            }

            return new List<SprocketCombo>();
        }

        /// <summary>
        /// Gets the wheel circumference (in meters) for a specific bike, or 0 if not found.
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <returns>Returns the wheel circumference, or 0 if none found</returns>
        public float GetWheelCircumference(string equipmentId)
        {
            foreach (BikeGearSetup item in bikeSetupList)
            {
                if (equipmentId == item.Id)
                {
                    return item.WheelCircumferenceMeters;
                }
            }

            return 0;
        }

        /// <summary>
        /// Set wheel circumference for a particular bike
        /// </summary>
        /// <param name="equipment"></param>
        /// <param name="meters"></param>
        public void SetWheelCircumference(string equipmentId, float meters)
        {
            foreach (BikeGearSetup bike in bikeSetupList)
            {
                if (bike.Id == equipmentId)
                {
                    bike.WheelCircumferenceMeters = meters;
                    GearChart.Common.Data.TriggerBikeSetupChangedEvent(this, bike.Id);
                    return;
                }
            }

            BikeGearSetup newBike = new BikeGearSetup(equipmentId);
            newBike.WheelCircumferenceMeters = meters;
            bikeSetupList.Add(newBike);
            GearChart.Common.Data.TriggerBikeSetupChangedEvent(this, newBike.Id);
        }

        /// <summary>
        /// Store tree column definitions.  This completely replaces any old definitions with new definitions.
        /// </summary>
        /// <param name="columns">New columns to save.</param>
        public void SetTreeColumns(List<TreeColumnDefinition> columns)
        {
            m_GearTreeColumns = columns;
        }

        #endregion

        public delegate void OptionsChangedEventHandler(PropertyChangedEventArgs changedProperty);

    }

    class BikeGearSetup
    {
        private string equipmentId;
        private float circumference;
        private List<float> bigGears;
        private List<float> smallGears;

        public BikeGearSetup(string id)
        {
            equipmentId = id;
        }

        /// <summary>
        /// Gets a list of large gears (chain ring.) Measured in number of teeth
        /// </summary>
        public List<float> BigGears
        {
            get
            {
                if (bigGears == null)
                {
                    bigGears = new List<float>();
                    //bigGears.Add(53);
                    //bigGears.Add(39);
                }

                bigGears.Sort();
                return bigGears;
            }
            set { bigGears = value; }
        }

        /// <summary>
        /// Gets a list of small gears (cassette.) Measured in number of teeth
        /// </summary>
        public List<float> SmallGears
        {
            get
            {
                if (smallGears == null)
                {
                    smallGears = new List<float>();
                    /*smallGears.Add(12);
                    smallGears.Add(13);
                    smallGears.Add(14);
                    smallGears.Add(15);
                    smallGears.Add(17);
                    smallGears.Add(19);
                    smallGears.Add(21);
                    smallGears.Add(23);
                    smallGears.Add(25);
                     */
                }
                smallGears.Sort();
                return smallGears;
            }
            set { smallGears = value; }
        }

        /// <summary>
        /// Equipment Id associated with this gear setup
        /// </summary>
        public string Id
        {
            get { return equipmentId; }
        }

        /// <summary>
        /// Gets a list of possible sprockets 
        /// </summary>
        public List<SprocketCombo> Sprockets
        {
            get
            {
                List<SprocketCombo> sprockets = new List<SprocketCombo>();

                foreach (float bigGear in BigGears)
                {
                    foreach (float smallGear in SmallGears)
                    {
                        sprockets.Add(new SprocketCombo(bigGear, smallGear,
                                       bigGear / smallGear * WheelCircumferenceMeters));
                    }
                }

                return sprockets;
            }
        }

        /// <summary>
        /// Gets a list of possible gear ratios. 
        /// </summary>
        public List<float> GearRatios
        {
            get
            {
                List<float> gearRatios = new List<float>();
                foreach (float bigGear in BigGears)
                {
                    foreach (float smallGear in SmallGears)
                    {
                        gearRatios.Add(bigGear / smallGear * WheelCircumferenceMeters);
                    }
                }

                gearRatios.Sort();
                return gearRatios;
            }
        }

        /// <summary>
        /// Gets a value representing the circumference of the rear wheel in meters.  Default value is 2.096 meters.
        /// </summary>
        public float WheelCircumferenceMeters
        {
            get
            {
                if (circumference == 0)
                {
                    return 2.096F;
                }
                else
                {
                    return circumference;
                }
            }
            set
            {
                circumference = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not this is a valid configuration
        /// </summary>
        public bool Valid
        {
            get
            {
                if (WheelCircumferenceMeters > 0 &&
                    BigGears.Count > 0 &&
                    SmallGears.Count > 0 &&
                    !string.IsNullOrEmpty(equipmentId))
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

    class TreeColumnDefinition
    {
        private string text;
        private string id;
        private StringAlignment alignment;
        private int width;

        public TreeColumnDefinition(string id, string text, int width, StringAlignment alignment)
        {
            this.text = text;
            this.id = id;
            this.alignment = alignment;
            this.width = width;
        }

        public string Text
        {
            get { return text; }
        }

        public string Id
        {
            get { return id; }
        }

        public StringAlignment Alignment
        {
            get { return alignment; }
        }

        public int Width
        {
            get { return width; }
        }
    }
}
