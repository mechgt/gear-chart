using System;
using System.IO;
using System.Reflection;

namespace GearChart.Data.FilteredStatisticsPlugin
{
    class ActivityDataTrackControllerWrapper
    {
        private ActivityDataTrackControllerWrapper()
        {
        }

        private void Initialize()
        {
            try
            {
                DetectMethodsAndClasses();
                m_PluginInstalled = true;
            }
            catch (Exception e)
            {
                m_PluginInstalled = false;
                throw e;
            }
        }

        private void DetectMethodsAndClasses()
        {
            foreach (Assembly loadedAssembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (loadedAssembly.FullName.StartsWith("FilteredStatistics"))
                {
                    Type activityDataTrackControllerType = loadedAssembly.GetType("FilteredStatistics.Common.Controller.ActivityDataTrackController");

                    if (activityDataTrackControllerType != null)
                    {
                        PropertyInfo instanceProperty = activityDataTrackControllerType.GetProperty("Instance");

                        if (instanceProperty != null)
                        {
                            MethodInfo registerMethod = activityDataTrackControllerType.GetMethod("RegisterDataTrackProvider", new Type[] { typeof(object) });

                            m_ControllerInstance = instanceProperty.GetGetMethod().Invoke(null, null);

                            if (registerMethod != null)
                            {
                                m_RegisterMethod = registerMethod;
                            }
                        }
                    }
                }
            }
        }

        public static ActivityDataTrackControllerWrapper Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new ActivityDataTrackControllerWrapper();
                    m_Instance.Initialize();
                }

                return m_Instance;
            }
        }

        public void RegisterDataTrackProvider(object provider)
        {
            if (IsPluginInstalled && RegisterMethodAvailable)
            {
                m_RegisterMethod.Invoke(m_ControllerInstance, new object[] { provider });
            }
        }

        public bool IsPluginInstalled
        {
            get { return m_PluginInstalled; }
        }

        public bool RegisterMethodAvailable
        {
            get { return m_RegisterMethod != null; }
        }

        private static ActivityDataTrackControllerWrapper m_Instance = null;
        private object m_ControllerInstance = null;
        private bool m_PluginInstalled = false;
        private MethodInfo m_RegisterMethod = null;
    }
}
