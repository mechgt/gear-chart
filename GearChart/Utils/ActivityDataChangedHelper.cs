using System;
using System.ComponentModel;
using ZoneFiveSoftware.Common.Data;
using ZoneFiveSoftware.Common.Data.Fitness;

namespace GearChart.Utils
{
    public class ActivityDataChangedHelper : INotifyPropertyChanged
    {
        public ActivityDataChangedHelper(IActivity activity)
        {
            Activity = activity;
            m_CurrentLogbook = PluginMain.GetApplication().Logbook;

            PluginMain.GetApplication().SystemPreferences.PropertyChanged += new PropertyChangedEventHandler(OnSystemPreferencesPropertyChanged);
            PluginMain.GetApplication().PropertyChanged += new PropertyChangedEventHandler(AppPropertyChanged);
            GearChart.Common.Data.BikeSetupChanged += new GearChart.Common.Data.BikeSetupChangedEventHandler(OnGearChartBikeSetupChanged);
            RegisterCategoryCallback(PluginMain.GetApplication().Logbook);
        }

        private void AppPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e != null && e.PropertyName == "Logbook")
            {
                UnregisterCategoryCallback(m_CurrentLogbook);

                m_CurrentLogbook = PluginMain.GetApplication().Logbook;

                RegisterCategoryCallback(m_CurrentLogbook);
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private void OnSystemPreferencesPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (Activity != null)
            {
                if (e.PropertyName == "DistanceUnits" ||
                    e.PropertyName == "ElevationUnits")
                {
                    if (m_Activity.Category.UseSystemLengthUnits)
                    {
                        TriggerPropertyChangedEvent(PluginMain.GetApplication().SystemPreferences, e.PropertyName);
                    }
                }
                else
                {
                    TriggerPropertyChangedEvent(sender, e.PropertyName);
                }
            }
        }

        private void OnActivityDataChanged(object sender, PropertyChangedEventArgs e)
        {
            if (Activity != null)
            {
                ActivityInfoCache.Instance.ClearInfo(m_Activity);

                if (e.PropertyName == "LapInfo")
                {
                    TriggerPropertyChangedEvent(m_Activity, "LapInfo." + e.PropertyName);
                }
                else
                {
                    TriggerPropertyChangedEvent(m_Activity, "Activity." + e.PropertyName);
                }
            }
        }

        private void OnActivityCategoryDataChanged(object sender, PropertyChangedEventArgs e)
        {
            if (Activity != null)
            {
                IActivityCategory modifiedCategory = sender as IActivityCategory;

                if (Utils.IsInActivityCategoryHierarchy(m_Activity, modifiedCategory))
                {
                    ActivityInfoCache.Instance.ClearInfo(m_Activity);

                    if (e.PropertyName != "StoppedMetersPerSecond" ||
                        !PluginMain.GetApplication().SystemPreferences.AnalysisSettings.IncludeStopped)
                    {
                        TriggerPropertyChangedEvent(m_Activity.Category, "ActivityCategory." + e.PropertyName);
                    }
                }

                if (e.PropertyName == "SubCategories")
                {
                    UnregisterCategoryCallback(PluginMain.GetApplication().Logbook);
                    RegisterCategoryCallback(PluginMain.GetApplication().Logbook);
                }

            }
        }

        private void OnGearChartBikeSetupChanged(object sender, string setupId)
        {
            if (Activity != null)
            {
                foreach (IEquipmentItem equipment in m_Activity.EquipmentUsed)
                {
                    if (equipment.ReferenceId.Equals(setupId))
                    {
                        TriggerPropertyChangedEvent(setupId, "BikeSetup");
                        break;
                    }
                }
            }
        }

        private void RegisterCategoryCallback(ILogbook logbook)
        {
            if (logbook != null)
            {
                foreach (IActivityCategory category in logbook.ActivityCategories)
                {
                    category.PropertyChanged += new PropertyChangedEventHandler(OnActivityCategoryDataChanged);
                }
            }
        }

        private void UnregisterCategoryCallback(ILogbook logbook)
        {
            if (logbook != null)
            {
                foreach (IActivityCategory category in logbook.ActivityCategories)
                {
                    category.PropertyChanged -= new PropertyChangedEventHandler(OnActivityCategoryDataChanged);
                }
            }
        }

        private void TriggerPropertyChangedEvent(object modifiedObject, string propertyName)
        {
            if (Activity != null && PropertyChanged != null)
            {
                PropertyChanged(modifiedObject, new PropertyChangedEventArgs(propertyName));

                // These criterias occur very often, so create a special event for them.
                //  Basically, they all change the activity's duration.
                if (propertyName == "AnalysisSettings.IncludePaused" ||
                    propertyName == "AnalysisSettings.IncludeStopped" ||
                    propertyName == "Activity.GPSRoute" ||
                    propertyName == "Activity.DistanceMetersTrack" ||
                    propertyName == "Activity.Category" ||
                    propertyName == "ActivityCategory.StoppedMetersPerSecond")
                {
                    PropertyChanged(Activity, new PropertyChangedEventArgs("Activity.Length"));
                }
            }
        }

        public static bool IsInActivityCategoryHierarchy(IActivity activity, IActivityCategory category)
        {
            IActivityCategory currentCategory = activity.Category;

            do
            {
                if (category == currentCategory)
                {
                    return true;
                }
                else if (!currentCategory.UseParentSettings)
                {
                    return false;
                }

                currentCategory = currentCategory.Parent;
            }
            while (currentCategory != null);

            return false;
        }


        public IActivity Activity
        {
            get { return m_Activity; }
            set
            {
                if (Activity != value)
                {
                    if (Activity != null)
                    {
                        Activity.PropertyChanged -= new PropertyChangedEventHandler(OnActivityDataChanged);
                    }

                    m_Activity = value;

                    if (Activity != null)
                    {
                        ActivityInfoCache.Instance.ClearInfo(m_Activity);
                        Activity.PropertyChanged += new PropertyChangedEventHandler(OnActivityDataChanged);
                    }
                }
            }
        }

        private IActivity m_Activity = null;
        private ILogbook m_CurrentLogbook = null;
    }
}
