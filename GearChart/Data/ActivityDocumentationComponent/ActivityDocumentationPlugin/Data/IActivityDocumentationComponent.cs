using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OldManBiking.SporttracksPlugins.Data;

namespace OldManBiking.ActivityDocumentationPlugin.Data
{

    /// <summary>
    /// A marker for components that need to be rendered as SPANs
    /// Let your component inherit from this interface, and the framework
    /// will render it as a SPAN.
    /// This was introduced later on as a separate marker interface in order to
    /// keep the original interface untouched
    /// </summary>
    public interface IActivityDocumentationComponentSpan
    {
    }

    /// <summary>
    /// Interface for classes that can be embedded as components in ActivityDocumentations,
    /// provided by the  Activity Documentation Plugin.
    /// 
    /// IActivityDocumentationComponents are created by implementations of the
    /// IActivityDocumentationComponentFactory interface
    /// </summary>
    public interface IActivityDocumentationComponent
    {
        /// <summary>
        /// A unique ID (Guid) for the type of component (same value as provided by its factory).
        /// </summary>
        String ReferenceId { get; }
        /// <summary>
        /// Returns true, when this component is enabled
        /// </summary>
        bool Enabled { get; }
        /// <summary>
        /// Returns an image representing this component in menues and/or the editor panel.
        /// </summary>
        Image Image { get; }
        /// <summary>
        /// Returns the (localized) display name for this component.
        /// May not be null.
        /// </summary>
        String DisplayName { get; }
        /// <summary>
        /// The group where this component belongs to. May be null for toplevel items.
        /// </summary>
        IGroup Group { get; }
        /// <summary>
        /// Returns the string that shall be shown as a tool tip for this component.
        /// May not be null.
        /// </summary>
        String ToolTip { get; }
        /// <summary>
        /// The representation of this component can depend upon parameters set by the user.
        /// Example: a user can chose a certain scaling for the X axis and Y axis of a line chart.
        /// This property is used to get these parameters after a call to Edit() - see below.
        /// This property is set by the plugin to set the parameters prior to calling GetHtml().
        /// May be null.
        /// </summary>
        String Parameters { get; set; }
        /// <summary>
        /// Does this component provide an implementation of Edit() that is not a noop
        /// If a component returns false, the plugin will not put an "Edit" item to the component menu
        /// </summary>
        bool SupportsEdit { get; }
        /// <summary>
        /// Let the user make his/her choice how to represent this component, e.g. let him/her scale the
        /// X axis and Y axis.
        /// This will be a no-op for components which have a representation that cannot be changed by the user.
        /// </summary>
        /// <returns>true, when the edit dialog was left with "OK", false otherwise</returns>
        bool Edit();
        /// <summary>
        /// Returns the HTML representation of this component
        /// </summary>
        /// <param name="images">out parameter which returns images that are part of the HTML representation.
        /// The HTML representation must contain positional parameters "{0}", "{1}", ... for every image
        /// in this list.
        /// May not be null (is an empty list, if no images exist)</param>
        /// <returns>A HTML representation of this component, null if the component has no representation
        /// for this activity and hence nothing should be added to the documentation</returns>
        String GetHtml(out IList<Image> images);
    }
}
