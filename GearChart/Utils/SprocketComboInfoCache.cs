using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using ZoneFiveSoftware.Common.Data;
using ZoneFiveSoftware.Common.Data.Fitness;
using GearChart.Common;

namespace GearChart.Utils
{
    class SprocketComboInfoCache
    {
        private class SprocketComboInfoCacheItem
        {
            public SprocketComboInfoCacheItem(IActivity activity, IList<SprocketComboInfo> info)
            {
                m_Activity = activity;
                m_ComboInfo = info;
                m_Dirty = false;
            }

            public IActivity m_Activity;
            public IList<SprocketComboInfo> m_ComboInfo;
            public bool m_Dirty;
        }

        private SprocketComboInfoCache()
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
                foreach (SprocketComboInfoCacheItem cachedItem in m_InfoCache.Values)
                {
                    cachedItem.m_Dirty = true;
                }
            }
        }

        void OnActivityCategoryChanged(object sender, IActivityCategory category)
        {
            foreach (SprocketComboInfoCacheItem cachedItem in m_InfoCache.Values)
            {
                if (Utils.IsInActivityCategoryHierarchy(cachedItem.m_Activity, category))
                {
                    // We might have changed our activity category hierarchy, stopped meters
                    //  per second, use parent settings, etc. so refresh the cached item
                    cachedItem.m_Dirty = true;
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
                m_InfoCache[activity].m_Dirty = true;
            }
        }

        void OnBikeSetupChanged(object sender, string setupId)
        {
            foreach (SprocketComboInfoCacheItem cachedItem in m_InfoCache.Values)
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
                    cachedItem.m_Dirty = true;
                }
            }
        }

        private void UpdateCachedInfo(IActivity activity, IList<SprocketComboInfo> sprocketInfo)
        {
            SprocketComboInfoCacheItem newCacheItem = new SprocketComboInfoCacheItem(activity, sprocketInfo);

            if (!m_InfoCache.ContainsKey(activity))
            {
                m_InfoCache.Add(activity, null);

                activity.PropertyChanged += new PropertyChangedEventHandler(OnActivityDataChanged);
            }

            m_InfoCache[activity] = newCacheItem;
        }

        public IList<SprocketComboInfo> CalculateSprocketComboInfo(IActivity activity)
        {
            if (activity != null)
            {
                bool retrieveInfo = true;
                SprocketComboInfoCacheItem cachedItem = null;

                if (m_InfoCache.ContainsKey(activity) && !m_InfoCache[activity].m_Dirty)
                {
                    retrieveInfo = false;
                }

                if (retrieveInfo)
                {
                    ITimeDataSeries<SprocketCombo> sprocketTrack = ActivityGearTrackCache.Instance.CalculateSprocketTrack(activity);
                    IList<SprocketComboInfo> sprocketInfo = Common.Data.Calculate(activity, sprocketTrack);

                    UpdateCachedInfo(activity, sprocketInfo);
                }

                cachedItem = m_InfoCache[activity];

                return cachedItem.m_ComboInfo;
            }

            return null;
        }

        public void Flush(IActivity activity)
        {
            if (activity != null)
            {
                if (m_InfoCache.ContainsKey(activity))
                {
                    m_InfoCache[activity].m_Dirty = true;
                }
            }
        }

        public static SprocketComboInfoCache Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new SprocketComboInfoCache();
                }

                return m_Instance;
            }
        }

        private static SprocketComboInfoCache m_Instance = null;
        private Dictionary<IActivity, SprocketComboInfoCacheItem> m_InfoCache = new Dictionary<IActivity, SprocketComboInfoCacheItem>();
    }
}
