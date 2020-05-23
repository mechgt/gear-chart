using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ZoneFiveSoftware.Common.Data;
using ZoneFiveSoftware.Common.Data.Fitness;
using System.Reflection;

namespace GearChart.Data.FilteredStatisticsPlugin
{
    class RangeInfoCacheWrapper
    {
        public RangeInfoCacheWrapper(object rangeInfo)
        {
            Debug.Assert(rangeInfo != null);

            m_RangeInfo = rangeInfo;

            Type infoType = m_RangeInfo.GetType();

            m_ActivityProperty = infoType.GetProperty("Activity", typeof(IActivity));
            m_ActivityInfoProperty = infoType.GetProperty("ActivityInfo", typeof(ActivityInfo));
            m_PauselessRangeProperty = infoType.GetProperty("PauselessRange", typeof(ValueRange<DateTime>));
            m_RangeWithPausesProperty = infoType.GetProperty("RangeWithPauses", typeof(ValueRange<DateTime>));
            m_ContainsStoredInfoMetod = infoType.GetMethod("ContainsStoredInfo", new Type[] { typeof(Guid) });
            m_SaveStoredInfoMetod = infoType.GetMethod("SaveStoredInfo", new Type[] { typeof(Guid), typeof(object) });
            m_RetrieveStoredInfoMetod = infoType.GetMethod("RetrieveStoredInfo", new Type[] { typeof(Guid) });
            m_ActivityPauselessCadenceTrackProperty = infoType.GetProperty("ActivityPauselessCadenceTrack", typeof(INumericTimeDataSeries));
            m_ActivityPauselessDistanceTrackProperty = infoType.GetProperty("ActivityPauselessDistanceTrack", typeof(INumericTimeDataSeries));
            m_ActivityPauselessElevationTrackProperty = infoType.GetProperty("ActivityPauselessElevationTrack", typeof(INumericTimeDataSeries));
            m_ActivityPauselessGradeTrackProperty = infoType.GetProperty("ActivityPauselessGradeTrack", typeof(INumericTimeDataSeries));
            m_ActivityPauselessHeartRateTrackProperty = infoType.GetProperty("ActivityPauselessHeartRateTrack", typeof(INumericTimeDataSeries));
            m_ActivityPauselessPowerTrackProperty = infoType.GetProperty("ActivityPauselessPowerTrack", typeof(INumericTimeDataSeries));
            m_ActivityPauselessSpeedTrackProperty = infoType.GetProperty("ActivityPauselessSpeedTrack", typeof(INumericTimeDataSeries));
            m_PauselessRangeCadenceTrackProperty = infoType.GetProperty("PauselessRangeCadenceTrack", typeof(INumericTimeDataSeries));
            m_PauselessRangeDistanceTrackProperty = infoType.GetProperty("PauselessRangeDistanceTrack", typeof(INumericTimeDataSeries));
            m_PauselessRangeElevationTrackProperty = infoType.GetProperty("PauselessRangeElevationTrack", typeof(INumericTimeDataSeries));
            m_PauselessRangeGradeTrackProperty = infoType.GetProperty("PauselessRangeGradeTrack", typeof(INumericTimeDataSeries));
            m_PauselessRangeHeartRateTrackProperty = infoType.GetProperty("PauselessRangeHeartRateTrack", typeof(INumericTimeDataSeries));
            m_PauselessRangePowerTrackProperty = infoType.GetProperty("PauselessRangePowerTrack", typeof(INumericTimeDataSeries));
            m_PauselessRangeSpeedTrackProperty = infoType.GetProperty("PauselessRangeSpeedTrack", typeof(INumericTimeDataSeries));
            m_RangeWithPausesCadenceTrackProperty = infoType.GetProperty("RangeWithPausesCadenceTrack", typeof(INumericTimeDataSeries));
            m_RangeWithPausesDistanceTrackProperty = infoType.GetProperty("RangeWithPausesDistanceTrack", typeof(INumericTimeDataSeries));
            m_RangeWithPausesElevationTrackProperty = infoType.GetProperty("RangeWithPausesElevationTrack", typeof(INumericTimeDataSeries));
            m_RangeWithPausesGradeTrackProperty = infoType.GetProperty("RangeWithPausesGradeTrack", typeof(INumericTimeDataSeries));
            m_RangeWithPausesHeartRateTrackProperty = infoType.GetProperty("RangeWithPausesHeartRateTrack", typeof(INumericTimeDataSeries));
            m_RangeWithPausesPowerTrackProperty = infoType.GetProperty("RangeWithPausesPowerTrack", typeof(INumericTimeDataSeries));
            m_RangeWithPausesSpeedTrackProperty = infoType.GetProperty("RangeWithPausesSpeedTrack", typeof(INumericTimeDataSeries));

            Debug.Assert(m_ActivityProperty != null && m_ActivityProperty.CanRead &&
                         m_ActivityInfoProperty != null && m_ActivityInfoProperty.CanRead &&
                         m_PauselessRangeProperty != null && m_PauselessRangeProperty.CanRead &&
                         m_RangeWithPausesProperty != null && m_RangeWithPausesProperty.CanRead &&
                         m_ContainsStoredInfoMetod != null && m_ContainsStoredInfoMetod.ReturnType == typeof(bool) &&
                         m_SaveStoredInfoMetod != null &&
                         m_RetrieveStoredInfoMetod != null && m_RetrieveStoredInfoMetod.ReturnType == typeof(object) &&
                         m_ActivityPauselessCadenceTrackProperty != null && m_ActivityPauselessCadenceTrackProperty.CanRead &&
                         m_ActivityPauselessDistanceTrackProperty != null && m_ActivityPauselessDistanceTrackProperty.CanRead &&
                         m_ActivityPauselessElevationTrackProperty != null && m_ActivityPauselessElevationTrackProperty.CanRead &&
                         m_ActivityPauselessGradeTrackProperty != null && m_ActivityPauselessGradeTrackProperty.CanRead &&
                         m_ActivityPauselessHeartRateTrackProperty != null && m_ActivityPauselessHeartRateTrackProperty.CanRead &&
                         m_ActivityPauselessPowerTrackProperty != null && m_ActivityPauselessPowerTrackProperty.CanRead &&
                         m_ActivityPauselessSpeedTrackProperty != null && m_ActivityPauselessSpeedTrackProperty.CanRead &&
                         m_PauselessRangeCadenceTrackProperty != null && m_PauselessRangeCadenceTrackProperty.CanRead &&
                         m_PauselessRangeDistanceTrackProperty != null && m_PauselessRangeDistanceTrackProperty.CanRead &&
                         m_PauselessRangeElevationTrackProperty != null && m_PauselessRangeElevationTrackProperty.CanRead &&
                         m_PauselessRangeGradeTrackProperty != null && m_PauselessRangeGradeTrackProperty.CanRead &&
                         m_PauselessRangeHeartRateTrackProperty != null && m_PauselessRangeHeartRateTrackProperty.CanRead &&
                         m_PauselessRangePowerTrackProperty != null && m_PauselessRangePowerTrackProperty.CanRead &&
                         m_PauselessRangeSpeedTrackProperty != null && m_PauselessRangeSpeedTrackProperty.CanRead &&
                         m_RangeWithPausesCadenceTrackProperty != null && m_RangeWithPausesCadenceTrackProperty.CanRead &&
                         m_RangeWithPausesDistanceTrackProperty != null && m_RangeWithPausesDistanceTrackProperty.CanRead &&
                         m_RangeWithPausesElevationTrackProperty != null && m_RangeWithPausesElevationTrackProperty.CanRead &&
                         m_RangeWithPausesGradeTrackProperty != null && m_RangeWithPausesGradeTrackProperty.CanRead &&
                         m_RangeWithPausesHeartRateTrackProperty != null && m_RangeWithPausesHeartRateTrackProperty.CanRead &&
                         m_RangeWithPausesPowerTrackProperty != null && m_RangeWithPausesPowerTrackProperty.CanRead &&
                         m_RangeWithPausesSpeedTrackProperty != null && m_RangeWithPausesSpeedTrackProperty.CanRead);
        }

        public IActivity Activity { get { return m_ActivityProperty.GetValue(m_RangeInfo, null) as IActivity; } }
        public ActivityInfo ActivityInfo { get { return m_ActivityInfoProperty.GetValue(m_RangeInfo, null) as ActivityInfo; } }

        public ValueRange<DateTime> PauselessRange { get { return m_PauselessRangeProperty.GetValue(m_RangeInfo, null) as ValueRange<DateTime>; } }
        public ValueRange<DateTime> RangeWithPauses { get { return m_RangeWithPausesProperty.GetValue(m_RangeInfo, null) as ValueRange<DateTime>; } }

        public bool ContainsStoredInfo(Guid infoId) { return (bool)m_ContainsStoredInfoMetod.Invoke(m_RangeInfo, new object[] { infoId }); }
        public void SaveStoredInfo(Guid infoId, object data) { m_SaveStoredInfoMetod.Invoke(m_RangeInfo, new object[] { infoId, data }); }
        public object RetrieveStoredInfo(Guid infoId) { return m_RetrieveStoredInfoMetod.Invoke(m_RangeInfo, new object[] { infoId }); }

        public INumericTimeDataSeries ActivityPauselessCadenceTrack { get { return m_ActivityPauselessCadenceTrackProperty.GetValue(m_RangeInfo, null) as INumericTimeDataSeries; } }
        public INumericTimeDataSeries ActivityPauselessDistanceTrack { get { return m_ActivityPauselessDistanceTrackProperty.GetValue(m_RangeInfo, null) as INumericTimeDataSeries; } }
        public INumericTimeDataSeries ActivityPauselessElevationTrack { get { return m_ActivityPauselessElevationTrackProperty.GetValue(m_RangeInfo, null) as INumericTimeDataSeries; } }
        public INumericTimeDataSeries ActivityPauselessGradeTrack { get { return m_ActivityPauselessGradeTrackProperty.GetValue(m_RangeInfo, null) as INumericTimeDataSeries; } }
        public INumericTimeDataSeries ActivityPauselessHeartRateTrack { get { return m_ActivityPauselessHeartRateTrackProperty.GetValue(m_RangeInfo, null) as INumericTimeDataSeries; } }
        public INumericTimeDataSeries ActivityPauselessPowerTrack { get { return m_ActivityPauselessPowerTrackProperty.GetValue(m_RangeInfo, null) as INumericTimeDataSeries; } }
        public INumericTimeDataSeries ActivityPauselessSpeedTrack { get { return m_ActivityPauselessSpeedTrackProperty.GetValue(m_RangeInfo, null) as INumericTimeDataSeries; } }

        public INumericTimeDataSeries PauselessRangeCadenceTrack { get { return m_PauselessRangeCadenceTrackProperty.GetValue(m_RangeInfo, null) as INumericTimeDataSeries; } }
        public INumericTimeDataSeries PauselessRangeDistanceTrack { get { return m_PauselessRangeDistanceTrackProperty.GetValue(m_RangeInfo, null) as INumericTimeDataSeries; } }
        public INumericTimeDataSeries PauselessRangeElevationTrack { get { return m_PauselessRangeElevationTrackProperty.GetValue(m_RangeInfo, null) as INumericTimeDataSeries; } }
        public INumericTimeDataSeries PauselessRangeGradeTrack { get { return m_PauselessRangeGradeTrackProperty.GetValue(m_RangeInfo, null) as INumericTimeDataSeries; } }
        public INumericTimeDataSeries PauselessRangeHeartRateTrack { get { return m_PauselessRangeHeartRateTrackProperty.GetValue(m_RangeInfo, null) as INumericTimeDataSeries; } }
        public INumericTimeDataSeries PauselessRangePowerTrack { get { return m_PauselessRangePowerTrackProperty.GetValue(m_RangeInfo, null) as INumericTimeDataSeries; } }
        public INumericTimeDataSeries PauselessRangeSpeedTrack { get { return m_PauselessRangeSpeedTrackProperty.GetValue(m_RangeInfo, null) as INumericTimeDataSeries; } }

        public INumericTimeDataSeries RangeWithPausesCadenceTrack { get { return m_RangeWithPausesCadenceTrackProperty.GetValue(m_RangeInfo, null) as INumericTimeDataSeries; } }
        public INumericTimeDataSeries RangeWithPausesDistanceTrack { get { return m_RangeWithPausesDistanceTrackProperty.GetValue(m_RangeInfo, null) as INumericTimeDataSeries; } }
        public INumericTimeDataSeries RangeWithPauseseGradeTrack { get { return m_RangeWithPausesGradeTrackProperty.GetValue(m_RangeInfo, null) as INumericTimeDataSeries; } }
        public INumericTimeDataSeries RangeWithPausesElevationTrack { get { return m_RangeWithPausesElevationTrackProperty.GetValue(m_RangeInfo, null) as INumericTimeDataSeries; } }
        public INumericTimeDataSeries RangeWithPausesHeartRateTrack { get { return m_RangeWithPausesHeartRateTrackProperty.GetValue(m_RangeInfo, null) as INumericTimeDataSeries; } }
        public INumericTimeDataSeries RangeWithPausesPowerTrack { get { return m_RangeWithPausesPowerTrackProperty.GetValue(m_RangeInfo, null) as INumericTimeDataSeries; } }
        public INumericTimeDataSeries RangeWithPausesSpeedTrack { get { return m_RangeWithPausesSpeedTrackProperty.GetValue(m_RangeInfo, null) as INumericTimeDataSeries; } }

        private object m_RangeInfo = null;
        private MethodInfo m_ContainsStoredInfoMetod = null;
        private MethodInfo m_SaveStoredInfoMetod = null;
        private MethodInfo m_RetrieveStoredInfoMetod = null;
        private PropertyInfo m_ActivityProperty = null;
        private PropertyInfo m_ActivityInfoProperty = null;
        private PropertyInfo m_PauselessRangeProperty = null;
        private PropertyInfo m_RangeWithPausesProperty = null;
        private PropertyInfo m_ActivityPauselessCadenceTrackProperty = null;
        private PropertyInfo m_ActivityPauselessDistanceTrackProperty = null;
        private PropertyInfo m_ActivityPauselessElevationTrackProperty = null;
        private PropertyInfo m_ActivityPauselessGradeTrackProperty = null;
        private PropertyInfo m_ActivityPauselessHeartRateTrackProperty = null;
        private PropertyInfo m_ActivityPauselessPowerTrackProperty = null;
        private PropertyInfo m_ActivityPauselessSpeedTrackProperty = null;
        private PropertyInfo m_PauselessRangeCadenceTrackProperty = null;
        private PropertyInfo m_PauselessRangeDistanceTrackProperty = null;
        private PropertyInfo m_PauselessRangeElevationTrackProperty = null;
        private PropertyInfo m_PauselessRangeGradeTrackProperty = null;
        private PropertyInfo m_PauselessRangeHeartRateTrackProperty = null;
        private PropertyInfo m_PauselessRangePowerTrackProperty = null;
        private PropertyInfo m_PauselessRangeSpeedTrackProperty = null;
        private PropertyInfo m_RangeWithPausesCadenceTrackProperty = null;
        private PropertyInfo m_RangeWithPausesDistanceTrackProperty = null;
        private PropertyInfo m_RangeWithPausesElevationTrackProperty = null;
        private PropertyInfo m_RangeWithPausesGradeTrackProperty = null;
        private PropertyInfo m_RangeWithPausesHeartRateTrackProperty = null;
        private PropertyInfo m_RangeWithPausesPowerTrackProperty = null;
        private PropertyInfo m_RangeWithPausesSpeedTrackProperty = null;

    }
}
