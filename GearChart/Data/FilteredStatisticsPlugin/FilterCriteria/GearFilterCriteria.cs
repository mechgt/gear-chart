using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using ZoneFiveSoftware.Common.Data;
using ZoneFiveSoftware.Common.Data.Fitness;
using GearChart.Common;
using GearChart.Resources;
using GearChart.Utils;

namespace GearChart.Data.FilteredStatisticsPlugin
{
	class GearFilterCriteria
    {
        public GearFilterCriteria() :
            this(null)
        {
        }

        public GearFilterCriteria(IActivity activity)
        {
            Activity = activity;

            m_ActivityDataChangedHelper.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(OnActivityDataChanged);
            Common.Data.BikeSetupChanged += new Common.Data.BikeSetupChangedEventHandler(OnGearChartBikeSetupChanged);

            BuildNamedZones();
        }

        void OnActivityDataChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Activity.EquipmentUsed")
            {
                BuildNamedZones();
            }
        }

        void OnGearChartBikeSetupChanged(object sender, string setupId)
        {
            // Check if the modified setup is used by our current activity
            if (m_Activity != null)
            {
                bool equipmentUsed = false;

                foreach (IEquipmentItem equipment in m_Activity.EquipmentUsed)
                {
                    if(equipment.ReferenceId.Equals(setupId))
                    {
                        equipmentUsed = true;
                        break;
                    }
                }

                if (equipmentUsed)
                {
                    BuildNamedZones();
                }
            }
        }

#region IFilterCriteria members

        public Guid ReferenceId
        {
            get { return new Guid("42ee071c-4fc4-4e90-a1a8-b86411d612c2"); }
        }

        public IActivity Activity
        {
            set
            {
                if (m_Activity != value)
                {
                    m_Activity = value;
                    m_ActivityDataChangedHelper.Activity = m_Activity;

                    BuildNamedZones();
                }
            }
        }

	    public string DisplayName
	    {
            get { return Strings.GearsText; }
        }

        public List<object> NamedZones
	    {
            get { return m_NamedZones; }
        }

        public bool IsTemplateOnly
        {
            get
            {
                return false;
            }
        }

        public bool IsMainViewOnly
        {
            get
            {
                return true;
            }
        }
        
        public object TemplateCompatibleCriteria
        {
            get
            {
                return new TemplateGearFilterCriteria(m_Activity, Common.Data.GetActivityEquipmentId(m_Activity));
            }
        }

        public bool IsSerializable
        {
            get { return false; }
        }

        public void SerializeCriteria(Stream stream)
        {
        }

        public UInt16 DataVersion
        {
            get { return 1; }
        }

        public object DeserializeCriteria(Stream stream, UInt16 version)
        {
            return this;
        }

        public event PropertyChangedEventHandler NamedZonedListChanged;

#endregion

        private void BuildNamedZones()
        {
            m_NamedZones.Clear();

            if (m_Activity != null)
            {
                List<SprocketCombo> sprockets = Common.Data.GetSprocketCombos(m_Activity);

                foreach(SprocketCombo gear in sprockets)
                {
                    m_NamedZones.Add(new GearNamedZone(m_Activity, gear));
                }
            }

            TriggerNamedZonesListChanged();
        }

        protected void TriggerNamedZonesListChanged()
        {
            if (NamedZonedListChanged != null)
            {
                NamedZonedListChanged(this, new PropertyChangedEventArgs("NamedZones"));
            }
        }

        private IActivity m_Activity = null;
        private ActivityDataChangedHelper m_ActivityDataChangedHelper = new ActivityDataChangedHelper(null);
        private List<object> m_NamedZones = new List<object>();
    }
}
