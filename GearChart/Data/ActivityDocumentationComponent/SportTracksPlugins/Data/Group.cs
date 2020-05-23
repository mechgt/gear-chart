using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace OldManBiking.SporttracksPlugins.Data
{
    /// <summary>
    /// A default implementation of IAttributeGroup, you won't need your own implementation
    /// </summary>
    public class Group : IGroup
    {
        /// <summary>
        /// A delegate that returns the name of this group in the current UICulture
        /// </summary>
        /// <returns>the name of this group in the current UICulture)</returns>
        public delegate string NameDelegate();

        /// <summary>
        /// Constructor for attribute groups that supports multilingual names
        /// </summary>
        /// <param name="id">must provide a unique ID in string representation (Guid.ToString())</param>
        /// <param name="nameDelegate">may NOT be null</param>
        /// <param name="image">may be null</param>
        public Group(Guid id,
                     NameDelegate nameDelegate,
                     Image image)
        {
            MyGuid = id;
            MyNameDelegate = nameDelegate;
            MyImage = image;
        }
        #pragma warning disable 1591 // don't mind if there's no XML comment for these elements
        #region IGroup Member
        public String ReferenceId { get { return MyGuid.ToString(); } }
        public String Name { get { return MyNameDelegate(); } }
        public Image Image { get { return MyImage; } }
        #endregion
        #region IComparable<IGroup> Member
        public int CompareTo(IGroup other)
        {
            if (ReferenceId == null ||
                other == null ||
                other.ReferenceId == null)
                return -1;
            else
                return ReferenceId.CompareTo(other.ReferenceId);
        }
        #endregion
        #pragma warning restore 1591

        private Guid MyGuid;
        private NameDelegate MyNameDelegate;
        private Image MyImage;
    }
}
