using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using ZoneFiveSoftware.Common.Data;
using ZoneFiveSoftware.Common.Data.Fitness;
using GearChart.Common;

namespace GearChart.Utils
{
    class ActivityGearTrackCache
    {
        private class ActivityGearTrackCacheItem
        {
            public ActivityGearTrackCacheItem(IActivity activity)
            {
                m_Activity = activity;
            }

            public IActivity m_Activity;
            public INumericTimeDataSeries m_RawTrack;
            public INumericTimeDataSeries m_GearTrack;
            public ITimeDataSeries<SprocketCombo> m_SprocketTrack;
        }

        private ActivityGearTrackCache()
        {
            PluginMain.ActivityCategoryChanged += new PluginMain.ActivityCategoryChangedEventHandler(OnActivityCategoryChanged);
            PluginMain.GetApplication().SystemPreferences.PropertyChanged += new PropertyChangedEventHandler(OnSystemPreferencesPropertyChanged);
            Common.Data.BikeSetupChanged += new Common.Data.BikeSetupChangedEventHandler(OnBikeSetupChanged);
        }

        void OnSystemPreferencesPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "AnalysisSettings.IncludePaused" ||
                e.PropertyName == "AnalysisSettings.IncludeStopped")
            {
                foreach (ActivityGearTrackCacheItem cachedItem in m_InfoCache.Values)
                {
                    cachedItem.m_RawTrack = null;
                    cachedItem.m_GearTrack = null;
                    cachedItem.m_SprocketTrack = null;
                }
            }
        }

        void OnActivityCategoryChanged(object sender, IActivityCategory category)
        {
            foreach (ActivityGearTrackCacheItem cachedItem in m_InfoCache.Values)
            {
                if (Utils.IsInActivityCategoryHierarchy(cachedItem.m_Activity, category))
                {
                    // We might have changed our activity category hierarchy, stopped meters
                    //  per second, use parent settings, etc. so refresh the cached item
                    cachedItem.m_RawTrack = null;
                    cachedItem.m_GearTrack = null;
                    cachedItem.m_SprocketTrack = null;
                }
            }
        }

        void OnActivityDataChanged(object sender, PropertyChangedEventArgs e)
        {
            // TODO: Test this in ST3
            IActivity activity = sender as IActivity;

            if (e.PropertyName.Contains("EquipmentUsed") ||
                 e.PropertyName.Contains("GPSRoute") ||
                 e.PropertyName.Contains("DistanceMetersTrack") ||
                 e.PropertyName.Contains("CadencePerMinuteTrack") ||
                 e.PropertyName.Contains("Category"))
            {
                m_InfoCache[activity].m_RawTrack = null;
                m_InfoCache[activity].m_GearTrack = null;
                m_InfoCache[activity].m_SprocketTrack = null;
            }
        }

        void OnBikeSetupChanged(object sender, string setupId)
        {
            foreach (ActivityGearTrackCacheItem cachedItem in m_InfoCache.Values)
            {
                bool equipmentUsed = false;

                foreach (IEquipmentItem equipment in cachedItem.m_Activity.EquipmentUsed)
                {
                    if (equipment.ReferenceId.Equals(setupId))
                    {
                        equipmentUsed = true;
                        break;
                    }
                }

                if (equipmentUsed)
                {
                    cachedItem.m_RawTrack = null;
                    cachedItem.m_GearTrack = null;
                    cachedItem.m_SprocketTrack = null;
                }
            }
        }

        private void UpdateCachedRawTrack(IActivity activity, INumericTimeDataSeries rawTrack)
        {
            ActivityGearTrackCacheItem newCacheItem = new ActivityGearTrackCacheItem(activity);

            if (!m_InfoCache.ContainsKey(activity))
            {
                m_InfoCache.Add(activity, null);

                activity.PropertyChanged += new PropertyChangedEventHandler(OnActivityDataChanged);
            }

            m_InfoCache[activity] = newCacheItem;
            m_InfoCache[activity].m_RawTrack = rawTrack;
        }

        private void UpdateCachedGearTrack(IActivity activity, INumericTimeDataSeries gearTrack)
        {
            ActivityGearTrackCacheItem newCacheItem = new ActivityGearTrackCacheItem(activity);

            if (!m_InfoCache.ContainsKey(activity))
            {
                m_InfoCache.Add(activity, null);

                activity.PropertyChanged += new PropertyChangedEventHandler(OnActivityDataChanged);
            }

            m_InfoCache[activity] = newCacheItem;
            m_InfoCache[activity].m_GearTrack = gearTrack;
        }

        private void UpdateCachedSprocketTrack(IActivity activity, ITimeDataSeries<SprocketCombo> sprocketTrack)
        {
            ActivityGearTrackCacheItem newCacheItem = new ActivityGearTrackCacheItem(activity);

            if (!m_InfoCache.ContainsKey(activity))
            {
                m_InfoCache.Add(activity, null);

                activity.PropertyChanged += new PropertyChangedEventHandler(OnActivityDataChanged);
            }

            m_InfoCache[activity] = newCacheItem;
            m_InfoCache[activity].m_SprocketTrack = sprocketTrack;
        }

        public INumericTimeDataSeries CalculateRawTrack(IActivity activity)
        {
            if (activity != null)
            {
                bool retrieveInfo = true;
                ActivityGearTrackCacheItem cachedItem = null;

                if (m_InfoCache.ContainsKey(activity) &&
                    m_InfoCache[activity].m_RawTrack != null)
                {
                    retrieveInfo = false;
                }

                if (retrieveInfo)
                {
                    INumericTimeDataSeries rawTrack = Common.Data.GetRawGearTrack(activity);

                    UpdateCachedRawTrack(activity, rawTrack);
                }

                cachedItem = m_InfoCache[activity];

                return cachedItem.m_RawTrack;
            }

            return null;
        }

        public INumericTimeDataSeries CalculateGearTrack(IActivity activity)
        {
            if (activity != null)
            {
                bool retrieveInfo = true;
                ActivityGearTrackCacheItem cachedItem = null;

                if (m_InfoCache.ContainsKey(activity) &&
                    m_InfoCache[activity].m_GearTrack != null)
                {
                    retrieveInfo = false;
                }

                if (retrieveInfo)
                {
                    INumericTimeDataSeries gearTrack = Common.Data.GetGearTrack(activity);

                    UpdateCachedGearTrack(activity, gearTrack);
                }

                cachedItem = m_InfoCache[activity];

                return cachedItem.m_GearTrack;
            }

            return null;
        }

        public ITimeDataSeries<SprocketCombo> CalculateSprocketTrack(IActivity activity)
        {
            if (activity != null)
            {
                bool retrieveInfo = true;
                ActivityGearTrackCacheItem cachedItem = null;

                if (m_InfoCache.ContainsKey(activity) &&
                    m_InfoCache[activity].m_SprocketTrack != null)
                {
                    retrieveInfo = false;
                }

                if (retrieveInfo)
                {
                    ITimeDataSeries<SprocketCombo> sprocketTrack = Common.Data.GetSprocketTrack(activity);

                    UpdateCachedSprocketTrack(activity, sprocketTrack);
                }

                cachedItem = m_InfoCache[activity];

                return cachedItem.m_SprocketTrack;
            }

            return null;
        }

        public void Flush(IActivity activity)
        {
            if (activity != null)
            {
                if (m_InfoCache.ContainsKey(activity))
                {
                    m_InfoCache[activity].m_RawTrack = null;
                    m_InfoCache[activity].m_GearTrack = null;
                    m_InfoCache[activity].m_SprocketTrack = null;
                }
            }
        }

        public static ActivityGearTrackCache Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new ActivityGearTrackCache();
                }

                return m_Instance;
            }
        }

        private static ActivityGearTrackCache m_Instance = null;
        private Dictionary<IActivity, ActivityGearTrackCacheItem> m_InfoCache = new Dictionary<IActivity, ActivityGearTrackCacheItem>();
    }
}
