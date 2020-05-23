using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using GearChart.Common;
using ZoneFiveSoftware.Common.Data.Fitness;
using ZoneFiveSoftware.Common.Data;
using GearChart.Utils;

namespace GearChart.Data.FilteredStatisticsPlugin
{
    class GearNamedZone
    {
        public GearNamedZone(IActivity activity, SprocketCombo gearCombo)
        {
            m_ActivityDataChangedHelper.PropertyChanged += new PropertyChangedEventHandler(OnActivityDataChanged);

            Activity = activity;
            m_Gear = gearCombo;

            m_ValidTimesDirty = true;
            TriggerValidTimesChanged();
        }

        void OnActivityDataChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Activity.EquipmentUsed" ||
                e.PropertyName == "Activity.GPSRoute" ||
                e.PropertyName == "Activity.DistanceMetersTrack" ||
                e.PropertyName == "Activity.CadencePerMinuteTrack" ||
                e.PropertyName == "Activity.Length")
            {
                m_ValidTimesDirty = true;
                TriggerValidTimesChanged();
            }
        }

#region INamedZone members

        public String Name
        {
            get { return String.Format("{0:0}x{1:0} ({2:0.00})", m_Gear.ChainringSize, m_Gear.CassetteSize, m_Gear.GearRatio); }
        }

        public IValueRangeSeries<DateTime> ValidTimes
        {
            get
            {
                RefreshValidTimes();

                return m_ValidTimes;
            }
        }

        public event PropertyChangedEventHandler ValidTimesChanged;

#endregion

        protected void TriggerValidTimesChanged()
        {
            if (ValidTimesChanged != null)
            {
                ValidTimesChanged(this, new PropertyChangedEventArgs("ValidTimes"));
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is GearNamedZone)
            {
                GearNamedZone namedZone = obj as GearNamedZone;

                return namedZone.m_Gear.Equals(m_Gear);
            }

            return base.Equals(obj);
        }

        private void RefreshValidTimes()
        {
            if (m_ValidTimesDirty)
            {
                m_ValidTimesDirty = false;

                m_ValidTimes.Clear();

                if (Activity != null)
                {
                    IList<SprocketComboInfo> sprocketZoneInfo = SprocketComboInfoCache.Instance.CalculateSprocketComboInfo(Activity);

                    foreach (SprocketComboInfo comboInfo in sprocketZoneInfo)
                    {
                        if (comboInfo.SprocketCombo == m_Gear)
                        {
                            foreach(ValueRange<DateTime> range in comboInfo.Times)
                            {
                                m_ValidTimes.Add(range);
                            }
                            break;
                        }
                    }
                }
            }
        }

        private IActivity Activity
        {
            get { return m_Activity; }
            set
            {
                if (Activity != value)
                {
                    m_Activity = value;
                    m_ActivityDataChangedHelper.Activity = Activity;

                    m_ValidTimesDirty = true;
                    TriggerValidTimesChanged();
                }
            }
        }

        public SprocketCombo SprocketCombo
        {
            get { return m_Gear; }
        }

        private IActivity m_Activity = null;
        private ActivityDataChangedHelper m_ActivityDataChangedHelper = new ActivityDataChangedHelper(null);
        private SprocketCombo m_Gear;
        private ValueRangeSeries<DateTime> m_ValidTimes = new ValueRangeSeries<DateTime>();
        private bool m_ValidTimesDirty = false;
    }
}
