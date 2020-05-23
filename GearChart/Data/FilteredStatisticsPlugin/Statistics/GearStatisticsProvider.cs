using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using ZoneFiveSoftware.Common.Data;
using ZoneFiveSoftware.Common.Data.Fitness;
using ZoneFiveSoftware.Common.Data.Measurement;
using ZoneFiveSoftware.Common.Visuals;
using GearChart.Resources;
using GearChart.Utils;
using GearChart.Common;

namespace GearChart.Data.FilteredStatisticsPlugin
{
    class GearStatisticsProvider
    {
        private enum ColumnSortingType
        {
            Unsorted = 0,
            Ascending,
            Descending
        }

        private class ComboUsageInfo
        {
            public ComboUsageInfo(SprocketCombo combo, TimeSpan usageDuration)
            {
                m_Combo = combo;
                m_UsageDuration = usageDuration;
            }

            public SprocketCombo m_Combo;
            public TimeSpan m_UsageDuration;
        }

        public GearStatisticsProvider()
        {
        }

#region IStatisticsProvider Members

        public IList<Guid> ColumnIds
        {
            get
            {
                List<Guid> columns = new List<Guid>();

                columns.Add(new Guid("61951eb0-464f-452c-903c-6241216f82d6")); // Min gear
                columns.Add(new Guid("f9115ad4-6ed0-4d96-a9cf-8ddf0adbffc5")); // Avg gear
                columns.Add(new Guid("f955c5f8-67f2-4a26-ba95-d110329be86b")); // Max gear
                columns.Add(new Guid("19b149fe-2ed5-4b9c-80b2-6a96e45e4c14")); // Gear vs. Avg
                columns.Add(new Guid("384210e3-d8cf-4696-b948-0981972751c7")); // Most used gear

                return columns;
            }
        }

        public String GetColumnHeader(Guid columnId, IActivity activity)
        {
            Dictionary<Guid, String> idToHeaderTable = new Dictionary<Guid, String>();

            String temp = String.Empty;
            if (activity != null)
            {
                temp = " (" + Length.LabelAbbr(MinorLengthUnit(activity.Category.DistanceUnits)) + Strings.PerRevText + ")";
            }

            idToHeaderTable.Add(new Guid("61951eb0-464f-452c-903c-6241216f82d6"), Strings.MinRatio + temp);
            idToHeaderTable.Add(new Guid("f9115ad4-6ed0-4d96-a9cf-8ddf0adbffc5"), Strings.AverageRatio + temp);
            idToHeaderTable.Add(new Guid("f955c5f8-67f2-4a26-ba95-d110329be86b"), Strings.MaxRatio + temp);
            idToHeaderTable.Add(new Guid("19b149fe-2ed5-4b9c-80b2-6a96e45e4c14"), Strings.GearVsAvg + temp);
            idToHeaderTable.Add(new Guid("384210e3-d8cf-4696-b948-0981972751c7"), Strings.MostUsedGear + temp);

            Debug.Assert(idToHeaderTable.ContainsKey(columnId));

            return idToHeaderTable[columnId];
        }

        public Color GetColumnColor(Guid columnId)
        {
            return Color.DarkOrange;
        }

        public String GetStatisticLabel(Guid columnId, object currentRange)
        {
            MethodInfo getLabelMethod = GetType().GetMethod("Get" + GetValueMemberFromGuid(columnId), new Type[] { typeof(RangeInfoCacheWrapper) });

            Debug.Assert(getLabelMethod != null && getLabelMethod.ReturnType == typeof(String));

            return getLabelMethod.Invoke(this, new object[] { new RangeInfoCacheWrapper(currentRange) }) as String;
        }

        public String GetTotalStatisticLabel(Guid columnId, IList<object> allRanges)
        {
            MethodInfo getLabelMethod = GetType().GetMethod("Get" + GetValueMemberFromGuid(columnId), new Type[] { typeof(List<RangeInfoCacheWrapper>) });

            Debug.Assert(getLabelMethod != null && getLabelMethod.ReturnType == typeof(String));

            List<RangeInfoCacheWrapper> allWrappers = new List<RangeInfoCacheWrapper>();

            foreach (object range in allRanges)
            {
                allWrappers.Add(new RangeInfoCacheWrapper(range));
            }

            return getLabelMethod.Invoke(this, new object[] { allWrappers }) as String;
        }

        public int CompareRanges(Guid columnId, int sortingOrder, object range1, object range2)
        {
            ColumnSortingType sortOrder = (ColumnSortingType)sortingOrder;
            String sortMember = GetValueMemberFromGuid(columnId);
            MethodInfo valueMethod = GetType().GetMethod("Get" + sortMember + "Value", new Type[] { typeof(RangeInfoCacheWrapper) });

            Debug.Assert(valueMethod != null);

            double valueX;
            double valueY;
            double result;

            valueX = (double)valueMethod.Invoke(this, new object[] { new RangeInfoCacheWrapper(range1) });
            valueY = (double)valueMethod.Invoke(this, new object[] { new RangeInfoCacheWrapper(range2) });

            if (sortOrder == ColumnSortingType.Ascending)
            {
                result = valueX - valueY;
            }
            else
            {
                result = valueY - valueX;
            }

            return Math.Sign(result);
        }

#endregion

        public double GetMinRatioValue(RangeInfoCacheWrapper range)
        {
            return GetRangeGearsTrack(range).Min;
        }

        public String GetMinRatio(RangeInfoCacheWrapper range)
        {
            if (range.ActivityPauselessCadenceTrack.Count > 0)
            {
                double minRatio = GetMinRatioValue(range);
                SprocketCombo combo = FindComboFromRatio(minRatio, range.Activity);

                // We must refresh our cache, something changed in our activity
                if (combo == null)
                {
                    range.SaveStoredInfo(m_GearTrackStoredInfoId, null);
                    range.SaveStoredInfo(m_SprocketTrackStoredInfoId, null);
                    range.SaveStoredInfo(m_SprocketStoredInfoId, null);

                    minRatio = GetMinRatioValue(range);
                    combo = FindComboFromRatio(minRatio, range.Activity);
                }

                Debug.Assert(combo != null);

                return String.Format("{0:0.00} ({1:0}x{2:0})", Math.Round(minRatio, 2), combo.ChainringSize, combo.CassetteSize);
            }

            return String.Empty;
        }

        public double GetMinRatioValue(List<RangeInfoCacheWrapper> allRanges)
        {
            if (allRanges.Count > 0 &&
                allRanges[0].ActivityPauselessCadenceTrack.Count > 0)
            {
                double minRatio = double.MaxValue;

                foreach (RangeInfoCacheWrapper currentRange in allRanges)
                {
                    minRatio = Math.Min(minRatio, GetMinRatioValue(currentRange));
                }

                return minRatio;
            }

            return 0;
        }

        public String GetMinRatio(List<RangeInfoCacheWrapper> allRanges)
        {
            if (allRanges.Count > 0 &&
                allRanges[0].ActivityPauselessCadenceTrack.Count > 0)
            {
                double minRatio = GetMinRatioValue(allRanges);

                if (minRatio != 0)
                {
                    SprocketCombo combo = FindComboFromRatio(minRatio, allRanges[0].Activity);

                    // We must refresh our cache, something changed in our activity
                    if (combo == null)
                    {
                        foreach (RangeInfoCacheWrapper range in allRanges)
                        {
                            range.SaveStoredInfo(m_GearTrackStoredInfoId, null);
                            range.SaveStoredInfo(m_SprocketTrackStoredInfoId, null);
                            range.SaveStoredInfo(m_SprocketStoredInfoId, null);
                        }

                        minRatio = GetMinRatioValue(allRanges);
                        combo = FindComboFromRatio(minRatio, allRanges[0].Activity);
                    }

                    Debug.Assert(combo != null);

                    return String.Format("{0:0.00} ({1:0}x{2:0})", Math.Round(minRatio, 2), combo.ChainringSize, combo.CassetteSize);
                }
            }

            return String.Empty;
        }

        public double GetAvgRatioValue(RangeInfoCacheWrapper range)
        {
            return GetRangeGearsTrack(range).Avg;
        }

        public String GetAvgRatio(RangeInfoCacheWrapper range)
        {
            if (range.ActivityPauselessCadenceTrack.Count > 0)
            {
                return String.Format("{0:0.00}", Math.Round(GetAvgRatioValue(range), 2));
            }

            return String.Empty;
        }

        public double GetAvgRatioValue(List<RangeInfoCacheWrapper> allRanges)
        {
            double avgRatioTotal = 0;
            double timeRatioTotal = 0;

            foreach (RangeInfoCacheWrapper currentRange in allRanges)
            {
                double timePercent = GetPauselessTimePercent(currentRange);

                avgRatioTotal += GetAvgRatioValue(currentRange) * timePercent;
                timeRatioTotal += timePercent;
            }

            if (timeRatioTotal > 0)
            {
                return avgRatioTotal / timeRatioTotal;
            }
            else
            {
                return 0;
            }
        }

        public String GetAvgRatio(List<RangeInfoCacheWrapper> allRanges)
        {
            double avgRatio = GetAvgRatioValue(allRanges);

            if(avgRatio != 0)
            {
                return String.Format("{0:0.00}", Math.Round(avgRatio, 2));
            }

            return String.Empty;
        }

        public double GetMaxRatioValue(RangeInfoCacheWrapper range)
        {
            return GetRangeGearsTrack(range).Max;
        }

        public String GetMaxRatio(RangeInfoCacheWrapper range)
        {
            if (range.ActivityPauselessCadenceTrack.Count > 0)
            {
                double maxRatio = GetMaxRatioValue(range);
                SprocketCombo combo = FindComboFromRatio(maxRatio, range.Activity);

                // We must refresh our cache, something changed in our activity
                if (combo == null)
                {
                    range.SaveStoredInfo(m_GearTrackStoredInfoId, null);
                    range.SaveStoredInfo(m_SprocketTrackStoredInfoId, null);
                    range.SaveStoredInfo(m_SprocketStoredInfoId, null);

                    maxRatio = GetMaxRatioValue(range);
                    combo = FindComboFromRatio(maxRatio, range.Activity);
                }

                Debug.Assert(combo != null);

                return String.Format("{0:0.00} ({1:0}x{2:0})", Math.Round(maxRatio, 2), combo.ChainringSize, combo.CassetteSize);
            }

            return String.Empty;
        }

        public double GetMaxRatioValue(List<RangeInfoCacheWrapper> allRanges)
        {
            if (allRanges.Count > 0 &&
                allRanges[0].ActivityPauselessCadenceTrack.Count > 0)
            {
                double maxRatio = double.MinValue;

                foreach (RangeInfoCacheWrapper currentRange in allRanges)
                {
                    maxRatio = Math.Max(maxRatio, GetMaxRatioValue(currentRange));
                }

                return maxRatio;
            }

            return 0;
        }

        public String GetMaxRatio(List<RangeInfoCacheWrapper> allRanges)
        {
            if (allRanges.Count > 0 &&
                allRanges[0].ActivityPauselessCadenceTrack.Count > 0)
            {
                double maxRatio = GetMaxRatioValue(allRanges);

                if (maxRatio != 0)
                {
                    SprocketCombo combo = FindComboFromRatio(maxRatio, allRanges[0].Activity);

                    // We must refresh our cache, something changed in our activity
                    if (combo == null)
                    {
                        foreach (RangeInfoCacheWrapper range in allRanges)
                        {
                            range.SaveStoredInfo(m_GearTrackStoredInfoId, null);
                            range.SaveStoredInfo(m_SprocketTrackStoredInfoId, null);
                            range.SaveStoredInfo(m_SprocketStoredInfoId, null);
                        }

                        maxRatio = GetMaxRatioValue(allRanges);
                        combo = FindComboFromRatio(maxRatio, allRanges[0].Activity);
                    }

                    Debug.Assert(combo != null);

                    return String.Format("{0:0.00} ({1:0}x{2:0})", Math.Round(maxRatio, 2), combo.ChainringSize, combo.CassetteSize);
                }
            }

            return String.Empty;
        }

        public double GetRatioVsAvgValue(RangeInfoCacheWrapper range)
        {
            INumericTimeDataSeries gears = ActivityGearTrackCache.Instance.CalculateGearTrack(range.Activity);

            return GetRangeGearsTrack(range).Avg - gears.Avg;
        }

        public String GetRatioVsAvg(RangeInfoCacheWrapper range)
        {
            if (range.ActivityPauselessCadenceTrack.Count > 0)
            {
                double ratioVsAvg = Math.Round(GetRatioVsAvgValue(range), 2);
                String sign = "";

                if (ratioVsAvg < 0)
                {
                    sign = "-";
                }
                else if (ratioVsAvg > 0)
                {
                    sign = "+";
                }

                return String.Format("{1}{0:0.00}", Math.Abs(ratioVsAvg), sign);
            }

            return String.Empty;
        }

        public double GetRatioVsAvgValue(List<RangeInfoCacheWrapper> allRanges)
        {
            if (allRanges.Count > 0 &&
                allRanges[0].ActivityPauselessCadenceTrack.Count > 0)
            {
                INumericTimeDataSeries activityGears = ActivityGearTrackCache.Instance.CalculateGearTrack(allRanges[0].Activity);

                return GetAvgRatioValue(allRanges) - activityGears.Avg;
            }

            return 0;
        }

        public String GetRatioVsAvg(List<RangeInfoCacheWrapper> allRanges)
        {
            if (allRanges.Count > 0 &&
                allRanges[0].ActivityPauselessCadenceTrack.Count > 0)
            {
                double ratioVsAvg = GetRatioVsAvgValue(allRanges);
                String sign = "";

                if (ratioVsAvg < 0)
                {
                    sign = "-";
                }
                else if (ratioVsAvg > 0)
                {
                    sign = "+";
                }

                return String.Format("{1}{0:0.00}", Math.Round(ratioVsAvg, 2), sign);
            }

            return String.Empty;
        }

        private ComboUsageInfo GetMostUsedGearValue(RangeInfoCacheWrapper range)
        {
            if (range.ActivityPauselessCadenceTrack.Count > 0)
            {
                ITimeDataSeries<SprocketCombo> rangeSprockets = GetRangeSprocketsTrack(range);
                IList<SprocketComboInfo> sprocketInfo = GetSprocketComboInfo(range, rangeSprockets);
                SprocketCombo mostUsedCombo = null;
                TimeSpan mostUsedDuration = new TimeSpan(0);

                foreach (SprocketComboInfo comboInfo in sprocketInfo)
                {
                    TimeSpan currentComboDuration = new TimeSpan(0);

                    foreach (IValueRange<DateTime> times in comboInfo.Times)
                    {
                        currentComboDuration += (times.Upper - times.Lower);
                    }

                    if (mostUsedCombo == null || mostUsedDuration < currentComboDuration)
                    {
                        mostUsedCombo = comboInfo.SprocketCombo;
                        mostUsedDuration = currentComboDuration;
                    }
                }

                return new ComboUsageInfo(mostUsedCombo, mostUsedDuration);
            }

            return null;
        }

        public String GetMostUsedGear(RangeInfoCacheWrapper range)
        {
            if (range.ActivityPauselessCadenceTrack.Count > 0)
            {
                ComboUsageInfo mostUsedGear = GetMostUsedGearValue(range);

                if (mostUsedGear.m_Combo != null)
                {
                    return String.Format("{0:0.00} ({1:0}x{2:0}) [{3:00}:{4:00}:{5:00}]",
                                         Math.Round(mostUsedGear.m_Combo.GearRatio, 2), mostUsedGear.m_Combo.ChainringSize,
                                         mostUsedGear.m_Combo.CassetteSize, mostUsedGear.m_UsageDuration.Hours,
                                         mostUsedGear.m_UsageDuration.Minutes, mostUsedGear.m_UsageDuration.Seconds);
                }
            }

            return String.Empty;
        }

        private ComboUsageInfo GetMostUsedGearValue(List<RangeInfoCacheWrapper> allRanges)
        {
            if (allRanges.Count > 0 &&
                allRanges[0].ActivityPauselessCadenceTrack.Count > 0)
            {
                List<ComboUsageInfo> allRangesUsageInfo = new List<ComboUsageInfo>();

                foreach (RangeInfoCacheWrapper range in allRanges)
                {
                    ITimeDataSeries<SprocketCombo> rangeSprockets = GetRangeSprocketsTrack(range);
                    IList<SprocketComboInfo> sprocketInfo = GetSprocketComboInfo(range, rangeSprockets);

                    for (int i = 0; i < sprocketInfo.Count; ++i)
                    {
                        TimeSpan currentComboDuration = new TimeSpan(0);

                        foreach (IValueRange<DateTime> times in sprocketInfo[i].Times)
                        {
                            currentComboDuration += (times.Upper - times.Lower);
                        }

                        if (allRangesUsageInfo.Count <= i)
                        {
                            allRangesUsageInfo.Add(new ComboUsageInfo(sprocketInfo[i].SprocketCombo, currentComboDuration));
                        }
                        else
                        {
                            allRangesUsageInfo[i].m_UsageDuration += currentComboDuration;
                        }
                    }
                }

                SprocketCombo mostUsedCombo = null;
                TimeSpan mostUsedDuration = new TimeSpan(0);

                foreach (ComboUsageInfo comboInfo in allRangesUsageInfo)
                {
                    if (mostUsedCombo == null || mostUsedDuration < comboInfo.m_UsageDuration)
                    {
                        mostUsedCombo = comboInfo.m_Combo;
                        mostUsedDuration = comboInfo.m_UsageDuration;
                    }
                }

                return new ComboUsageInfo(mostUsedCombo, mostUsedDuration);
            }

            return null;
        }

        public String GetMostUsedGear(List<RangeInfoCacheWrapper> allRanges)
        {
            if (allRanges.Count > 0 &&
                allRanges[0].ActivityPauselessCadenceTrack.Count > 0)
            {
                ComboUsageInfo mostUsedGear = GetMostUsedGearValue(allRanges);

                if (mostUsedGear != null)
                {
                    return String.Format("{0:0.00} ({1:0}x{2:0}) [{3:00}:{4:00}:{5:00}]",
                                         Math.Round(mostUsedGear.m_Combo.GearRatio, 2), mostUsedGear.m_Combo.ChainringSize,
                                         mostUsedGear.m_Combo.CassetteSize, mostUsedGear.m_UsageDuration.Hours,
                                         mostUsedGear.m_UsageDuration.Minutes, mostUsedGear.m_UsageDuration.Seconds);
                }
            }

            return String.Empty;
        }

        private double GetPauselessTimePercent(RangeInfoCacheWrapper range)
        {
            double pauselessDuration = (range.PauselessRange.Upper - range.PauselessRange.Lower).TotalSeconds;

            return pauselessDuration / Utils.Utils.GetActiveActivityTme(range.Activity).TotalSeconds;
        }

        private SprocketCombo FindComboFromRatio(double ratio, IActivity activity)
        {
            IList<SprocketCombo> sprockets = GearChart.Common.Data.GetSprocketCombos(activity);

            foreach (SprocketCombo combo in sprockets)
            {
                if (combo.GearRatio == ratio)
                {
                    return combo;
                }
            }

            return null;
        }

        private String GetValueMemberFromGuid(Guid id)
        {
            Dictionary<Guid, String> valueMemberToGuidTable = new Dictionary<Guid, String>();

            valueMemberToGuidTable.Add(new Guid("61951eb0-464f-452c-903c-6241216f82d6"), "MinRatio");
            valueMemberToGuidTable.Add(new Guid("f9115ad4-6ed0-4d96-a9cf-8ddf0adbffc5"), "AvgRatio");
            valueMemberToGuidTable.Add(new Guid("f955c5f8-67f2-4a26-ba95-d110329be86b"), "MaxRatio");
            valueMemberToGuidTable.Add(new Guid("19b149fe-2ed5-4b9c-80b2-6a96e45e4c14"), "RatioVsAvg");
            valueMemberToGuidTable.Add(new Guid("384210e3-d8cf-4696-b948-0981972751c7"), "MostUsedGear");

            Debug.Assert(valueMemberToGuidTable.ContainsKey(id));

            return valueMemberToGuidTable[id];
        }

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

        public NumericTimeDataSeries GetRangeGearsTrack(RangeInfoCacheWrapper range)
        {
            NumericTimeDataSeries result = null;

            if (range.ContainsStoredInfo(m_GearTrackStoredInfoId) &&
                range.RetrieveStoredInfo(m_GearTrackStoredInfoId) != null)
            {
                result = range.RetrieveStoredInfo(m_GearTrackStoredInfoId) as NumericTimeDataSeries;
            }
            else
            {
                INumericTimeDataSeries gears = ActivityGearTrackCache.Instance.CalculateGearTrack(range.Activity);

                gears = Utils.Utils.RemovePausedTimesInTrack(gears, range.Activity);

                result = new NumericTimeDataSeries();
                Utils.Utils.ExtractRangeFromDataTrack(gears, range.PauselessRange, result);
                result = GearChart.UI.GearUtils.GuessGears(result, Common.Data.GetSprocketCombos(range.Activity));

                range.SaveStoredInfo(m_GearTrackStoredInfoId, result);
            }

            return result;
        }

        public ITimeDataSeries<SprocketCombo> GetRangeSprocketsTrack(RangeInfoCacheWrapper range)
        {
            ITimeDataSeries<SprocketCombo> result = null;

            if (range.ContainsStoredInfo(m_SprocketTrackStoredInfoId) &&
                range.RetrieveStoredInfo(m_SprocketTrackStoredInfoId) != null)
            {
                result = range.RetrieveStoredInfo(m_SprocketTrackStoredInfoId) as ITimeDataSeries<SprocketCombo>;
            }
            else
            {
                INumericTimeDataSeries gears = ActivityGearTrackCache.Instance.CalculateGearTrack(range.Activity);

                gears = Utils.Utils.RemovePausedTimesInTrack(gears, range.Activity);

                NumericTimeDataSeries tempResult = new NumericTimeDataSeries();
                Utils.Utils.ExtractRangeFromDataTrack(gears, range.PauselessRange, tempResult);
                result = GearChart.UI.GearUtils.GuessSprockets(tempResult, Common.Data.GetSprocketCombos(range.Activity));

                range.SaveStoredInfo(m_SprocketTrackStoredInfoId, result);
            }

            return result;
        }

        public IList<SprocketComboInfo> GetSprocketComboInfo(RangeInfoCacheWrapper range, ITimeDataSeries<SprocketCombo> sprocketTrack)
        {
            IList<SprocketComboInfo> result;

            if (range.ContainsStoredInfo(m_SprocketStoredInfoId) &&
                range.RetrieveStoredInfo(m_SprocketStoredInfoId) != null)
            {
                result = range.RetrieveStoredInfo(m_SprocketStoredInfoId) as IList<SprocketComboInfo>;
            }
            else
            {
                result = Common.Data.Calculate(range.Activity, sprocketTrack);

                // Remove totals row
                result.RemoveAt(result.Count - 1);

                range.SaveStoredInfo(m_SprocketStoredInfoId, result);
            }

            return result;
        }

        private static readonly Guid m_GearTrackStoredInfoId = new Guid("cd26d9d5-182d-4399-b05e-6bb86d23c7bb");
        private static readonly Guid m_SprocketTrackStoredInfoId = new Guid("756fecc8-45d8-475d-9f5a-3694fe6680a3");
        private static readonly Guid m_SprocketStoredInfoId = new Guid("439ab6fe-3188-452f-9b38-b39f3e23aded");
    }
}
