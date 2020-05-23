using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using ZoneFiveSoftware.Common.Data;
using ZoneFiveSoftware.Common.Data.Fitness;
using ZoneFiveSoftware.Common.Data.Measurement;
using ZoneFiveSoftware.Common.Visuals;
using ZoneFiveSoftware.Common.Visuals.Chart;
using GearChart.Resources;
using GearChart.Utils;

namespace GearChart.Data.FilteredStatisticsPlugin
{
    class GearsDataTrackProvider
    {
        public GearsDataTrackProvider()
        {
            m_ActivityDataChangedHelper.PropertyChanged += new PropertyChangedEventHandler(OnActivityDataChanged);
        }

        void OnActivityDataChanged(object sender, PropertyChangedEventArgs e)
        {
            if (DataTrackChanged != null)
            {
                bool trackNeedsRefresh = false;
                bool axisNeedsRefresh = false;

                if (e.PropertyName == "Activity.CadencePerMinuteTrack" ||
                    e.PropertyName == "Activity.DistanceMetersTrack")
                {
                    trackNeedsRefresh = true;
                }
                else if (e.PropertyName == "Activity.Length")
                {
                    trackNeedsRefresh = true;
                }
                else if (e.PropertyName == "Activity.EquipmentUsed")
                {
                    trackNeedsRefresh = true;
                }
                else if (e.PropertyName == "ActivityCategory.DistanceUnits" ||
                         e.PropertyName == "ActivityCategory.UseSystemLengthUnits")
                {
                    trackNeedsRefresh = true;
                    axisNeedsRefresh = true;
                }
                else if (e.PropertyName == "AnalysisSettings.CadenceSmoothingSeconds" ||
                         e.PropertyName == "AnalysisSettings.SpeedSmoothingSeconds")
                {
                    trackNeedsRefresh = true;
                }
                else if(e.PropertyName == "BikeSetup")
                {
                    trackNeedsRefresh = true;
                }

                // Throw events
                if (trackNeedsRefresh)
                {
                    DataTrackChanged(sender, new PropertyChangedEventArgs("DataTrack"));
                }

                if (axisNeedsRefresh)
                {
                    DataTrackChanged(sender, new PropertyChangedEventArgs("Axis"));
                }
            }
        }

#region IActivityDataTrackProvider Members

        public Guid ReferenceId
        {
            get { return new Guid("d5ee6bf2-244f-4885-8934-7cc323e29a1c"); }
        }

        public bool IsPrimaryDataTrack
        {
            get { return true; }
        }

        public IActivity Activity
        {
            set
            {
                if (m_Activity != value)
                {
                    m_Activity = value;
                    m_ActivityDataChangedHelper.Activity = value;
                }
            }
        }

        public string YAxisLabel
        {
            get
            {
                return Strings.GearRatioText;
            }
        }

        public string YAxisUnitLabel
        {
            get
            {
                if (m_Activity != null)
                {
                    return "(" + Length.LabelAbbr(MinorLengthUnit(m_Activity.Category.DistanceUnits)) + Strings.PerRevText + ")";
                }

                return String.Empty;
            }
        }

        public Color TrackColor
        {
            get { return Color.DarkOrange; }
        }

        public IAxisFormatter AxisFormatter 
        {
            get
            {
                return new Formatter.General(1);
            }
        }

        public INumericTimeDataSeries SmoothedDataTrack
        {
            get
            {
                if (m_Activity != null)
                {
                    INumericTimeDataSeries tempResult = ActivityGearTrackCache.Instance.CalculateGearTrack(m_Activity);

                    // Value is in m so convert to the right unit
                    INumericTimeDataSeries result = new NumericTimeDataSeries();
                    foreach (ITimeValueEntry<float> entry in tempResult)
                    {
                        double temp = Length.Convert(entry.Value, Length.Units.Meter, MinorLengthUnit(m_Activity.Category.DistanceUnits));

                        result.Add(tempResult.EntryDateTime(entry), (float)temp);
                    }

                    return result;
                }

                return null;
            }
        }

        public IList<Guid> DataTrackZoneGuids
        {
            get
            {
                return null;
            }
        }

        public Guid DefaultZone
        {
            get
            {
                return Guid.Empty;
            }
        }

        public string GetDataTrackZoneName(Guid zoneId)
        {
            return String.Empty;
        }

        public IList<AxisStripe> GetDataTrackZones(Guid zoneId)
        {
            return null;
        }

        public event PropertyChangedEventHandler DataTrackChanged;

#endregion

        private Length.Units MinorLengthUnit(Length.Units unit)
        {
            if (IsMetric(unit))
            {
                return Length.Units.Meter;
            }
            else
            {
                return Length.Units.Yard;
            }
        }

        private bool IsMetric(Length.Units unit)
        {
            return (int)unit <= (int)Length.Units.Kilometer;
        }

        private IActivity m_Activity = null;
        private ActivityDataChangedHelper m_ActivityDataChangedHelper = new ActivityDataChangedHelper(null);
    }
}
