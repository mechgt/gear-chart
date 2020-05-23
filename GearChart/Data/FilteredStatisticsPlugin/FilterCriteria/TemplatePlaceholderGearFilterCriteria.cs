using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using ZoneFiveSoftware.Common.Data;
using ZoneFiveSoftware.Common.Data.Fitness;
using GearChart.Common;
using GearChart.Resources;

namespace GearChart.Data.FilteredStatisticsPlugin
{
    class TemplateGearPlaceholderFilterCriteria
    {
        public TemplateGearPlaceholderFilterCriteria()
        {
        }

#region IFilterCriteria members

        public Guid ReferenceId
        {
            get { return new Guid("8b2b2575-3535-4d97-b5c7-cf136e9d88dd"); }
        }

        public IActivity Activity
        {
            set { }
        }

        public string DisplayName
        {
            get { return Strings.GearsText; }
        }

        public List<object> NamedZones
        {
            get { return null; }
        }

        public bool IsTemplateOnly
        {
            get
            {
                return true;
            }
        }

        public bool IsMainViewOnly
        {
            get
            {
                return false;
            }
        }

        public object TemplateCompatibleCriteria
        {
            get { return null; }
        }

        public bool IsSerializable
        {
            get { return true; }
        }

        public void SerializeCriteria(Stream stream)
        {
        }

        public UInt16 DataVersion
        {
            get { return 1; }
        }

        public object DeserializeCriteria(Stream stream, UInt16 version)
        {
            if (version >= 1)
            {
                byte[] intBuffer = new byte[sizeof(Int32)];
                byte[] stringBuffer;
                Int32 stringLength;

                stream.Read(intBuffer, 0, sizeof(Int32));
                stringLength = BitConverter.ToInt32(intBuffer, 0);
                stringBuffer = new byte[stringLength];
                stream.Read(stringBuffer, 0, stringLength);

                return new TemplateGearFilterCriteria(null, Encoding.UTF8.GetString(stringBuffer));
            }
            else
            {
                throw new Exception("Invalid version");
            }
        }

        public object OnSelectedInList(object previousSelection)
        {
            if (GearCriteriaSelected != null)
            {
                // Make sure we have maximum 1 registered object since we have a return value
                Debug.Assert(GearCriteriaSelected.GetInvocationList().GetLength(0) == 1);

                object result;
                GearCriteriaSelected(this, previousSelection, out result);

                return result;
            }

            return previousSelection;
        }

        public event PropertyChangedEventHandler NamedZonedListChanged;

#endregion

        protected void TriggerNamedZonesListChanged()
        {
            if (NamedZonedListChanged != null)
            {
                NamedZonedListChanged(this, new PropertyChangedEventArgs("NamedZones"));
            }
        }

        public delegate void GearCriteriaSelectedEventHandler(TemplateGearPlaceholderFilterCriteria criteria, object previousCriteria, out object resultCriteria);
        public event GearCriteriaSelectedEventHandler GearCriteriaSelected;
    }
}
