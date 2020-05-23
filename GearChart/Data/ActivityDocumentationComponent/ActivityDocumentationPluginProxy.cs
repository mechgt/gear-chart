using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using OldManBiking.ActivityDocumentationPlugin.Data;

namespace GearChart.Data.ActivityDocumentationComponent
{
    /// <summary>
    /// A class that is a proxy for the ActivityDocumentationComponent plugin
    /// Implements dynamic binding to the target assembly to avoid static dependencies and
    /// problems when the target assembly is not loaded
    /// </summary>
    public class ActivityDocumentationPluginProxy
    {
        private readonly String AssemblyName = "ActivityDocumentation Plugin";
        private readonly String ComponentRegistryClassName = "OldManBiking.ActivityDocumentationPlugin.Data.ActivityDocumentationComponentRegistry";

        /// <summary>
        /// Creates the proxy
        /// </summary>
        /// <exception cref="System.InvalidOperationException">throws an invalid operation exception,
        /// when the target plugin is not loaded</exception>
        public ActivityDocumentationPluginProxy()
        {
            // scan the loaded assemblies for the desired one
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                AssemblyName name = assembly.GetName();
                if (name.Name.Equals(AssemblyName) ||
                    name.FullName.Equals(AssemblyName))
                {
                    MyAssembly = assembly;
                    return;
                }
            }
            //throw new InvalidOperationException(AssemblyName + " is not loaded");
        }

        /// <summary>
        /// Register an activity documentation component component factory
        /// </summary>
        /// <param name="componentFactory">the activity documentation component factory</param>
        /// <exception cref="Exception">throws, when the target assembly is not loaded or the
        /// registry class or methods cannot be found</exception>
        public void Register(IActivityDocumentationComponentFactory componentFactory)
        {
            try
            {
                Type componentRegistryType = MyAssembly.GetType(ComponentRegistryClassName);
                PropertyInfo theInstanceProperty = componentRegistryType.GetProperty("TheInstance");
                MethodInfo registerMethod = componentRegistryType.GetMethod("Register", new Type[] { typeof(object) });
                object componentRegistry = theInstanceProperty.GetValue(null, null);
                registerMethod.Invoke(componentRegistry, new object[] { componentFactory });
            }
            catch (Exception e)
            {
                // If AD is not installed, this should not cause Gear Chart to error
                //throw new InvalidOperationException("Cannot register component", e);
            }
        }

        private Assembly MyAssembly;
    }
}
