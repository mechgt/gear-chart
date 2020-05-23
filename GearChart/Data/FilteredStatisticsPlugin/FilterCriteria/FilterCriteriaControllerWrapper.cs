using System;
using System.IO;
using System.Reflection;

namespace GearChart.Data.FilteredStatisticsPlugin
{
    class FilterCriteriaControllerWrapper
    {
        private FilterCriteriaControllerWrapper()
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
                    Type filterCriteriaControllerType = loadedAssembly.GetType("FilteredStatistics.Common.Controller.FilterCriteriaController");

                    if (filterCriteriaControllerType != null)
                    {
                        PropertyInfo instanceProperty = filterCriteriaControllerType.GetProperty("Instance");

                        if (instanceProperty != null &&
                            instanceProperty.CanRead)
                        {
                            MethodInfo registerMethod = filterCriteriaControllerType.GetMethod("RegisterFilterCriteriaProvider", new Type[] { typeof(object) });

                            m_ControllerInstance = instanceProperty.GetValue(null, null);

                            if (registerMethod != null)
                            {
                                m_RegisterMethod = registerMethod;
                            }
                        }
                    }
                }
            }
        }

        public static FilterCriteriaControllerWrapper Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new FilterCriteriaControllerWrapper();
                    m_Instance.Initialize();
                }

                return m_Instance;
            }
        }

        public void RegisterFilterCriteriaProvider(object provider)
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

        private static FilterCriteriaControllerWrapper m_Instance = null;
        private object m_ControllerInstance = null;
        private bool m_PluginInstalled = false;
        private MethodInfo m_RegisterMethod = null;
    }
}
