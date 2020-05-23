using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using ZoneFiveSoftware.Common.Data.Fitness;
using ZoneFiveSoftware.Common.Data.Measurement;
using ZoneFiveSoftware.Common.Data;
using ZoneFiveSoftware.Common.Visuals;
using ZoneFiveSoftware.Common.Visuals.Chart;

namespace GearChart.UI.DetailPage
{
    internal class GearStats : IComparable
    {
        #region Fields

        private TimeSpan timeInZone;
        private TimeSpan coastingTime;
        private float gear;
        private float avgPower;
        private float avgHr;
        private float avgCadence;
        private float avgGrade;
        private float distance;
        private float ascend;
        private float descend;
        IActivity activity;

        #endregion

        #region Properties

        public string Ratio
        {
            get
            {
                if (gear == -1)
                {
                    return CommonResources.Text.LabelTotal;
                }
                else
                {
                    string ratioDesc = GetGearPair(gear);
                    return ratioDesc + " (" + gear.ToString("N2") + ")";
                }
            }
        }

        public string Ascend
        {
            get
            {
                Length.Units units = activity.Category.ElevationUnits;
                float value = (float)Length.Convert(ascend, Length.Units.Meter, units);
                return value.ToString("N" + Length.DefaultDecimalPrecision(units).ToString());
            }
        }

        public string Cadence
        {
            get { return avgCadence.ToString("N0"); }
        }

        public string Descend
        {
            get
            {
                Length.Units units = activity.Category.ElevationUnits;
                float value = (float)Length.Convert(descend, Length.Units.Meter, units);
                return value.ToString("N" + Length.DefaultDecimalPrecision(units).ToString());
            }
        }

        public string Distance
        {
            get
            {
                Length.Units units = activity.Category.DistanceUnits;
                float value = (float)Length.Convert(distance, Length.Units.Meter, units);
                return value.ToString("N" + Length.DefaultDecimalPrecision(units).ToString());
            }
        }

        public string PercentDistance
        {
            get
            {
                ActivityInfo info = ActivityInfoCache.Instance.GetInfo(activity);
                Length.Units units = activity.Category.DistanceUnits;
                float value = distance / (float)info.DistanceMeters;
                value = value * 100;

                return value.ToString("N1");
            }
        }

        public string PercentTime
        {
            get
            {
                ActivityInfo info = ActivityInfoCache.Instance.GetInfo(activity);
                double value = timeInZone.TotalSeconds / info.Time.TotalSeconds;
                value = value * 100;

                return value.ToString("N1");
            }
        }

        public string Grade
        {
            get
            {
                if (distance != 0)
                {
                    avgGrade = (ascend - descend) / distance;
                    float value = avgGrade * 100;
                    return value.ToString("N1");
                }
                else
                {
                    return "0";
                }
            }
        }

        public string HR
        {
            get { return avgHr.ToString("N0"); }
        }

        public string Power
        {
            get { return avgPower.ToString("N0"); }
        }

        public string Speed
        {
            get
            {
                if (timeInZone != TimeSpan.Zero)
                {
                    Length.Units units = activity.Category.DistanceUnits;
                    float value = distance / (float)timeInZone.TotalHours;
                    value = (float)Length.Convert(value, Length.Units.Meter, units);
                    return value.ToString("N" + Length.DefaultDecimalPrecision(units).ToString());
                }
                else
                {
                    return "-";
                }
            }
        }

        public string Time
        {
            get { return Utilities.FormatTimeSpan(timeInZone); }
        }

        public string CoastingTime
        {
            get { return Utilities.FormatTimeSpan(coastingTime); }
        }

        #endregion

        # region Constructors

        public GearStats(INumericTimeDataSeries gearTrack, float gear, IActivity activity)
        {
            // Initialize data
            timeInZone = TimeSpan.Zero;
            this.gear = gear;
            uint nextSecs = 0;
            float lastDistance = 0;
            float lastElev = 0;
            distance = 0;

            this.activity = activity;
            ActivityInfo info = ActivityInfoCache.Instance.GetInfo(activity);

            IDistanceDataTrack distanceTrack = GearUtils.GetDistanceTrack(activity);
            INumericTimeDataSeries elevationTrack = info.SmoothedElevationTrack;

            if (elevationTrack != null && elevationTrack.Count > 0)
            {
                lastElev = elevationTrack[0].Value;
            }

            // Calculate stats for the selected gear
            for (int i = 0; i < gearTrack.Count; i++)
            {
                // Get Elapsed time for next item
                // If smart recording only records a value when data changes, this implies a value is recorded
                //  and remains the same until the NEXT value is recorded (as opposed to using time delta from previous item)
                //  This means that the current data value is valid from CURRENT until NEXT and this timespan should be used.
                ITimeValueEntry<float> item = gearTrack[i];
                int nextItem = i + 1;

                if (gearTrack.Count > nextItem)
                {
                    nextSecs = gearTrack[nextItem].ElapsedSeconds;
                }
                else
                {
                    nextSecs = gearTrack.TotalElapsedSeconds;
                }

                if (item.Value == gear)
                {
                    // Time riding in gear
                    uint secondsChange = nextSecs - item.ElapsedSeconds;
                    timeInZone = timeInZone.Add(new TimeSpan(0, 0, (int)secondsChange));

                    // Avg Power
                    avgPower = AddTimeAverage(activity.PowerWattsTrack, avgPower, gearTrack.EntryDateTime(item), item.ElapsedSeconds, nextSecs);

                    // Avg HR
                    avgHr = AddTimeAverage(activity.HeartRatePerMinuteTrack, avgHr, gearTrack.EntryDateTime(item), item.ElapsedSeconds, nextSecs);

                    // Avg Cadence
                    // TODO: Update to read CadenceCutoff: ActivityInfoCache.Instance.Options.CadenceCutoff
                    //   Admin says this is not possible at the moment.  Sounds like a bug in the API.
                    //int cadenceCutoff = ActivityInfoCache.Instance.Options.CadenceCutoff;
                    int cadenceCutoff = 30;
                    if (GearUtils.ContainsTime(activity.CadencePerMinuteTrack, gearTrack.EntryDateTime(item)))
                    {
                        if (activity.CadencePerMinuteTrack.GetInterpolatedValue(gearTrack.EntryDateTime(item)).Value > cadenceCutoff)
                        {
                            avgCadence = AddTimeAverage(activity.CadencePerMinuteTrack, avgCadence, gearTrack.EntryDateTime(item), item.ElapsedSeconds, nextSecs);
                        }
                        else if (activity.CadencePerMinuteTrack.GetInterpolatedValue(gearTrack.EntryDateTime(item)).Value <= cadenceCutoff)
                        {
                            coastingTime = coastingTime.Add(new TimeSpan(0, 0, (int)secondsChange));
                        }
                    }

                    // Distance covered
                    if (GearUtils.ContainsTime(distanceTrack, gearTrack.EntryDateTime(item)))
                    {
                        float distanceChange = distanceTrack.GetInterpolatedValue(gearTrack.EntryDateTime(item)).Value - lastDistance;
                        distance += distanceChange;
                    }

                    // Ascend/Descend
                    float elevChange = 0;
                    if (GearUtils.ContainsTime(elevationTrack, gearTrack.EntryDateTime(item)))
                    {
                        elevChange = elevationTrack.GetInterpolatedValue(gearTrack.EntryDateTime(item)).Value - lastElev;
                        if (elevChange > 0)
                        {
                            ascend += elevChange;
                        }
                        else
                        {
                            descend += elevChange;
                        }
                    }
                }

                // Store previous values for next calculation
                if (GearUtils.ContainsTime(elevationTrack, gearTrack.EntryDateTime(item)))
                {
                    lastElev = elevationTrack.GetInterpolatedValue(gearTrack.EntryDateTime(item)).Value;
                }

                if (GearUtils.ContainsTime(distanceTrack, gearTrack.EntryDateTime(item)))
                {
                    lastDistance = distanceTrack.GetInterpolatedValue(gearTrack.EntryDateTime(item)).Value;
                }
            }
        }

        /// <summary>
        /// Used to Create the GearStats 'Total' line items.
        /// </summary>
        /// <param name="gearStats">List of items to totalize.</param>
        public GearStats(List<GearStats> gearStats)
        {
            gear = -1;

            if (gearStats != null && gearStats.Count > 0)
            {
                // Initialize values
                timeInZone = TimeSpan.Zero;
                activity = gearStats[0].activity;
                ActivityInfo info = ActivityInfoCache.Instance.GetInfo(activity);
                avgCadence = info.AverageCadence;
                avgGrade = info.AverageGrade;
                avgHr = info.AverageHeartRate;
                avgPower = info.AveragePower;

                // Totalize values - list below simply used to ensure gears aren't double-counted (see notes below).
                List<float> totaledGears = new List<float>();

                foreach (GearStats item in gearStats)
                {
                    // Skip duplicate gear entries while totaling
                    // Cannot distinguish between 'repeat gears', so they're included in the stats twice.  
                    // Don't double count them when creating totals.
                    if (!totaledGears.Contains(item.gear))
                    {
                        timeInZone += item.timeInZone;
                        distance += item.distance;
                        ascend += item.ascend;
                        descend += item.descend;
                        coastingTime += item.coastingTime;
                    }

                    totaledGears.Add(item.gear);
                }
            }
        }

        # endregion

        #region Methods

        /// <summary>
        /// Add/incorporate an a value into an average.  Note that this is a time based average, not distance based average.
        /// </summary>
        /// <param name="series"></param>
        /// <param name="avgValue"></param>
        /// <param name="time"></param>
        /// <param name="elapsedSecs"></param>
        /// <param name="lastSecs"></param>
        /// <returns></returns>
        private float AddTimeAverage(INumericTimeDataSeries series, float avgValue, DateTime time, uint elapsedSecs, uint nextSecs)
        {
            float calcValue = float.NaN;

            // Average over data series
            if (GearUtils.ContainsTime(series, time))
            {
                float value = series.GetInterpolatedValue(time).Value;
                float seconds = nextSecs - elapsedSecs;
                calcValue = avgValue * ((float)(timeInZone.TotalSeconds - seconds) / (float)timeInZone.TotalSeconds) + value * seconds / (float)timeInZone.TotalSeconds;
            }

            if (!float.IsNaN(calcValue))
            {
                return calcValue;
            }
            else if (!float.IsNaN(avgValue))
            {
                return avgValue;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ratio"></param>
        /// <returns></returns>
        private string GetGearPair(float ratio)
        {
            string equipId = Options.Instance.GetGearEquipmentId(activity);

            List<float> bigGears = Options.Instance.GetBigGears(equipId);
            List<float> smallGears = Options.Instance.GetSmallGears(equipId);
            float wheelCircum = Options.Instance.GetWheelCircumference(equipId);

            foreach (float big in bigGears)
            {
                foreach (float small in smallGears)
                {
                    if (Math.Abs((big / small * wheelCircum) - ratio) < 0.0001)
                    {
                        return big + "x" + small;
                    }
                }
            }

            return "";
        }

        #endregion

        #region IComparable Members

        // Default Sort
        public int CompareTo(object obj)
        {
            GearStats a = obj as GearStats;
            return this.gear.CompareTo(a.gear);
        }

        // Column specific sort
        public int CompareTo(GearStats a2, GearStatsComparer.ComparisonType comparisonMethod, GearStatsComparer.Order sortOrder)
        {
            int result = 0;

            if (a2.gear == -1)
            {
                return -1;
            }

            switch (comparisonMethod)
            {
                case GearStatsComparer.ComparisonType.Ascend:
                    result = Compare(a2, a2.ascend, this.ascend);
                    break;
                case GearStatsComparer.ComparisonType.Cadence:
                    result = Compare(a2, a2.avgCadence, this.avgCadence);
                    break;
                case GearStatsComparer.ComparisonType.Descend:
                    result = Compare(a2, a2.descend, this.descend);
                    break;
                case GearStatsComparer.ComparisonType.PercentDistance:
                case GearStatsComparer.ComparisonType.Distance:
                    result = Compare(a2, a2.distance, this.distance);
                    break;
                case GearStatsComparer.ComparisonType.Grade:
                    result = Compare(a2, a2.avgGrade, this.avgGrade);
                    break;
                case GearStatsComparer.ComparisonType.HR:
                    result = Compare(a2, a2.avgHr, this.avgHr);
                    break;
                case GearStatsComparer.ComparisonType.Power:
                    result = Compare(a2, a2.avgPower, this.avgPower);
                    break;
                case GearStatsComparer.ComparisonType.Speed:
                    float thisSpeed = this.distance / (float)this.timeInZone.TotalSeconds;
                    float a2Speed = a2.distance / (float)a2.timeInZone.TotalSeconds;
                    result = Compare(a2, a2Speed, thisSpeed);
                    break;
                case GearStatsComparer.ComparisonType.PercentTime:
                case GearStatsComparer.ComparisonType.Time:
                    result = Compare(a2, a2.timeInZone, this.timeInZone);
                    break;
                case GearStatsComparer.ComparisonType.Coasting:
                    result = Compare(a2, a2.coastingTime, this.coastingTime);
                    break;
                case GearStatsComparer.ComparisonType.Ratio:
                default:
                    result = CompareTo(a2);
                    break;
            }

            if (sortOrder == GearStatsComparer.Order.Descending)
            {
                result = result * -1;
            }

            return result;
        }

        // Float comparison
        private int Compare(GearStats obj, float a, float b)
        {
            int result;

            if (a != b)
            {
                result = a.CompareTo(b);
            }
            else
            {
                result = CompareTo(obj);
            }

            return result;
        }

        // Timespan comparison
        private int Compare(GearStats obj, TimeSpan a, TimeSpan b)
        {
            int result;

            if (a != b)
            {
                result = a.CompareTo(b);
            }
            else
            {
                result = CompareTo(obj);
            }

            return result;
        }

        #endregion
    }

    internal class GearStatsComparer : IComparer<GearStats>
    {
        private ComparisonType comparisonType;
        private Order sortOrder;

        public enum ComparisonType
        {
            Ratio,
            Ascend,
            Cadence,
            Descend,
            Distance,
            PercentDistance,
            PercentTime,
            Grade,
            HR,
            Power,
            Speed,
            Time,
            Coasting
        }

        public enum Order
        {
            Ascending = 1, Descending = 2
        }

        public ComparisonType ComparisonMethod
        {
            set { this.comparisonType = value; }
        }

        public Order SortOrder
        {
            set { this.sortOrder = value; }
        }

        #region IComparer<GearStats> Members

        public int Compare(GearStats x, GearStats y)
        {
            return x.CompareTo(y, this.comparisonType, this.sortOrder);
        }

        #endregion
    }
}
