using System;
using System.Collections.Generic;
using System.Text;
using OldManBiking.ActivityDocumentationPlugin.Data;
using ZoneFiveSoftware.Common.Data.Fitness;

namespace GearChart.Data.ActivityDocumentationComponent
{
    /// <summary>
    /// A class that can create components of a certain type.
    /// Must be registered with the ActivityDocumentation plugin
    /// </summary>
    class ADComponentFactory : IActivityDocumentationComponentFactory
    {
        #region IActivityDocumentationComponentFactory Member

        public IActivityDocumentationComponent CreateComponent(IActivity activity)
        {
            return new GearComponent(activity);
        }

        public string ReferenceId
        {
            // Important: this must be the component's reference ID!
            get { return GearComponent.RefId; }
        }

        #endregion
    }
}
