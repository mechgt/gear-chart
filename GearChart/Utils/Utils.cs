using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ZoneFiveSoftware.Common.Data.Measurement;
using ZoneFiveSoftware.Common.Data.Fitness;
using ZoneFiveSoftware.Common.Visuals;
using ZoneFiveSoftware.Common.Data;

namespace GearChart.Utils
{
    static class Utils
    {
        [DllImport("gdi32.dll")]
        private static extern int BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight,
                                         IntPtr hdcSrc, int nXSrc, int nYSrc, Int32 dwRop);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport("gdi32.dll")]
        private static extern bool DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        private static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        static public void RenderBitmapToGraphics(Bitmap source, Graphics destination, Rectangle destinationRect)
        {
            try
            {
                IntPtr destHdc = destination.GetHdc();
                IntPtr bitmapHdc = CreateCompatibleDC(destHdc);

                SelectObject(bitmapHdc, source.GetHbitmap());
                BitBlt(destHdc, destinationRect.Left, destinationRect.Top,
                       destinationRect.Width, destinationRect.Height,
                       bitmapHdc, 0, 0, 0x00CC0020);

                destination.ReleaseHdc(destHdc);
                DeleteDC(bitmapHdc);
            }
            catch//(System.DllNotFoundException e)
            {
                // Mono/Linux - Just go on right now
                //throw e;
            }
        }

        public static Length.Units MajorLengthUnit(Length.Units unit)
        {
            if (IsMetric(unit))
            {
                return Length.Units.Kilometer;
            }
            else
            {
                return Length.Units.Mile;
            }
        }

        public static bool IsMetric(Length.Units unit)
        {
            return (int)unit <= (int)Length.Units.Kilometer;
        }

        public static bool IsStatute(Length.Units unit)
        {
            return !IsMetric(unit);
        }

        public static string GetSpeedUnitLabelForActivity(IActivity activity)
        {
            // TODO: Migrate to ST3 - Localize Strings
            //string speedUnitLabel = CommonResources.Text.LabelKmPerHour;
            string speedUnitLabel = "km/hr";

            if (activity != null)
            {
                if (activity.Category.SpeedUnits == Speed.Units.Speed)
                {
                    if (IsMetric(activity.Category.DistanceUnits))
                    {
                        //speedUnitLabel = CommonResources.Text.LabelKmPerHour;
                        speedUnitLabel = "km/hr";
                    }
                    else
                    {
                        //speedUnitLabel = CommonResources.Text.LabelMilePerHour;
                        speedUnitLabel = "mph";
                    }
                }
                else
                {
                    if (IsMetric(activity.Category.DistanceUnits))
                    {
                        //speedUnitLabel = CommonResources.Text.LabelMinPerKm;
                        speedUnitLabel = "min/km";
                    }
                    else
                    {
                        //speedUnitLabel = CommonResources.Text.LabelMinPerMile;
                        speedUnitLabel = "min/mile";
                    }
                }
            }

            return speedUnitLabel;
        }

        public static double SpeedToPace(double speed)
        {
            return Constants.MinutesPerHour / speed;
        }

        public static double PaceToSpeed(double pace)
        {
            return Constants.MinutesPerHour / pace;
        }

        public static INumericTimeDataSeries RemovePausedTimesInTrack(INumericTimeDataSeries sourceTrack, IActivity activity)
        {
            ActivityInfo activityInfo = ActivityInfoCache.Instance.GetInfo(activity);

            if (activityInfo != null && sourceTrack != null)
            {
                if (activityInfo.NonMovingTimes.Count == 0)
                {
                    return sourceTrack;
                }
                else
                {
                    INumericTimeDataSeries result = new NumericTimeDataSeries();
                    DateTime currentTime = sourceTrack.StartTime;
                    IEnumerator<ITimeValueEntry<float>> sourceEnumerator = sourceTrack.GetEnumerator();
                    IEnumerator<IValueRange<DateTime>> pauseEnumerator = activityInfo.NonMovingTimes.GetEnumerator();
                    double totalPausedTimeToDate = 0;
                    bool sourceEnumeratorIsValid;
                    bool pauseEnumeratorIsValid;

                    pauseEnumeratorIsValid = pauseEnumerator.MoveNext();
                    sourceEnumeratorIsValid = sourceEnumerator.MoveNext();

                    while (sourceEnumeratorIsValid)
                    {
                        bool addCurrentSourceEntry = true;
                        bool advanceCurrentSourceEntry = true;

                        // Loop to handle all pauses up to this current track point
                        if (pauseEnumeratorIsValid)
                        {
                            if (currentTime >= pauseEnumerator.Current.Lower &&
                                currentTime <= pauseEnumerator.Current.Upper)
                            {
                                addCurrentSourceEntry = false;
                            }
                            else if (currentTime > pauseEnumerator.Current.Upper)
                            {
                                // Advance pause enumerator
                                totalPausedTimeToDate += (pauseEnumerator.Current.Upper - pauseEnumerator.Current.Lower).TotalSeconds;
                                pauseEnumeratorIsValid = pauseEnumerator.MoveNext();

                                // Make sure we retry with the next pause
                                addCurrentSourceEntry = false;
                                advanceCurrentSourceEntry = false;
                            }
                        }

                        if (addCurrentSourceEntry)
                        {
                            result.Add(currentTime - new TimeSpan(0, 0, (int)totalPausedTimeToDate), sourceEnumerator.Current.Value);
                        }

                        if (advanceCurrentSourceEntry)
                        {
                            sourceEnumeratorIsValid = sourceEnumerator.MoveNext();
                            currentTime = sourceTrack.StartTime + new TimeSpan(0, 0, (int)sourceEnumerator.Current.ElapsedSeconds);
                        }
                    }

                    return result;
                }
            }

            return null;
        }

        public static void ExtractRangeFromDataTrack(INumericTimeDataSeries sourceTrack, ValueRange<DateTime> timeToKeep, INumericTimeDataSeries resultTrack)
        {
            resultTrack.Clear();

            if (sourceTrack != null && sourceTrack.Count > 0)
            {
                TimeSpan endElapsedTime = timeToKeep.Upper - sourceTrack.StartTime;
                TimeSpan totalElapsedTime = timeToKeep.Upper - timeToKeep.Lower;
                int endElapsedSeconds = (int)endElapsedTime.TotalSeconds;
                int totalElapsedSeconds = (int)totalElapsedTime.TotalSeconds;
                DateTime timeStart = timeToKeep.Lower;
                DateTime timeEnd = timeToKeep.Upper;
                DateTime sourceDataEndTime = sourceTrack.StartTime + new TimeSpan(0, 0, (int)sourceTrack.TotalElapsedSeconds);

                // Readjust to match the track since it can start after the activity start time
                //  or end before the end time
                if (timeStart < sourceTrack.StartTime)
                {
                    timeStart = sourceTrack.StartTime;
                }
                else if (timeStart > sourceDataEndTime)
                {
                    timeStart = sourceDataEndTime;
                }

                if (timeEnd < sourceTrack.StartTime)
                {
                    timeEnd = sourceTrack.StartTime;
                }
                else if (timeEnd > sourceDataEndTime)
                {
                    timeEnd = sourceDataEndTime;
                }

                ITimeValueEntry<float> startDistance = sourceTrack.GetInterpolatedValue(timeStart);
                int currentItemIndex = sourceTrack.IndexOf(startDistance);

                // Since we use an interpolated value, it is possible that there is no matching
                //  index, in which case we add it so we can have a start point in the array
                if (currentItemIndex == -1)
                {
                    sourceTrack.Add(timeStart, startDistance.Value);
                    startDistance = sourceTrack.GetInterpolatedValue(timeStart);
                    currentItemIndex = sourceTrack.IndexOf(startDistance);
                }

                // Just make sure we have a start index.  Can be == -1 if there is no track?
                if (currentItemIndex != -1)
                {
                    // Now go through all indexes until we hit the end
                    while (currentItemIndex < sourceTrack.Count && sourceTrack[currentItemIndex].ElapsedSeconds <= endElapsedSeconds)
                    {
                        ITimeValueEntry<float> currentDistance = sourceTrack[currentItemIndex];
                        DateTime currentTime = timeStart + new TimeSpan(0, 0, (int)(currentDistance.ElapsedSeconds - startDistance.ElapsedSeconds));

                        resultTrack.Add(currentTime, currentDistance.Value);

                        ++currentItemIndex;
                    }
                }

                // Since we check the elapsed seconds, it is possible that our end point needs to
                //  be interpolated.  Let's make sure we have our end point in our cached data
                ITimeValueEntry<float> lastEntry = sourceTrack.GetInterpolatedValue(timeEnd);

                // Should never be empty, we at least added the start above...
                if (resultTrack.Count != 0 && lastEntry != null)
                {
                    if (resultTrack[resultTrack.Count - 1].ElapsedSeconds != totalElapsedSeconds)
                    {
                        // Add our final interpolated point
                        resultTrack.Add(timeEnd, lastEntry.Value);
                    }
                }
            }
        }

        public static TimeSpan GetActiveActivityTme(IActivity activity)
        {
            ActivityInfo activityInfo = ActivityInfoCache.Instance.GetInfo(activity);
            TimeSpan activeTime = activityInfo.ActualTrackTime;

            // The only way to get the active tiem considering the settings is to use the non moving times
            foreach (IValueRange<DateTime> nonMovingTime in activityInfo.NonMovingTimes)
            {
                activeTime -= nonMovingTime.Upper - nonMovingTime.Lower;
            }

            return activeTime;
        }

        public static bool IsInActivityCategoryHierarchy(IActivity activity, IActivityCategory category)
        {
            IActivityCategory currentCategory = activity.Category;

            do
            {
                if (category == currentCategory)
                {
                    return true;
                }
                else if (!currentCategory.UseParentSettings)
                {
                    return false;
                }

                currentCategory = currentCategory.Parent;
            }
            while (currentCategory != null);

            return false;
        }
    }
}
