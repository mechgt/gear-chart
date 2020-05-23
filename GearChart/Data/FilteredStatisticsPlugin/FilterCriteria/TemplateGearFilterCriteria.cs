using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using ZoneFiveSoftware.Common.Data;
using ZoneFiveSoftware.Common.Data.Fitness;
using GearChart.Common;
using GearChart.Resources;
using GearChart.Utils;

namespace GearChart.Data.FilteredStatisticsPlugin
{
    class TemplateGearFilterCriteria
    {
        public TemplateGearFilterCriteria(IActivity activity, string equipmentId)
        {
            Activity = activity;
            m_EquipmentId = equipmentId;

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
            if (m_EquipmentId.Equals(setupId))
            {
                BuildNamedZones();
            }
        }

#region IFilterCriteria members

        public Guid ReferenceId
        {
            get { return new Guid("8b2b2575-3535-4d97-b5c7-cf136e9d88dd"); }
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
                return true;
            }
        }

        public bool IsMainViewOnly
        {
            get
            {
                return false;
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
            get { return true; }
        }

        public void SerializeCriteria(Stream stream)
        {
            stream.Write(BitConverter.GetBytes(Encoding.UTF8.GetByteCount(m_EquipmentId)), 0, sizeof(Int32));
            stream.Write(Encoding.UTF8.GetBytes(m_EquipmentId), 0, Encoding.UTF8.GetByteCount(m_EquipmentId));
        }

        public UInt16 DataVersion
        {
            get { return 1; }
        }

        public object DeserializeCriteria(Stream stream, UInt16 version)
        {
            return null;
        }

        public bool ValidateConsistency()
        {
            IList<string> equipmentIds = Common.Data.GetEquipmentIds();

            return equipmentIds.IndexOf(EquipmentId) != -1;
        }

        public event PropertyChangedEventHandler NamedZonedListChanged;

#endregion

        private void BuildNamedZones()
        {
            m_NamedZones.Clear();

            List<SprocketCombo> sprockets = Common.Data.GetSprocketCombos(m_EquipmentId);

            foreach(SprocketCombo gear in sprockets)
            {
                m_NamedZones.Add(new GearNamedZone(m_Activity, gear));
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

        public string EquipmentId
        {
            get { return m_EquipmentId; }
        }

        private IActivity m_Activity = null;
        private ActivityDataChangedHelper m_ActivityDataChangedHelper = new ActivityDataChangedHelper(null);
        private string m_EquipmentId = null;
        private List<object> m_NamedZones = new List<object>();
    }
}
