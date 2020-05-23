using System;
using System.Collections.Generic;

using ZoneFiveSoftware.Common.Data;
using ZoneFiveSoftware.Common.Data.Fitness;
using GearChart.Common;

namespace GearChart.UI
{
    static class GearUtils
    {
        #region Fields

        #endregion

        /// <summary>
        /// Removes the track pauses within a data track.
        /// </summary>
        /// <param name="sourceTrack">Data track</param>
        /// <param name="activity">Activity containing pause definitions</param>
        /// <returns>Returns the source track with the track pause times removed</returns>
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

        /// <summary>
        /// Determines whether or not a series contains a particular time entry
        /// </summary>
        /// <param name="series"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static bool ContainsTime(INumericTimeDataSeries series, DateTime time)
        {
            if (series != null &&
                series.Count > 0 &&
                series.StartTime <= time &&
                series.StartTime.AddSeconds(series.TotalElapsedSeconds) >= time)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Create a distance track from an activity's GPS Route
        /// </summary>
        /// <param name="gpsActivity"></param>
        /// <returns>A distance track created from the GPS route</returns>
        public static IDistanceDataTrack CreateDistanceDataTrack(IActivity gpsActivity)
        {
            IDistanceDataTrack distanceTrack = new DistanceDataTrack();

            if (gpsActivity.GPSRoute != null && gpsActivity.GPSRoute.Count > 0)
            {
                float distance = 0;

                // First Point
                distanceTrack.Add(gpsActivity.GPSRoute.StartTime, 0);

                for (int i = 1; i < gpsActivity.GPSRoute.Count; i++)
                {
                    DateTime pointTime = gpsActivity.GPSRoute.StartTime.AddSeconds(gpsActivity.GPSRoute[i].ElapsedSeconds);
                    distance += gpsActivity.GPSRoute[i].Value.DistanceMetersToPoint(gpsActivity.GPSRoute[i - 1].Value);
                    distanceTrack.Add(pointTime, distance);
                }
            }

            return distanceTrack;
        }

        public static INumericTimeDataSeries GetRawGearTrack(IActivity activity)
        {
            if (activity != null && activity.CadencePerMinuteTrack != null)
            {
                return GetRawGearTrack(activity.CadencePerMinuteTrack, GetDistanceTrack(activity));
            }

            return new NumericTimeDataSeries();
        }

        public static INumericTimeDataSeries GetRawGearTrack(INumericTimeDataSeries cadenceTrack, INumericTimeDataSeries distanceTrack)
        {
            if (cadenceTrack == null || cadenceTrack.Count == 0 ||
                distanceTrack == null || distanceTrack.Count == 0)
            {
                return new NumericTimeDataSeries();
            }

            // Set min & max ratio for filtering data
            float minRatio = 0;
            float maxRatio = 12;
            double speed, deltaDist, deltaSeconds, cadence;
            float mPerRev;
            DateTime currTime, prevTime;
            ITimeValueEntry<float> currPoint, prevPoint;
            INumericTimeDataSeries gearSelection = new NumericTimeDataSeries();

            // This loops through the cadence track, and interpolates the distance track
            // This is in case the distance track and cadence track don't match for whatever reason.
            // Cadence was chosen becase it will change erratically, while distance track should be comparitavely smooth.
            for (int i = 1; i < cadenceTrack.Count; i++)
            {
                // Store current point time
                prevTime = cadenceTrack.EntryDateTime(cadenceTrack[i - 1]);
                currTime = cadenceTrack.EntryDateTime(cadenceTrack[i]);

                prevPoint = distanceTrack.GetInterpolatedValue(prevTime);
                currPoint = distanceTrack.GetInterpolatedValue(currTime);

                if (prevPoint != null && currPoint != null)
                {
                    deltaDist = currPoint.Value - prevPoint.Value;
                    deltaSeconds = (currTime - prevTime).TotalSeconds;
                    speed = deltaDist / deltaSeconds;

                    cadence = cadenceTrack[i].Value;
                    mPerRev = (float)(speed / cadence * 60);

                    /* Filter bad values
                     *  1) Not pedaling: cad == 0
                     *  2) Bad GPS (unlikely): deltaDist == 0
                     *  3) Out of bounds: minRatio < gearSelection < maxRatio
                     */
                    if (cadence != 0 && deltaDist != 0 && mPerRev > minRatio && mPerRev < maxRatio)
                    {
                        gearSelection.Add(currTime, mPerRev);
                    }
                }
            }

            return gearSelection;
        }

        /// <summary>
        /// Get the distance track of an activity.  In order of priority, gets the actual distance track, or creates a track from GPS, or returns an empty track if no data exists.
        /// </summary>
        /// <param name="activity">Activity to get a distance track for</param>
        /// <returns>Distance track for an activity.  Returns an empty track if none can be found/calculated.</returns>
        public static IDistanceDataTrack GetDistanceTrack(IActivity activity)
        {
            IDistanceDataTrack distanceTrack;

            if (activity.DistanceMetersTrack != null)
            {
                // #1 Use Distance track from activity
                distanceTrack = activity.DistanceMetersTrack;
            }
            else
            {
                if (activity.GPSRoute != null)
                {
                    // #2 Otherwise create a distance track from GPS
                    distanceTrack = GearUtils.CreateDistanceDataTrack(activity);
                }
                else
                {
                    // Else, no distance track, and cannot create one.
                    distanceTrack = new DistanceDataTrack();
                }
            }

            return distanceTrack;
        }

        /// <summary>
        /// Create gear guess selection chart based on gear ratios
        /// </summary>
        /// <param name="input">Pre-calculated raw data to estimate from.  This should already be smoothed or filtered if desired.</param>
        /// <returns>Gear guess NumericTimeDataSeries</returns>
        public static ITimeDataSeries<SprocketCombo> GuessSprockets(INumericTimeDataSeries input, List<SprocketCombo> sprockets)
        {
            ITimeDataSeries<SprocketCombo> guessSeries = new TimeDataSeries<SprocketCombo>();
            SprocketCombo lastValue = null;

            // Iterate through entire data series guessing each point
            foreach (ITimeValueEntry<float> item in input)
            {
                float diff = float.MaxValue;
                SprocketCombo guess = null;

                // Find closest match out of all gear ratios
                foreach (SprocketCombo sprocketCombo in sprockets)
                {
                    float n = Math.Abs(sprocketCombo.GearRatio - item.Value);
                    if (n < diff)
                    {
                        // Current closest match
                        diff = n;
                        guess = sprocketCombo;
                    }
                }

                if (guessSeries.Count > 0 && lastValue != guess)
                {
                    guessSeries.Add(input.EntryDateTime(item).AddMilliseconds(-1), lastValue);
                }

                guessSeries.Add(input.EntryDateTime(item), guess);
                lastValue = guess;
            }

            return guessSeries;
        }

        /// <summary>
        /// Create gear guess selection chart based on gear ratios
        /// </summary>
        /// <param name="input">Pre-calculated raw data to estimate from.  This should already be smoothed or filtered if desired.</param>
        /// <returns>Gear guess NumericTimeDataSeries</returns>
        public static NumericTimeDataSeries GuessGears(INumericTimeDataSeries input, List<SprocketCombo> sprockets)
        {
            ITimeDataSeries<SprocketCombo> guessSprockets = GuessSprockets(input, sprockets);
            NumericTimeDataSeries guessSeries = new NumericTimeDataSeries();
            DateTime lastTime = input.StartTime;

            // Iterate through entire data series, recreating the result
            foreach (ITimeValueEntry<SprocketCombo> item in guessSprockets)
            {
                float ratio = 0;

                if (item.Value != null)
                {
                    ratio = item.Value.GearRatio;
                }

                guessSeries.Add(guessSprockets.EntryDateTime(item), ratio);
            }

            return guessSeries;
        }
    }
}
