using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace OldManBiking.SporttracksPlugins.Data
{
    /// <summary>
    /// A group for activity attributes. Used to group attributes in listings like menues
    /// </summary>
    public interface IGroup : IComparable<IGroup>
    {
        /// <summary>
        /// Get the unique ID of this group
        /// </summary>
        String ReferenceId { get; }
        /// <summary>
        /// Get the (localized) name of this group, e.g. "Weather"
        /// </summary>
        String Name { get; }
        /// <summary>
        /// Get the image associated with this group
        /// e.g. a "weather" symbol
        /// image size must be 16x16 px
        /// may be null, if there is no image
        /// </summary>
        Image Image { get; }
    }
}
