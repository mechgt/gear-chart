using System;
using System.Collections.Generic;
using System.Text;
using ZoneFiveSoftware.Common.Data.Fitness;

namespace OldManBiking.ActivityDocumentationPlugin.Data
{
    /// <summary>
    /// Interface of a factory that creates IActivityDocumentationComponent objects.
    /// The factory will create objects of exactly one type.
    /// </summary>
    public interface IActivityDocumentationComponentFactory
    {
        /// <summary>
        /// A unique ID (Guid) for the type of components created by this factory.
        /// </summary>
        String ReferenceId { get; }
        /// <summary>
        /// Create an IActivityDocumentationComponent for the given activity
        /// </summary>
        /// <param name="activity">an activity</param>
        /// <returns>an IActivityDocumentationComponent for this activity</returns>
        IActivityDocumentationComponent CreateComponent(IActivity activity);
    }
}
