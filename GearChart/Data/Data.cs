using System;
using System.Collections.Generic;
using System.Text;
using ZoneFiveSoftware.Common.Data;
using ZoneFiveSoftware.Common.Data.Fitness;
using GearChart.UI;
using GearChart.Utils;

namespace GearChart.Common
{
	public class SprocketCombo
	{
		public SprocketCombo(float chainring, float cassette, float gearRatio)
		{
			m_ChainringSize = chainring;
			m_CassetteSize = cassette;
			m_GearRatio = gearRatio;
		}

		public float ChainringSize
		{
			get { return m_ChainringSize; }
		}

		public float CassetteSize
		{
			get { return m_CassetteSize; }
		}

		public float GearRatio
		{
			get { return m_GearRatio; }
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj is SprocketCombo)
			{
				SprocketCombo combo = obj as SprocketCombo;

				return combo.CassetteSize == CassetteSize &&
					   combo.ChainringSize == ChainringSize &&
					   combo.GearRatio == GearRatio;
			}

			return base.Equals(obj);
		}

		public static bool operator !=(SprocketCombo combo1, SprocketCombo combo2)
		{
			return !Equals(combo1, combo2);
		}

		public static bool operator ==(SprocketCombo combo1, SprocketCombo combo2)
		{
			return Equals(combo1, combo2);
		}

		private float m_ChainringSize;
		private float m_CassetteSize;
		private float m_GearRatio;
	}

	public class SprocketComboInfo
	{
		public SprocketComboInfo(SprocketCombo combo)
		{
			m_Combo = combo;
		}

		public SprocketCombo SprocketCombo
		{
			get { return m_Combo; }
		}

		public IValueRangeSeries<DateTime> Times
		{
			get { return m_Times; }
		}

		private SprocketCombo m_Combo;
		private IValueRangeSeries<DateTime> m_Times = new ValueRangeSeries<DateTime>();

		// Add other stats at will.  Distance, avg HR, etc.
		//  See ZoneFiveSoftware.Common.Data.Fitness.ZoneInfo for examples.  Ideally both would match
	}

    public static class Data
    {
        /// <summary>
        /// Gets the list of equipment ids defined in the settings
        /// </summary>
        /// <returns></returns>
        public static IList<string> GetEquipmentIds()
        {
            return Options.Instance.GetGearEquipmentIds();
        }
        
        /// <summary>
        /// Gets the equipment id used for a given activity
        /// </summary>
        /// <returns></returns>
        public static string GetActivityEquipmentId(IActivity activity)
        {
            return Options.Instance.GetGearEquipmentId(activity);
        }

        /// <summary>
        /// Gets the user list of the large gears (chainring).
        /// </summary>
        /// <returns></returns>
        public static List<float> GetChainringGears(IActivity activity)
        {
            string id = Options.Instance.GetGearEquipmentId(activity);
            return Options.Instance.GetBigGears(id);
        }

        /// <summary>
        /// Gets the user list of the small gears (cassette).
        /// </summary>
        /// <returns></returns>
        public static List<float> GetCassetteGears(IActivity activity)
        {
            string id = Options.Instance.GetGearEquipmentId(activity);
            return Options.Instance.GetSmallGears(id);
        }

        /// <summary>
        /// Gets the user list of sprocket combinations
        /// </summary>
        /// <returns></returns>
        public static List<SprocketCombo> GetSprocketCombos(IActivity activity)
        {
            string id = Options.Instance.GetGearEquipmentId(activity);
            return GetSprocketCombos(id);
        }

        /// <summary>
        /// Gets the user list of sprocket combinations
        /// </summary>
        /// <returns></returns>
        public static List<SprocketCombo> GetSprocketCombos(string equipmentId)
        {
            return Options.Instance.GetSprocketCombos(equipmentId);
        }

        /// <summary>
        /// Gets the Gear Track for a given activity.  Returns an empty track if unable to calculate (for instance if no GPS or Distance track).
        /// </summary>
        /// <param name="activity"></param>
        /// <returns>Returns an INumericDataSeries of the Gear track, or an empty track if nothing can be calculated.</returns>
        public static INumericTimeDataSeries GetGearTrack(IActivity activity)
        {
            // Raw data
            INumericTimeDataSeries gearTrack = ActivityGearTrackCache.Instance.CalculateRawTrack(activity);

            if (gearTrack.Count > 0)
            {
                // Smooth data track
                float min, max;
                gearTrack = ZoneFiveSoftware.Common.Data.Algorithm.NumericTimeDataSeries.Smooth(gearTrack, Constants.GearTrackSmoothing, out max, out min);

                // Estimate/round gear track
                string id = Options.Instance.GetGearEquipmentId(activity);
				List<SprocketCombo> sprockets = Options.Instance.GetSprocketCombos(id);
				gearTrack = GearUtils.GuessGears(gearTrack, sprockets);

                return gearTrack;
            }
            else
            {
                return new NumericTimeDataSeries();
            }
        }

        public static INumericTimeDataSeries GetGearTrack(IActivity activity, INumericTimeDataSeries cadenceTrack, INumericTimeDataSeries distanceTrack)
        {
            // Raw data
            INumericTimeDataSeries gearTrack = GearUtils.GetRawGearTrack(cadenceTrack, distanceTrack);

            if (gearTrack.Count > 0)
            {
                // Smooth data track
                float min, max;
                gearTrack = ZoneFiveSoftware.Common.Data.Algorithm.NumericTimeDataSeries.Smooth(gearTrack, Constants.GearTrackSmoothing, out max, out min);

                // Estimate/round gear track
                string id = Options.Instance.GetGearEquipmentId(activity);
                List<SprocketCombo> sprockets = Options.Instance.GetSprocketCombos(id);
                gearTrack = GearUtils.GuessGears(gearTrack, sprockets);

                return gearTrack;
            }
            else
            {
                return new NumericTimeDataSeries();
            }
        }

        /// <summary>
        /// Gets a data track representing the raw data used to calculate the gear track.
        /// </summary>
        /// <param name="activity"></param>
        /// <returns>Returns the raw gear track, or an empty INumericTimeDataSeries if none can be calculated.</returns>
        public static INumericTimeDataSeries GetRawGearTrack(IActivity activity)
        {
            return GearUtils.GetRawGearTrack(activity);
        }

		/// <summary>
		/// Gets the Sprocket Track for a given activity.  Returns an empty track if unable to calculate (for instance if no GPS or Distance track).
		/// </summary>
		/// <param name="activity"></param>
		/// <returns>Returns an ITimeDataSeries<SprocketCombo> of the sprocket combo used, or an empty track if nothing can be calculated.</returns>
		public static ITimeDataSeries<SprocketCombo> GetSprocketTrack(IActivity activity)
		{
			// Raw data
            INumericTimeDataSeries gearTrack = ActivityGearTrackCache.Instance.CalculateRawTrack(activity);
			ITimeDataSeries<SprocketCombo> sprocketTrack = new TimeDataSeries<SprocketCombo>();

			if (gearTrack.Count > 0)
			{
				// Smooth data track
				float min, max;
				gearTrack = ZoneFiveSoftware.Common.Data.Algorithm.NumericTimeDataSeries.Smooth(gearTrack, Constants.GearTrackSmoothing, out max, out min);

				// Estimate/round gear track
				string id = Options.Instance.GetGearEquipmentId(activity);
				List<SprocketCombo> sprockets = Options.Instance.GetSprocketCombos(id);
				sprocketTrack = GearUtils.GuessSprockets(gearTrack, sprockets);
			}

			return sprocketTrack;
		}

		/// <summary>
		/// Caculate basic stats for each sprocket.  This mostly allows to know the times during which we used a given gear
		/// </summary>
		/// <param name="activity"></param>
		/// <param name="sprocketTrack"></param>
		/// <returns>Returns an IList<SprocketComboInfo> of the sprocket combo info</returns>
		public static IList<SprocketComboInfo> Calculate(IActivity activity, ITimeDataSeries<SprocketCombo> sprocketTrack)
		{
			List<SprocketCombo> sprockets = GetSprocketCombos(activity);
			List<SprocketComboInfo> result = new List<SprocketComboInfo>(sprockets.Count + 1);
			SprocketCombo lastItemSprocket = null;
			DateTime sectionStartTime = sprocketTrack.StartTime;

			// Create the results for each zone
			foreach(SprocketCombo combo in sprockets)
			{
				result.Add(new SprocketComboInfo(combo));
			}

			// Go through and find the corresponding sprocket combo for each point
			foreach (ITimeValueEntry<SprocketCombo> item in sprocketTrack)
			{
				// We found a change in gear, add section to times
				if (lastItemSprocket != item.Value)
				{
					DateTime currentItemTime = sprocketTrack.EntryDateTime(item);

					// First item, ignore it
					if (lastItemSprocket != null)
					{
						int index = sprockets.IndexOf(lastItemSprocket);

						result[index].Times.Add(new ValueRange<DateTime>(sectionStartTime, currentItemTime));
					}

					sectionStartTime = currentItemTime;
					lastItemSprocket = item.Value;
				}
			}

			// Add last section
			if (lastItemSprocket != null && sprocketTrack.Count > 0)
			{
				int index = sprockets.IndexOf(lastItemSprocket);
				DateTime endTime = sprocketTrack.EntryDateTime(sprocketTrack[sprocketTrack.Count - 1]);

				result[index].Times.Add(new ValueRange<DateTime>(sectionStartTime, endTime));

				// There is also a "totals" when Calculate is used in ST, add it although it's a bit useless for now
				SprocketComboInfo totals = new SprocketComboInfo(null);

				totals.Times.Add(new ValueRange<DateTime>(sprocketTrack.StartTime, endTime));
				result.Add(totals);
			}

			return result;
		}

        public static void TriggerBikeSetupChangedEvent(object sender, string setupId)
        {
            if (GearChart.Common.Data.BikeSetupChanged != null)
            {
                GearChart.Common.Data.BikeSetupChanged(sender, setupId);
            }
        }

        public delegate void BikeSetupChangedEventHandler(object sender, string setupId);
        public static event BikeSetupChangedEventHandler BikeSetupChanged;
	}
}
